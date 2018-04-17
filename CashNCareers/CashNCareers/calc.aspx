<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calc.aspx.cs" Inherits="CashNCareers.calc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
		<meta name="description" content="">
		<title>Cash N Careers</title>
		<link rel="stylesheet" type="text/css" href="css/style.css"/>
        <style>
		table, th, td {
			border: 1px solid black;
			border-collapse: collapse;
		}
		th, td {
			padding: 5px;
			text-align: left;
		}
        .table{
            display:table;
            text-align:center;
        }
        .row{
            display:table-row;
            text-align:center;
        }
        .cell{
            display:table-cell;
            text-align:center;
        }
        #col {
            width: 50%;
            padding: 5px;
        }
        #HS {
            width: 50%;
            padding: 5px;
        }
		</style>
	</head>
	<body>
		<div id="container">
			<div id="header">
				<hr>
				<center><h1 id="title">Cash N Careers</h1></center>
				<hr>
			</div>
			<div id="body">
				<center>
					<p><b>Welcome to the Cash and Careers financial comparison application.
					</b></p>
                    <p>This application will allow you to compare the costs of a obtaining a college
                        degree versus working full-time.
                    </p>
                    <p>To begin please input the required values and then click Calculate
                     when finished to compare your results.<br />

                    All fields are required.  Hover over any of the input or output fields for additional information.
                    </p>
                    <br />
                    <asp:Label ID="mode_message" Text="" runat="server"></asp:Label><br />
                    <form runat="server">
                        <asp:Button ID="basic_mode" runat="server" Text="Basic Mode" OnClick="basic_mode_Click"/><asp:Button ID="advanced_mode" runat="server" Text="Advanced Mode" OnClick="advanced_mode_Click"/><br /><br />
                        <p>Use the links below to find information on careers and colleges. <br/>
                            For career information please visit the <a href="https://www.bls.gov/ooh/">Bureau of Labor Statistics</a>.<br/>
                            For college information please visit <a href="http://www.collegecalc.org/colleges/michigan/?view=all">College Calc</a>.<br />
                            Additional information: <a href="https://studentaid.ed.gov/sa/types/grants-scholarships">Scholarships and Grants</a>, <a href="http://npc.umich.edu/publications/working_papers/?publication_id=239&">Gifts</a>,
                            <a href="https://www.bls.gov/news.release/wkyeng.t06.htm">Part-Time Income</a>
                        </p><hr />
                        <asp:Label ID="err_message" runat="server" Text="" style="color:red"></asp:Label><br />
                        <b>Name Scenario: </b><asp:TextBox ID="In_ScenarioName" runat="server" placeholder="Name the current scenario" ToolTip="Enter the name of the current scenario."></asp:TextBox><br /><br />
                        <div class="table">
                            <div class="row">
                                <div id="col" class="cell">
                                    <div class="table">
                                        <h3 style="display:table-caption">College Education</h3>
                                        <div class="row">
                                            <div class="cell"><b>Career: </b></div><div class="cell"><asp:TextBox ID="In_ColCareer" runat="server" placeholder="Enter name of after college career" ToolTip="Enter the name of the career you will be working after graduating from college." Visible="false"></asp:TextBox><asp:DropDownList ID="JobList" AutoPostBack="true" OnSelectedIndexChanged="Job_Change_Col" runat="server" Visible="false" style="width:75%;" ToolTip="Select a career from this list generated from the Bureau of Labor Statistics."/></div><br>
                                        </div>
                                        <div class="row">
                                            <div class="cell"><b>Salary: </b></div><div class="cell"><asp:TextBox ID="In_ColSalary" runat="server" placeholder="Enter the yearly salary of chosen career" ToolTip="Enter the yearly salary for the career you have chosen." Visible="false"></asp:TextBox><asp:Label ID="JobSalary" runat="server" Text="Select an item from the list above." Visible="false" ToolTip="This is the salary for the career you selected above."/></div><br>
                                        </div>
					                    <div class="row">
                                            <div class="cell"><b>College: </b></div><div class="cell"><asp:TextBox ID="In_College" runat="server" placeholder="Enter College" ToolTip="Enter the name of the college you wish to attend here." Visible="false"></asp:TextBox><asp:DropDownList ID="SchoolList" AutoPostBack="true" OnSelectedIndexChanged="School_Change" runat="server" visible="false" ToolTip="Please select the college you would like to attend from the list of Michigan colleges."/></div><br>
					                    </div>
                                        <div class="row">
                                            <div class="cell"><b>Tuition: </b></div><div class="cell"><asp:TextBox ID="In_Tuition" runat="server" placeholder="Enter tuition cost" ToolTip="Enter the amount in dollars that tuition costs for a single year." Visible="false"></asp:TextBox><asp:Label ID="SchoolTuition" runat="server" Text="Select an item from the list above." Visible="false" ToolTip="This is the tuition for the college you selected above."></asp:Label></div><br>
                                        </div>
                                        <div class="row">
                                            <div class="cell"><b>Scholarships and Grants: </b></div><div class="cell"><asp:TextBox ID="In_Scholarships" runat="server" placeholder="Enter scholarship amount" ToolTip="Enter the total amount in dollars of scholarships recieved per year."></asp:TextBox></div><br>
                                        </div>
                                        <div class="row">
                                            <div class="cell"><b>Part-Time Income: </b></div><div class="cell"><asp:TextBox ID="In_PartTimeWork" runat="server" placeholder="Enter yearly income during schooling" ToolTip="Enter the total yearly income of part-time work in dollars."></asp:TextBox></div><br>
                                        </div>
                                        <div class="row">
                                            <div class="cell"><b>Gifts: </b></div><div class="cell"><asp:TextBox ID="In_Gifts" runat="server" placeholder="Enter amount in dollars of gifts recieved" ToolTip="Enter the total amount in dollars of any gifts recieved."></asp:TextBox></div><br>
                                        </div>
                                    </div> 
					            <br>
                                </div>
                                <div id="HS" class="cell">
                                    <div class="table">
                                        <h3 style="display:table-caption">High School Education</h3>
                                        <div class="row">
                                            <div class="cell"><b>Job Title: </b></div><div class="cell"><asp:TextBox ID="In_HsCareer" runat="server" placeholder="Enter the name of after high school career" ToolTip="Enter the name of the career you would choose to do right after high school." visible="false"></asp:TextBox><asp:DropDownList ID="JobList_HS" AutoPostBack="true" OnSelectedIndexChanged="Job_Change_HS" runat="server" Visible="false" style="width:75%;" ToolTip="Select a career from this list generated from the Bureau of Labor Statistics."/></div><br>
                                        </div>
                                        <div class="row">
                                            <div class="cell"><b>Yearly Income: </b></div><div class="cell"><asp:TextBox ID="In_HsSalary" runat="server" placeholder="Enter the yearly salary of chosen career" ToolTip="Enter the yearly income for the career you have chosen." Visible="false"></asp:TextBox><asp:Label ID="JobSalary_HS" runat="server" Text="Select an item from the list above." Visible="false" ToolTip="This is the salary for the career you selected above."/></div><br>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <asp:Button ID="calculate" Text="Calculate" runat="server" OnClick="calculate_Click"/>

                        <h3><u>Results</u></h3>

				<div style="text-align: center;">
					<div style="display: inline-block; text-align: left;">
						<b>Student Loans: $<asp:Label ID="Out_StudentLoan" runat="server" Text="" ToolTip="The student loans balance includes  tuition and the cost of living less part-time work income, gifts, and scholarships & grants. Interest on the loans is also included."></asp:Label></b><br>
						<b>Monthly Payment: $<asp:Label ID="Out_MonthlyPayment" runat="server" Text="" ToolTip="The monthly payment is the amortized amount required to completely pay all student loans with interest over a working life time."></asp:Label></b><br>
						<b>Highschool Savings: $<asp:Label ID="Out_Savings" runat="server" Text="" ToolTip="The amount a worker right out of highschool would have in savings."></asp:Label></b><br>
					</div>
				</div>
                <center>
				<table style="width:75%">
			  <tr>
			  	<th></th>
			    <th>Monthly Raw Dollar Value</th>
			    <th>Initial Monthly Discretionary</th>
			    <th>Lifetime Discretionary</th>
			    <th>Lifetime NPV</th>
			  </tr>
			  <tr>
			    <th>College</th>
			    <td>$<asp:Label ID="Out_ColMonthlyRaw" runat="server" Text="" ToolTip="This is the monthly take home pay before taxes, student loans, or other expenses."></asp:Label></td>
					<td>$<asp:Label ID="Out_ColInitDisc" runat="server" Text="" ToolTip="This is the monthly take home amount after student loans, federal taxes, social security, medicare, and state tax payments."></asp:Label></td>
					<td>$<asp:Label ID="Out_ColLifetimeDisc" runat="server" Text="" ToolTip="This is the sum of monthly discretionary over the working lifetime after adjusting for inflation."></asp:Label></td>
					<td>$<asp:Label ID="Out_ColLifetimeNPV" runat="server" Text="" ToolTip="Lifetime Net Present Value is the sum of the monthly discretionary pay amounts reduced by the discount rate. This considers the time value of money and is the primary decision making metric in finance."></asp:Label></td>
			  </tr>
			  <tr>
			    <th>Highschool</th>
			    <td>$<asp:Label ID="Out_HsMonthlyRaw" runat="server" Text="" ToolTip="This is the monthly take home pay before taxes, student loans, or other expenses."></asp:Label></td>
			    <td>$<asp:Label ID="Out_HsInitDisc" runat="server" Text="" ToolTip="This is the monthly take home amount after student loans, federal taxes, social security, medicare, and state tax payments."></asp:Label></td>
					<td>$<asp:Label ID="Out_HsLifetimeDisc" runat="server" Text="" ToolTip="This is the sum of monthly discretionary over the working lifetime after adjusting for inflation."></asp:Label></td>
					<td>$<asp:Label ID="Out_HsLifetimeNPV" runat="server" Text="" ToolTip="Lifetime Net Present Value is the sum of the monthly discretionary pay amounts reduced by the discount rate. This considers the time value of money and is the primary decision making metric in finance."></asp:Label></td>
			  </tr>
			  <tr>
			    <th>Difference</th>
			    <td>$<asp:Label ID="Out_DiffMonthlyRaw" runat="server" Text="" ToolTip="This is the monthly take home pay before taxes, student loans, or other expenses."></asp:Label></td>
			    <td>$<asp:Label ID="Out_DiffInitDisc" runat="server" Text="" ToolTip="This is the monthly take home amount after student loans, federal taxes, social security, medicare, and state tax payments."></asp:Label></td>
					<td>$<asp:Label ID="Out_DiffLifetimeDisc" runat="server" Text="" ToolTip="This is the sum of monthly discretionary over the working lifetime after adjusting for inflation."></asp:Label></td>
			    <td>$<asp:Label ID="Out_DiffLifetimeNPV" runat="server" Text="" ToolTip="Lifetime Net Present Value is the sum of the monthly discretionary pay amounts reduced by the discount rate. This considers the time value of money and is the primary decision making metric in finance."></asp:Label></td>
			  </tr>
				</table>
                </center>
                        <br />
                        <asp:Button ID="save_senario" runat="server" Text="Save Scenario" OnClick="save_senario_Click"/>
                        <br /><br /><br />
                    </form>
				</center>
			</div>
			<div id="footer">
				<font size="2">Last updated: April 16, 2018<br />
                    Western Michigan University<br />Haworth College of business<br />CIS 4990 Enterprise Project
                    Tool Designed by Dr. Matthew Ross.  Project for Dr. Muhammad Razi<br />
                    Project leads: Rachel Larson, Kelsey Hood.  Business Analysts: Justin Johnson, Kruti Patel.<br />
                    Systems Analysts: Hanen Alwafi, John Bruestle, Marzouq Albaiji.  Systems Developers: Austin Lemacks, Zack Filary, Steven Freds.<br /><br />
				</font>
			</div>
		</div>
	</body>
</html>
