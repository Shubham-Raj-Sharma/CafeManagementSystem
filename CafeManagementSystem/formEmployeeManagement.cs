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
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;
using ToolTip = System.Windows.Forms.ToolTip;

namespace CafeManagementSystem
{
    public partial class formEmployeeManagement : Form
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        private ToolTip Tooltip1;
        DataTable EmployeeRecords;
        public formEmployeeManagement()
        {
            InitializeComponent();
            Tooltip1 = new ToolTip();
            EmployeeRecords = new DataTable();
            /* ------------------------- Initializing the states of visual elements in the form ----------------------*/
            panelSearchEmployee.Visible = false;
            panelAddEmployee.Visible = false;
            selectSearch.Visible = false;
            selectAdd.Visible = false;
           
        }

        private int selectedOption;  //Tracks the selected option in the menu 


        /* -----------------------------------------  For   Exit     Button  -----------------------------------------*/
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        /* ----------------  Functions to update the state of the form when event is generated  ----------------------*/
        private void formEmployeeManagement_Load(object sender, EventArgs e)
        {
            selectedOption = 2;
            panelSearchEmployee.Visible = true;
            selectSearch.Visible = true;

            Tooltip1.SetToolTip(pictureBox1, "Add Employees");
            Tooltip1.SetToolTip(pictureBox2, "Search Employees");

        }

        private void deselectOtherOption()  //Deselect other options once a option has been selected
        {
            if (selectedOption == 1)
            {
                panelSearchEmployee.Visible = false;
                selectSearch.Visible = false;
            }
            else
            {
                panelAddEmployee.Visible = false;
                selectAdd.Visible = false;
            }
        }
        private void ClearAllFields() //After the employee has been successffully registered clear all fields (refresh)
        {
            txtName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtSalary.Clear();
            txtGender.SelectedIndex = 0;
            txtRoleName.SelectedIndex = 0;
            txtPhoneNumber.Clear();
            DateTime txtDateOfBirth = DateTime.Now.Date;
            DateTime txthireDate = DateTime.Now.Date;
        }

        /* ---------------------------  Buttons that generate events when pressed   ----------------------------------*/
        private void pictureBox1_Click(object sender, EventArgs e) // Add Employee details (option 1 in menu)
        {
            selectedOption = 1;
            panelAddEmployee.Visible = true;
            selectAdd.Visible = true;
            deselectOtherOption();
        }

        private void pictureBox2_Click(object sender, EventArgs e) // Add Employee details (option 2 in menu)
        {
            selectedOption = 2;
            panelSearchEmployee.Visible = true;
            selectSearch.Visible = true;
            deselectOtherOption();
        }

        private void btnSubmit_Click(object sender, EventArgs e) // Submit Button in Add Employee panel
        {
            if (!checkValuesValidity()) //checkValuesValidity returns true if data is valid
            {
                return;
            }
            else
            {
                string fullName = txtName.Text;
                string[] nameParts = fullName.Split(' ');
                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1];
                string dob = txtDateOfBirth.Text;
                string hireDate = txtHireDate.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string gender = txtGender.Text;
                string address = txtAddress.Text;
                string email = txtEmail.Text.ToLower();
                float salary = float.Parse(txtSalary.Text);
                string roleName = txtRoleName.Text;
                if (Employee_Management_Module.AddEmployeeRecords(firstName, lastName, dob, gender, phoneNumber, email, hireDate, roleName, salary, address))
                {
                    MessageBox.Show("Employee has been successfully registered to the database.", "Registration Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAllFields();
                }
            }
        }




        /* ----------------  Functions that check the validity of the text entered by users  ----------------------*/
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

