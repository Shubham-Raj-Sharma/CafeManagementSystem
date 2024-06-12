using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    class Billing_Module
    {
        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");

        public static int getActiveOrderCount()
        {
            int count = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"SELECT Count(OrderID) FROM Orders WHERE OrderStatus = 1;", conn);
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

        public static void getActiveOrders(DataTable dt, string query)
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

        public static bool saveBill(int BillNumber, int OrderId, decimal PaidAmount)
        {
            try
            {
                string query1 = "INSERT INTO Billing VALUES (" + BillNumber + ", " + OrderId + ", " + PaidAmount + ")";
                string query2 = "UPDATE Orders SET OrderStatus = 0 WHERE OrderID = " + OrderId + ";";
                string query3 = "UPDATE CafeTables SET TableStatus = 0 WHERE TableNo = (SELECT TableNo FROM Orders WHERE OrderID = " + OrderId + ");";
                conn.Open();

                SqlCommand cmd1 = new SqlCommand(query1, conn);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.ExecuteNonQuery();
                
                SqlCommand cmd3 = new SqlCommand(query1, conn);
                cmd3.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Billing Successfull.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message + "Billing Unsuccessfull.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool uadateBill(int BillNumber, int OrderId, decimal PaidAmount)
        {
            try
            {
                string query1 = "UPDATE Billing SET PaidAmount = " + PaidAmount + ")";
                string query2 = "UPDATE Orders SET OrderStatus = -1 WHERE OrderID = " + OrderId + ";";
                string query3 = "UPDATE CafeTables SET TableStatus = 0 WHERE TableNo = (SELECT TableNo FROM Orders WHERE OrderID = " + OrderId + ");";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                cmd1.ExecuteNonQuery();
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Billing Successfull.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (SqlException ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message + "Billing Unsuccessfull.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public static void getOrderItems(string query, DataTable dt)
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

        public static int getLastBillNumber()
        {
            string query = "SELECT MAX(BillNo) FROM Billing;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                int LastBillNumber = (int)cmd.ExecuteScalar();
                conn.Close();
                return LastBillNumber;
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                conn.Close();
                MessageBox.Show("Failed to connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                conn.Close();
                return -1;
            }
        }

        public static void LoadData(string query, DataTable dt)
        {
            dt.Clear();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Error Connecting to the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool openAllBills(DataTable dt)
        {
            dt.Clear();
            try
            {
                string query = "SELECT * FROM Billing Where PaidAmount IS NOT NULL;";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("No record was found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Error Connecting to the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool openByOrderID(int id, DataTable dt)
        {
            dt.Clear();
            try
            {
                string query = "SELECT m.ItemName, o.Quantity, m.Price FROM OrderItemList AS o INNER JOIN MenuItems AS m ON o.ItemID = m.MenuItemID WHERE o.OrderID = " + id + "AND OrderID > 0;";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("No record was found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception ex) 
            {
                conn.Close();
                MessageBox.Show(ex.Message+ ": Error Connecting to the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool openByBillNumber(int bill, DataTable dt)
        {
            try
            {
                string query = "SELECT m.ItemName, o.Quantity, m.Price FROM OrderItemList AS o INNER JOIN MenuItems AS m ON o.ItemID = m.MenuItemID WHERE o.OrderID = (SELECT OrderID FROM Billing Where BillNo = " + bill + " AND PaidAmount IS NOT NULL);";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("No record was found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            catch (Exception)
            {
                conn.Close();
                MessageBox.Show("Error Connecting to the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
