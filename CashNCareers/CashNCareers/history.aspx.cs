using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CashNCareers
{
    public partial class history : System.Web.UI.Page
    {
        //Class scope variables
        User user;
        List<string> user_data = new List<string>(); //List of strings to hold user data
        static List<string> historyID = new List<string>(); //List of identifiers for each row of history data
        protected void Page_Load(object sender, EventArgs e)
        {
            //Try to run this code
            try
            {
                //Set the user variable to the same user we used in the index page
                user = (User)Session["User"];
                //Check to see if there is currently a user ID (which means you can't go here unless you've logged in)
                if (user.UserID == -1)
                {
                    //Move the user back to the index page
                    Response.Redirect("index.aspx");
                }
                //Get their email and display it for them.
                string email = user.GetUserEmail();
                logged_as.Text = "Logged in as " + email + ".";
                //Store the user's data into a list
                user_data = GetUserData(user.UserID); //GetUserData gets the user's data
                //This checks to see if the user has any data
                if (user_data[0] == null)
                {
                    //Tell the user that no previous data was found
                    info.Text = "No previous data found, please press create new to begin.";
                }
                else
                {
                    //Make sure this only happens once, aka the page is not posting back
                    if(!IsPostBack)
                    {
                        //Display the user's data
                        DisplayData(user_data);
                    }
                }
            }
            //Something went wrong
            catch (Exception error)
            {
                //Kick the user back to the index page
                info.Text = error.Message;
                Response.Redirect("index.aspx");
            }
        }
        //This method get's the user's information from the database
        protected List<string> GetUserData(int ID)
        {
            List<string> data = new List<string>(); //List to hold all of their data
            ArrayList al = new ArrayList(); //List to hold each row of data coming in from the database
            //Connecting to the database
            using (SqlConnection openCon = new SqlConnection(""))
            {
                //SQL query
                string getHistoryInfo = "SELECT HistID, SessionName, College, CollegeCareer, CollegePay, HSJob, HSPay, DateCreated FROM UserHistory WHERE UserID = @UID";
                SqlDataReader reader; //Data reader
                //Connecting to the database using the query
                using (SqlCommand queryGetID = new SqlCommand(getHistoryInfo))
                {
                    queryGetID.Connection = openCon; //Assigning connection
                    queryGetID.Parameters.AddWithValue("@UID", ID); //Assigning varaibles to the query
                    queryGetID.CommandType = CommandType.Text;
                    try
                    {
                        //Opening the connection 
                        openCon.Open();
                        reader = queryGetID.ExecuteReader(); //Execute the sql query
                        if (reader.HasRows) //Check if the query has results
                        {
                            while (reader.Read())
                            {
                                //Asign the value of the sql query to an object array
                                Object[] values = new Object[8]; //8 is the number of things being selected from the sql query
                                reader.GetValues(values); //Gather the values in the query and store them into the object array
                                al.Add(values); //Add the object array to the ArrayList
                            }
                            //Parse the ArrayList and store the values from that into the data list
                            data = ParseData(al);
                            return data; //return all of the data as a string list
                        }
                        else
                        {
                            //No results, so add null to the list
                            data.Add(null);
                            return data; //return the data
                        }
                    }
                    //Something went wrong
                    catch (SqlException)
                    {
                        //Return null
                        data.Add(null);
                        return data;
                    }
                }
            }
        }
        //This method displays the user's past calculations data
        protected void DisplayData(List<string> data)
        {
            //Counters for formatting
            int col_counter = 1;
            int row_counter = 0;
            //foreach to sort through each item in the list
            foreach(string item in data)
            {
                //Switch case for the different cases that col counter could be in.  This is so that each item in the list can be formatted correctly,
                //For example, first comes the histID so when col_counter is 1 (histID), we add the item to the historyID list.  Then we increase the col counter and move on.
                //When it's 2 we just place the item in the table and so on.
                switch (col_counter)
                {
                    //For each case add html elements to the history table
                    case 1:
                        history_div.InnerHtml += "<tr><td><input type='radio' id='hist_" + row_counter + "' name='history_edits' value='" + row_counter + "'></td>";
                        historyID.Add(item);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        history_div.InnerHtml += "<td>" + item + "</td>";
                        break;
                    case 8:
                        history_div.InnerHtml += "<td>" + item + "</td></tr>";
                        col_counter = 0; //Reset the column counter
                        row_counter++; //Increase the row counter
                        break;
                }
                col_counter++; //Increase the column counter after each item
            }
        }
        //This method parses out the data from an ArrayList into a List<string>
        protected List<string> ParseData(ArrayList list)
        {
            List<string> data = new List<string>(); //Will hold our data
            foreach(Object[] row in list) //Selects each row of data from the sql query earlier
            {
                foreach(object col in row) //Selects each value in that row
                {
                    data.Add(col.ToString()); //Adds the value to the list
                }
            }
            return data; //retuns the list
        }
        //Method that fires when the user clicks the create new button 
        protected void create_new_Click(object sender, EventArgs e)
        {
            //Update the user session variable
            Session["User"] = user;
            Response.Redirect("calc.aspx"); //Move the user to the calculations page
        }
        //Method that fires when you click the edit button
        protected void edit_btn_Click(object sender, EventArgs e)
        {
            //This checks to see if you have a radio button selected
            if(Request.Form["history_edits"] != null)
            {
                //Get the selected radio button
                int selected_radio = int.Parse(Request.Form["history_edits"]);
                int histID = int.Parse(historyID[selected_radio]); //Assign the value of the radio button to histID
                user.SetCurrentSituation(histID); //Set the user's current scenario to the histID
                Session["User"] = user; //Update the user session variable
                Response.Redirect("calc.aspx"); //Move the user to the calc page
            }
            else
            {
                //Tell the user that they have to pick a radio button in order to edit a scenario
                info.Text = "You must select a scenario to edit or view using the radio buttons.";
            }
        }
    }
}