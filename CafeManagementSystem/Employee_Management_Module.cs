using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;

namespace CafeManagementSystem
{
    internal class Employee_Management_Module
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");


        /*-------- This is called from the formEmployeeManagement to store employee information in database -------- */
        public static bool AddEmployeeRecords(string FirstName, string lastName, string dob, string gender, string phoneNumber, string email, string hiredate, string roleName, float salary, string address)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddEmployeeDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Firstname", FirstName);
                cmd.Parameters.AddWithValue("@Lastname", lastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", dob);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Hiredate", hiredate);
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                cmd.Parameters.AddWithValue("@Salary", salary);
                cmd.Parameters.AddWithValue("@PhoneNo", phoneNumber);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("There was an error connecting to database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public static void SearchEmployeeRecords(string query, DataTable dt)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
            }
            catch (SqlException)
            {
                conn.Close();
                MessageBox.Show("There was an error connecting to database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void UpdateEmployeeRecords(DataTable dt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True"))
                {
                    conn.Open();
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM tbl_Employee", conn))
                    {
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                        dataAdapter.Update(dt);
                        dt.AcceptChanges();

                        MessageBox.Show("Changes saved to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
