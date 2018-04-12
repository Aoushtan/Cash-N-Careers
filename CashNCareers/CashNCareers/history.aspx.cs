using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            try
            {
                user = (User)Session["User"];
                if (user.UserID == -1)
                {
                    Response.Redirect("index.aspx");
                }
                string email = user.GetUserEmail();
                logged_as.Text = "Logged in as " + email + ".";
                user_data = GetUserData(user.UserID);
                if (user_data[0] == null)
                {
                    info.Text = "No previous data found, please press create new to begin.";
                }
                else
                {
                    if(!IsPostBack)
                    {
                        DisplayData(user_data);
                    }
                }
            }
            catch (Exception error)
            {
                info.Text = error.Message;
                Response.Redirect("index.aspx");
            }
        }
        
        protected List<string> GetUserData(int ID)
        {
            List<string> data = new List<string>();
            ArrayList al = new ArrayList();
            using (SqlConnection openCon = new SqlConnection("Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996"))
            {
                string getHistoryInfo = "SELECT HistID, SessionName, College, CollegeCareer, CollegePay, HSJob, HSPay, DateCreated FROM UserHistory WHERE UserID = @UID";
                SqlDataReader reader;
                using (SqlCommand queryGetID = new SqlCommand(getHistoryInfo))
                {
                    queryGetID.Connection = openCon;
                    queryGetID.Parameters.AddWithValue("@UID", ID);
                    queryGetID.CommandType = CommandType.Text;
                    try
                    {
                        openCon.Open();
                        reader = queryGetID.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Object[] values = new Object[8];
                                reader.GetValues(values);
                                al.Add(values);
                            }
                            data = ParseData(al);
                            return data;
                        }
                        else
                        {
                            data.Add(null);
                            return data;
                        }
                    }
                    catch (SqlException)
                    {
                        data.Add(null);
                        return data;
                    }
                }
            }
        }
        protected void DisplayData(List<string> data)
        {
            int col_counter = 1;
            int row_counter = 0;
            foreach(string item in data)
            {
                switch (col_counter)
                {
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
                        col_counter = 0;
                        row_counter++;
                        break;
                }
                col_counter++;
            }
        }
        protected List<string> ParseData(ArrayList list)
        {
            List<string> data = new List<string>();
            foreach(Object[] row in list)
            {
                foreach(object col in row)
                {
                    data.Add(col.ToString());
                }
            }
            return data;
        }

        protected void create_new_Click(object sender, EventArgs e)
        {
            Session["User"] = user;
            Response.Redirect("calc.aspx");
        }

        protected void edit_btn_Click(object sender, EventArgs e)
        {
            if(Request.Form["history_edits"] != null)
            {
                int selected_radio = int.Parse(Request.Form["history_edits"]);
                int histID = int.Parse(historyID[selected_radio]);
                user.SetCurrentSituation(histID);
                Session["User"] = user;
                Response.Redirect("calc.aspx");
            }
            else
            {
                info.Text = "You must select a scenario to edit.";
            }
        }
    }
}