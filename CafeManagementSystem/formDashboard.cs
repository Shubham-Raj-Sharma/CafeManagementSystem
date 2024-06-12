using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    public partial class formDashboard : Form
    {
        bool isSidebarCollapsed = false;
        int selectedOption;
        public formDashboard()
        {
            InitializeComponent();
            labelProfile.Text = Login_Registration_Module.USER + " as " + Login_Registration_Module.ROLE;
            menu1.Visible = false;
            timerSidebar.Enabled = false;
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            panel8.Margin = new Padding(4, 60, 2, 1);
            selectedOption = 4;
            selectedMenu(selectedOption);
        }




        public void restoreOriginal()
        {
            panelHomebtn.BackColor = Color.FromArgb(212, 188, 140);
            panelInventory.BackColor = Color.FromArgb(212, 188, 140);
            panel1.BackColor = Color.FromArgb(212, 188, 140);
            panel5.BackColor = Color.FromArgb(212, 188, 140);
            panel3.BackColor = Color.FromArgb(212, 188, 140);
            panel6.BackColor = Color.FromArgb(212, 188, 140);
            panel8.BackColor = Color.FromArgb(212, 188, 140);
        }
    
        public void loadForm(object form)
        {
            panelBody.Visible = true;
            panelBody.BringToFront();
            if (this.panelBody.Controls.Count > 0)
            {
                Control existingForm = this.panelBody.Controls[0];
                existingForm.Dispose();
                this.panelBody.Controls.Clear();
            }
            Form frm = form as Form;
            frm.TopLevel = false;
            frm.TopLevel = false;
            frm.Dock = DockStyle.Fill;
            this.panelBody.Controls.Add(frm);
            this.panelBody.Tag = frm;
            frm.Show();          
        }


        

        private void selectedMenu(int selectedOption)
        {
            if(isSidebarCollapsed) 
            {
                resetSelection();
            }
            else
            {
                restoreOriginal();
            }
            switch(selectedOption) 
            {
                case 1:
                    panel1.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 2:
                    panelInventory.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 3:
                    panel3.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 4:
                    panelHomebtn.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 5:
                    panel5.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 6:
                    panel6.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                case 8:
                    panel8.BackColor = Color.FromArgb(250, 242, 202);
                    break;
                default:
                    break;
            }
        }

        private void resetSelection()
        {
            panel1.BackColor = Color.FromArgb(91, 51, 11);
            panelHomebtn.BackColor = Color.FromArgb(91, 51, 11);
            panelInventory.BackColor = Color.FromArgb(91, 51, 11);
            panel3.BackColor = Color.FromArgb(91, 51, 11);
            panel5.BackColor = Color.FromArgb(91, 51, 11);
            panel6.BackColor = Color.FromArgb(91, 51, 11);
            panel8.BackColor = Color.FromArgb(91, 51, 11);
        }
        //----------------------------------------------------------- Making the Dashboard Menu --------------------------------------------------



        private void panelHomebtn_Click(object sender, EventArgs e)
        {
            selectedOption = 4;
            selectedMenu(selectedOption);
            panelBody.Visible = false;
            panelHome.Visible = true;
            panelHome.BringToFront();
        }

        private void lblHomebtn_Click(object sender, EventArgs e)
        {
            selectedOption = 4;
            selectedMenu(selectedOption);
            panelBody.Visible = false;
            panelHome.Visible = true;
            panelHome.BringToFront();
        }

        private void iconHomebtn_Click(object sender, EventArgs e)
        {
            selectedOption = 4;
            selectedMenu(selectedOption);
            panelBody.Visible = false;
            panelHome.Visible = true;
            panelHome.BringToFront();
        }

        private void CMSlogo_Click(object sender, EventArgs e)
        {
            selectedOption = 4;
            selectedMenu(selectedOption);
            panelBody.Visible = false;
            panelHome.Visible = true;
            panelHome.BringToFront();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            loadForm(new formInventoryManagement());
            selectedOption = 2;
            selectedMenu(selectedOption);
        }
        private void panel2_Click(object sender, EventArgs e)
        {
            loadForm(new formInventoryManagement());
            selectedOption = 2;
            selectedMenu(selectedOption);
        }
        private void btnInventory1_Click(object sender, EventArgs e)
        {
            loadForm(new formInventoryManagement());
            selectedOption = 2;
            selectedMenu(selectedOption);
        }





        private void panel1_Click(object sender, EventArgs e)
        {
            loadForm(new formBilling());
            selectedOption = 1;
            selectedMenu(selectedOption);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            loadForm(new formBilling());
            selectedOption = 1;
            selectedMenu(selectedOption);
        }

        private void btnBilling1_Click(object sender, EventArgs e)
        {
            loadForm(new formBilling());
            selectedOption = 1;
            selectedMenu(selectedOption);
        }




        private void panel5_Click(object sender, EventArgs e)
        {
            loadForm(new formEmployeeManagement());
            selectedOption = 5;
            selectedMenu(selectedOption);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            loadForm(new formEmployeeManagement());
            selectedOption = 5;
            selectedMenu(selectedOption);
        }
        private void btnEmpManagement1_Click(object sender, EventArgs e)
        {
            loadForm(new formEmployeeManagement());
            selectedOption = 5;
            selectedMenu(selectedOption);
        }



        private void panel3_Click(object sender, EventArgs e)
        {
            loadForm(new formMenuAndOrders());
            selectedOption = 3;
            selectedMenu(selectedOption);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            loadForm(new formMenuAndOrders());
            selectedOption = 3;
            selectedMenu(selectedOption);
        }

        private void btnMenuOrder1_Click(object sender, EventArgs e)
        {
            loadForm(new formMenuAndOrders());
            selectedOption = 3;
            selectedMenu(selectedOption);
        }


        private void panel6_Click(object sender, EventArgs e)
        {
            loadForm(new formSettings());
            selectedOption = 6;
            selectedMenu(selectedOption);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            loadForm(new formSettings());
            selectedOption = 6;
            selectedMenu(selectedOption);
        }

        private void btnSettings_Click_1(object sender, EventArgs e)
        {
            loadForm(new formSettings());
            selectedOption = 6;
            selectedMenu(selectedOption);
        }


        // --------------------------------------------------------------------- Logout Button ------------------------------------------------------------------------------------------------------
        private void pictureBox3_Click(object sender, EventArgs e)  
        {
            if (isSidebarCollapsed)
            {
                selectedMenu(8);
            }
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form frm = new formLogin();
                frm.Show();
                this.Hide();
            } 
        }

        private void label10_Click(object sender, EventArgs e)       
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form frm = new formLogin();
                frm.Show();
                this.Hide();
            }
        }

        private void panel8_Click(object sender, EventArgs e)  
        {
            if (isSidebarCollapsed)
            {
                selectedMenu(8);
            }
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form frm = new formLogin();
                frm.Show();
                this.Hide();
            }
        }







        // ---------------------------------------------------------------------- Window Buttons (Close, Maximize, Minimize) --------------------------------------------------------------------------

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btn_Maximize_Restore_Click(object sender, EventArgs e)
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
     

        // --------------------------------------------------------------------------- Expand and Collapse Sidebars ---------------------------------------------------------------------------------------------
        private void menu_Click(object sender, EventArgs e)
        {
            timerSidebar.Start();
        }
        private void menu1_Click(object sender, EventArgs e)
        {
            timerSidebar.Start();
        }

        private void timerSidebar_Tick(object sender, EventArgs e)
        {
            if (isSidebarCollapsed)
            {
                panelSideBar.Width+=16;
                panelContentOpener.Left += 8;   
               // panelContentOpener.Location = System.Drawing.Point(panel1.Location.X + 8, panel1.Location.Y);

                if (panelSideBar.Width  >= panelSideBar.MaximumSize.Width)
                {
                    timerSidebar.Stop();
                    isSidebarCollapsed = false; 
                    setUpSidebar();
                }
            }
            else
            {
                panelSideBar.Width-=16;
                panelContentOpener.Left -= 8;
                //panelContentOpener.Location = System.Drawing.Point(panel1.Location.X - 8, panel1.Location.Y);
                if (panelSideBar.Width <= panelSideBar.MinimumSize.Width)
                {
                    isSidebarCollapsed = true;
                    timerSidebar.Stop();
                    setUpSidebar();
                }
            }
        }

        private void setUpSidebar()
        {
            
            if (!isSidebarCollapsed) 
            {
                panelSideBar.Width = panelSideBar.MaximumSize.Width;
                CMSlogo.Visible = true;
                label1.Visible = true;
                menu1.Visible = false;
                menu.Visible = true;
                label1.Visible = true;
                if (this.WindowState == FormWindowState.Maximized)
                {
                    panelHomebtn.Margin = new Padding(4, 65, 2, 1);
                    panel8.Margin = new Padding(4, 235, 2, 1);
                }
                else
                {
                    panelHomebtn.Margin = new Padding(4, 35, 2, 1);
                    panel8.Margin = new Padding(4, 60, 2, 1);
                }
                panel1.BackColor = Color.FromArgb(212, 188, 140);
                panelHomebtn.BackColor = Color.FromArgb(212, 188, 140);
                panelInventory.BackColor = Color.FromArgb(212, 188, 140);
                panel3.BackColor = Color.FromArgb(212, 188, 140);
                panel5.BackColor = Color.FromArgb(212, 188, 140);
                panel6.BackColor = Color.FromArgb(212, 188, 140);
                panel8.BackColor = Color.FromArgb(212, 188, 140);
            }
            else
            {
                panelSideBar.Width = panelSideBar.MinimumSize.Width;
                CMSlogo.Visible = false;
                label1.Visible = false;
                menu1.Visible = true;
                menu1.Enabled = true;
                menu.Visible = false;
                label1.Visible= false;
                if (this.WindowState == FormWindowState.Maximized)
                {
                    panelHomebtn.Margin = new Padding(4, 325, 2, 1);
                    panel8.Margin = new Padding(4, 235, 2, 1);
                }
                else
                {
                    panelHomebtn.Margin = new Padding(4, 240, 2, 1);
                    panel8.Margin = new Padding(4, 115, 2, 1);
                }
                panel1.BackColor = Color.FromArgb(91, 51, 11);
                panelHomebtn.BackColor = Color.FromArgb(91, 51, 11);
                panelInventory.BackColor = Color.FromArgb(91, 51, 11);
                panel3.BackColor = Color.FromArgb(91, 51, 11);
                panel5.BackColor = Color.FromArgb(91, 51, 11);
                panel6.BackColor = Color.FromArgb(91, 51, 11);
                panel8.BackColor = Color.FromArgb(91, 51, 11);

            }

            selectedMenu(selectedOption);
        }





        //----------------------------------------------------------------------------- Dashboard Home by clicking logo  -------------------------------------------------------------------------------------------------
        

        private void formDashboard_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if(!isSidebarCollapsed) 
                {
                    panelHomebtn.Margin = new Padding(4, 65, 2, 1);
                    panel8.Margin = new Padding(4, 235, 2, 1);
                }
                else
                {
                    panelHomebtn.Margin = new Padding(4, 325, 2, 1);
                    panel8.Margin = new Padding(4, 235, 2, 1);


                }
            }
            else
            {
                if(!isSidebarCollapsed)
                {
                    panelHomebtn.Margin = new Padding(4, 35, 2, 1);
                    panel8.Margin = new Padding(4, 60, 2, 1);
                }
                else
                {
                    panelHomebtn.Margin = new Padding(4, 240, 2, 1);
                    panel8.Margin = new Padding(4, 115, 2, 1);
                }   
            }
        }

        
    }
}
