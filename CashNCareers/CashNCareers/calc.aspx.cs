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
        //Class variables
        User user;
        static string path = "C:\\CNC\\CNC.xlsm"; //Path to the excel file for calculations (THIS MUST BE EXACT).
        static string connectionString = "Data Source=141.218.104.41,1433;Network=DBMSSOCN;Initial Catalog=Cash-n-CareerTeam02;User ID=Austin;Password=Lema1996";
        SqlConnection openCon; //SQL connection
        int currentID; //The current scenario's ID
        bool editing; //A bool to see if we're editing or creating a new scenario
        //Lists to hold the drop down menu information (these are static so that they don't get updated on post back)
        static List<string> job_titles = new List<string>();
        static List<string> job_salary = new List<string>();
        static List<string> school_name = new List<string>();
        static List<string> school_tuition = new List<string>();
        //Method for page loading
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Assign the user session variable to user
                user = (User)Session["User"];
                //Check if the user exists right now
                if (user.UserID == -1)
                {
                    //if not redirect the user to the index page
                    Response.Redirect("index.aspx");
                }
                //Check if this is a post back (makes things inside only run once)
                if (!IsPostBack)
                {
                    //Loads all of the dropdown menu data
                    LoadBasicMode();
                    //Enables basic mode by default
                    basic_mode_Click(basic_mode, EventArgs.Empty);

                }
                //Checks to see if the user is currently editing 
                if (user.CurrentHistID != -1)
                {
                    //Enable advanced mode (makes editing way easier than trying to load basic mode)
                    advanced_mode_Click(advanced_mode, EventArgs.Empty);
                    GetHistoryData(); //Get the data for the selected scenario
                    DropOldRecord(); //Drops the old data so we can replace it
                    user.SetCurrentSituation(-1); //Defaults the current scenario to avoid issues with saving a new one
                }
            }
            catch (Exception)
            {
                //Move the user to the index page if they aren't logged in
                Response.Redirect("index.aspx");
            }
        }
        //Method that ensures all fields have the correct data in them
        protected bool ValidateInputs()
        {
            int int_test; //just a variable used as an integer
            //Checks to see if we're in advanced mode
            if((string)Session["Mode"] == "advanced")
            {
                //This check to make sure all fields are filled in, and that all relevant fields contain whole numbers for advanced mode
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
            else //basic mode
            {
                //Checks to make sure all fields have a value and that all relevant fields contain whole numbers
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
        //This method handles all connections to the excel file.  YOU WERE DOCUMENTING ALL THE CODE MAROON
        protected void SendToExcel()
        {
            //Excel variables
            Application excelApp = new Application(); //Application
            excelApp.Visible = true; //Makes the workbook visible on the server
            Workbook workBook = excelApp.Workbooks.Open(path); //Opens the workbook
            Worksheet workSheet = workBook.Sheets["Career Comparison"]; //Name of the worksheet
            //Get required user data
            int tuition;
            int col_salary;
            int hs_salary;
            int scholarships = int.Parse(In_Scholarships.Text);
            int part_time = int.Parse(In_PartTimeWork.Text);
            int gifts = int.Parse(In_Gifts.Text);
            if((string)Session["Mode"] == "advanced") //Check for advanced mode to get the right stuff
            {
                tuition = int.Parse(In_Tuition.Text);
                col_salary = int.Parse(In_ColSalary.Text);
                hs_salary = int.Parse(In_HsSalary.Text);
            }
            else //basic mode
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

            //Read values from excel cells
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
            workBook.Close(0); //Close the workbook
            excelApp.Quit(); //Quit the application and close it out of memory
            err_message.Text = "";
        }
        //Method that fires when the user clicks calculate
        protected void calculate_Click(object sender, EventArgs e)
        {
            //Makes sure inputs are valid
            if (ValidateInputs())
            {
                //Calls the excel function
                SendToExcel();
            }
        }
        //Method that fires when the user clicks the save scenario button
        protected void save_senario_Click(object sender, EventArgs e)
        {
            //again makes sure the inputs are valid
            if (ValidateInputs())
            {
                string saveScenario;
                SqlCommand querySaveScenario;
                openCon = new SqlConnection(connectionString);
                //Super long query string to insert the data into the database
                saveScenario = "INSERT INTO UserHistory (UserID, CollegeCareer, HSJob, College, CollegePay, HSPay, PartTimeWork, Gifts, Scholarships," +
                "Tuition, StudentLoan, Savings, MonthlyPayment, CollegeMonthlyRaw, CollegeInitialMonthlyRaw, CollegeLifetimeDiscretionary, CollegeNPV, " +
                "HSMonthlyRaw, HSInitialMonthlyRaw, HSLifetimeDiscretionary, HSNPV, DifferenceMonthly, DifferenceInitialMonthly, DifferenceLifetime, DifferenceNPV," +
                "DateCreated, SessionName) VALUES (@UID, @ColCareer, @HsJob, @College, @ColSalary, @HsSalary, @PartTime, @Gifts, @Scholarships, @Tuition, @StudentLoan," +
                "@Savings, @MonthlyPay, @ColMR, @ColIMR, @ColLD, @ColNPV, @HsMR, @HsIMR, @HsLD, @HsNPV, @DifM, @DifIM, @DifL, @DifNPV, @Date, @Session)";
                try
                {
                    //Open the connection and assign variables to the sql query
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
                    //Check for advanced mode to make sure we have the right fields
                    if ((string)Session["Mode"] == "advanced")
                    {
                        querySaveScenario.Parameters.AddWithValue("@ColCareer", In_ColCareer.Text);
                        querySaveScenario.Parameters.AddWithValue("@HsJob", In_HsCareer.Text);
                        querySaveScenario.Parameters.AddWithValue("@College", In_College.Text);
                        querySaveScenario.Parameters.AddWithValue("@ColSalary", int.Parse(In_ColSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@HsSalary", int.Parse(In_HsSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@Tuition", int.Parse(In_Tuition.Text));
                    }
                    else //basic mode
                    {
                        querySaveScenario.Parameters.AddWithValue("@ColCareer", JobList.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@HsJob", JobList_HS.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@College", SchoolList.SelectedItem.Value);
                        querySaveScenario.Parameters.AddWithValue("@ColSalary", int.Parse(JobSalary.Text));
                        querySaveScenario.Parameters.AddWithValue("@HsSalary", int.Parse(JobSalary_HS.Text));
                        querySaveScenario.Parameters.AddWithValue("@Tuition", int.Parse(SchoolTuition.Text));
                    }
                    querySaveScenario.CommandType = CommandType.Text;
                    querySaveScenario.ExecuteNonQuery(); //Execute the query
                    querySaveScenario.Dispose();
                    openCon.Close();
                    Session["User"] = user; //save the user 
                    Response.Redirect("history.aspx"); //move the user to the history page
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
        //Get the data for the given scenario (for editing)
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
        //Method that removes the old record to replace it with the new for editing
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
        //Method that fires when you click the basic mode button
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
        //Method that fires when you click the advanced mode button
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
        //Method that calls other methods to load up the basic mode data
        protected void LoadBasicMode()
        {
            GetSchoolData();
            GetJobData();
            BindBasicData();
        }
        //Method that fires when you change the selected college in the dropdown menu
        protected void School_Change(Object sender, EventArgs e)
        {
            int index = school_name.IndexOf(SchoolList.SelectedItem.Value);
            SchoolTuition.Text = school_tuition[index].ToString();
        }
        //Method that fires when you change the selected job in the dropdown menu for college education
        protected void Job_Change_Col(Object sender, EventArgs e)
        {
            int index = job_titles.IndexOf(JobList.SelectedItem.Value);
            JobSalary.Text = job_salary[index].ToString();
        }
        //Method that fires when you change the selected job in the dropdown menu for highschool education
        protected void Job_Change_HS(Object sender, EventArgs e)
        {
            int index = job_titles.IndexOf(JobList_HS.SelectedItem.Value);
            JobSalary_HS.Text = job_salary[index].ToString();
        }
        //Method that gets the college names and tuition data from the sql server and assigns it to the list in the class variables
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
        //Method that gets the career information (name and salary) from the sql server and assigns it to the lists in the class variables
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
        //Method that takes one list and splits it into 2 lists alternatively (every other item in the list)
        //This is used to split the college name and college tuition into different lists, same with careers and salary
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
        //Method that binds all of the data to the dropdown menus
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
        //Creates the list of data for the college dropdown menu
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
        //Creates the list of data for the career dropdown menu
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
        //Creates a new row of data in the dropdown menu, this is called for each college/career in the lists and gives them their values
        protected DataRow CreateRow(string text, string value, System.Data.DataTable dt)
        {
            DataRow dr = dt.NewRow();

            dr[0] = text;
            dr[1] = value;
            return dr;
        }
    }
}