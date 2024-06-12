using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Threading;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CafeManagementSystem
{
    public partial class formAdminFirstLogin : Form
    {


        public static string OTP;
        public static bool valid_email = false;
        public formAdminFirstLogin()
        {
            InitializeComponent();
            panelOTPVerification.Visible = false;
            if (Login_Registration_Module.USER != "CAFEADMIN")
            {
                linkLabel1.Text = "Use security questions instead!";
            }
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtDate.Text == "" || txtAddress.Text == "" || txtPhoneNumber.Text == "" || txtGender.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error");
            }
            else
            {
                string fullName = txtName.Text;
                if (!IsValidFullName(fullName))
                {
                    return;
                }
                string[] nameParts = fullName.Split(' ');
                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1];
                string date = txtDate.Text;
                string gender = txtGender.Text;
                string address = txtAddress.Text;
                string email = txtEmail.Text.ToLower();
                string phone = txtPhoneNumber.Text;
                if (!valid_email)
                {
                    MessageBox.Show("Please validate your email by OTP verification before proceeding");
                }
                else
                {
                    if (!IsValidPhoneNumber(phone))
                    {
                        MessageBox.Show("Please enter a valid phone number.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        try
                        {
                            Reset_Password_Module.AddAdminInformation(firstName, lastName, date, gender, address, email, phone);
                        }
                        catch (SqlException ex)
                        {
                            DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        MessageBox.Show("Sucessfully updated the information.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        Form frm = new formResetAccountInfo();
                        frm.Show();
                    }
                }
            }
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            string fullName = txtName.Text;
            string[] nameParts = fullName.Split(' ');
            string firstName = nameParts[0];
            if (txtName.Text == "" || txtDate.Text == "" || txtAddress.Text == "" || txtPhoneNumber.Text == "" || txtGender.Text == "" || txtEmail.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error");
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
                        formAdminFirstLogin.OTP = Send_Emails_Module.sendOTP(txtEmail.Text.ToLower(), firstName);
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
                    if (formAdminFirstLogin.OTP.Equals(txt6DigitOTP.Text))
                    {
                        valid_email = true;
                        MessageBox.Show("Email verification successfull. The email Field has been locked, however you can continue to edit other information.");
                        panelOTPVerification.Visible = false;
                        txtEmail.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("The OTP does not match. Please check your inbox to find the valid OTP.");
                    }
                }

            }
        }

        private void linkTryAgain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelOTPVerification.Visible = false;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            for (int i = 0; i < phoneNumber.Length; i++)
            {
                if (!char.IsDigit(phoneNumber[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool HasSymbolsOrNumbers(string nameparts)
        {
            return nameparts.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c));
        }

        public static bool IsValidFullName(string name)
        {
            string[] nameParts = name.Split(' ');

            // Check if the full name has at least two parts
            if (nameParts.Length >= 2)
            {
                string firstName = nameParts[0];
                string lastName = nameParts[1];

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("Invalid input. First name or last name cannot be empty.");
                    return false;
                }
                else if (HasSymbolsOrNumbers(firstName) && HasSymbolsOrNumbers(lastName))
                {
                    MessageBox.Show("Invalid input. First name or last name contains symbols or numbers.");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Invalid input.Please enter your full name with a space between first name and last name.");
                return false;
            }


        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form form = new formDashboard();
            form.Show();
            this.Hide();
        }
    }
}



