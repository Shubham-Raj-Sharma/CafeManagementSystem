using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using ToolTip = System.Windows.Forms.ToolTip;


//------------------------------ "रु॰"
namespace CafeManagementSystem
{
    public partial class formMenuAndOrders : Form
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        String fileName;
        DataTable NewOrder;
        int ItemsCount = Menu_And_Orders_Module.getItemCount();
        int ActiveItemsCount = Menu_And_Orders_Module.getActiveItemCount();
        Panel[] OrderItemPanels = new Panel[Menu_And_Orders_Module.getItemCount()];
        Panel[] MenuItemPanels = new Panel[Menu_And_Orders_Module.getItemCount()];
        PictureBox[] pictureBox = new PictureBox[Menu_And_Orders_Module.getItemCount()];
        Label[] labelName = new Label[Menu_And_Orders_Module.getItemCount()];
        Label[] labelPrice = new Label[Menu_And_Orders_Module.getItemCount()];
        Label[] lblcategory1 = new Label[Menu_And_Orders_Module.getItemCount()];
        Label[] lblisActive = new Label[Menu_And_Orders_Module.getItemCount()];
        LinkLabel[] linkLabel = new LinkLabel[Menu_And_Orders_Module.getItemCount()];

        int rowValue = 1;
       // int panelOrderSummaryState = false;
        private int selectedOption;  //Tracks the selected option in the menu 
        private ToolTip Tooltip1;
        private int numberOfCategories;
        Panel activeMenuCategory;
        Panel activeOrderCategory;

        private SqlDataAdapter sda;
        private DataTable OrdersList;

        public formMenuAndOrders()
        {
            InitializeComponent();
            
            Tooltip1 = new ToolTip();
            /* ------------------------- Initializing the states of visual elements in the form ----------------------*/
            panelMenuItems.Visible = false;
            panelOrderContainer.Visible = false;
            selectOrder.Visible = false;
            selectMenu.Visible = false;
            panelAddItem.Visible = false;
            btnAdd1.Visible = false;
            panelOrderSummary.Visible = false;

            OrdersList = new DataTable();
            DateTime currentDate = DateTime.Now.Date;
            string date = currentDate.ToString("yyyy-MM-dd");
            Menu_And_Orders_Module.LoadData(date, OrdersList);
            dataGridView2.DataSource = OrdersList;
            dataGridView2.ReadOnly = true;

            NewOrder = new DataTable();
            NewOrder.Columns.Add("Sno", typeof(int));
            NewOrder.Columns.Add("ItemID", typeof(int));
            NewOrder.Columns.Add("ItemName", typeof(string));
            NewOrder.Columns.Add("Quantity", typeof(int));
            NewOrder.Columns.Add("Price", typeof(decimal));


        }

        private void formMenuAndOrders_Load(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelOrderContainer.Visible = true;
            selectOrder.Visible = true;
            activeMenuCategory = panel3;
            activeOrderCategory = panel44;
            
            searchTerm.Text = "Search By Item Name";
            textBox5.Text = "Search By Item Name";
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);

            loadMenuCategories();
            LoadMenuItemAsPanels();

            loadOrderCategories();
            LoadOrderItemAsPanels();

