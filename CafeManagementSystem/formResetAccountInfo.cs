using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    public partial class formResetAccountInfo : Form
    {   
        public formResetAccountInfo()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!validateUserInput(txtUsername.Text,txtPassword.Text,txtConfirmPassword.Text))
            {
                return;
            }
            else
            {
                
                try
                {
                    Reset_Password_Module.ResetAdminPassword(txtUsername.Text, txtPassword.Text);
                }
                catch (SqlException ex)
                {
                    DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Account Credentials Successfully updated!");
                DialogResult result1 = MessageBox.Show("Some information in login credentials have changed. Session expired.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Form form1 = new formLogin();
                form1.Show();
                this.Hide();
            }
        }

        private void chkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkBoxShowPassword.Checked ? '\0' : '*';
            txtConfirmPassword.PasswordChar = chkBoxShowPassword.Checked ? '\0' : '*';
        }

        public static bool validateUserInput(string username, string password, string confirmPassword)
        {
            if(username == "")
            {
                MessageBox.Show("Username cannot be left empty", "Error");
                return false;
            }

            if (password == "")
            {
                MessageBox.Show("Password cannot be left empty", "Error");
                return false;
            }

            if (confirmPassword == "")
            {
                MessageBox.Show("Confirm password cannot be left empty", "Error");
                return false;
            }
             
            if (!ValidatePassword(password))
            {
                MessageBox.Show("Password must fulfill the following criteria:\n\n1. Minimum length of 8 characters.\n2. Contain at least one lowercase letter.\n3. Contains at least one uppercase letter.\n4. Contains at least one digit.\n5. Contains at least one special character.", "Error");
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password and Confirm Password do not match.", "Error");
                return false;
            }

            
            return true;
        }
        public static bool ValidatePassword(string password)
        {
            // Password validation criteria
            bool isPasswordValid =
                password.Length >= 8 && // Minimum length of 8 characters
                Regex.IsMatch(password, @"[a-z]") && // Contains at least one lowercase letter
                Regex.IsMatch(password, @"[A-Z]") && // Contains at least one uppercase letter
                Regex.IsMatch(password, @"\d") && // Contains at least one digit
                Regex.IsMatch(password, @"[!@#$%^&*()]"); // Contains at least one special character

            return isPasswordValid;
        }
    }
}
