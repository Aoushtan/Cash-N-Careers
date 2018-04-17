using System;
using System.Web.UI;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;

namespace CashNCareers.cs
{
    public partial class WebForm1 : Page
    {
        //Class scope variables
        public int user_ID;  //Local variable used before confirming the user's ID
        public User user = new User(); //Creating the User Object for the current user
        //This string is a long string used to connect to the sql server database.  Its format is as follows:
        //[IP];[Network protocol (have to use DBMSSOCN)];[Database];[Username];[Password]
        string connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
        SqlConnection openCon; //Creates a new SQL connection
        //This method runs when the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sign the user out if they're on the index page.
            user.SetCurrentSituation(-1);
            user.SetUserEmail("");
            user.SetUserID(-1);
        }
        //Method that runs when the register button is clicked.
        protected void register_Click(object sender, EventArgs e)
        {
            //Method variables
            string user_email = register_user_email.Text; //Takes the user's input for email and assigns it to a string
            string user_pass = register_user_pass.Text; //Takes the user's input for password and assigns it to a string
            
            //Check if either the email or password are blank.
            if (user_email != "" && user_pass != "")
            {
                //Check to see if the email the user has entered is a valid email address
                if (CheckEmail(user_email))
                {
                    //Check to see if an account with the given email already exists
                    if (CanUseEmail(user_email))
                    {
                        //Calls the AddUser method which takes an email and password and creates a new record in the user database.
                        //It returns the number of records affected, so if it isn't 0, it worked correctly.
                        //This checks to see if the user has been accurately added to the database.
                        if (AddUser(user_email, user_pass) != 0)
                        {
                            //Sets the userID to the return value of GetUserID
                            user_ID = GetUserID(user_email);
                            //Check to see if the userID is not -1 (the error value)
                            if (user_ID != -1)
                            {
                                //Setting information for the user
                                user.SetUserEmail(user_email); //Assigns the user's email to their User object
                                user.SetUserID(user_ID);  //Assigns the user's ID from the database to their User object
                                Session["User"] = user; //Create a session variable of the user, so that it will be used on each page of the app.
                                Response.Redirect("history.aspx"); //Move the user to the history page
                            }
                            else //UserID for some reason is -1
                            {
                                err_message.Text = "Something went wrong.  Please try again.";
                            }
                        }
                    }
                    else //If an account with that email already exists
                    {
                        err_message.Text = "An account with that email already exists.";
                    }   
                }
                else //if email is NOT a valid email address
                {
                    err_message.Text = "Please enter a valid email address";
                }
            }
            else //If email or password ARE blank
            {
                //Tell the user
                err_message.Text = "Please enter both an email and password";
            }
                
        }
        //This method runs when the login button is clicked.
        protected void login_button_Click(object sender, EventArgs e)
        {
            //Method variables
            string user_email = login_user_email.Text;
            string user_pass = login_user_pass.Text;
            //Check to see if the email or password are blank
            if(user_email != "" && user_pass != "")
            {
                //Make sure it's a valid email
                if (CheckEmail(user_email))
                {
                    //This checks the email and password that the user entered against their information on the database.
                    //Will return true if the entered email and password match the email and password on the database.
                    if (ValidateUser(user_email, user_pass))
                    {
                        //Assign the user ID to the return value of GetUserID
                        user_ID = GetUserID(user_email);
                        //Check to see if the user ID was correctly assigned
                        if (user_ID != -1)
                        {
                            //Set information for the user
                            user.SetUserEmail(user_email); //set the user email
                            user.SetUserID(user_ID); //set the user ID 
                            Session["User"] = user; //Create the session variable so that all pages can access the same User object
                            Response.Redirect("history.aspx"); //Move the user to the history page
                        }
                        else //UserID is somehow -1
                        {
                            err_message.Text = "Something went wrong.  Please try again";
                        }
                    }
                    else //Either the email or password is incorrect
                    {
                        err_message.Text = "Incorrect username or password";
                    }
                }
                else //Email entered is invalid
                {
                    err_message.Text = "Please enter a valid email address";
                }
            }
            else //Email or password is blank
            {
                err_message.Text = "Please enter both an email and password";
            }
            
        }
        //Method that checks if a given email is a valid email address
        protected bool CheckEmail(string email)
        {
            //This method works through the try catch methods.
            //It purposely expects to to throw an exception if the email given is not a valid email address
            try
            {
                //The MailAddress object has all of the neccessary formatting for a valid email address.
                MailAddress m = new MailAddress(email);
                //The email is fine so return true
                return true;
            }
            //Will catch any exceptions that are thrown if the email is incorrectly formatted
            catch (FormatException)
            {
                //The email is wrong so return false
                return false;
            }
        }
        //Method to add the user to the database given an email and password
        protected int AddUser(string email, string password)
        {
            //Method variables
            SqlCommand queryInsertUser; //Creates a new SQL command
            openCon = new SqlConnection(connectionString); //Initialize the connection using the connection string 
            string saveUser = "INSERT INTO UserAccount (Email,Pass) VALUES (@UserEmail,@UserPassword)"; //The SQL statment
            try //Try catch to deal with any sql exceptions (errors connecting to server)
            {
                //Open the connection
                openCon.Open();
                //Initialize the sql command with the statement saveUser and on the openCon connection
                queryInsertUser = new SqlCommand(saveUser, openCon);
                //Assign values to the sql command.  @UserEmail refers to the @UserEmail in the saveUser string
                queryInsertUser.Parameters.AddWithValue("@UserEmail", email); //Assigns the value of email to @UserEmail
                queryInsertUser.Parameters.AddWithValue("@UserPassword", password); //Assigns the value of password to @UserPassword
                queryInsertUser.CommandType = CommandType.Text; //The type of command, it's always text for this project
                int num = queryInsertUser.ExecuteNonQuery(); //ExecuteNonQuery runs the sql statement and returns the number of records affected.  Assign that value to num
                queryInsertUser.Dispose(); //Dispose of the sql command
                openCon.Close(); //Close the sql server connection
                return num; //Return the value of num
            }
            //If there was an error connecting to the server
            catch (SqlException e)
            {
                //Close the connection
                openCon.Close();
                //Tell the user what's up
                err_message.Text = "Error connecting to server";
                return 0; //return that 0 records were changed
            }
        }
        //Method that gets a user's ID based on their email
        protected int GetUserID(string email)
        {
            //Method variables
            int userID = -1; //Debug value for the userID is -1
            //A different way to connect to the SQL server, this "Using" method creates a new instance of the openCon and will close it automatically when we're done using it
            using (openCon = new SqlConnection(connectionString))
            {
                //the SQL statement
                string getID = "SELECT UserID FROM UserAccount WHERE Email = @email";
                //Creating a SQLDataReader that holds information from the database as it moves through records
                SqlDataReader reader;
                //Using a SQL command (works just like the one above) with the statement getID
                using (SqlCommand queryGetID = new SqlCommand(getID))
                {
                    //Sets the connection property of the sqlcommand
                    queryGetID.Connection = openCon;
                    queryGetID.Parameters.AddWithValue("@email", email); //Adds the value of email to @email
                    queryGetID.CommandType = CommandType.Text; //SQL command type
                    try //Try to connect and run the SQL
                    {
                        //Opens the connection
                        openCon.Open();
                        //Initialize the reader and execute the sql command
                        reader = queryGetID.ExecuteReader();
                        //Checks to see if the reader has any records
                        if (reader.HasRows)
                        {
                            //While it has data to read
                            while (reader.Read())
                            {
                                //Assigns the only response the sql command will return to the userID variable
                                userID = reader.GetInt32(0);
                            }
                        }
                        //Return that user ID
                        return userID;
                    }
                    //Catch a connection error
                    catch (SqlException)
                    {
                        //Tell the user
                        err_message.Text = "Error getting user ID.";
                        //Return the userID of -1
                        return userID;
                    }
                }
            }
        }
        //This method checks if an email and password combination is correct and belongs to an account
        protected bool ValidateUser(string email, string pass)
        {
            //Method variables
            string obtained_pass = "";  //String to hold the password obtained from the database
            SqlCommand queryValidateUser; //The sql command
            SqlDataReader reader; //Sql data reader
            openCon = new SqlConnection(connectionString); //Initialize the connection
            string getUser = "SELECT Pass FROM UserAccount WHERE Email = @email"; //the SQL statement we will be running
            try
            {
                //Open the connection
                openCon.Open();
                //initialize our sql command
                queryValidateUser = new SqlCommand(getUser, openCon);
                //Assign parameters to the sql string
                queryValidateUser.Parameters.AddWithValue("@email", email);
                queryValidateUser.CommandType = CommandType.Text;
                reader = queryValidateUser.ExecuteReader(); //Execute the sql command and read the data
                //If there are records
                if (reader.HasRows)
                {
                    //While there is data to read
                    while (reader.Read())
                    {
                        //Assign the value of the user's password in the database to obtained_pass
                        obtained_pass = reader.GetString(0);
                    }
                }
                //Compare the password in the database to the password entered by the user
                if (obtained_pass.Equals(pass))
                {
                    //If they're the same, dispose of the sql command and close the connection
                    queryValidateUser.Dispose();
                    openCon.Close();
                    return true; //Passwords are the same 
                }
                //Passwords are not the same
                else
                {
                    //Dispose of the sql command and close the connection
                    queryValidateUser.Dispose();
                    openCon.Close();
                    return false; //passwords are not the same
                }
            }
            //Catch a connection error
            catch (SqlException e)
            {
                //Close the connection, inform the user and return false
                openCon.Close();
                err_message.Text = "Error connecting to server.";
                return false;
            }
        }
        //Method to determine if an account with a given email already exists
        protected bool CanUseEmail(string email)
        {
            //Method variables
            SqlCommand queryEmailExists;  //Sql command
            SqlDataReader reader; //Sql data reader
            openCon = new SqlConnection(connectionString); //initializing the connection
            string emailExists = "SELECT UserID FROM UserAccount WHERE Email = @email"; //The sql statement
            try
            {
                //Open the connection
                openCon.Open();
                //Initialize the sql command
                queryEmailExists = new SqlCommand(emailExists, openCon);
                queryEmailExists.Parameters.AddWithValue("@email", email);
                queryEmailExists.CommandType = CommandType.Text;
                reader = queryEmailExists.ExecuteReader(); //execute the sql command and read the data
                //If it has any rows, the email already exists and cannot be used to create another account
                if (reader.HasRows)
                {
                    return false;
                }
                //The email does not exist already and can be used
                else
                {
                    return true;
                }
            }
            //Catch sql connection errors
            catch (SqlException e)
            {
                openCon.Close();
                err_message.Text = e.Message;
                return false;
            }
        }
    }
}