            Tooltip1.SetToolTip(pictureBox1, "Menu Items");
            Tooltip1.SetToolTip(pictureBox2, "Orders");
            Tooltip1.SetToolTip(pictureBox3, "Search");
            Tooltip1.SetToolTip(pictureBox6, "Search");
            Tooltip1.SetToolTip(btnOrderSummary, "View Order Summary");

        }




        //  ---------------------------- This is for selection of option (Order or Menu) in the title bar ------------------------------------------------------------
        private void deselectOtherOption()  //Deselect other options once a option has been selected
        {
            if (selectedOption == 1)
            {
                activeOrderCategory = panel44;
                panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
                loadOrderCategories();
                LoadOrderItemAsPanels();
                panelMenuItems.Visible = false;
                selectMenu.Visible = false;
                panelCategories.Controls.Clear();
                gridPanel.Controls.Clear();
            }
            else
            {
                activeMenuCategory = panel3;
                panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
                loadMenuCategories();
                LoadMenuItemAsPanels();
                panelOrderContainer.Visible = false;
                selectOrder.Visible = false;
                tableLayoutPanel4.Controls.Clear();
                tableLayoutPanel3.Controls.Clear();
                
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelOrderContainer.Visible = true;
            selectOrder.Visible = true;
            deselectOtherOption();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            selectedOption = 2;
            panelMenuItems.Visible = true;
            selectMenu.Visible = true;
            deselectOtherOption();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }



        //  ==================================================================== Start of Menu items ============================================================================
        //  =====================================================================================================================================================================



        // -------------------------------------------- This refreshes Menu page (as it updates every time it is repainted) ----------------------------------------------------


        // ------------------------------------------------- Loads categories for Menu page ----------------------------------------------------------------------------------
        private void loadMenuCategories()
        {  
            panelCategories.Controls.Clear();
            DataTable dataTable = new DataTable();
            Menu_And_Orders_Module.getMenuCategories(dataTable);
            numberOfCategories = dataTable.Rows.Count;
            Panel[] panel = new Panel[numberOfCategories];
            Label[] label = new Label[numberOfCategories];
            int i = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                string value = row["category"].ToString();
                panel[i] = new Panel();
                label[i] = new Label();
                label[i].Text = value;
                setUpCategoriesDesign(label[i], panel[i]);
                panelCategories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
                panelCategories.Controls.Add(panel[i]);

                panel[i].Enabled = true;
                panel[i].Click += new System.EventHandler(Panel_Click);
                label[i].Click += new System.EventHandler(Label_Click);
                i++;
            }
        }

        // ---------------------------------------------------- Design categories of Menu page ----------------------------------------------------------------------------------
        void setUpCategoriesDesign(Label label, Panel panel)
        {
            label.AutoSize = true;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Font = new System.Drawing.Font("Cooper Black", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            label.Location = new System.Drawing.Point(11, 7);
            label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label.Size = new System.Drawing.Size(50, 27);
            label.TabIndex = 97;

            panel.Controls.Add(label);
            panel.Location = new System.Drawing.Point(82, 102);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Cursor = System.Windows.Forms.Cursors.Hand;
            panel.Margin = new System.Windows.Forms.Padding(15, 20, 3, 3);
            panel.Size = new System.Drawing.Size(210, 45);
            panel.TabIndex = 102;
        }

        //---------------------------------------------------- Loads Menu Items for each category of Menu page ------------------------------------------------------------------
        private void LoadMenuItemAsPanels()
        {
            gridPanel.Controls.Clear();
            String query = @"Select * from MenuItems;";
            DataTable dt = new DataTable();
            Menu_And_Orders_Module.getItem_From_Categories(dt, query);

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string id = row["MenuItemID"].ToString();
                string Name = row["ItemName"].ToString();
                string Price = row["Price"].ToString();
                string category = row["Category"].ToString();
                string isActive = row["isActive"].ToString();
                byte[] pictureData = (byte[])row["ItemImage"];
                MenuItemPanels[i] = new Panel();
                pictureBox[i] = new PictureBox();
                labelName[i] = new Label();
                labelPrice[i] = new Label();
                linkLabel[i] = new LinkLabel();
                lblcategory1[i] = new Label();
                lblisActive[i] = new Label();
                MenuItemPanels[i] = new Panel();
                MenuItemPanels[i].Name = id;
                setUpMenuElementDesign(MenuItemPanels[i], lblcategory1[i], lblisActive[i], pictureBox[i], labelName[i], labelPrice[i], linkLabel[i]);
                labelName[i].Text = Name;
                labelPrice[i].Text = Price;
                linkLabel[i].Text = "Show More";
                lblcategory1[i].Text = category;
                lblisActive[i].Text = isActive; 
                pictureBox[i].Image = ConvertBinaryToImage(pictureData);
                if(MenuItemPanels[i].Controls[4].Text == "1")
                gridPanel.Controls.Add(MenuItemPanels[i]);
                i++;
            }
        }

        public void MenuItemFilter_Categories(string category)
        {
            gridPanel.Controls.Clear();
            if (category == "All")
            {
                for (int i = 0; i < ItemsCount; i++)
                {
                    if(MenuItemPanels[i].Controls[4].Text == "1")
                        gridPanel.Controls.Add(MenuItemPanels[i]);
                }
            }
            else if(category == "NotInMenu")
            {
                for (int i = 0; i < ItemsCount; i++)
                {
                    if (MenuItemPanels[i].Controls[4].Text == "0")
                    {
                        
                        gridPanel.Controls.Add(MenuItemPanels[i]);
                    }
                        
                }
            }
            else
            {
                for (int i = 0; i < ItemsCount; i++)
                {
                    if (MenuItemPanels[i].Controls[5].Text == category && MenuItemPanels[i].Controls[4].Text == "1")
                    {
                        gridPanel.Controls.Add(MenuItemPanels[i]);
                    }
                }
            }
        }

        public void MenuItemFilter_SearchTerm(string search)
        {
            gridPanel.Controls.Clear();
            deselectMenuCategory();
            activeMenuCategory = panel3;
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            DataTable dataTable = new DataTable();
            String query = @"Select * from MenuItems where isActive = 1 and ItemName like '%" + search + "%';";
            Menu_And_Orders_Module.getItem_From_Categories(dataTable, query);

            foreach (DataRow row in dataTable.Rows)
            {
                string itemName = row["ItemName"].ToString();

                for (int i = 0; i < ItemsCount; i++)
                {
                    if (MenuItemPanels[i].Controls[0].Text == itemName)
                    {
                        gridPanel.Controls.Add(MenuItemPanels[i]);
                    }
                }
            }
        }


        //--------------------------------------------------- Design Menu Items for each category of Menu page ------------------------------------------------------------------
        void setUpMenuElementDesign(Panel panel, Label lblcategory, Label lblisActive, PictureBox pictureBox, Label labelName, Label labelPrice, LinkLabel linkLabel)
        {
            //Picture Box
            pictureBox.BackColor = System.Drawing.Color.LightGray;
            pictureBox.Location = new System.Drawing.Point(7, 8);
            pictureBox.Size = new System.Drawing.Size(166, 126);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;


            //Name Label
            labelName.AutoSize = true;
            labelName.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            labelName.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelName.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            labelName.Location = new System.Drawing.Point(11, 141);
            labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelName.Size = new System.Drawing.Size(110, 21);
            labelName.TabIndex = 98;

            //Price Label
            labelPrice.AutoSize = true;
            labelPrice.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            labelPrice.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelPrice.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            labelPrice.Location = new System.Drawing.Point(11, 176);
            labelPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelPrice.Size = new System.Drawing.Size(60, 21);
            labelPrice.TabIndex = 99;

            //LinkLabel
            linkLabel.AutoSize = true;
            linkLabel.DisabledLinkColor = System.Drawing.Color.FromArgb(91, 51, 11);
            linkLabel.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            linkLabel.LinkColor = System.Drawing.Color.FromArgb(91, 51, 11);      //Need to change the color of this
            linkLabel.Location = new System.Drawing.Point(11, 207);
            linkLabel.Size = new System.Drawing.Size(85, 19);
            linkLabel.TabIndex = 100;
            linkLabel.TabStop = true;
            linkLabel.Click += new System.EventHandler(linkLabel_Click);

            //lblcategory        <---------not visible just to store category value
            lblcategory.Visible = false;

            //lblisActive        <---------not visible just to store active status;
            lblisActive.Visible = false;

            //Panel
            panel.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel.Location = new System.Drawing.Point(10, 10);
            panel.Margin = new System.Windows.Forms.Padding(10);
            panel.Size = new System.Drawing.Size(180, 238);
            panel.TabIndex = 100;
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(labelName);
            panel.Controls.Add(labelPrice);
            panel.Controls.Add(linkLabel);
            panel.Controls.Add(lblisActive);
            panel.Controls.Add(lblcategory);
        }


        // -------------------------------------------------- Event for when the categories are clicked the items in the category are loaded ------------------------------------
        private void Panel_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            activeMenuCategory = (Panel)sender;
            string category = activeMenuCategory.Controls[0].Text;
            activeMenuCategory.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories(category);                      //This will load the menu category
        }

        private void Label_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            Label clickedLabel = (Label)sender;
            activeMenuCategory = (Panel)clickedLabel.Parent;
            string category = clickedLabel.Text;
            clickedLabel.Parent.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories(category);                    //This will load the menu category
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            activeMenuCategory = (Panel)sender;
            string category = panel3.Controls[0].Text;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories(category);                    //This will load the menu category
        }
        private void label5_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            Label clickedLabel = (Label)sender;
            activeMenuCategory = (Panel)clickedLabel.Parent;
            string category = clickedLabel.Text;
            clickedLabel.Parent.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories(category);                    //This will load the menu category
        }

        private void panel11_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            activeMenuCategory = panel22;
            panel22.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories("NotInMenu");                  //This will load the menu category
        }
        private void label12_Click(object sender, EventArgs e)
        {
            deselectMenuCategory();
            activeMenuCategory = panel22;
            label5.Parent.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            MenuItemFilter_Categories("NotInMenu");                    //This will load the menu category
        }

        void deselectMenuCategory()                         // Deselecting the previous category when the new one is clicked
        {
            activeMenuCategory.BackColor = System.Drawing.Color.FromArgb(250, 242, 202);
        }

        // -------------------------------------------- This is all for searching in the menu ------------------------------------------------------------------------------
        private void pictureBox3_Click(object sender, EventArgs e)      //Search Icon: when icon is clicked
        {
            if (string.IsNullOrWhiteSpace(searchTerm.Text))
            {
                return;
            }
            String search = searchTerm.Text;
            MenuItemFilter_SearchTerm(search);
        }

        private void searchTerm_KeyDown(object sender, KeyEventArgs e)               //Search Text box: when enter is pressed
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(searchTerm.Text))
                {
                    return;
                }
                String search = searchTerm.Text;
                MenuItemFilter_SearchTerm(search);
            }
        }

        private void searchTerm_TextChanged(object sender, EventArgs e)
        {
            String search = searchTerm.Text;
            MenuItemFilter_SearchTerm(search);
        }

        private void searchTerm_Click(object sender, EventArgs e)    //Just to select entire string in search text box when it is clicked
        {
            searchTerm.SelectAll();
        }

        // --------------------- When the show more show more linked label is clicked a detail of the item is presented ---------------------------------------------------
        private void linkLabel_Click(object sender, EventArgs e)
        {
            setUpAddItemPanel(false);
            LinkLabel clickedLabel = (LinkLabel)sender;
            Panel selectedItem = (Panel)clickedLabel.Parent;
            int id = Convert.ToInt32(selectedItem.Name);
            DataTable dataTable = new DataTable();
            String query = @"Select * from MenuItems where MenuItemID = '" + id + "';";
            Menu_And_Orders_Module.getItem_From_Categories(dataTable, query);
            string Name = dataTable.Rows[0]["ItemName"].ToString();
            string Price = dataTable.Rows[0]["Price"].ToString();
            string category = dataTable.Rows[0]["Category"].ToString();
            string description = dataTable.Rows[0]["ItemDescription"].ToString();
            string isActive = dataTable.Rows[0]["isActive"].ToString();
            byte[] pictureData = (byte[])dataTable.Rows[0]["ItemImage"];
            if (isActive == "0")
                btnAdd1.Visible = true;
            txtCategory.Text = category;
            txtitemName.Text = Name;
            txtPrice.Text = Price;
            txtitemDescription.Text = description;
            ItemPicture.Image = ConvertBinaryToImage(pictureData);
            lblid.Text = selectedItem.Name;
        }

        // For setting up add item panel that displays the item info and allows us to add items thus it has to switch between (view item <==> add item)
        private void setUpAddItemPanel(bool state)
        {
            ResetAddItemPanel();
            if (state)
            {
                btnSaveChanges.Visible = false;
                btnAdd.Visible = true;
                btnAddPicture.Text = "Add Picture";
            }
            else
            {
                btnAdd.Visible = false;
                btnSaveChanges.Visible = true;
                btnAddPicture.Text = "Change Picture";
            }
            panelAddItem.Visible = true;
            panelAddItem.BringToFront();
        }

        private void ResetAddItemPanel()
        {
            txtCategory.Text = "";
            txtitemName.Text = "";
            txtPrice.Text = "";
            txtitemDescription.Text = "";
            ItemPicture.Image = global::CafeManagementSystem.Properties.Resources.addImage; 
        }

        private void btnSubmit_Click(object sender, EventArgs e)  //Reset the add item panel when the submit button is clicked to remove the previously entered record
        {
            setUpAddItemPanel(true);
        }

        // ------------------------------------------------Clear all textbox to remove the previously entered data -------------------------------------------------
        private void ClearAllFields() //After the employee has been successffully registered clear all fields (refresh)
        {
            txtitemName.Clear();
            txtPrice.Clear();
            txtCategory.Clear();
            txtitemDescription.Clear();
            ItemPicture.Image = null;
        }

        //--------------------------------------------------- To import the image from a file --------------------------------------------------------------
        private void btnAddPicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Files (*.png)|*.png", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    ItemPicture.Image = Image.FromFile(fileName);
                }
            }
        }

        //------------ To convert the image to binary and vice versa (For storing image to database and retrieving it and  displaying it) ----------------------------------------
        byte[] ConvertImageToBinary(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (image.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    // Handle other image formats if needed
                    throw new NotSupportedException("Image format not supported.");
                }

                return ms.ToArray();
            }
        }

        Image ConvertBinaryToImage(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null; // or return a default image placeholder if desired
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"Error converting binary to image: {ex.Message}");
                return null; // or return a default image placeholder or handle the error accordingly
            }
        }

        // ----------------------------------------------Equalizes the size of categories and item panel ------------------------------------------------------------------------
        private void panel2_SizeChanged(object sender, EventArgs e) 
        {
            panel4.Height = panel2.Height;
        }

        // ---------------------------------Add entered menu item to the database when submit button is clicked --------------------------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            if (verifyInput())
            {
                String itemName = txtitemName.Text;
                double price = Convert.ToDouble(txtPrice.Text);
                decimal itemPrice = (decimal)Math.Round(price, 2, MidpointRounding.AwayFromZero);
                String itemCategory = txtCategory.Text;
                String itemDescription = txtitemDescription.Text;
                try
                {
                    byte[] imageValue = ConvertImageToBinary(ItemPicture.Image);
                    if (Menu_And_Orders_Module.StoreNewMenuItem(itemName, itemPrice, itemCategory, itemDescription, imageValue))
                    {
                        MessageBox.Show("Item has been successfully saved to the database.", "Item Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            deselectMenuCategory();
            activeMenuCategory = panel3;
            activeOrderCategory = panel44;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            loadMenuCategories();
            LoadMenuItemAsPanels();
            loadOrderCategories();
            LoadOrderItemAsPanels();
        }

        // -------------------------------Verify all the information entered by user about the item ----------------------------------------------------------
        private bool verifyInput()
        {
            if (txtitemName.Text == "")
            {
                MessageBox.Show("Item name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (IsValidPrice(txtPrice.Text))
            {
                return false;
            }

            if (txtitemDescription.Text == "")
            {
                MessageBox.Show("Item description cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtCategory.Text == null)
            {
                MessageBox.Show("Item category cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ItemPicture.Image == null)
            {
                MessageBox.Show("Please add a suitable image for the item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool IsValidPrice(string price)
        {
            if (price == "")
            {
                MessageBox.Show("Price Field cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            for (int i = 0; i < price.Length; i++)
            {
                if (!char.IsDigit(price[i]) && price[i] != '.')
                {
                    MessageBox.Show("Price Field can only have numbers and decimal point.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            return false;
        }

        // --------------------------------------------------------- To save changes made to item when its details are opened -----------------------------------------------
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (verifyInput())
            {
                int id = Convert.ToInt32(lblid.Text);
                String itemName = txtitemName.Text;
                double price = Convert.ToDouble(txtPrice.Text);
                decimal itemPrice = (decimal)Math.Round(price, 2, MidpointRounding.AwayFromZero);
                String itemCategory = txtCategory.Text;
                String itemDescription = txtitemDescription.Text;
                try
                {
                    byte[] imageValue = ConvertImageToBinary(ItemPicture.Image);
                    if (Menu_And_Orders_Module.UpdateMenuItem(id, itemName, itemPrice, itemCategory, itemDescription, imageValue))
                    {
                        MessageBox.Show("Item has been successfully added to the Menu.", "Item Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAllFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            deselectMenuCategory();
            activeMenuCategory = panel3;
            activeOrderCategory = panel44;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            loadMenuCategories();
            LoadMenuItemAsPanels();
            loadOrderCategories();
            LoadOrderItemAsPanels();
        }


        // --------------------------------------------------------  Remove items from the menu -----------------------------------------------------------------------------
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblid.Text);
            try
            {

                if (Menu_And_Orders_Module.RemoveItem(id))
                {
                    MessageBox.Show("Item has been successfully removed from the Menu.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            deselectMenuCategory();
            activeMenuCategory = panel3;
            activeOrderCategory = panel44;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            loadMenuCategories();
            LoadMenuItemAsPanels();
            loadOrderCategories();
            LoadOrderItemAsPanels();
            panelAddItem.SendToBack();
            panelAddItem.Visible = false;
        }

        // --------------------------------------------------------  Add items from the menu -----------------------------------------------------------------------------

        private void btnAdd1_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblid.Text);
            try
            {

                if (Menu_And_Orders_Module.AddItem(id))
                {
                    MessageBox.Show("Item has been successfully added to the Menu.", "Item Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAllFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            deselectMenuCategory();
            activeMenuCategory = panel3;
            activeOrderCategory = panel44;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            loadMenuCategories();
            LoadMenuItemAsPanels();
            loadOrderCategories();
            LoadOrderItemAsPanels();
            btnAdd1.Visible = false;
            panelAddItem.SendToBack();
            panelAddItem.Visible = false;
        }

        // ------------------------------------------------- Close the add item panel when the close button is clicked --------------------------------------------------

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            panelAddItem.Visible = false;
            panelAddItem.SendToBack();
        }


        //  =====================================================================   End of Menu items Categories  ===============================================================
        //  =====================================================================================================================================================================







        //  =====================================================================   Start of Order items ========================================================================
        //  =====================================================================================================================================================================


        // ------------------------------------------------- Loads categories for Order page -----------------------------------------------------------------------------------

        private void loadOrderCategories()
        {
            tableLayoutPanel4.Controls.Clear();
            DataTable dataTable = new DataTable();
            Menu_And_Orders_Module.getMenuCategories(dataTable);
            numberOfCategories = dataTable.Rows.Count;
            Panel[] panel = new Panel[numberOfCategories];
            Label[] label = new Label[numberOfCategories];
            int i = 0;
            tableLayoutPanel4.Controls.Add(panel44);
            foreach (DataRow row in dataTable.Rows)
            {
                string value = row["category"].ToString();
                panel[i] = new Panel();
                label[i] = new Label();
                label[i].Text = value;
                setUpOrderCategoriesDesign(label[i], panel[i]);
                tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
                tableLayoutPanel4.Controls.Add(panel[i]);

                panel[i].Enabled = true;
                panel[i].Click += new System.EventHandler(PanelOrder_Click);
                label[i].Click += new System.EventHandler(LabelOrder_Click);
                i++;
            }
        }
        
        // -------------------------------------------------------- Design categories of Order page ----------------------------------------------------------------------------
        void setUpOrderCategoriesDesign(Label label, Panel panel)
        {
            label.AutoSize = true;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Font = new System.Drawing.Font("Cooper Black", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            label.Location = new System.Drawing.Point(11, 7);
            label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label.Size = new System.Drawing.Size(50, 27);
            label.TabIndex = 97;

            panel.Controls.Add(label);
            panel.Location = new System.Drawing.Point(82, 102);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Cursor = System.Windows.Forms.Cursors.Hand;
            panel.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            panel.Size = new System.Drawing.Size(210, 45);
            panel.TabIndex = 102;
        }



        //------------------------------------------------- Loads Order Items for each category of Order page ------------------------------------------------------------------

        public void LoadOrderItemAsPanels()
        {
            tableLayoutPanel3.Controls.Clear();
            String query = @"Select * from MenuItems where isActive = 1;";
            DataTable dt = new DataTable();
            Menu_And_Orders_Module.getItem_From_Categories(dt, query);
            NumericUpDown[] QtyUpDown = new NumericUpDown[dt.Rows.Count];
            Label[] labelName = new Label[dt.Rows.Count];
            Label[] labelPrice = new Label[dt.Rows.Count];
            Label[] lblQty = new Label[dt.Rows.Count];
            Label[] lblcategory = new Label[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string id = row["MenuItemID"].ToString();
                string Name = row["ItemName"].ToString();
                string Price = row["Price"].ToString();
                string category = row["Category"].ToString();
                byte[] pictureData = (byte[])row["ItemImage"];
                OrderItemPanels[i] = new Panel();
                QtyUpDown[i] = new NumericUpDown();
                labelName[i] = new Label();
                labelPrice[i] = new Label();
                lblQty[i] = new Label();
                lblcategory[i] = new Label();
                OrderItemPanels[i] = new Panel();
                OrderItemPanels[i].Name = id;
                setUpOrderElement(OrderItemPanels[i], lblcategory[i], lblQty[i], QtyUpDown[i], labelPrice[i], labelName[i]);
                labelName[i].Text = Name;
                labelPrice[i].Text = Price;
                lblQty[i].Text = "Qty.";
                lblcategory[i].Text = category;
                tableLayoutPanel3.Controls.Add(OrderItemPanels[i]);
                i++;
            }
        }
        public void OrderItemFilter_Categories(string category)
        {
            tableLayoutPanel3.Controls.Clear();
            if (category == "All")
            {
                for (int i = 0; i < ActiveItemsCount; i++)
                {
                    if (OrderItemPanels[i].BackColor == System.Drawing.Color.FromArgb(91, 51, 11))
                        tableLayoutPanel3.Controls.Add(OrderItemPanels[i]);
                }
                for (int i = 0; i < ActiveItemsCount; i++)
                {
                    if (OrderItemPanels[i].BackColor != System.Drawing.Color.FromArgb(91, 51, 11))
                        tableLayoutPanel3.Controls.Add(OrderItemPanels[i]);
                }
            }
            else
            {
                for (int i = 0; i < ActiveItemsCount; i++)
                {
                    
                    if (OrderItemPanels[i].Controls[4].Text == category)
                    {
                        tableLayoutPanel3.Controls.Add(OrderItemPanels[i]);
                    }
                }
            }
        }

        public void OrderItemFilter_SearchTerm(string search)
        {
            deselectOrderCategory();
            activeOrderCategory = panel44;
            panel44.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            tableLayoutPanel3.Controls.Clear();
            DataTable dataTable = new DataTable();
            String query = @"Select * from MenuItems where isActive = 1 and ItemName like '%" + search + "%';";
            Menu_And_Orders_Module.getItem_From_Categories(dataTable, query);

            foreach (DataRow row in dataTable.Rows)
            {
                string itemName = row["ItemName"].ToString();
                for (int i = 0; i < ActiveItemsCount; i++)
                {
                    if (OrderItemPanels[i].Controls[3].Text == itemName)
                    {
                        tableLayoutPanel3.Controls.Add(OrderItemPanels[i]);
                    }
                }
            }
        }
        //------------------------------------------------- Design Menu Items for each category of Menu page ------------------------------------------------------------------
        public void setUpOrderElement(Panel panel, Label lblcategory, Label lblQty, NumericUpDown QtyUpDown, Label labelPrice, Label labelName)
        {

            //Name Label
            labelName.AutoSize = true;
            labelName.BackColor = System.Drawing.Color.Transparent;
            labelName.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelName.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            labelName.Location = new System.Drawing.Point(18, 15);
            labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelName.Size = new System.Drawing.Size(120, 24);
            labelName.TabIndex = 99;
            labelName.Click += new System.EventHandler(Label2Item_Click);


            //Price Label
            labelPrice.AutoSize = true;
            labelPrice.BackColor = System.Drawing.Color.Transparent;
            labelPrice.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            labelPrice.ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
            labelPrice.Location = new System.Drawing.Point(18, 48);
            labelPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelPrice.Size = new System.Drawing.Size(64, 24);
            labelPrice.TabIndex = 99;
            labelPrice.Click += new System.EventHandler(Label1Item_Click);

            // numericUpDown1 
            QtyUpDown.BackColor = System.Drawing.Color.FromArgb(91, 51, 11);
            QtyUpDown.Font = new System.Drawing.Font("Cooper Black", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            QtyUpDown.ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);
            QtyUpDown.Size = new System.Drawing.Size(52, 35);
            QtyUpDown.Location = new System.Drawing.Point(150, 95);
            QtyUpDown.TabIndex = 100;
            QtyUpDown.ValueChanged += new System.EventHandler(QtyUpDown_ValueChanged);
            QtyUpDown.Visible = false;

            //lblQty
            lblQty.BackColor = System.Drawing.Color.Transparent;
            lblQty.Font = new System.Drawing.Font("Cooper Black", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblQty.ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);
            lblQty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            lblQty.Size = new System.Drawing.Size(52, 21);
            lblQty.Location = new System.Drawing.Point(95, 100);
            lblQty.TabIndex = 101;
            lblQty.Text = "Qty.";
            lblQty.Visible = false;

            //lblcategory        <---------not visible just to store category value
            lblcategory.Visible = false;

            //Panel
            panel.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Size = new System.Drawing.Size(216, 140); 
            panel.TabIndex = 104;
            panel.Click += new System.EventHandler(PanelItem_Click);
            panel.Controls.Add(lblQty);
            panel.Controls.Add(QtyUpDown);
            panel.Controls.Add(labelPrice);
            panel.Controls.Add(labelName);
            panel.Controls.Add(lblcategory);
        }


        // -------------------------------------------------- Event for when the categories are clicked the items in the category are loaded -------------------------

        private void PanelOrder_Click(object sender, EventArgs e)
        {
            deselectOrderCategory();
            activeOrderCategory = (Panel)sender;
            string category = activeOrderCategory.Controls[0].Text;
            activeOrderCategory.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            OrderItemFilter_Categories(category);                    //This will load the menu category
        }

        private void LabelOrder_Click(object sender, EventArgs e)
        {
            deselectOrderCategory();
            Label clickedLabel = (Label)sender;
            activeOrderCategory = (Panel)clickedLabel.Parent;
            string category = clickedLabel.Text;
            clickedLabel.Parent.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            OrderItemFilter_Categories(category);                    //This will load the menu category
        }

        private void panel44_Click(object sender, EventArgs e)
        {
            deselectOrderCategory();
            activeOrderCategory = (Panel)sender;
            string category = panel44.Controls[0].Text;
            panel3.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            OrderItemFilter_Categories(category);                    //This will load the menu category
        }

        private void label20_Click(object sender, EventArgs e)
        {
            deselectOrderCategory();
            activeOrderCategory = (Panel)label12.Parent;
            string category = label12.Text;
            label12.Parent.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
            OrderItemFilter_Categories(category);                    //This will load the menu category
        }

        void deselectOrderCategory()                         // Deselecting the previous category when the new one is clicked
        {
            activeOrderCategory.BackColor = System.Drawing.Color.FromArgb(250, 242, 202);
        }


        // -------------------------------------------- This is all for searching in the menu ------------------------------------------------------------------------------

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                return;
            }
            String search = textBox5.Text;
            OrderItemFilter_SearchTerm(search);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    return;
                }
                String search = textBox5.Text;
                OrderItemFilter_SearchTerm(search);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            String search = textBox5.Text;
            OrderItemFilter_SearchTerm(search);
        }

        private void textBox5_Click(object sender, EventArgs e)   //Just to select entire string in search text box when it is clicked
        {
            textBox5.SelectAll();
        }


        // --------------------- When the panel is clicked the item is added as order and quantity is shown to select quantity ---------------------------------------------------
        private void PanelItem_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            setSelectedOrderItem(panel);
        }

        private void Label1Item_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Panel panel = (Panel)label.Parent;
            setSelectedOrderItem(panel);
        }

        private void Label2Item_Click(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            Panel panel = (Panel)label.Parent;
            setSelectedOrderItem(panel);
        }

        public void setSelectedOrderItem(Panel panel)
        {
            bool isSelected = (panel.BackColor == System.Drawing.Color.FromArgb(212, 188, 140)) ? false : true;
            if (!isSelected)
            {
                panel.BackColor = System.Drawing.Color.FromArgb(91, 51, 11);
                panel.Controls[0].Visible = true;
                panel.Controls[1].Visible = true;
                panel.Controls[2].ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);
                panel.Controls[3].ForeColor = System.Drawing.Color.FromArgb(212, 188, 140);

            }
            else
            {
                panel.BackColor = System.Drawing.Color.FromArgb(212, 188, 140);
                panel.Controls[0].Visible = false;
                panel.Controls[1].Visible = false;
                panel.Controls[2].ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
                panel.Controls[3].ForeColor = System.Drawing.Color.FromArgb(91, 51, 11);
                panel.Controls[1].Text = "0";
            }
        }

        private void QtyUpDown_ValueChanged(object sender, EventArgs e)
        {
            
            NumericUpDown QtyUpDown = (NumericUpDown)sender;
            Panel panel = (Panel)QtyUpDown.Parent;
            int itemid = Convert.ToInt32(panel.Name);
            string itemName = panel.Controls[3].Text;
            int quantity = Convert.ToInt32(QtyUpDown.Value);
            decimal price = Convert.ToDecimal(panel.Controls[2].Text);
            DataRow row = NewOrder.NewRow();
            row["Sno"] = rowValue;
            row["ItemID"] = itemid;
            row["ItemName"] = itemName;
            row["Quantity"] = quantity;
            row["Price"] = price;
            DataRow[] matchingRows = NewOrder.Select($"ItemName = '{itemName}'");
            if (quantity == 0)         //If the quantity of item is 0. remove the item form the dataTable
            {
                if (matchingRows.Length > 0)
                {
                    --rowValue;                           //Decrement the rowValue since a row has been removed
                    DataRow rowToDelete = matchingRows[0];
                    NewOrder.Rows.Remove(rowToDelete);

                    //Add code to rearrange the serial no for items in datatable, once the item is removed
                    int index = 1;
                    foreach (DataRow dataRow in NewOrder.Rows)
                    {
                        dataRow["Sno"] = index++;
                    }
                }

            }
            else       //If the quantity is greater than 0, add the item to the dataTable
            {
                if(matchingRows.Length > 0)    //if that item already exists in the table update the value
                {
                    DataRow rowToUpdate = matchingRows[0];
                    rowToUpdate["Quantity"] = quantity;

                } 
                else  //If the item does not exist in the table add it to the dataTable
                {
                    ++rowValue;
                    NewOrder.Rows.Add(row);
                }
            }
            dataGridView1.DataSource = NewOrder;
            dataGridView1.Refresh();
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        private void panel40_Resize(object sender, EventArgs e)
        {
            panel42.Height = panel40.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(EmpIDtext.Text == "")
            {
                MessageBox.Show("Enter the Employee ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Enter the Table No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(NewOrder.Rows.Count==0)
            {
                MessageBox.Show("Empty order list. Please add items to the order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int EmployeeID = Convert.ToInt32(EmpIDtext.Text);
            int TableNumber = Convert.ToInt32(comboBox1.Text);
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy-MM-dd hh:mm:ss tt");
            if (Menu_And_Orders_Module.StoreNewOrder(TableNumber, EmployeeID, date, NewOrder) )
            {
                ClearAllFields();
                NewOrder.Clear();  
                LoadOrderItemAsPanels();
                MessageBox.Show("Order created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                date = now.ToString("yyyy-MM-dd");
                Menu_And_Orders_Module.LoadData(date, OrdersList);
                dataGridView2.DataSource = OrdersList;
                dataGridView2.ReadOnly = true;
            }
            else
            {
                return;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            NewOrder.Clear();
            dataGridView1.DataSource = null;
            loadOrderCategories();
            LoadOrderItemAsPanels();
        }

        // ---------------------------------------- Exit Button of the form ----------------------------------------------------------------

        private void btnOrderSummary_Click(object sender, EventArgs e)
        {
            panelOrderSummary.BringToFront();
            panelOrderSummary.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelOrderSummary.SendToBack();
            panelOrderSummary.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dat(object sender, EventArgs e)
        {
            DateTime pickedDate = dateTimePicker1.Value.Date;
            string date = pickedDate.ToString("yyyy-MM-dd");
            Menu_And_Orders_Module.LoadData(date, OrdersList);
            dataGridView2.DataSource = OrdersList;
            dataGridView2.ReadOnly = true;
        }
    }
}
 