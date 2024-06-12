using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CafeManagementSystem
{

    //To be Completed: If the user does not have internet give him a choice to register via admin password. For that make panelVerifyviaAdmin, and a link label that appears after failed OTP verfication that opens up the panel  
    public partial class formRegister : Form
    {
        public formRegister()
        {
            InitializeComponent();
            panelOTPVerification.Visible = false;       //This panel should be visible only when user clicks te send OTP button
        }


        public static string OTP; //To store the random 6 digit OTP sent by Send_Email_Module to the user
        public static bool valid_email = false;  //Once the email is verified by OTP this will be set to true to signify that this email is valid
                                                 

        /*------------------------------------- Functions to handle users event -------------------------------------------------*/
        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)  //Back to Login
        {
            Form frm = new formLogin();
            frm.Show();
            this.Hide();
        }

        private void chkBoxShowPassword_CheckedChanged(object sender, EventArgs e)  //Show Password and Confirm Password
        {
            txtRegPassword.PasswordChar = chkBoxShowPassword.Checked ? '\0' : '*';
            txtRegConfirmPassword.PasswordChar = chkBoxShowPassword.Checked ? '\0' : '*';
        }

        /* --------------------------------- This is to register users account information ----------------------------------*/
        // This calls the Login_Registration_Module
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (!(checkValidity()))
            {
                return;
            }
            if (!ValidatePassword(txtRegPassword.Text))
            {
                MessageBox.Show("Password must fulfill the following criteria:\n\n1. Minimum length of 8 characters.\n2. Contain at least one lowercase letter.\n3. Contains at least one uppercase letter.\n4. Contains at least one digit.\n5. Contains at least one special character.", "Error");
                return;
            }
            else if (!valid_email)
            {
                MessageBox.Show("Please verify your email first.", "Error");
            }
            else
            {
                string REGusername = txtRegUsername.Text;
                string REGpassword = txtRegPassword.Text;
                string email = txtEmployeeEmail.Text.ToLower();
                try
                {
                    Login_Registration_Module.RegisterUser(REGusername, REGpassword, email);
                }
                catch (SqlException ex)
                {
                    DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Account Registered Successfully.");
                 Form frm2 = new formLogin();
                 frm2.Show();
                 this.Hide();
            }
        }


        private bool isValidEmail(string email)
        {
            MailMessage mail = new MailMessage();
            if (Send_Emails_Module.verifyEmailFromDatabase(email))
            {
                try
                {
                    mail.To.Add(email);
                }
                catch (Exception)
                {
                    MessageBox.Show("It seems that the email registered in the database is invalid. Please use admin password to register your account.", "Email Verfication Compromised");
                    //Now show the login via admin password linklabel
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid email");
                return false;
            }
            return true;
        }

        private bool checkValidity()
        {
            if(txtRegUsername.Text == "" || txtRegPassword.Text == "" || txtRegConfirmPassword.Text == "" || txtEmployeeEmail.Text == "" || txtFullName.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return false;
            }

            if(txtRegPassword.Text != txtRegConfirmPassword.Text)
            {
                MessageBox.Show("Passwords and Confirm password do not match.", "Error");
                return false;
            }

            if(!(isValidEmail(txtEmployeeEmail.Text.ToLower())))
            {
                return false;
            }

            return true;            
        }

        private async void btnSendOTP_Click_1(object sender, EventArgs e)
        {
            if (!(checkValidity()))
            {
                return;
            }
            else
            {
                string fullName = txtFullName.Text;
                string[] nameParts = fullName.Split(' ');
                string firstName = nameParts[0];
                string email = txtEmployeeEmail.Text.ToLower();
                if (!checkBox1.Checked)
                {
                    MessageBox.Show("Please check the \"Send 6-digit OTP\" box before continuing.");
                }
                else
                {
                    panelOTPVerification.Visible = true;
                    panelBody.Visible = false;
                    panelLoading.Visible = true;
                    await Task.Delay(5000);
                    try
                    {
                        formRegister.OTP = Send_Emails_Module.sendOTP(email, firstName);
                    }
                    catch (SmtpException ex)
                    {
                        panelOTPVerification.Visible = false;
                        DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (formRegister.OTP == "")
                    {
                        panelOTPVerification.Visible = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show("OTP has been sent to the your email. Please check your inbox.");
                        panelBody.Visible = true;
                        panelLoading.Visible = false;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt6DigitOTP.Text == "")
            {
                MessageBox.Show("Please enter the 6-digit OTP.");
            }
            else
            {
                txt6DigitOTP.Text.Trim();
                if (txt6DigitOTP.Text.Length != 6)
                {
                    MessageBox.Show("OTP can only have 6 digits.");
                }
                else
                {
                    if (formRegister.OTP.Equals(txt6DigitOTP.Text))
                    {
                        valid_email = true;
                        MessageBox.Show("Email verification successfull. The email Field has been locked, however you can continue to edit other information.");
                        txtEmployeeEmail.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("The OTP does not match. Please check your inbox to find the valid OTP.");
                    }
                }

            }
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
