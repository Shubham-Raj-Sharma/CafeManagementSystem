using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ToolTip = System.Windows.Forms.ToolTip;

namespace CafeManagementSystem
{
    public partial class formInventoryManagement : Form
    {

        private ToolTip Tooltip1;
        private int selectedOption;  //Tracks the selected option in the menu 
        DataTable ItemsBought;
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        public formInventoryManagement()
        {
            InitializeComponent();
            Tooltip1 = new ToolTip();
            ItemsBought = new DataTable();

            panelManageItems.Visible = false;
            selectManage.Visible = false;
            selectedOption = 1;
            panelAddItems.Visible = true;
            selectAdd.Visible = true;

            Tooltip1.SetToolTip(btnAddItems, "Add Items to Inventory.");
            Tooltip1.SetToolTip(pictureBox2, "Manage Items in Inventory.");
        }

        private void formInventoryManagement_Load(object sender, EventArgs e)
        {
            
            try
            {
                string query = "Select * from InventoryPurchase;";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ItemsBought);
                conn.Close();
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("Error connecting to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn.Open();

                // Retrieve all the items from the InventoryStock table
                string query = "SELECT itemName FROM InventoryStock";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                // Clear existing items from the ComboBox
                txtcomboItemName.Items.Clear();

                // Add items from the table to the ComboBox
                while (reader.Read())
                {
                    string itemName = reader["itemName"].ToString();
                    txtcomboItemName.Items.Add(itemName);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items into ComboBox: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /* ---------------------------  Buttons that generate events when pressed   ----------------------------------*/

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelAddItems.Visible = true;
            selectAdd.Visible = true;
            deselectOtherOption();
        }

        private void btnManageItems_Click(object sender, EventArgs e)
        {
            selectedOption = 2;
            panelManageItems.Visible = true;
            selectManage.Visible = true;
            deselectOtherOption();
        }


        private void deselectOtherOption()  //Deselect other options once a option has been selected
        {
            if (selectedOption == 1)
            {
                panelManageItems.Visible = false;
                selectManage.Visible = false;
            }
            else
            {
                panelAddItems.Visible = false;
                selectAdd.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e) //Insert button
        {
            if (txtItemID.Text == "" || txtItemName.Text == "" || txtQuantity.Text == "" || txtPrice.Text == "" || txtSupplier.Text == "")
            {
                MessageBox.Show("Fields cannot be empty. Please enter values in all fields.");
            }
            else
            {

                string ItemName;
                int itemID;
                int quantity;
                decimal ItemPrice;
                string supplier;
                bool isIDNumber = int.TryParse(txtItemID.Text, out itemID);
                bool isQtyNumber = int.TryParse(txtQuantity.Text, out quantity);
                bool isItemPriceValid = Decimal.TryParse(txtPrice.Text, out ItemPrice);
                if (!isIDNumber)
                {
                    MessageBox.Show("Please enter integer value for item id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (!isQtyNumber)
                {
                    MessageBox.Show("Please enter integer values for quantity field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (!isItemPriceValid)
                {
                    MessageBox.Show("Please enter floating values for item price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                ItemName = txtItemName.Text;
                supplier = txtSupplier.Text;
                quantity = Convert.ToInt32(txtQuantity.Text);
                itemID = Convert.ToInt32(txtItemID.Text);
                ItemPrice = Convert.ToDecimal(txtPrice.Text);
                try
                {
                    conn.Open();
                    string query = "INSERT INTO InventoryPurchase (PurchaseID, ItemName, ItemPrice, Supplier, Quantity) " +
                                         "VALUES (@PurchaseID, @ItemName, @ItemPrice, @Supplier, @Quantity)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@PurchaseID", itemID);
                    cmd.Parameters.AddWithValue("@ItemName", ItemName);
                    cmd.Parameters.AddWithValue("@ItemPrice", ItemPrice);
                    cmd.Parameters.AddWithValue("@Supplier", supplier);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Entered Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtItemID.Clear();
                txtPrice.Clear();
                txtQuantity.Clear();
                txtItemName.Clear();
                txtSupplier.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)  //View Button
        {
            ItemsBought.Clear();
            View1.DataSource = null;
            View1.DataSource = ItemsBought;
            try
            {
                string query = "Select * from InventoryPurchase;";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ItemsBought);
                conn.Close();
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("Error connecting to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            View1.DataSource = ItemsBought;
            View1.ReadOnly = true;
        }

        private void View1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the selected row index and the DataGridViewRow object
                int selectedRowIndex = e.RowIndex;
                DataGridViewRow selectedRow = View1.Rows[selectedRowIndex];

                // Get the values from the selected row
                int purchaseID = Convert.ToInt32(selectedRow.Cells["PurchaseID"].Value);
                string itemName = Convert.ToString(selectedRow.Cells["ItemName"].Value);
                decimal itemPrice = Convert.ToDecimal(selectedRow.Cells["ItemPrice"].Value);
                string supplier = Convert.ToString(selectedRow.Cells["Supplier"].Value);
                int quantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value);

                txtItemID.Text = purchaseID.ToString();
                txtPrice.Text = itemPrice.ToString();
                txtQuantity.Text = quantity.ToString();
                txtItemName.Text = itemName;
                txtSupplier.Text = supplier;
            }
        }

        private void button7_Click(object sender, EventArgs e) //Update button
        {
            if (txtItemID.Text == "")
            {
                MessageBox.Show("Please enter the purchase id of the item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int itemID;
            bool isIDNumber = int.TryParse(txtItemID.Text, out itemID);
            if (!isIDNumber)
            {
                MessageBox.Show("Please enter integer value for item id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                string query = "SELECT 1 FROM InventoryPurchase WHERE PurchaseID = " + itemID + ";";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                if (!(dt.Rows.Count > 0))
                {
                    MessageBox.Show("The entered purchase id does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            catch(SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (txtItemName.Text == "" || txtQuantity.Text == "" || txtPrice.Text == "" || txtSupplier.Text == "")
            {
                MessageBox.Show("Fields cannot be empty. Please enter values in all fields.");
            }
            else
            {
                string ItemName;
                int quantity;
                decimal ItemPrice;
                string supplier;
                bool isQtyNumber = int.TryParse(txtQuantity.Text, out quantity);
                bool isItemPriceValid = Decimal.TryParse(txtPrice.Text, out ItemPrice);
                if (!isQtyNumber)
                {
                    MessageBox.Show("Please enter integer values for quantity field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                if (!isItemPriceValid)
                {
                    MessageBox.Show("Please enter floating values for item price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                ItemName = txtItemName.Text;
                supplier = txtSupplier.Text;
                quantity = Convert.ToInt32(txtQuantity.Text);
                itemID = Convert.ToInt32(txtItemID.Text);
                ItemPrice = Convert.ToDecimal(txtPrice.Text);
                try
                {
                    conn.Open();
                    string query2 = "UPDATE InventoryPurchase SET ItemName = @ItemName, ItemPrice = @ItemPrice, Supplier = @Supplier, Quantity = @Quantity " +
                                         "WHERE PurchaseID= @PurchaseID)";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    // Add parameters to the command
                    cmd2.Parameters.AddWithValue("@PurchaseID", itemID);
                    cmd2.Parameters.AddWithValue("@ItemName", ItemName);
                    cmd2.Parameters.AddWithValue("@ItemPrice", ItemPrice);
                    cmd2.Parameters.AddWithValue("@Supplier", supplier);
                    cmd2.Parameters.AddWithValue("@Quantity", quantity);
                    MessageBox.Show("Data Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    conn.Close();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtItemID.Clear();
                txtPrice.Clear();
                txtQuantity.Clear();
                txtItemName.Clear();
                txtSupplier.Clear();
            }
        }

        private void button5_Click(object sender, EventArgs e) //Delete button
        {
            if (txtItemID.Text == "")
            {
                MessageBox.Show("Please enter the purchase id of the item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int itemID;
            bool isIDNumber = int.TryParse(txtItemID.Text, out itemID);
            if (!isIDNumber)
            {
                MessageBox.Show("Please enter integer value for item id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                string query = "SELECT 1 FROM InventoryPurchase WHERE PurchaseID = " + itemID + ";";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                if (!(dt.Rows.Count > 0))
                {
                    MessageBox.Show("The entered purchase id does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }
            catch(SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                conn.Open();
                string query2 = "DELETE FROM InventoryPurchase WHERE PurchaseID = @PurchaseID)";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                // Add parameters to the command
                cmd2.Parameters.AddWithValue("@PurchaseID", itemID);
                MessageBox.Show("Data Deleted Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtItemID.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtItemName.Clear();
            txtSupplier.Clear();
        }

        private void button3_Click(object sender, EventArgs e) //btn Save
        {
            string itemName = txtcomboItemName.Text;
            int itemConsumedQty, QtyInStock;
            bool isitemConsumedQtyValid = int.TryParse(txtConsumedQty.Text, out itemConsumedQty);
            if (!isitemConsumedQtyValid)
            {
                MessageBox.Show("Please enter integer value for item id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            itemConsumedQty = Convert.ToInt32(txtConsumedQty.Text);
            try
            {
                string query = "SELECT Quantity FROM InventoryStock WHERE ItemName = " + itemName + ";";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    QtyInStock = (int)dt.Rows[0]["Quantity"];
                    if (QtyInStock <= 0)
                    {
                        MessageBox.Show("Item has run out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Item is not registered to stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int remainingQty = QtyInStock - itemConsumedQty;
            try
            {
                string query = "Update InventoryStock Set Quantity = " + remainingQty.ToString() + " WHERE ItemName = " + itemName + ";";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        //==================================================================================================================================================

    }
}
