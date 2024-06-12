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

namespace CafeManagementSystem
{
    
    public partial class PrintForm : Form
    {

        DataTable MenuCategories;
        DataTable MenuItemsofEachCategory;
        public PrintForm()
        {
            MenuCategories = new DataTable();
            MenuItemsofEachCategory = new DataTable();
            InitializeComponent();
        }

        private void PrintForm_LOAD(object sender, EventArgs e)
        {
            
            Menu_And_Orders_Module.getMenuCategories(MenuCategories);
            TreeNode []tn = new TreeNode[MenuCategories.Rows.Count];
            int i = 0;
            string category;
            string item;
            foreach (DataRow row in MenuCategories.Rows) 
            {
                category = row["Category"].ToString();
                tn[i] = new TreeNode(category);
                Menu_And_Orders_Module.getItem_From_Categories(MenuItemsofEachCategory, category);
                foreach(DataRow Row in MenuItemsofEachCategory.Rows)
                {
                    item = Row["ItemName"].ToString();
                    tn[i].Nodes.Add(new TreeNode(item));
                }
                MenuTreeView.Nodes.Add(tn[i]);
                i++;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
