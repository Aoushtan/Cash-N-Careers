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
        User user;
        List<string> user_data = new List<string>();
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
                DisplayData(user_data);
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
                string getHistoryInfo = "SELECT College, CollegeCareer, CollegePay, HSJob, HSPay, DateCreated FROM UserHistory WHERE UserID = @UID";
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
                                Object[] values = new Object[6];
                                reader.GetValues(values);
                                al.Add(values);
                            }
                        }
                        data = ParseData(al);
                        return data;
                    }
                    catch (SqlException)
                    {
                        return null;
                    }
                }
            }
        }
        protected void DisplayData(List<string> data)
        {
            int col_counter = 1;
            foreach(string item in data)
            {
                switch (col_counter)
                {
                    case 1:
                        history_div.InnerHtml += "<tr><td>" + item + "</td>";
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        history_div.InnerHtml += "<td>" + item + "</td>";
                        break;
                    case 6:
                        history_div.InnerHtml += "<td>" + item + "</td></tr>";
                        col_counter = 0;
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
    }
}