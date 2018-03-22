using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;

namespace CashNCareers.cs
{
    public partial class WebForm1 : Page
    {
        public int user_ID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Do something when page loads
        }

        protected void register_Click(object sender, EventArgs e)
        {
            string user_email = register_user_email.Text;
            string user_pass = register_user_pass.Text;
            if (CheckEmail(user_email))
            {
                if(AddUser(user_email, user_pass) != 0)
                {
                    user_ID = GetUserID(user_email);
                    if(user_ID != -1)
                    {
                        Response.Redirect("history.aspx");
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                err_message.Text = "Please enter a valid email address";
            }
        }

        protected void login_button_Click(object sender, EventArgs e)
        {
            
        }
        protected bool CheckEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        protected int AddUser(string email, string password)
        {
            string connectionString = null;
            SqlConnection openCon;
            SqlCommand queryInsertUser;
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            string saveUser = "INSERT INTO UserAccount (Email,Pass) VALUES (@UserEmail,@UserPassword)";
            try
            {
                openCon.Open();
                queryInsertUser = new SqlCommand(saveUser, openCon);
                queryInsertUser.Parameters.AddWithValue("@UserEmail", email);
                queryInsertUser.Parameters.AddWithValue("@UserPassword", password);
                queryInsertUser.CommandType = CommandType.Text;
                int num = queryInsertUser.ExecuteNonQuery();
                queryInsertUser.Dispose();
                openCon.Close();
                return num;
            }
            catch (SqlException e)
            {
                err_message.Text = e.Message;
                return 0;
            }
        }
        protected int GetUserID(string email)
        {
            int userID = -1;
            using (SqlConnection openCon = new SqlConnection("Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996"))
            {
                string saveUser = "SELECT UserID FROM UserAccount WHERE Email = @email";
                SqlDataReader reader;
                using (SqlCommand queryGetID = new SqlCommand(saveUser))
                {
                    queryGetID.Connection = openCon;
                    queryGetID.Parameters.AddWithValue("@email",email);
                    queryGetID.CommandType = CommandType.Text;
                    try
                    {
                        openCon.Open();
                        reader = queryGetID.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                userID = reader.GetInt32(0);
                            }
                        }
                        return userID;
                    }
                    catch (SqlException)
                    {
                        err_message.Text = "Error getting user ID.";
                        return userID;
                    }
                }
            }
        }
    }
}