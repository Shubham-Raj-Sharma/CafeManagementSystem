using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CafeManagementSystem
{
    public partial class formForgotPassword_OTP : Form
    {

        static bool valid_email = false;
        public formForgotPassword_OTP()
        {
            InitializeComponent();
            panelOTPVerification.Visible = false;
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            Form frm = new formLogin();
            frm.Show();
            this.Hide();
        }

        private async void btnProceed_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string email = txtEmail.Text;
            if (txtUsername.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error");
                return;
            }
            if (!isValidEmail(email))
            {
                return;
            }
            else
            {
                if (checkBox1.Checked)
                {
                    panelOTPVerification.Visible = true;
                    panelBody.Visible = false;
                    panelLoading.Visible = true;
                    await Task.Delay(5000);

                    try
                    {
                        
                        formAdminFirstLogin.OTP = Send_Emails_Module.sendOTP(email.ToLower(), username);
                    }
                    catch (SmtpException ex)
                    {
                        panelOTPVerification.Visible = false;
                        DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (formAdminFirstLogin.OTP == "")
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
                else
                {
                    MessageBox.Show("Please check the \"Send 6-digit OTP\" box before continuing.");
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (txt6DigitOTP.Text == "")
            {
                MessageBox.Show("Please enter the 6-digit OTP.", "Error");
            }
            else
            {
                txt6DigitOTP.Text.Trim();
                if (txt6DigitOTP.Text.Length != 6)
                {
                    MessageBox.Show("OTP can only have 6 digits.", "Error");
                }
                else
                {
                    if (formAdminFirstLogin.OTP.Equals(txt6DigitOTP.Text))
                    {
                        valid_email = true;
                        MessageBox.Show("Email verification successfull. Redirecting to Change Password.", "Error");
                        panelOTPVerification.Visible = false;
                        
                    }
                    else
                    {
                        MessageBox.Show("The OTP does not match. Please check your inbox to find the valid OTP.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    }
                }

            }
        }

        private bool isValidEmail(string email)
        {
            MailMessage mail = new MailMessage();
            if (Send_Emails_Module.verifyEmailFromDatabase(email) || Login_Registration_Module.USER.Equals("CAFEADMIN"))
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

        private void linkConfirmAdminPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new formForgotPassword_viaAdmin();
            frm.Show(); 
            this.Hide();
        }
    }
}
