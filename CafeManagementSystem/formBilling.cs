using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CafeManagementSystem
{
    public partial class formBilling : Form
    {
        private int selectedOption;
        DataTable ActiveOrderDetails;
        DataTable ListOfBills;
        DataTable BillDetails;
        DataTable CafeDetails;
        Panel[] ActiveOrderPanels;
        Panel SelectedOrder;
        static int lastBillNumber;
        bool expanded;
        bool check = false;
        decimal GrossTotal;
        decimal GrossTotal1;
        decimal vat;
        decimal serviceCharge;

        static int OrderID1;
        static int BillNo1;

        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        public formBilling()
        {
            InitializeComponent();
            selectedOption = 1;
            ActiveOrderDetails = new DataTable();
            ListOfBills = new DataTable();
            BillDetails = new DataTable();
            CafeDetails = new DataTable();
            selectViewBills.Visible = false;
            selectBilling.Visible = false;
            Settings_Module.LoadCafeInformation(CafeDetails);
            LoadVaiableValues();

        }

        public void LoadVaiableValues()
        {
            lblCafeName.Text = CafeDetails.Rows[0]["CafeName"].ToString();
            label3.Text = CafeDetails.Rows[0]["CafeAddress"].ToString();
            label7.Text += ": " + CafeDetails.Rows[0]["CafePAN"].ToString();
            label9.Text += ": " + CafeDetails.Rows[0]["CafeContact"].ToString();
            try
            {
                vat = Convert.ToDecimal(CafeDetails.Rows[0]["CafeContact"].ToString());
            }
            catch(Exception) 
            {
                vat = 7m;
            }
            try
            {
                serviceCharge = Convert.ToDecimal(CafeDetails.Rows[0]["CafeContact"].ToString());
                
            }
            catch (Exception)
            {
                serviceCharge = 3.5m;
            }
        }


        private void formBilling_Load(object sender, EventArgs e)
        {
            selectBilling.Visible = true;
            panelViewBills.Visible = false;
            lastBillNumber = Billing_Module.getLastBillNumber();
            lastBillNumber = GenerateBillNumber();
            ActiveOrderPanels = new Panel[Billing_Module.getActiveOrderCount()];
            LoadActiveOrdersAsPanels();
            AutoRefreshTimer.Start();

            Billing_Module.openAllBills(ListOfBills);
            View1.DataSource = ListOfBills;

        }

        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            ActiveOrderPanels = null;
            ActiveOrderPanels = new Panel[Billing_Module.getActiveOrderCount()];
            LoadActiveOrdersAsPanels();
        }

        public static int GenerateBillNumber()
        {
            DateTime currentDate = DateTime.Now;
            string month = currentDate.ToString("MM");
            string year = currentDate.ToString("yy");

            string LastBillNumber = lastBillNumber.ToString();
            string lastBillMonth = LastBillNumber.Substring(2, 2);
            int last4digits = Convert.ToInt32(LastBillNumber.Substring(4));
            int next4digits = -1;
            if (string.Equals(month, lastBillMonth))
            {
                next4digits = last4digits + 1;
            }
            else
            {
                next4digits = next4digits + 1;
            }
            string billNumber = $"{year}{month}{next4digits:D4}";
            return Convert.ToInt32(billNumber);
        }

        private void deselectOtherOption()  //Deselect other options once a option has been selected
        {
            if (selectedOption == 1)
            {
                selectViewBills.Visible = false;
                panelViewBills.Visible = false;
            }
            else
            {
                selectBilling.Visible = false;
                panelBilling.Visible = false;
            }
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelBilling.Visible = true;
            selectBilling.Visible = true;
            deselectOtherOption();
        }

        private void btnViewBills_Click(object sender, EventArgs e)
        {
            selectedOption = 2;
            panelViewBills.Visible = true;
            selectViewBills.Visible = true;
            deselectOtherOption();
        }

        public void LoadActiveOrdersAsPanels()
        {

            tableLayoutPanel1.Controls.Clear();
            String query = @"Select * from Orders where OrderStatus = 1;";
            DataTable dt = new DataTable();
            Billing_Module.getActiveOrders(dt, query);
            System.Windows.Forms.Label[] lblDate = new System.Windows.Forms.Label[dt.Rows.Count];
            System.Windows.Forms.Label[] labelId = new System.Windows.Forms.Label[dt.Rows.Count];
            System.Windows.Forms.Label[] lblEmpId = new System.Windows.Forms.Label[dt.Rows.Count];
            System.Windows.Forms.Label[] labelTableNumber = new System.Windows.Forms.Label[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string id = row["OrderID"].ToString();
                string tableNumber = row["TableNumber"].ToString();
                string EmployeeID = row["EmployeeID"].ToString();
                string date = row["OrderDate"].ToString();
                ActiveOrderPanels[i] = new Panel();
                labelId[i] = new System.Windows.Forms.Label();
                labelTableNumber[i] = new System.Windows.Forms.Label();
                lblDate[i] = new System.Windows.Forms.Label();
                lblEmpId[i] = new System.Windows.Forms.Label();
                setUpActiveOrders(ActiveOrderPanels[i], labelId[i], labelTableNumber[i], lblDate[i], lblEmpId[i]);
                ActiveOrderPanels[i].Name = id;
                labelId[i].Text = "Order ID: " + id;
                labelTableNumber[i].Text = "Table: " + tableNumber;
                lblEmpId[i].Text = EmployeeID;
                lblDate[i].Text = date;
                tableLayoutPanel1.Controls.Add(ActiveOrderPanels[i]);
                i++;
            }

        }
        public void setUpActiveOrders(Panel panel, System.Windows.Forms.Label labelId, System.Windows.Forms.Label TableNumber, System.Windows.Forms.Label Date, System.Windows.Forms.Label EmployeeID)
        {

            labelId.AutoSize = true;
            labelId.BackColor = System.Drawing.Color.Transparent;
            labelId.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelId.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            labelId.Location = new System.Drawing.Point(18, 15);
            labelId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelId.Size = new System.Drawing.Size(120, 24);
            labelId.TabIndex = 99;
            labelId.Click += new System.EventHandler(LabelOrder_Click);

            TableNumber.AutoSize = true;
            TableNumber.BackColor = System.Drawing.Color.Transparent;
            TableNumber.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TableNumber.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            TableNumber.Location = new System.Drawing.Point(18, 48);
            TableNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            TableNumber.Size = new System.Drawing.Size(120, 24);
            TableNumber.TabIndex = 99;
            TableNumber.Click += new System.EventHandler(LabelOrder_Click);

            Date.Visible = false;
            EmployeeID.Visible = false;

            //Panel
            panel.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Size = new System.Drawing.Size(216, 140);
            panel.TabIndex = 104;
            panel.Click += new System.EventHandler(PanelOrder_Click);
            panel.Controls.Add(labelId);
            panel.Controls.Add(TableNumber);
            panel.Controls.Add(Date);
            panel.Controls.Add(EmployeeID);
        }

        public void LabelOrder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Label label = (System.Windows.Forms.Label)sender;
            Panel panel = (Panel)label.Parent;
            if (SelectedOrder == panel)
            {
                resetSelectedOrderItem(SelectedOrder);
            }
            else
            {
                resetSelectedOrderItem(SelectedOrder);
                setSelectedOrderItem(panel);
            }

        }
        public void PanelOrder_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (SelectedOrder == panel)
            {
                resetSelectedOrderItem(SelectedOrder);
            }
            else
            {
                resetSelectedOrderItem(SelectedOrder);
                setSelectedOrderItem(panel);
            }
        }

        public void setSelectedOrderItem(Panel panel)
        {
            AutoRefreshTimer.Stop();
            bool isSelected = (panel.BackColor == System.Drawing.Color.FromArgb(212, 188, 140)) ? false : true;
            if (!isSelected)
            {
                SelectedOrder = panel;
                lbltxtOrderID.Text = panel.Name;
                string[] Parts = panel.Controls[1].Text.Split(' ');
                lbltxtTable.Text = Parts[1];
                lbltxtDate.Text = panel.Controls[2].Text;
                lbltxtBillNo.Text = lastBillNumber.ToString();
                lbltxtEmployeeID.Text = panel.Controls[3].Text;
                panel.BackColor = System.Drawing.Color.FromArgb(91, 51, 11);
                panel.Controls[0].ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);
                panel.Controls[1].ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);

                int id = Convert.ToInt32(panel.Name);
                string query = "SELECT ItemName, Quantity, Price FROM OrderItemList INNER JOIN MenuItems ON OrderItemList.ItemID = MenuItems.MenuItemID WHERE OrderID = " + id;
                Billing_Module.getOrderItems(query, ActiveOrderDetails);
                // Add a new column 'TotalPrice'
                if (ActiveOrderDetails.Columns.Count == 3)
                    ActiveOrderDetails.Columns.Add("TotalPrice", typeof(decimal));

                // Calculate and add the TotalPrice for each row
                foreach (DataRow row in ActiveOrderDetails.Rows)
                {
                    int quantity = (int)row["Quantity"];
                    decimal rate = (decimal)row["Price"];
                    decimal totalPrice = quantity * rate;
                    totalPrice = Math.Round(totalPrice, 2);
                    row["TotalPrice"] = totalPrice;
                }

                ActiveOrderDetails.Rows.Add();
                ActiveOrderDetails.Rows.Add();
                ActiveOrderDetails.Rows.Add();
                dataGridView1.DataSource = ActiveOrderDetails;
                setUpBillingTable();
            }
        }

        public void resetSelectedOrderItem(Panel panel)
        {
            if (panel != null)
            {
                AutoRefreshTimer.Start();
                SelectedOrder = null;
                lbltxtBillNo.Text = "-------";
                lbltxtDate.Text = "---- -- -- -- -- -- --";
                lbltxtEmployeeID.Text = "----";
                lbltxtOrderID.Text = "--";
                lbltxtTable.Text = "--";
                panel.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
                panel.Controls[0].ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
                panel.Controls[1].ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
                ActiveOrderDetails.Clear();
                dataGridView1.DataSource = null;
            }
        }

        public void setUpBillingTable()
        {
            int quantity;
            decimal price;
            decimal totalPrice = 0;
            foreach (DataRow row in ActiveOrderDetails.Rows)
            {
                if (row["ItemName"].ToString() != "" && row["ItemName"].ToString() != null)
                {
                    quantity = Convert.ToInt32(row["Quantity"]);
                    price = Convert.ToDecimal(row["Price"]);
                    totalPrice += quantity * price;
                }
                else
                {
                    break;
                }
            }
            int lastRowIndex = ActiveOrderDetails.Rows.Count - 1;
            ActiveOrderDetails.Rows[lastRowIndex]["ItemName"] = "Sub Total";
            ActiveOrderDetails.Rows[lastRowIndex]["TotalPrice"] = totalPrice;

            ActiveOrderDetails.Rows.Add();
            lastRowIndex++;
            decimal serviceTax = totalPrice * (3.5m / 100);
            serviceTax = Math.Round(serviceTax, 2);
            ActiveOrderDetails.Rows[lastRowIndex]["ItemName"] = "Service Tax @3.5%";
            ActiveOrderDetails.Rows[lastRowIndex]["TotalPrice"] = serviceTax;

            ActiveOrderDetails.Rows.Add();
            lastRowIndex++;
            decimal VAT_Tax = totalPrice * (7m / 100);
            VAT_Tax = Math.Round(VAT_Tax, 2);
            ActiveOrderDetails.Rows[lastRowIndex]["ItemName"] = "VAT @7%";
            ActiveOrderDetails.Rows[lastRowIndex]["TotalPrice"] = VAT_Tax;

            ActiveOrderDetails.Rows.Add();
            lastRowIndex++;
            GrossTotal = totalPrice + serviceTax + VAT_Tax;
            decimal temp = Math.Round(GrossTotal, 0);
            string formattedValue = temp.ToString("F2");
            GrossTotal = Convert.ToDecimal(formattedValue);
            ActiveOrderDetails.Rows[lastRowIndex]["ItemName"] = "Amount Incl Of All Taxes"; //Service Tax @+getServiceTaxFromCafeInfo %
            ActiveOrderDetails.Rows[lastRowIndex]["TotalPrice"] = GrossTotal;

            dataGridView1.Columns[0].Width = (int)(dataGridView1.Width * 0.5);
            dataGridView1.Columns[1].Width = (int)(dataGridView1.Width * 0.1);
            dataGridView1.Columns[2].Width = (int)(dataGridView1.Width * 0.2);
            dataGridView1.Columns[3].Width = (int)(dataGridView1.Width * 0.2);
            dataGridView1.Columns["Price"].HeaderText = "Rate";
            dataGridView1.Columns["TotalPrice"].HeaderText = "Amount";
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.ReadOnly = true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (SelectedOrder != null)
            {
                DialogResult confirmationResult = MessageBox.Show("Are you sure you want to cancel the order?", "Order Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmationResult == DialogResult.Yes)
                {
                    int OrderID = Convert.ToInt32(SelectedOrder.Name);
                    Menu_And_Orders_Module.cancelOrder(OrderID);
                    ActiveOrderPanels = null;
                    ActiveOrderPanels = new Panel[Billing_Module.getActiveOrderCount()];
                    LoadActiveOrdersAsPanels();
                    AutoRefreshTimer.Start();
                }
            }
        }


        private void formBilling_Resize(object sender, EventArgs e)
        {
            panel5.Width = 100;
            panel6.Width = 100;
        }



        private void panel6_Resize(object sender, EventArgs e)
        {
            expanded = (tableLayoutPanel1.ColumnCount == 3);

            if (!expanded && check)
            {
                tableLayoutPanel1.ColumnCount = 3;
                expanded = true;
            }
            else
            {
                tableLayoutPanel1.ColumnCount = 2;
                expanded = false;
                check = true;
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (SelectedOrder != null)
            {
                int OrderID = Convert.ToInt32(lbltxtOrderID.Text);
                int BillNo = Convert.ToInt32(lbltxtBillNo.Text);
                if (Billing_Module.saveBill(BillNo, OrderID, GrossTotal))
                {
                    lastBillNumber++;
                    ActiveOrderPanels = null;



                    //print sequence

                    ActiveOrderPanels = new Panel[Billing_Module.getActiveOrderCount()];
                    LoadActiveOrdersAsPanels();
                    AutoRefreshTimer.Start();
                }
            }
        }




        //-==================================================================================================================================================================
        private void searchDate_ValueChanged(object sender, EventArgs e)
        {
            ListOfBills.Clear();
            txtBillNo.Clear();
            txtOrderID.Clear();
            DateTime pickedDate = searchDate.Value.Date;
            string date = pickedDate.ToString("yyyy-MM-dd");
            string query = "SELECT b.BillNo, o.OrderID, o.OrderDate, o.TableNumber, o.EmployeeID, b.PaidAmount FROM Orders AS o INNER JOIN Billing AS b ON o.OrderID = b.OrderID WHERE CONVERT(date, OrderDate) = '" + date + "';";
            Billing_Module.LoadData(query, ListOfBills);
            View1.DataSource = null;
            View1.DataSource = ListOfBills;
            
        }

        private void txtOrderID_KeyDown(object sender, KeyEventArgs e)
        {
            BillDetails.Clear();
            txtBillNo.Text = "";
            searchDate.Value = DateTime.Now.Date;
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtOrderID.Text))
                {
                    return;
                }
                int OrderID = Convert.ToInt32(txtOrderID.Text);
                if(Billing_Module.openByOrderID(OrderID, BillDetails))
                {
                    string query = "SELECT b.BillNo, o.OrderID, o.OrderDate, o.TableNumber, o.EmployeeID, b.PaidAmount FROM Orders AS o INNER JOIN Billing AS b ON o.OrderID = b.OrderID WHERE o.OrderID = "+OrderID+";";
                    Billing_Module.LoadData(query, ListOfBills);
                    if(ListOfBills.Rows.Count > 0)
                    {
                        txtBillNo.Text = ListOfBills.Rows[0]["BillNo"].ToString();
                    }
                    setupBillDetails();
                    View1.DataSource = null;
                    View1.DataSource = BillDetails;
                    setupView();
                }
            }
        }

        private void txtBillNo_KeyDown(object sender, KeyEventArgs e)
        {
            BillDetails.Clear();
            txtOrderID.Text = "";
            searchDate.Value = DateTime.Now.Date;
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txtBillNo.Text))
                {
                    return;
                }
                int BillNo = Convert.ToInt32(txtBillNo.Text);
                if (Billing_Module.openByBillNumber(BillNo, BillDetails))
                {
                    string query = "SELECT b.BillNo, o.OrderID, o.OrderDate, o.TableNumber, o.EmployeeID, b.PaidAmount FROM Orders AS o INNER JOIN Billing AS b ON o.OrderID = b.OrderID WHERE b.BillNo = " + BillNo + ";";
                    Billing_Module.LoadData(query, ListOfBills);
                    if (ListOfBills.Rows.Count > 0)
                    {
                        txtOrderID.Text = ListOfBills.Rows[0]["OrderID"].ToString();
                    }
                    
                    setupBillDetails();
                    View1.DataSource = null;
                    View1.DataSource = BillDetails;
                    setupView();
                }  
            }
        }

        public void setupBillDetails()
        {
            int quantity1;
            decimal rate1;
            decimal totalPrice1 = 0;

                // Add a new column 'TotalPrice'
            if (BillDetails.Columns.Count == 3)
            {
                BillDetails.Columns.Add("TotalPrice", typeof(decimal));
            }    
                
            if(BillDetails.Rows.Count == 0)
            {
                return;
            }

            // Calculate and add the TotalPrice for each row
            foreach (DataRow row in BillDetails.Rows)
            {
                if (!string.IsNullOrEmpty(row["Quantity"].ToString()) && !string.IsNullOrEmpty(row["Price"].ToString()))
                {
                    quantity1 = (int)row["Quantity"];
                    rate1 = (decimal)row["Price"];

                    decimal totalPrice2 = quantity1 * rate1;
                    totalPrice1 = Math.Round(totalPrice2, 2);
                    row["TotalPrice"] = totalPrice1;
                }
            }

            BillDetails.Rows.Add();
            BillDetails.Rows.Add();
            BillDetails.Rows.Add();
            int rowcount = BillDetails.Rows.Count - 1;
            BillDetails.Rows[rowcount]["ItemName"] = "Sub Total";
            BillDetails.Rows[rowcount]["TotalPrice"] = totalPrice1;


            BillDetails.Rows.Add();
            rowcount++;
            decimal serviceTax = (decimal)(totalPrice1 * (serviceCharge / 100));
            serviceTax = Math.Round(serviceTax, 2);
            BillDetails.Rows[rowcount]["ItemName"] = "Service Tax @" + serviceCharge + "%";
            BillDetails.Rows[rowcount]["TotalPrice"] = serviceTax;


            BillDetails.Rows.Add();
            rowcount++;
            decimal VAT_Tax = totalPrice1 * (5.0m / 100);
            VAT_Tax = Math.Round(VAT_Tax, 2);
            BillDetails.Rows[rowcount]["ItemName"] = "VAT @" + vat + "%";
            BillDetails.Rows[rowcount]["TotalPrice"] = VAT_Tax;


            BillDetails.Rows.Add();
            rowcount++;
            GrossTotal1 = totalPrice1 + serviceTax + VAT_Tax;
            decimal temp = Math.Round(GrossTotal1, 0);
            string formattedValue = temp.ToString("F2");
            GrossTotal1 = Convert.ToDecimal(formattedValue);
            BillDetails.Rows[rowcount]["ItemName"] = "Amount Incl Of All Taxes"; 
            BillDetails.Rows[rowcount]["TotalPrice"] = GrossTotal1;

            int quantity;
            decimal price;
            decimal totalPrice = 0;
            foreach (DataRow row in ActiveOrderDetails.Rows)
            {
                if (row["ItemName"].ToString() != "" && row["ItemName"].ToString() != null)
                {
                    quantity = Convert.ToInt32(row["Quantity"]);
                    price = Convert.ToDecimal(row["Price"]);
                    totalPrice += quantity * price;
                }
                else
                {
                    break;
                }
            }
        }

        public void setupView()
        {
            View1.Columns[0].Width = (int) (View1.Width* 0.5);
            View1.Columns[1].Width = (int) (View1.Width* 0.1);
            View1.Columns[2].Width = (int) (View1.Width* 0.2);
            View1.Columns[3].Width = (int) (View1.Width* 0.2);
            View1.Columns["Price"].HeaderText = "Rate";
            View1.Columns["TotalPrice"].HeaderText = "Amount";
            View1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            View1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            View1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void button7_Click(object sender, EventArgs e)
        {
   
            if (Billing_Module.saveBill(BillNo1, OrderID1, GrossTotal1))
            {
                searchDate.Value = DateTime.Today.Date;        
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Billing_Module.saveBill(BillNo1, OrderID1, GrossTotal1))
            {
                searchDate.Value = DateTime.Today.Date;
            }
        }

      
    }
}

