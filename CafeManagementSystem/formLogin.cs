using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    public partial class formLogin : Form
    {
        private bool isButtonOn = false;
        public formLogin()
        {
            InitializeComponent();
        }

        private void closeButton_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!(Login_Registration_Module.isValidUser(txtUsername.Text, txtPassword.Text)))
            {
                DialogResult result = MessageBox.Show("Invalid Username or Password. Try Again!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linkForgotPassword.Text = "Forgot Password?";
                return;
            }
            else
            {
                
                    
                        Form frm2 = new formDashboard();
                        frm2.Show();
                        this.Hide();
                    
                    
            }
        }

        private void showPassword_Click(object sender, EventArgs e)
        {
            if (isButtonOn)
            {
                showPassword.Image = Properties.Resources.showPassword; // Set the image for the "off" state
                isButtonOn = false;
                txtPassword.PasswordChar = '*';
            }
            else
            {
                showPassword.Image = Properties.Resources.hidePassword; // Set the image for the "on" state
                isButtonOn = true;
                txtPassword.PasswordChar = '\0';
            }
        }
        private void linkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm3 = new formForgotPassword_OTP();
            frm3.Show(); 
            this.Hide();
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm3 = new formRegister();
            frm3.Show();
            this.Hide();
        }

    }
}
