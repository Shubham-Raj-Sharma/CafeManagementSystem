using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.IO;
using System.Data.Common;
using System.Collections.ObjectModel;
using CafeManagementSystem.Properties;

namespace CafeManagementSystem
{
    public partial class formSettings : Form
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        public string fileName;
        public static string OTP; //To store the random 6 digit OTP sent by Send_Email_Module to the user
        public static bool valid_email = false;  //Once the email is verified by OTP this will be set to true to signify that this email is valid

        DataTable PersonalInformation;
        DataTable AccountInformation;
        DataTable CafeInformation;
        DataTable TableInformation;
        private int selectedOption;  //Tracks the selected option in the menu

        string FirstName;
        string LastName;
        DateTime dob;
        string gender;
        string address;
        string contact;
        string email;

        string role;
        string username;
        string password;
        
        string CafeName;
        string CafeAddress;
        string CafeContact;
        string PAN_Number;
        string VAT;
        string ServiceTax;

        public formSettings()
        {
            InitializeComponent();
            CafeInformation = new DataTable();
            AccountInformation = new DataTable();   
            PersonalInformation = new DataTable();
            TableInformation = new DataTable();

            panelTables.Visible = false;
            panelOTPVerification.Visible = false;
            panelAccountDetails.Visible = false;
            lblConfirmPassword.Visible = false;
            ConfirmPasswordtxt.Visible = false;
            lblverifyEmail.Visible = false;

            btnSaveChanges.Visible = false;
            btnSaveChanges1.Visible = false;

            btnReset.Visible = false;
            btnReset1.Visible = false;

            role = Login_Registration_Module.ROLE;
            username = Login_Registration_Module.USER;
            password = null;
            email = null;

            lblUsername.Text = username;
            lblRole.Text = role;
            
        }

        private void formSettings_Load(object sender, EventArgs e)
        {
            selectedOption = 1;
            deselectOtherOption();
            panelAccountDetails.Visible = true;

            if(Settings_Module.LoadCafeInformation(CafeInformation))
            {
                DisplayCafeInfoText();
            }
            if(Settings_Module.LoadAccountInformation(AccountInformation, username))
            {
                DisplayAccountInfoText();
            }
            if(Settings_Module.LoadPersonalInformation(PersonalInformation, username))
            {
                DisplayUserInfoText();
            }

            
            try
            {
                byte[] profilePictureData = (byte[])AccountInformation.Rows[0]["AccountPicture"];
                pictureBox1.Image = ConvertBinaryToImage(profilePictureData);
            }
            catch (Exception)
            {
                pictureBox1.Image = Properties.Resources.UsernameIcon;
            }
        }


        private void deselectOtherOption()  //Deselect other options once a option has been selected
        {
            if (selectedOption == 1)
            {
                panel11.BackColor = Color.FromArgb(212, 188, 140);
                panel2.BackColor = Color.FromArgb(250, 242, 202);
                CafeInformationPanel.Visible = false;
            }
            else
            {
                panel2.BackColor = Color.FromArgb(212, 188, 140);
                panel11.BackColor = Color.FromArgb(250, 242, 202);
                panelAccountDetails.Visible = false;
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelAccountDetails.Visible = true;
            deselectOtherOption();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            selectedOption = 1;
            panelAccountDetails.Visible = true;
            deselectOtherOption();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            selectedOption = 2;
            CafeInformationPanel.Visible = true;
            deselectOtherOption();
        }

        private void panel11_Click(object sender, EventArgs e)
        {
            selectedOption = 2;
            CafeInformationPanel.Visible = true;
            deselectOtherOption();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void DisplayUserInfoText()
        {
            if(PersonalInformation.Rows.Count == 0)
            {
                MessageBox.Show("Failed to Load User Information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                firstNametxt.Text = PersonalInformation.Rows[0]["FirstName"].ToString();
            }
            catch(Exception)
            {
                firstNametxt.Text = " ";
            }
            FirstName = firstNametxt.Text;
           
            try
            {
                lastNametxt.Text = PersonalInformation.Rows[0]["LastName"].ToString();
            }
            catch (Exception)
            {
                lastNametxt.Text = " ";
            }
            LastName = lastNametxt.Text;

            try
            {
                gendertxt.Text = PersonalInformation.Rows[0]["Gender"].ToString(); 
            }
            catch (Exception)
            {
                gendertxt.Text = " ";
            }
            gender = gendertxt.Text;

            try
            {
                Addresstxt.Text = PersonalInformation.Rows[0]["AdminAddress"].ToString();
            }
            catch (Exception)
            {
                Addresstxt.Text = " ";
            }
            address = Addresstxt.Text;

            try
            {
                emailtxt.Text = PersonalInformation.Rows[0]["Email"].ToString();
            }
            catch (Exception)
            {
                emailtxt.Text = " ";
            }
            email = emailtxt.Text;

            try
            {
                txtContactNumber.Text = PersonalInformation.Rows[0]["Phone"].ToString();
            }
            catch (Exception)
            {
                txtContactNumber.Text = " ";
            }
            contact = txtContactNumber.Text;

            if (DateTime.TryParse(PersonalInformation.Rows[0]["DateOfBirth"].ToString(), out dob))
            {
                dobDateTimePicker.Value = dob;
            }
            else
            {
                dobDateTimePicker.Value = DateTime.Today;
            }
            dob = dobDateTimePicker.Value.Date;
        }

        private void firstNametxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }

        private void lastNametxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }

        private void dobDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }

