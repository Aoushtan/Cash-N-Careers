using Microsoft.Office.Interop.Excel;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace CashNCareers
{
    public partial class calc : System.Web.UI.Page
    {
        User user;
        static string path = "C:\\CNC\\CNC.xlsm";
        int currentID;
        bool editing;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                user = (User)Session["User"];
                if (user.UserID == -1)
                {
                    Response.Redirect("index.aspx");
                }
                if (user.CurrentHistID != -1)
                {
                    GetHistoryData();
                    DropOldRecord();
                    user.SetCurrentSituation(-1);
                }
            }
            catch (Exception)
            {
                Response.Redirect("index.aspx");
            }
        }
        protected bool ValidateInputs()
        {
            int int_test;
            //Returns true if there are blanks (Not valid values)
            if (In_ScenarioName.Text == "" || In_College.Text == "" || In_Tuition.Text == "" || In_Scholarships.Text == "" || In_PartTimeWork.Text == "" || In_Gifts.Text == "" ||
                In_ColCareer.Text == "" || In_ColSalary.Text == "" || In_HsCareer.Text == "" || In_HsSalary.Text == "")
            {
                err_message.Text = "Please enter a value for each field.";
                return false;
            }
            else if ((!int.TryParse(In_Tuition.Text, out int_test)) || (!int.TryParse(In_Scholarships.Text, out int_test)) || (!int.TryParse(In_PartTimeWork.Text,out int_test)) || (!int.TryParse(In_Gifts.Text,out int_test)) ||
                (!int.TryParse(In_ColSalary.Text,out int_test)) || (!int.TryParse(In_HsSalary.Text,out int_test)))
            {
                err_message.Text = "Please make sure all number fields contain only whole numbers.";
                return false;
            }
            else
            {
                return true;
            }
        }
        protected void SendToExcel()
        {
            //Excel variables
            Application excelApp = new Application();
            excelApp.Visible = true;
            Workbook workBook = excelApp.Workbooks.Open(path);
            Worksheet workSheet = workBook.Sheets["InsertedValues"];
            //Get required user data
            int tuition = int.Parse(In_Tuition.Text);
            int scholarships = int.Parse(In_Scholarships.Text);
            int part_time = int.Parse(In_PartTimeWork.Text);
            int gifts = int.Parse(In_Gifts.Text);
            int col_salary = int.Parse(In_ColSalary.Text);
            int hs_salary = int.Parse(In_HsSalary.Text);

            //Send values to the excel cells
            workSheet.Cells[4, "B"] = col_salary;
            workSheet.Cells[5, "B"] = hs_salary;
            workSheet.Cells[6, "B"] = part_time;
            workSheet.Cells[7, "B"] = gifts;
            workSheet.Cells[8, "B"] = scholarships;
            workSheet.Cells[9, "B"] = tuition;

            //Read values from excel
            string student_loan = (workSheet.Cells[10, "B"] as Range).Value.ToString();
            string savings = (workSheet.Cells[11, "B"] as Range).Value.ToString();
            string monthly_payment = (workSheet.Cells[13, "B"] as Range).Value.ToString();
            string col_monthly_raw = (workSheet.Cells[14, "B"] as Range).Value.ToString();
            string col_init_monthly_disc = (workSheet.Cells[15, "B"] as Range).Value.ToString();
            string col_lifetime_disc = (workSheet.Cells[16, "B"] as Range).Value.ToString();
            string col_NPV = (workSheet.Cells[17, "B"] as Range).Value.ToString();
            string hs_monthly_raw = (workSheet.Cells[18, "B"] as Range).Value.ToString();
            string hs_init_monthly_disc = (workSheet.Cells[19, "B"] as Range).Value.ToString();
            string hs_lifetime_disc = (workSheet.Cells[20, "B"] as Range).Value.ToString();
            string hs_NPV = (workSheet.Cells[21, "B"] as Range).Value.ToString();
            string diff_monthly = (workSheet.Cells[22, "B"] as Range).Value.ToString();
            string diff_init_monthly = (workSheet.Cells[23, "B"] as Range).Value.ToString();
            string diff_lifetime = (workSheet.Cells[24, "B"] as Range).Value.ToString();
            string diff_NPV = (workSheet.Cells[25, "B"] as Range).Value.ToString();

            //Display data
            Out_StudentLoan.Text = student_loan;
            Out_Savings.Text = savings;
            Out_MonthlyPayment.Text = monthly_payment;
            Out_ColMonthlyRaw.Text = col_monthly_raw;
            Out_ColInitDisc.Text = col_init_monthly_disc;
            Out_ColLifetimeDisc.Text = col_lifetime_disc;
            Out_ColLifetimeNPV.Text = col_NPV;
            Out_HsMonthlyRaw.Text = hs_monthly_raw;
            Out_HsInitDisc.Text = hs_init_monthly_disc;
            Out_HsLifetimeDisc.Text = hs_lifetime_disc;
            Out_HsLifetimeNPV.Text = hs_NPV;
            Out_DiffMonthlyRaw.Text = diff_monthly;
            Out_DiffInitDisc.Text = diff_init_monthly;
            Out_DiffLifetimeDisc.Text = diff_lifetime;
            Out_DiffLifetimeNPV.Text = diff_NPV;

            //Exit excel
            workBook.Close(0);
            excelApp.Quit();
        }

        protected void calculate_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                SendToExcel();
            }
        }

        protected void save_senario_Click(object sender, EventArgs e)
        {
            string connectionString = null;
            string saveScenario;
            SqlConnection openCon;
            SqlCommand querySaveScenario;
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            saveScenario = "INSERT INTO UserHistory (UserID, CollegeCareer, HSJob, College, CollegePay, HSPay, PartTimeWork, Gifts, Scholarships," +
            "Tuition, StudentLoan, Savings, MonthlyPayment, CollegeMonthlyRaw, CollegeInitialMonthlyRaw, CollegeLifetimeDiscretionary, CollegeNPV, " +
            "HSMonthlyRaw, HSInitialMonthlyRaw, HSLifetimeDiscretionary, HSNPV, DifferenceMonthly, DifferenceInitialMonthly, DifferenceLifetime, DifferenceNPV," +
            "DateCreated, SessionName) VALUES (@UID, @ColCareer, @HsJob, @College, @ColSalary, @HsSalary, @PartTime, @Gifts, @Scholarships, @Tuition, @StudentLoan," +
            "@Savings, @MonthlyPay, @ColMR, @ColIMR, @ColLD, @ColNPV, @HsMR, @HsIMR, @HsLD, @HsNPV, @DifM, @DifIM, @DifL, @DifNPV, @Date, @Session)";
            try
            {
                openCon.Open();
                querySaveScenario = new SqlCommand(saveScenario, openCon);
                querySaveScenario.Parameters.AddWithValue("@UID", user.GetUserID());
                querySaveScenario.Parameters.AddWithValue("@ColCareer", In_ColCareer.Text);
                querySaveScenario.Parameters.AddWithValue("@HsJob", In_HsCareer.Text);
                querySaveScenario.Parameters.AddWithValue("@College", In_College.Text);
                querySaveScenario.Parameters.AddWithValue("@ColSalary",int.Parse(In_ColSalary.Text));
                querySaveScenario.Parameters.AddWithValue("@HsSalary", int.Parse(In_HsSalary.Text));
                querySaveScenario.Parameters.AddWithValue("@PartTime", int.Parse(In_PartTimeWork.Text));
                querySaveScenario.Parameters.AddWithValue("@Gifts", int.Parse(In_Gifts.Text));
                querySaveScenario.Parameters.AddWithValue("@Scholarships", int.Parse(In_Scholarships.Text));
                querySaveScenario.Parameters.AddWithValue("@Tuition", int.Parse(In_Tuition.Text));
                querySaveScenario.Parameters.AddWithValue("@StudentLoan", int.Parse(Out_StudentLoan.Text));
                querySaveScenario.Parameters.AddWithValue("@Savings", int.Parse(Out_Savings.Text));
                querySaveScenario.Parameters.AddWithValue("@MonthlyPay", int.Parse(Out_MonthlyPayment.Text));
                querySaveScenario.Parameters.AddWithValue("@ColMR", int.Parse(Out_ColMonthlyRaw.Text));
                querySaveScenario.Parameters.AddWithValue("@ColIMR", int.Parse(Out_ColInitDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@ColLD", int.Parse(Out_ColLifetimeDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@ColNPV", int.Parse(Out_ColLifetimeNPV.Text));
                querySaveScenario.Parameters.AddWithValue("@HsMR", int.Parse(Out_HsMonthlyRaw.Text));
                querySaveScenario.Parameters.AddWithValue("@HsIMR", int.Parse(Out_HsInitDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@HsLD", int.Parse(Out_HsLifetimeDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@HsNPV", int.Parse(Out_HsLifetimeNPV.Text));
                querySaveScenario.Parameters.AddWithValue("@DifM", int.Parse(Out_DiffMonthlyRaw.Text));
                querySaveScenario.Parameters.AddWithValue("@DifIM", int.Parse(Out_DiffInitDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@DifL", int.Parse(Out_DiffLifetimeDisc.Text));
                querySaveScenario.Parameters.AddWithValue("@DifNPV", int.Parse(Out_DiffLifetimeNPV.Text));
                querySaveScenario.Parameters.AddWithValue("@Date", DateTime.Now);
                querySaveScenario.Parameters.AddWithValue("@Session", In_ScenarioName.Text);
                querySaveScenario.CommandType = CommandType.Text;
                querySaveScenario.ExecuteNonQuery();
                querySaveScenario.Dispose();
                openCon.Close();
                Session["User"] = user;
                Response.Redirect("history.aspx");
            }
            catch (SqlException error)
            {
                openCon.Close();
                err_message.Text = error.Message;
            }
        }
        protected void GetHistoryData()
        {
            string connectionString = null;
            SqlConnection openCon;
            SqlCommand queryHistoryData;
            SqlDataReader reader;
            ArrayList list = new ArrayList();
            List<string> data = new List<string>();
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            string saveScenario = "SELECT * FROM UserHistory WHERE UserID = @UID AND HistID = @HID";
            try
            {
                openCon.Open();
                queryHistoryData = new SqlCommand(saveScenario, openCon);
                queryHistoryData.Parameters.AddWithValue("@UID", user.GetUserID());
                queryHistoryData.Parameters.AddWithValue("@HID", user.CurrentHistID);
                queryHistoryData.CommandType = CommandType.Text;
                reader = queryHistoryData.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[28]; //28 refers to the number of columns in the UserHistory table
                        reader.GetValues(values);
                        list.Add(values);
                    }
                    data = ParseData(list);
                    DisplayHistory(data);
                }
                queryHistoryData.Dispose();
                openCon.Close();
            }
            catch (SqlException error)
            {
                openCon.Close();
                err_message.Text = error.Message;
            }
        }
        protected List<string> ParseData(ArrayList list)
        {
            List<string> data = new List<string>();
            foreach (Object[] row in list)
            {
                foreach (object col in row)
                {
                    data.Add(col.ToString());
                }
            }
            return data;
        }
        protected void DisplayHistory(List<string> data)
        {
            //skip 1 and 2 since they are not displayed
            In_ColCareer.Text = data[2];
            In_HsCareer.Text = data[3];
            In_College.Text = data[4];
            In_ColSalary.Text = data[5];
            In_HsSalary.Text = data[6];
            In_PartTimeWork.Text = data[7];
            In_Gifts.Text = data[8];
            In_Scholarships.Text = data[9];
            In_Tuition.Text = data[10];
            Out_StudentLoan.Text = data[11];
            Out_Savings.Text = data[12];
            Out_MonthlyPayment.Text = data[13];
            Out_ColMonthlyRaw.Text = data[14];
            Out_ColInitDisc.Text = data[15];
            Out_ColLifetimeDisc.Text = data[16];
            Out_ColLifetimeNPV.Text = data[17];
            Out_HsMonthlyRaw.Text = data[18];
            Out_HsInitDisc.Text = data[19];
            Out_HsLifetimeDisc.Text = data[20];
            Out_HsLifetimeNPV.Text = data[21];
            Out_DiffMonthlyRaw.Text = data[22];
            Out_DiffInitDisc.Text = data[23];
            Out_DiffLifetimeDisc.Text = data[24];
            Out_DiffLifetimeNPV.Text = data[25];
            //Skip 27 because it isn't being displayed
            In_ScenarioName.Text = data[27];
        }
        protected void DropOldRecord()
        {
            string connectionString = null;
            string deleteRecord;
            SqlConnection openCon;
            SqlCommand queryDeleteRecord;
            connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
            openCon = new SqlConnection(connectionString);
            deleteRecord = "DELETE FROM UserHistory WHERE HistID = @HID";
            try
            {
                openCon.Open();
                queryDeleteRecord = new SqlCommand(deleteRecord, openCon);
                queryDeleteRecord.Parameters.AddWithValue("@HID", user.CurrentHistID);
                queryDeleteRecord.CommandType = CommandType.Text;
                queryDeleteRecord.ExecuteNonQuery();
                queryDeleteRecord.Dispose();
                openCon.Close();
            }
            catch (SqlException error)
            {
                openCon.Close();
                err_message.Text = error.Message;
            }
        }
    }
}