using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CafeManagementSystem
{
    public partial class formForgotPassword_viaAdmin : Form
    {
        public formForgotPassword_viaAdmin()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtNewPassword.Text == "" || txtNewConfirmPassword.Text == "" || txtAdminPassword.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }
            else
            {

                if (txtNewPassword.Text != txtNewConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match. Try Again!");
                    return;
                }
                else
                {
                    if (!Login_Registration_Module.IsvalidAdminPassword(txtAdminPassword.Text))
                    {
                        MessageBox.Show("Incorrect Admin Password! Try again");
                        return;
                    }
                    else
                    {
                        try
                        {
                            Reset_Password_Module.ResetUserPassword(txtUsername.Text, txtNewPassword.Text);
                            MessageBox.Show("Password changed successfully!");
                            Form frm2 = new formLogin();
                            frm2.Show();
                            this.Hide();
                        }
                        catch (SqlException ex)
                        {
                            DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Form frm3 = new formResetAccountInfo();
                            frm3.Show();
                            this.Hide();
                        }
                    }
                }
            }
        }



        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            Form frm = new formLogin();
            frm.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            txtNewPassword.PasswordChar = checkBox1.Checked ? '\0' : '*';
            txtNewConfirmPassword.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            txtAdminPassword.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void linkConfirmAdminPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new formAdminFirstLogin();
            frm.Show();
            this.Hide();
        }
    }
}