        private void Addresstxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }

        private void gendertxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }

        private void txtContactNumber_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
        }
        
        private void emailtxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInPersonalInformation();
            lblverifyEmail.Visible = true;
            if (emailtxt.Text == email || email == null)
            {
                lblverifyEmail.Visible = false;
            }

        }

        public void checkForChangesInPersonalInformation()
        {
            if (firstNametxt.Text == FirstName && lastNametxt.Text == LastName && dobDateTimePicker.Value.Date == dob && Addresstxt.Text == address && gendertxt.Text == gender && emailtxt.Text == email && txtContactNumber.Text == contact)
            {
                btnSaveChanges.Visible = false;
                btnReset.Visible = false;
            }
            else
            {
                btnSaveChanges.Visible = true;
                btnReset.Visible = true;
            }
        }

        private async void verifyEmailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(!isValidEmail(emailtxt.Text))
            {
                return;
            }
            string firstName1 = firstNametxt.Text;
            string email1 = emailtxt.Text.ToLower();

            panelOTPVerification.Visible = true;
            panelBody.Visible = false;
            panelLoading.Visible = true;
            await Task.Delay(5000);
            try
            {
                formRegister.OTP = Send_Emails_Module.sendOTP(email1, firstName1);
            }
            catch (SmtpException ex)
            {
                panelOTPVerification.Visible = false;
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void linkTryAgain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            verifyEmailLinkLabel_LinkClicked(this, null);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            firstNametxt.Text = FirstName;
            lastNametxt.Text = LastName;
            dobDateTimePicker.Value = dob;
            Addresstxt.Text = address;
            gendertxt.Text = gender;
            emailtxt.Text = email;
            txtContactNumber.Text = contact;
        }


        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Settings_Module.LoadPersonalInformation(PersonalInformation, Login_Registration_Module.USER);
            if (!(verifyUserDetailsInput()))
            {
                return;
            }
            else
            {
                int AdminID;
                try
                {
                    AdminID = Convert.ToInt32(PersonalInformation.Rows[0]["AdminID"].ToString());
                }
                catch
                {
                    MessageBox.Show("Error loading data. Please try again later.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Reset_Password_Module.UpdateAdminInformation(firstNametxt.Text, lastNametxt.Text, dobDateTimePicker.Value.Date.ToString(), gendertxt.Text, Addresstxt.Text, emailtxt.Text, txtContactNumber.Text, AdminID);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message+ " Error while updating the information. Please try again later.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Information Successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                emailtxt.Enabled = true;
            }

            btnSaveChanges.Visible = false;
            btnReset.Visible = false;
            lblverifyEmail.Visible = false;
            
            FirstName = firstNametxt.Text;
            LastName = lastNametxt.Text;
            gender = gendertxt.Text;
            address = Addresstxt.Text;
            email = emailtxt.Text;
            contact = txtContactNumber.Text;
            dob = dobDateTimePicker.Value.Date;
        }

        public bool verifyUserDetailsInput()
        {
            if (firstNametxt.Text == "" || lastNametxt.Text == "" || gendertxt.Text == "" || Addresstxt.Text == "" || emailtxt.Text == "" || txtContactNumber.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(txtContactNumber.Text.All(char.IsDigit)))
            {
                MessageBox.Show("Phone number must only contain digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtContactNumber.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!isValidEmail(emailtxt.Text))
            {
                return false;
            }
            if(lblverifyEmail.Visible == true)
            {
                MessageBox.Show("Please verify the email before you proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool isValidEmail(string email)
        {
            MailMessage mail = new MailMessage();
            try
            {
                mail.To.Add(email);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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
                        emailtxt.Enabled = false;
                        lblverifyEmail.Visible = false;
                        panelOTPVerification.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("The OTP does not match. Please check your inbox to find the valid OTP.");
                    }
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelOTPVerification.Visible = false;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void DisplayAccountInfoText()
        {
            if (AccountInformation.Rows.Count == 0)
            {
                MessageBox.Show("Failed to Load Account Information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Usernametxt.Text = AccountInformation.Rows[0]["Username"].ToString();
            }
            catch
            {
                Usernametxt.Text = " ";
            }
            username = Usernametxt.Text;

            try
            {
                Passwordtxt.Text = AccountInformation.Rows[0]["UserPassword"].ToString();
            }
            catch (Exception)
            {
                Passwordtxt.Text = " ";
            }
            password = Passwordtxt.Text;
        }

        private void Usernametxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInAccountInformation();
        }

        private void Passwordtxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInAccountInformation();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                Passwordtxt.PasswordChar = '*';
                ConfirmPasswordtxt.PasswordChar = '*';
            }
            else
            {
                Passwordtxt.PasswordChar = '\0';
                ConfirmPasswordtxt.PasswordChar = '\0';
            }
        }
        public void checkForChangesInAccountInformation()
        { 
            if (Usernametxt.Text == username && (Passwordtxt.Text == password || password == null))
            {
                lblConfirmPassword.Visible = false;
                ConfirmPasswordtxt.Visible = false;
                btnSaveChanges1.Visible = false;
                btnReset1.Visible = false;
            }
            else
            {
                lblConfirmPassword.Visible = true;
                ConfirmPasswordtxt.Clear();
                ConfirmPasswordtxt.Visible = true;
                btnSaveChanges1.Visible = true;
                btnReset1.Visible = true;
            }
        }

        private void btnReset1_Click(object sender, EventArgs e)
        {
            Usernametxt.Text = username;
            Passwordtxt.Text = password;
        }

        
        private void btnSaveChanges1_Click(object sender, EventArgs e)
        {
            Settings_Module.LoadAccountInformation(AccountInformation, username);
            if (!(verifyAccountDetailsInput()))
            {
                return;
            }
            else
            {
                try
                {
                    Reset_Password_Module.ResetAdminPassword(Usernametxt.Text, Passwordtxt.Text);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Account Credentials Successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            btnSaveChanges1.Visible = false;
            btnReset1.Visible = false;
            lblConfirmPassword.Visible = false;
            ConfirmPasswordtxt.Visible = false;
            username = Usernametxt.Text;
            password = Passwordtxt.Text;
            lblUsername.Text = username;
        }

        public bool verifyAccountDetailsInput()
        {
            if (Usernametxt.Text == "" || Passwordtxt.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                return false;
            }
            if (Passwordtxt.Text != ConfirmPasswordtxt.Text)
            {
                MessageBox.Show("Password and Confirm Password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!ValidatePassword(Passwordtxt.Text))
            {
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
            if (!isPasswordValid)
                MessageBox.Show("Password must fulfill the following criteria:\n\n1. Minimum length of 8 characters.\n2. Contain at least one lowercase letter.\n3. Contains at least one uppercase letter.\n4. Contains at least one digit.\n5. Contains at least one special character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return isPasswordValid;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public void DisplayCafeInfoText()
        {
            if (CafeInformation.Rows.Count == 0)
            {
                MessageBox.Show("Failed to Load Cafe Information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                CafeNametxt.Text = CafeInformation.Rows[0]["Cafename"].ToString();
            }
            catch (Exception)
            {
                CafeNametxt.Text = " ";
            }
            CafeName = CafeNametxt.Text;

            try
            {
                CafeAddresstxt.Text = CafeInformation.Rows[0]["CafeAddress"].ToString();
            }
            catch (Exception)
            {
                CafeAddresstxt.Text = " ";
            }
            CafeAddress = CafeAddresstxt.Text;

            try
            {
                CafeContacttxt.Text = CafeInformation.Rows[0]["CafeContact"].ToString();
            }
            catch (Exception)
            {
                CafeContacttxt.Text = " ";
            }
            CafeContact = CafeContacttxt.Text;

            try
            {
                CafePANtxt.Text = CafeInformation.Rows[0]["CafePAN"].ToString();
            }
            catch (Exception)
            {
                CafePANtxt.Text = " ";
            }
            PAN_Number = CafePANtxt.Text;

            try
            {
                VATtxt.Text = CafeInformation.Rows[0]["VAT_Rate"].ToString();
            }
            catch (Exception)
            {
                VATtxt.Text = " ";
            }
            VAT = VATtxt.Text;

            try
            {
                CafeServiceTaxtxt.Text = CafeInformation.Rows[0]["ServiceTax"].ToString();
            }
            catch (Exception)
            {
                CafeServiceTaxtxt.Text = " ";
            }
            ServiceTax = CafeServiceTaxtxt.Text;

            try
            {
                byte[] logoData = (byte[])CafeInformation.Rows[0]["CafeLogo"];
                CafeLogo.Image = ConvertBinaryToImage(logoData);
            }
            catch (Exception)
            {
                CafeLogo.Image = Properties.Resources.logo2;
            }

            btnSaveChanges2.Visible = false;
            btnReset2.Visible = false;
        }

        private void CafeNametxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        private void CafeContacttxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        private void CafeAddresstxt_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        private void txtCafePAN_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        private void txtVAT_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        private void txtServiceTax_TextChanged(object sender, EventArgs e)
        {
            checkForChangesInCafeInformation();
        }

        public void checkForChangesInCafeInformation()
        {
            if(CafeNametxt.Text == CafeName && CafeAddresstxt.Text == CafeAddress && CafeContacttxt.Text == CafeContact && CafePANtxt.Text == PAN_Number && VATtxt.Text == VAT && CafeServiceTaxtxt.Text == ServiceTax)
            {
                btnSaveChanges2.Visible = false;
                btnReset2.Visible = false;
            }
            else
            {
                btnSaveChanges2.Visible = true;
                btnReset2.Visible = true;
            }
        }

        private void btnReset2_Click(object sender, EventArgs e)
        {
            CafeNametxt.Text = CafeName;
            CafeAddresstxt.Text = CafeAddress;
            CafeContacttxt.Text = CafeContact;
            CafePANtxt.Text = PAN_Number;
            VATtxt.Text = VAT;
            CafeServiceTaxtxt.Text = ServiceTax;
        }

        public bool checkValidityOfCafeInfo()
        {
            if (CafeNametxt.Text == "" || CafeAddresstxt.Text == "" || CafeContacttxt.Text == ""|| CafePANtxt.Text == "" || VATtxt.Text == "" || CafeServiceTaxtxt.Text == "")
            {
                MessageBox.Show("Please enter values in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!CafeContacttxt.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(CafeContacttxt.Text.Length != 10)
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (CafePANtxt.Text.Length != 10)
            {
                MessageBox.Show("PAN number must be only 10 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!(CafePANtxt.Text.All(char.IsDigit)))
            {
                MessageBox.Show("PAN number must only contain digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!float.TryParse( VATtxt.Text , out float Value1))
            {
                MessageBox.Show("VAT percent must only contain digits or a decimal point.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (!float.TryParse(CafeServiceTaxtxt.Text, out float Value2))
            {
                MessageBox.Show("Service tax must only contain digits or a decimal point.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void btnSaveChanges2_Click(object sender, EventArgs e)
        {
            if (!checkValidityOfCafeInfo())
            {  
                return;
            }
            else
            { 
                try
                {
                    double valueVAT = Convert.ToDouble(VATtxt.Text);
                    double valueServiceTax = Convert.ToDouble(CafeServiceTaxtxt.Text);
                    byte[] LogoByteValue = ConvertImageToBinary(CafeLogo.Image);
                    if (Settings_Module.updateCafeInfo(CafeNametxt.Text, CafeAddresstxt.Text, CafeContacttxt.Text, LogoByteValue, CafePANtxt.Text, valueVAT, valueServiceTax))
                    {
                        MessageBox.Show("Changes have been successfully saved to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnSaveChanges2.Visible = false;
                        btnReset2.Visible = false;
                    }
                    else
                    {
                        btnSaveChanges2.Visible = true;
                        btnReset2.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CafeNametxt.Text = CafeName;
                    CafeAddresstxt.Text = CafeAddress;
                    CafeContacttxt.Text = CafeContact;
                    CafePANtxt.Text = PAN_Number;
                    VATtxt.Text = VAT;
                    CafeServiceTaxtxt.Text = ServiceTax;
                }
            }
        }
        

        //--------------------------------------------------- To import the image from a file --------------------------------------------------------------

        private void btnAddLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Files (*.png)|*.png", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    CafeLogo.Image = Image.FromFile(fileName);
                    btnSaveChanges2.Visible = true;
                    btnReset2.Visible = true;
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

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnAddTables_Click(object sender, EventArgs e)
        {
            TableInformation.Clear();
            if (Settings_Module.LoadCafeTablesInformation(TableInformation))
            {
                panelTables.BringToFront();
                panelTables.Visible = true;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = TableInformation;
            }
            else
            {
                MessageBox.Show("Error Loading the table information from database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panelTables.Visible = false;
            panelTables.SendToBack();
        } 

        private void button3_Click(object sender, EventArgs e)
        {
            int tableNumber;
            int numberOfSeats;
            try
            {
                tableNumber = int.Parse(txtTableNumber.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter valid integer values for Table Number.");
                return;
            }
            
            if (button3.Text == "Add")
            {
                try
                {
                    numberOfSeats = int.Parse(txtNumberOfSeats.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Invalid input. Please enter valid integer values for Number of Seats.");
                    return;
                }
                string insertQuery = "INSERT INTO CafeTables (TableNumber, NumberOfSeats, TableStatus) VALUES (@TableNumber, @NumberOfSeats, @TableState)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, conn);
                insertCommand.Parameters.AddWithValue("@TableNumber", tableNumber);
                insertCommand.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                insertCommand.Parameters.AddWithValue("@TableState", 0);

                try
                {
                    conn.Open();
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("Table added successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding table: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                    TableInformation.Clear();   
                    Settings_Module.LoadCafeTablesInformation(TableInformation);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = TableInformation;
                }
            }
            else
            {
                string deleteQuery = "DELETE FROM CafeTables WHERE TableNumber = @TableNumber";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, conn);
                deleteCommand.Parameters.AddWithValue("@TableNumber", tableNumber);
                try
                {
                    conn.Open();
                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Table deleted successfully!");
                    else
                        MessageBox.Show("Table not found. Deletion failed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting table: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                    TableInformation.Clear();
                    Settings_Module.LoadCafeTablesInformation(TableInformation);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = TableInformation;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int tableNumber = int.Parse(txtTableNumber.Text);
            int numberOfSeats = int.Parse(txtNumberOfSeats.Text);

            string updateQuery = "UPDATE CafeTables SET NumberOfSeats = @NumberOfSeats WHERE TableNumber = @TableNumber";
            SqlCommand updateCommand = new SqlCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
            updateCommand.Parameters.AddWithValue("@TableNumber", tableNumber);

            try
            {
                conn.Open();
                int rowsAffected = updateCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                    MessageBox.Show("Table updated successfully!");
                else
                    MessageBox.Show("Table not found. Update failed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating table: " + ex.Message);
            }
            finally
            {
                conn.Close();
                TableInformation.Clear();
                Settings_Module.LoadCafeTablesInformation(TableInformation);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = TableInformation;
            }
        }

        private void txtTableNumber_TextChanged(object sender, EventArgs e)
        {
            if(txtTableNumber.Text == "")
            {
                return;
            }
            DataRow[] matchingRows = TableInformation.Select($"TableNumber = '{Convert.ToInt32(txtTableNumber.Text)}'");
            if (matchingRows.Length > 0)
            {
                button3.Text = "Delete";
            }
            else
            {
                button3.Text = "Add";
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void changePictureLinkLabel_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Files (*.png)|*.png", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(fileName);
                }
            }

            byte[] AccountPictureValue = ConvertImageToBinary(pictureBox1.Image);
            try
            { 
                conn.Open();
                string query = "Update tbl_UserLogin Set AccountPicture = @AccPictureValue Where Username = @user;";
                SqlCommand updateCommand = new SqlCommand(query, conn);
                updateCommand.Parameters.AddWithValue("@AccPictureValue", AccountPictureValue);
                updateCommand.Parameters.AddWithValue("@user", username);
                updateCommand.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Profile picture was saved succesfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fileName = null;
                pictureBox1.Image = Properties.Resources.UsernameIcon;
            }
            

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}



    

