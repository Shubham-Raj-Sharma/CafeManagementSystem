using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CafeManagementSystem
{
    class Menu_And_Orders_Module
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");

        public static int getItemCount()
        {
            int count = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT Count(MenuItemID) FROM MenuItems;", conn);
                count = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Error Connecting to Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            return count;
        }

        public static int getActiveItemCount()
        {
            int count = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT Count(MenuItemID) FROM MenuItems where IsActive = 1;", conn);
                count = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Error Connecting to Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            return count;
        }

        public static void getMenuCategories(DataTable dt)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT DISTINCT Category FROM MenuItems where isActive = 1;", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                conn.Close();
                sda.Fill(dt);
            }
            catch (SqlException)
            {
                conn.Close();
            }
        }

        public static void getItem_From_Categories(DataTable dt, string query)
        {
            try
            { 
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                conn.Close();
                sda.Fill(dt);
            }
            catch (SqlException)
            {
                conn.Close();
            }
        }

        public static bool StoreNewMenuItem(String itemName, decimal itemPrice, String itemCategory, String itemDescription, byte[] ImageValue)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("AddMenuItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", itemName);
                cmd.Parameters.AddWithValue("@ItemPrice", itemPrice);
                cmd.Parameters.AddWithValue("@ItemCategory", itemCategory);
                cmd.Parameters.AddWithValue("@ItemDescription", itemDescription);
                cmd.Parameters.AddWithValue("@ItemImage", ImageValue);
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


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static bool UpdateMenuItem(int id, String itemName, decimal itemPrice, String itemCategory, String itemDescription, byte[] ImageValue)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UpdateMenuItem", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@ItemName", itemName);
                cmd.Parameters.AddWithValue("@ItemPrice", itemPrice);
                cmd.Parameters.AddWithValue("@ItemCategory", itemCategory);
                cmd.Parameters.AddWithValue("@ItemDescription", itemDescription);
                cmd.Parameters.AddWithValue("@ItemImage", ImageValue);
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

        public static bool RemoveItem(int id)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update MenuItems set isActive = 0 where MenuItemID = "+id+";", conn);
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

        public static bool AddItem(int id)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update MenuItems set isActive = 1 where MenuItemID = " + id + ";", conn);
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

        public static bool StoreNewOrder(int TableNumber, int EmployeeID, String date, DataTable OrderItems)
        {
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("AddOrder", conn, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableNumber", TableNumber);
                cmd.Parameters.AddWithValue("@OrderDate", date);
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand("SELECT MAX(OrderID) AS OrderId FROM Orders", conn, transaction);
                int OrderID = Convert.ToInt32(cmd2.ExecuteScalar());

                SqlCommand cmd3 = new SqlCommand("AddOrderItems", conn, transaction);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@OrderID", SqlDbType.Int);
                cmd3.Parameters.AddWithValue("@ItemID", SqlDbType.Int);
                cmd3.Parameters.AddWithValue("@Quantity", SqlDbType.Int);

                for (int i = 0; i < OrderItems.Rows.Count; i++)
                {
                    int ItemID = Convert.ToInt32(OrderItems.Rows[i]["ItemID"].ToString());
                    int Quantity = Convert.ToInt32(OrderItems.Rows[i]["Quantity"].ToString());
                    cmd3.Parameters["@OrderID"].Value = OrderID;
                    cmd3.Parameters["@ItemID"].Value = ItemID;
                    cmd3.Parameters["@Quantity"].Value = Quantity;
                    cmd3.ExecuteNonQuery();
                }

                transaction.Commit();
                conn.Close();
                return true;
            }
            catch (SqlException)
            {
                if (transaction != null)
                    transaction.Rollback();

                conn.Close();
                MessageBox.Show("Error creating the order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void LoadData(string date, DataTable dt)
        {
            dt.Clear();
            try
            {
                conn.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Orders WHERE CONVERT(date, OrderDate) = '" + date+"';", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);               
                sda.Fill(dt);
                if (!dt.Columns.Contains("Order_Status"))
                    dt.Columns.Add("Order_Status", typeof(string));
                if (dt.Rows.Count > 0) 
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int orderStatusValue = Convert.ToInt32(row["OrderStatus"].ToString());
                        if (orderStatusValue == -1)
                        {
                            row["Order_Status"] = "Cancelled";
                        }
                        else if (orderStatusValue == 0)
                        {
                            row["Order_Status"] = "Completed";
                        }
                        else
                        {
                            row["Order_Status"] = "Active";
                        }
                    }
                }
                if (dt.Columns.Contains("OrderStatus"))
                    dt.Columns.Remove("OrderStatus");
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void cancelOrder(int OrderID)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Update Orders SET OrderStatus = -1 WHERE OrderID = " + OrderID + ";", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Order cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Error cancelling the order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
