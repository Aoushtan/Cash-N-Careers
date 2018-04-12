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
        static string connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
        SqlConnection openCon;
        int currentID;
        bool editing;
        static List<string> job_titles = new List<string>();
        static List<string> job_salary = new List<string>();
        static List<string> school_name = new List<string>();
        static List<string> school_tuition = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                user = (User)Session["User"];
                if (user.UserID == -1)
                {
                    Response.Redirect("index.aspx");
                }
                if (!IsPostBack)
                {
                    LoadBasicMode();
                    basic_mode_Click(basic_mode, EventArgs.Empty);

                }
                //Could potentially change this whole system for history editing by using if(!IsPostBack) here, which does something only on page load rather than refresh, but keep this for now.
                if (user.CurrentHistID != -1)
                {
                    advanced_mode_Click(advanced_mode, EventArgs.Empty);
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
            if((string)Session["Mode"] == "advanced")
            {
                //Returns true if there are blanks (Not valid values)
                if (In_ScenarioName.Text == "" || In_College.Text == "" || In_Tuition.Text == "" || In_Scholarships.Text == "" || In_PartTimeWork.Text == "" || In_Gifts.Text == "" ||
                    In_ColCareer.Text == "" || In_ColSalary.Text == "" || In_HsCareer.Text == "" || In_HsSalary.Text == "")
                {
                    err_message.Text = "Please enter a value for each field.";
                    return false;
                }
                else if ((!int.TryParse(In_Tuition.Text, out int_test)) || (!int.TryParse(In_Scholarships.Text, out int_test)) || (!int.TryParse(In_PartTimeWork.Text, out int_test)) || (!int.TryParse(In_Gifts.Text, out int_test)) ||
                    (!int.TryParse(In_ColSalary.Text, out int_test)) || (!int.TryParse(In_HsSalary.Text, out int_test)))
                {
                    err_message.Text = "Please make sure all number fields contain only whole numbers.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                //Returns true if there are blanks (Not valid values)
                if (In_ScenarioName.Text == "" || SchoolList.SelectedItem.Value == "" || SchoolTuition.Text == "" || In_Scholarships.Text == "" || In_PartTimeWork.Text == "" || In_Gifts.Text == "" ||
                    JobList.SelectedItem.Value == "" || JobSalary.Text == "" || JobList_HS.SelectedItem.Value == "" || JobSalary_HS.Text == "")
                {
                    err_message.Text = "Please enter a value for each field.";
                    return false;
                }
                else if ((!int.TryParse(SchoolTuition.Text, out int_test)) || (!int.TryParse(In_Scholarships.Text, out int_test)) || (!int.TryParse(In_PartTimeWork.Text, out int_test)) || (!int.TryParse(In_Gifts.Text, out int_test)) ||
                    (!int.TryParse(JobSalary.Text, out int_test)) || (!int.TryParse(JobSalary_HS.Text, out int_test)))
                {
                    err_message.Text = "Please make sure all number fields contain only whole numbers.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            
        }
        protected void SendToExcel()
        {
            //Excel variables
            Application excelApp = new Application();
            excelApp.Visible = true;
            Workbook workBook = excelApp.Workbooks.Open(path);
            Worksheet workSheet = workBook.Sheets["Career Comparison"];
            //Get required user data
            int tuition;
            int col_salary;
            int hs_salary;
            int scholarships = int.Parse(In_Scholarships.Text);
            int part_time = int.Parse(In_PartTimeWork.Text);
            int gifts = int.Parse(In_Gifts.Text);
            if((string)Session["Mode"] == "advanced")
            {
                tuition = int.Parse(In_Tuition.Text);
                col_salary = int.Parse(In_ColSalary.Text);
                hs_salary = int.Parse(In_HsSalary.Text);
            }
            else
            {
                tuition = int.Parse(SchoolTuition.Text);
                col_salary = int.Parse(JobSalary.Text);
                hs_salary = int.Parse(JobSalary_HS.Text);
            }
            

            //Send values to the excel cells
            workSheet.Cells[15, "C"] = col_salary;
            workSheet.Cells[15, "F"] = hs_salary;
            workSheet.Cells[20, "C"] = part_time;
            workSheet.Cells[20, "D"] = gifts;
            workSheet.Cells[20, "E"] = scholarships;
            workSheet.Cells[20, "F"] = tuition;

            //Read values from excel
            string student_loan = Math.Round((workSheet.Cells[24, "C"] as Range).Value).ToString();
            string savings = Math.Round((workSheet.Cells[24, "F"] as Range).Value).ToString();
            string monthly_payment = Math.Round((workSheet.Cells[28, "F"] as Range).Value).ToString();
            string col_monthly_raw = Math.Round((workSheet.Cells[31, "D"] as Range).Value).ToString();
            string col_init_monthly_disc = Math.Round((workSheet.Cells[31, "E"] as Range).Value).ToString();
            string col_lifetime_disc = Math.Round((workSheet.Cells[31, "F"] as Range).Value).ToString();
            string col_NPV = Math.Round((workSheet.Cells[31, "G"] as Range).Value).ToString();
            string hs_monthly_raw = Math.Round((workSheet.Cells[32, "D"] as Range).Value).ToString();
            string hs_init_monthly_disc = Math.Round((workSheet.Cells[32, "E"] as Range).Value).ToString();
            string hs_lifetime_disc = Math.Round((workSheet.Cells[32, "F"] as Range).Value).ToString();
            string hs_NPV = Math.Round((workSheet.Cells[32, "G"] as Range).Value).ToString();
            string diff_monthly = Math.Round((workSheet.Cells[34, "D"] as Range).Value).ToString();
            string diff_init_monthly = Math.Round((workSheet.Cells[34, "E"] as Range).Value).ToString();
            string diff_lifetime = Math.Round((workSheet.Cells[34, "F"] as Range).Value).ToString();
            string diff_NPV = Math.Round((workSheet.Cells[34, "G"] as Range).Value).ToString();

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
            if (ValidateInputs())
            {
                string saveScenario;
                SqlCommand querySaveScenario;
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
                    querySaveScenario.Parameters.AddWithValue("@PartTime", int.Parse(In_PartTimeWork.Text));
                    querySaveScenario.Parameters.AddWithValue("@Gifts", int.Parse(In_Gifts.Text));
                    querySaveScenario.Parameters.AddWithValue("@Scholarships", int.Parse(In_Scholarships.Text));
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
                    if ((string)Session["Mode"] == "advanced")
                    {
                        querySaveScenario.Parameters.AddWithValue("@ColCareer", In_ColCareer.Text);
                        querySaveScenario.Parameters.AddWithValue("@HsJob", In_HsCareer.Text);
                        querySaveScenario.Parameters.AddWithValue("@College", In_College.Text);
                        querySaveScenario.Parameters.AddWithValue("@ColSalary", int.Parse(In_ColSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@HsSalary", int.Parse(In_HsSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@Tuition", int.Parse(In_Tuition.Text));
                    }
                    else
                    {
                        querySaveScenario.Parameters.AddWithValue("@ColCareer", JobList.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@HsJob", JobList_HS.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@College", SchoolList.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@ColSalary", int.Parse(JobSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@HsSalary", int.Parse(JobSalary_HS.Text));
                        querySaveScenario.Parameters.AddWithValue("@Tuition", int.Parse(SchoolTuition.Text));
                    }
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
            else
            {
                err_message.Text = "Please make sure all fields have values.";
            }
            
        }
        protected void GetHistoryData()
        {
            SqlCommand queryHistoryData;
            SqlDataReader reader;
            ArrayList list = new ArrayList();
            List<string> data = new List<string>();
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
            string deleteRecord;
            SqlCommand queryDeleteRecord;
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
        protected void basic_mode_Click(object sender, EventArgs e)
        {
            mode_message.Text = "You are currently in basic mode.  Basic mode allows you to select your college and career from a dropdown menu.  Once a college is selected, the tuition field will automatically be filled" +
                " in.  The same applies to career and salary.  To change to advanced mode, click the button below.  Warning, changing modes while editing a previous calculation will require you to re-enter certain information.";
            //Enable the viewing of all basic mode buttons/fields
            SchoolList.Visible = true;
            SchoolTuition.Visible = true;
            JobList.Visible = true;
            JobSalary.Visible = true;
            JobList_HS.Visible = true;
            JobSalary_HS.Visible = true;
            //Disable the viewing of all advanced mode buttons/fields
            In_College.Visible = false;
            In_Tuition.Visible = false;
            In_ColCareer.Visible = false;
            In_ColSalary.Visible = false;
            In_HsCareer.Visible = false;
            In_HsSalary.Visible = false;
            //Set the session variable to basic mode
            Session["Mode"] = "basic";
        }

        protected void advanced_mode_Click(object sender, EventArgs e)
        {
            mode_message.Text = "You are currently in advanced mode.  Advanced mode requires you to manually enter tuition and salary for entered colleges and careers.  You can use the links near the fields to help with " +
                "your reasearch.  To change to basic mode, click the button below.  Warning, changing modes while editing a previous calculation will require you to re-enter certain information.";
            //Disable the viewing of all basic mode buttons/fields
            SchoolList.Visible = false;
            SchoolTuition.Visible = false;
            JobList.Visible = false;
            JobSalary.Visible = false;
            JobList_HS.Visible = false;
            JobSalary_HS.Visible = false;
            //Enable the viewing of all advanced mode buttons/fields
            In_College.Visible = true;
            In_Tuition.Visible = true;
            In_ColCareer.Visible = true;
            In_ColSalary.Visible = true;
            In_HsCareer.Visible = true;
            In_HsSalary.Visible = true;
            //Set the session variable to basic mode
            Session["Mode"] = "advanced";
        }
        protected void LoadBasicMode()
        {
            GetSchoolData();
            GetJobData();
            BindBasicData();
        }
        protected void School_Change(Object sender, EventArgs e)
        {
            int index = school_name.IndexOf(SchoolList.SelectedItem.Value);
            SchoolTuition.Text = school_tuition[index].ToString();
        }
        protected void Job_Change_Col(Object sender, EventArgs e)
        {
            int index = job_titles.IndexOf(JobList.SelectedItem.Value);
            JobSalary.Text = job_salary[index].ToString();
        }
        protected void Job_Change_HS(Object sender, EventArgs e)
        {
            int index = job_titles.IndexOf(JobList_HS.SelectedItem.Value);
            JobSalary_HS.Text = job_salary[index].ToString();
        }
        protected void GetSchoolData()
        {
            SqlCommand queryGetSchoolInfo;
            SqlDataReader reader;
            ArrayList list = new ArrayList();
            List<string> data = new List<string>();
            openCon = new SqlConnection(connectionString);
            string getSchools = "SELECT Name, Tuition FROM Schools";
            try
            {
                openCon.Open();
                queryGetSchoolInfo = new SqlCommand(getSchools, openCon);
                queryGetSchoolInfo.CommandType = CommandType.Text;
                reader = queryGetSchoolInfo.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[2]; //2 refers to the number of columns in the Schools table
                        reader.GetValues(values);
                        list.Add(values);
                    }
                    data = ParseData(list);
                    SplitList(data, "school");
                }
                queryGetSchoolInfo.Dispose();
                openCon.Close();
            }
            catch (SqlException error)
            {
                openCon.Close();
                err_message.Text = error.Message;
            }
        }
        protected void GetJobData()
        {
            SqlCommand queryGetJobInfo;
            SqlDataReader reader;
            ArrayList list = new ArrayList();
            List<string> data = new List<string>();
            openCon = new SqlConnection(connectionString);
            string getJobs = "SELECT Title, Pay FROM Jobs";
            try
            {
                openCon.Open();
                queryGetJobInfo = new SqlCommand(getJobs, openCon);
                queryGetJobInfo.CommandType = CommandType.Text;
                reader = queryGetJobInfo.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Object[] values = new Object[2]; //2 refers to the number of items we're selecting from the Jobs table
                        reader.GetValues(values);
                        list.Add(values);
                    }
                    data = ParseData(list);
                    SplitList(data, "job");
                }
                queryGetJobInfo.Dispose();
                openCon.Close();
            }
            catch (SqlException error)
            {
                openCon.Close();
                err_message.Text = error.Message;
            }
        }
        protected void SplitList(List<string> data, string type)
        {
            int counter = 0;
            foreach (string item in data)
            {
                if (type == "school")
                {
                    if (counter % 2 == 0)
                    {
                        school_name.Add(item);
                    }
                    else
                    {
                        school_tuition.Add(item);
                    }
                }
                else
                {
                    if (counter % 2 == 0)
                    {
                        job_titles.Add(item);
                    }
                    else
                    {
                        job_salary.Add(item);
                    }
                }
                counter++;
            }
        }
        protected void BindBasicData()
        {
            SchoolList.DataSource = CreateSchoolList();
            SchoolList.DataTextField = "SchoolName";
            SchoolList.DataValueField = "SchoolValue";
            JobList.DataSource = CreateJobList();
            JobList.DataTextField = "JobName";
            JobList.DataValueField = "JobValue";
            JobList_HS.DataSource = CreateJobList();
            JobList_HS.DataTextField = "JobName";
            JobList_HS.DataValueField = "JobValue";
            SchoolList.DataBind();
            JobList.DataBind();
            JobList_HS.DataBind();
        }
        protected ICollection CreateSchoolList()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new DataColumn("SchoolName", typeof(string)));
            dt.Columns.Add(new DataColumn("SchoolValue", typeof(string)));
            foreach (string school in school_name)
            {
                dt.Rows.Add(CreateRow(school, school, dt));
            }
            DataView dv = new DataView(dt);
            return dv;
        }
        protected ICollection CreateJobList()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new DataColumn("JobName", typeof(string)));
            dt.Columns.Add(new DataColumn("JobValue", typeof(string)));
            foreach (string job in job_titles)
            {
                dt.Rows.Add(CreateRow(job, job, dt));
            }
            DataView dv = new DataView(dt);
            return dv;
        }
        protected DataRow CreateRow(string text, string value, System.Data.DataTable dt)
        {
            DataRow dr = dt.NewRow();

            dr[0] = text;
            dr[1] = value;
            return dr;
        }
    }
}