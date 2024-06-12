using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace CafeManagementSystem
{
    //Add code to for sending emails for OTP verification and for news and updates on cafe
    /*Send Emails [to roles concerned] when:
     * New person logs into the the system [Admin]
     * Inventory levels running low [Manager]
     * Monthly updates and news [Admin and Manager] 
     * OTP verification [All roles for account creation]
     *
     */

    internal class Send_Emails_Module
    {

        public static SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-CAAAQI2\SQLEXPRESS;Initial Catalog=CafeManagementDB;Integrated Security=True");

        private static string sender_email = "cmsnotifications.noreply@gmail.com";
        private static string sender_email_application_password = "ielgfkzfmjmcogqu";

        public static bool verifyEmailFromDatabase(string email)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"Select * from tbl_Employee where Email = '" + email + "'", conn);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                conn.Close();
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(SqlException)
            {
                MessageBox.Show("Error connecting to database. Please try again.");
                conn.Close();
            }
            return false;
        }

        public static string sendOTP(string reciever_email, string username)
        {
            string randomcode, messagebody;
            Random rand = new Random();
            randomcode = (rand.Next(100000, 999999)).ToString();

            MailMessage mail = new MailMessage();
            messagebody = "Dear "+username+",\n\nYour Password Reset code is: " + randomcode;
            try
            {
                mail.To.Add(reciever_email);
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Invalid Email: "+ ex);
                return "";
            }
            mail.From = new MailAddress(sender_email);
            mail.Body = messagebody;
            mail.Subject = "Password Reset Code";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(sender_email, sender_email_application_password);
            smtp.Send(mail);
            return randomcode;

        }
    }
}
