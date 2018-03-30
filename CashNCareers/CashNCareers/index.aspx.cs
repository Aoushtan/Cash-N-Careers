using System;
using System.Web.UI;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;

namespace CashNCareers.cs
{
    public partial class WebForm1 : Page
    {
        public int user_ID;
        public User user = new User();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void register_Click(object sender, EventArgs e)
        {
            string user_email = register_user_email.Text;
            string user_pass = register_user_pass.Text;
            if (user_email != "" && user_pass != "")
            {
                if (CheckEmail(user_email))
                {
                    if (CanUseEmail(user_email))
                    {
                        if (AddUser(user_email, user_pass) != 0)
                        {
                            user_ID = GetUserID(user_email);
                            if (user_ID != -1)
                            {
                                user.SetUserEmail(user_email);
                                user.SetUserID(user_ID);
                                Session["User"] = user;
                                Response.Redirect("history.aspx");
                            }
                            else
                            {
                                err_message.Text = "Something went wrong.  Please try again.";
                            }
                        }
                    }
                    else
                    {
                        err_message.Text = "An account with that email already exists.";
                    }   
                }
                else
                {
                    err_message.Text = "Please enter a valid email address";
                }
            }
            else
            {
                err_message.Text = "Please enter both an email and password";
            }
                
        }
        protected void login_button_Click(object sender, EventArgs e)
        {
            string user_email = login_user_email.Text;
            string user_pass = login_user_pass.Text;
            if(user_email != "" && user_pass != "")
            {
                if (CheckEmail(user_email))
                {
                    if (ValidateUser(user_email, user_pass))
                    {
                        user_ID = GetUserID(user_email);
                        if (user_ID != -1)
                        {
                            user.SetUserEmail(user_email);
                            user.SetUserID(user_ID);
                            Session["User"] = user;
                            Response.Redirect("history.aspx");
                        }
                        else
                        {
                            err_message.Text = "Something went wrong.  Please try again";
                        }
                    }
                    else
                    {
                        err_message.Text = "Incorrect username or password";
                    }
                }
                else
                {
                    err_message.Text = "Please enter a valid email address";
                }
            }
            else
            {
                err_message.Text = "Please enter both an email and password";
            }
            
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
                openCon.Close();
                err_message.Text = "Error connecting to server";
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
                    queryGetID.Parameters.AddWithValue("@email", email);
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
        protected bool ValidateUser(string email, string pass)
        {
            string connectionString = null;
            string obtained_pass = "";
            SqlConnection openCon;
            SqlCommand queryValidateUser;
            SqlDataReader reader;
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            string getUser = "SELECT Pass FROM UserAccount WHERE Email = @email";
            try
            {
                openCon.Open();
                queryValidateUser = new SqlCommand(getUser, openCon);
                queryValidateUser.Parameters.AddWithValue("@email", email);
                queryValidateUser.CommandType = CommandType.Text;
                reader = queryValidateUser.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        obtained_pass = reader.GetString(0);
                    }
                }
                if (obtained_pass.Equals(pass))
                {
                    queryValidateUser.Dispose();
                    openCon.Close();
                    return true;
                }
                else
                {
                    queryValidateUser.Dispose();
                    openCon.Close();
                    return false;
                }
            }
            catch (SqlException e)
            {
                openCon.Close();
                err_message.Text = "Error connecting to server.";
                return false;
            }
        }
        protected bool CanUseEmail(string email)
        {
            string connectionString = null;
            SqlConnection openCon;
            SqlCommand queryEmailExists;
            SqlDataReader reader;
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            string emailExists = "SELECT UserID FROM UserAccount WHERE Email = @email";
            try
            {
                openCon.Open();
                queryEmailExists = new SqlCommand(emailExists, openCon);
                queryEmailExists.Parameters.AddWithValue("@email", email);
                queryEmailExists.CommandType = CommandType.Text;
                reader = queryEmailExists.ExecuteReader();
                if (reader.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (SqlException e)
            {
                openCon.Close();
                err_message.Text = e.Message;
                return false;
            }
        }
    }
}