        public static bool isValidEmail(string email)
        {
            MailMessage mail = new MailMessage();
            try
            {
                mail.To.Add(email);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid email", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool isValidSalary(string salarystr)
        {
            float salary;
            bool isNumber = float.TryParse(salarystr, out salary);
            if (!isNumber)
            {
                MessageBox.Show("Please enter a valid salary", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public static bool HasSymbolsOrNumbers(string nameparts)
        {
            return nameparts.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c));
        }

        public static bool isValidFullName(string name)
        {
            string[] nameParts = name.Split(' ');

            // Check if the full name has at least two parts
            if (nameParts.Length >= 2)
            {
                string firstName = nameParts[0];
                string lastName = nameParts[1];

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    MessageBox.Show("Invalid input. First name or last name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else if (HasSymbolsOrNumbers(firstName) && HasSymbolsOrNumbers(lastName))
                {
                    MessageBox.Show("Invalid input. First name or last name contains symbols or numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Invalid input.Please enter your full name with a space between first name and last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
        private bool checkValuesValidity()
        {
            if (txtName.Text == "" || txtDateOfBirth.Text == "" || txtGender.Text == "" || txtPhoneNumber.Text == "" || txtEmail.Text == "" || txtHireDate.Text == "" || txtSalary.Text == "" || txtRoleName.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Please fill out all information!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!(IsValidPhoneNumber(txtPhoneNumber.Text)))
            {
                MessageBox.Show("Please enter a valid phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!(isValidEmail(txtEmail.Text)))
            {
                return false;
            }

            if (!(isValidSalary(txtSalary.Text)))
            {
                return false;
            }

            if (!(isValidFullName(txtName.Text)))
            {
                return false;
            }

            return true;
        }

        //===============================================================================================================================================================================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            EmployeeRecords.Clear();
            string searchTerm, query = "";
            searchTerm = searchText.Text;
            switch (Categorytxt.Text)
            {
                case "EmployeeID":

                    if (!(searchTerm.All(char.IsDigit)))
                    {
                        MessageBox.Show("Employee ID must only contain digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    query = "SELECT * FROM tbl_Employee WHERE EmployeeID = " + searchTerm;
                    break;
                case "Name":
                    query = "SELECT * FROM tbl_Employee WHERE FirstName LIKE '%" + searchTerm + "%' OR LastName LIKE '%" + searchTerm + "%';";
                    break;
                case "Hire date":
                    if (int.TryParse(searchTerm, out int hireYear))
                    {
                        query = "SELECT * FROM tbl_Employee WHERE YEAR(HireDate) = " + hireYear;
                    }
                    else
                    {
                        MessageBox.Show("Invalid hire year format. Please enter a valid year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case "Address":
                    query = "SELECT * FROM tbl_Employee WHERE Address LIKE '%" + searchTerm + "%';";
                    break;
                case "Phone Number":
                    if (!(searchTerm.All(char.IsDigit)))
                    {
                        MessageBox.Show("Phone number must only contain digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    query = "SELECT * FROM tbl_Employee WHERE Phone LIKE '%" + searchTerm + "%';";
                    break;
                case "Role":
                    string role;
                    if (string.Equals(searchTerm, "Admin", StringComparison.OrdinalIgnoreCase))
                        role = "1";
                    else if (string.Equals(searchTerm, "Cashier", StringComparison.OrdinalIgnoreCase))
                        role = "2";
                    else if (string.Equals(searchTerm, "Manager", StringComparison.OrdinalIgnoreCase))
                        role = "3";
                    else if (string.Equals(searchTerm, "Waitstaff", StringComparison.OrdinalIgnoreCase))
                        role = "4";
                    else
                        role = "-1";
                    query = "SELECT * FROM tbl_Employee WHERE role = " + role;
                    break;
                case "Email":
                    query = "SELECT * FROM tbl_Employee WHERE Email LIKE '%" + searchTerm + "%';";
                    break;
                case "Salary":
                    if (!Double.TryParse(searchTerm, out double sal))
                    {
                        MessageBox.Show("Salary must only contain digits or decimal point.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    query = "SELECT * FROM tbl_Employee WHERE Salary = " + searchTerm;
                    break;
                case "Date of birth":
                    if (int.TryParse(searchTerm, out int birthYear))
                    {
                        query = "SELECT * FROM tbl_Employee WHERE YEAR(DateOfBirth) = " + birthYear;
                    }
                    else
                    {
                        MessageBox.Show("Invalid birth year format. Please enter a valid year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                default:
                    MessageBox.Show("Please enter a valid catgory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            if(query == "")
            {
                return;
            }
            Employee_Management_Module.SearchEmployeeRecords(query, EmployeeRecords);
            dataGridView1.DataSource = EmployeeRecords;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Employee_Management_Module.UpdateEmployeeRecords(EmployeeRecords);
        }
    }
}

