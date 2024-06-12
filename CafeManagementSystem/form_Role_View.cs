using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    public partial class form_Role_View : Form
    {
        bool BillingLoadedState = false;
        bool EmployeeManagementLoadedState = false;
        bool MenuandOrdersLoadedState = false;
        bool ReservationsLoadedState = false;
        bool InventoryLoadedState = false;
        bool SalesandAnalyticsLoadedState = false;
        public form_Role_View()
        {
            InitializeComponent();
            
        }


        // ---------------------------------------------------- Role -> Views ---------------------------------------------------------------------------------------

        private void form_Role_View_Load(object sender, EventArgs e)
        {
            btnBilling.TabStop = false;
            btnMenuandOrders.TabStop = false;
            btnInventory.TabStop = false;
            buttonEmployeeManagement.TabStop = false;  
            btnBilling.Visible = false;
            buttonEmployeeManagement.Visible = false;
            btnMenuandOrders.Visible = false;
            btnInventory.Visible = false;
            string role = "Manager";//Login_Registration_Module.ROLE;
            switch(role)
            {
                case "Manager":
                    btnBilling.Visible = true;
                    buttonEmployeeManagement.Visible = true;
                    btnMenuandOrders.Visible = true;
                    btnInventory.Visible = true;
                    break;
                case "Cashier":
                    btnBilling.Visible = true;
                    btnMenuandOrders.Visible = true;
                    break;
                case "WaitStaff":
                    btnMenuandOrders.Visible= true;
                    break;           
            }
        }

        // -------------------------------------------------------------------- Tab Selection ------------------------------------------------------------------------------

        private void btnBilling_Click(object sender, EventArgs e)
        {
            if(BillingLoadedState)
            {
                showForm(panelBilling);
            }
            loadForm(new formBilling(), panelBilling);
            BillingLoadedState = true;
        }

        private void btnMenuandOrders_Click(object sender, EventArgs e)
        {
            if (MenuandOrdersLoadedState)
            {
                showForm(panelMenuandOrders);
            }
            loadForm(new formMenuAndOrders(), panelMenuandOrders);
            MenuandOrdersLoadedState = true;
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            if (InventoryLoadedState)
            {
                showForm(panelInventory);
            }
            loadForm(new formInventoryManagement(), panelInventory);
            InventoryLoadedState = true;
        }

        private void buttonEmployeeManagement_Click(object sender, EventArgs e)
        {
            if (EmployeeManagementLoadedState)
            {
                showForm(panelEmployeeManagement);
            }
            loadForm(new formEmployeeManagement(), panelEmployeeManagement);
            EmployeeManagementLoadedState = true;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            resetAllPanels();
            panelHome.Visible = true;
            panelHome.BringToFront();
        }

        public void loadForm(object form, Panel panel)
        {
            resetAllPanels();
            panel.Visible = true;
            panel.BringToFront();
            Form frm = form as Form;
            frm.TopLevel = false;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            panel.Controls.Add(frm);
            panel.Tag = frm;
            frm.Show();
        }

        public void showForm(Panel panel)
        {
            panel.Visible = true;
            panel.BringToFront();
        }

        public void resetAllPanels()
        {
            panelBilling.Visible = false;
            panelEmployeeManagement.Visible = false;
            panelInventory.Visible = false;
            panelSalesandAnalytics.Visible = false;
            panelMenuandOrders.Visible = false;
            panelReservations.Visible = false;
            panelHome.Visible = false; 
        }





        //---------------------------------------------------------------------- Window Buttons ------------------------------------------------------------------------
        private void btnMinimize_Click(object sender, EventArgs e)  //Minimize button
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btn_Maximize_Restore_Click(object sender, EventArgs e)  //Maximize button
        {
            if (WindowState == FormWindowState.Maximized)
            {
                btn_Maximize_Restore.Image = Properties.Resources.Maximize;
                WindowState = FormWindowState.Normal;
            }
            else
            {
                btn_Maximize_Restore.Image = Properties.Resources.RestoreDown;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
