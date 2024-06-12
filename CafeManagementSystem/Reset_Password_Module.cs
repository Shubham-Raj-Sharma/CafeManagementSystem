using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;

namespace CafeManagementSystem
{

    internal class Reset_Password_Module
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        public static void ResetUserPassword(string username, string password)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("ResetUserPassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@NewPassword", password);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(SqlException ex) 
            { 
                conn.Close();
                throw ex;
            }
        }

        public static void ResetAdminPassword(string username, string password)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("ResetAdminPassword", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewUsername", username);
                cmd.Parameters.AddWithValue("@OldUsername", Login_Registration_Module.USER);
                cmd.Parameters.AddWithValue("@AdminPassword", password);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex) 
            {
                conn.Close();
                throw ex;
            }
        }

        public static void AddAdminInformation(string firstName, string lastName, string date, string gender, string address, string email, string phone)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddAdminInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Firstname", firstName);
                cmd.Parameters.AddWithValue("@Lastname", lastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", date);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@PhoneNo", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(SqlException ex) 
            { 
                conn.Close();
                throw ex;
            }
        }

        public static void UpdateAdminInformation(string firstName, string lastName, string date, string gender, string address, string email, string phone, int AdminID)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateAdminInformation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Firstname", firstName);
                cmd.Parameters.AddWithValue("@Lastname", lastName);
                cmd.Parameters.AddWithValue("@DateOfBirth", date);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@PhoneNo", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ID", AdminID);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                throw ex;
            }
        }

        public static void SetUpRecoveryQuestions(string question1, string question2, string question3, string answer1, string answer2, string answer3)
        { 
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SetUpRecoveryQuestionAnswers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Question1", question1);
                cmd.Parameters.AddWithValue("@Question2", question2);
                cmd.Parameters.AddWithValue("@Question3", question3);
                cmd.Parameters.AddWithValue("@Answer1", answer1);
                cmd.Parameters.AddWithValue("@Answer2", answer2);
                cmd.Parameters.AddWithValue("@Answer3", answer3);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                DialogResult result = MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
        }
    }
}
    
