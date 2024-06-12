using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CafeManagementSystem
{
    internal class Settings_Module
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");
        public static bool LoadCafeInformation(DataTable dt)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM CafeInfo;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                return true;
            }
            catch(Exception)
            {
                conn.Close();
                MessageBox.Show("Error connecting to database.(From CafeInfo)", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;   
            }
                
        }

        public static bool LoadCafeTablesInformation(DataTable dt)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM CafeTables;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                if (!dt.Columns.Contains("Table_Status"))
                    dt.Columns.Add("Table_Status", typeof(string));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int orderStatusValue = Convert.ToInt32(row["TableStatus"].ToString());
                        if (orderStatusValue == -1)
                        {
                            row["Table_Status"] = "Reserved";
                        }
                        else if (orderStatusValue == 0)
                        {
                            row["Table_Status"] = "Vacant";
                        }
                        else
                        {
                            row["Table_Status"] = "Occupied";
                        }
                    }
                }
                if (dt.Columns.Contains("TableStatus"))
                    dt.Columns.Remove("TableStatus");
                return true;
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Error connecting to database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public static bool LoadPersonalInformation(DataTable dt, string user)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM tbl_SuperUser WHERE AdminID = (SELECT AdminID FROM tbl_UserLogin WHERE Username = '" + user + "');";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message+"Error connecting to database.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static bool LoadAccountInformation(DataTable dt, string user)
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM tbl_UserLogin WHERE Username = '" + user + "';";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message+"Error connecting to database.(From AccountInfo)", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public static bool updateCafeInfo(string cafeName, string cafeAddress, string cafeContact, byte[] cafeLogo, string pan, double vat, double tax)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateCafeInformation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CafeName", cafeName);
                cmd.Parameters.AddWithValue("@CafeAddress", cafeAddress);
                cmd.Parameters.AddWithValue("@CafeContact", cafeContact);
                cmd.Parameters.AddWithValue("@CafeLogo", cafeLogo);
                cmd.Parameters.AddWithValue("@CafePAN", pan);
                cmd.Parameters.AddWithValue("@CafeVAT", vat);
                cmd.Parameters.AddWithValue("@CafeServiceTax", tax);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
