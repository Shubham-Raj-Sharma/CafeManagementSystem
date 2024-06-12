using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CafeManagementSystem
{
    class Login_Registration_Module
    {

        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        /* ---------------- To Capture the username and the role of the user during login -------------------------- */
        //Required to dispaly the user profile and views he/she can access.
        public static string user;
        public static string role;
        public static string USER //Find the usernale of logged in user
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public static string ROLE  //Find the role of the logged in user
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }


        /* ---------------- This function is called by formLogin to validate entered credentials ------------------- */
        public static bool isValidUser(string username, string password)
        {
            int Roleid;
            bool isValid = false;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"Select * from tbl_UserLogin where Username COLLATE SQL_Latin1_General_CP1_CS_AS = '" + username + "' and UserPassword COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    isValid = true;    // This means the user has logged in successfully

                    USER = dt.Rows[0]["Username"].ToString(); //So we capture the username and role of the user logged in
                    Roleid = Convert.ToInt32(dt.Rows[0]["roleid"]);
                    try
                    {
                        conn.Open();
                        SqlCommand cmd2 = new SqlCommand(@"Select * from tbl_Roles where roleID = " + Roleid + ";", conn);
                        cmd2.ExecuteNonQuery();
                        SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                        DataTable dt2 = new DataTable();
                        conn.Close();
                        sda2.Fill(dt2);
                        ROLE = dt2.Rows[0]["roleName"].ToString();
                    }
                    catch(Exception)
                    {
                        conn.Close();
                        MessageBox.Show("Having issues connecting to the database. Try Again", "Error");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Having issues connecting to the database. Try Again", "Error");
                return false;
            }
            return isValid;
        }

        /* ------------------------------------- Admin Password validation for various super user activities ----------------------------------------------------*/
        public static bool IsvalidAdminPassword(string password)
        {
            int Roleid;
            string Role;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"Select * from tbl_UserLogin where UserPassword COLLATE SQL_Latin1_General_CP1_CS_AS = '" + password + "'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                conn.Close();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Roleid = Convert.ToInt32(dt.Rows[0]["roleid"]);
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand(@"Select * from tbl_Roles where roleID = " + Roleid + ";", conn);
                    cmd2.ExecuteNonQuery();
                    SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);
                    conn.Close();
                    Role = dt2.Rows[0]["roleName"].ToString();
                    if (string.Equals(Role, "Admin"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception) 
            {
                conn.Close();
                MessageBox.Show("Having issues connecting to the database. Try Again", "Error");
                return false;
            }


        }
    

        public static void RegisterUser(string username, string password, string email)
        {
           try
            { 
                conn.Open();
                SqlCommand cmd = new SqlCommand("RegisterUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@UserPassword", password);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException ex)
            {
                conn.Close();
                throw ex;
            }
        }          
    }
}
