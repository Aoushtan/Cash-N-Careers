<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calc.aspx.cs" Inherits="CashNCareers.calc" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
		<meta name="author" content="Austin Lemacks">
		<meta name="description" content="">
		<title>Cash N Careers</title>
		<link rel="stylesheet" type="text/css" href="css/style.css"/>
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
                     when finished to compare your results.

                    All fields are required.
                    </p>

                    <form runat="server">
                        <asp:Label ID="err_message" runat="server" Text=""></asp:Label><br />
                        Name Scenario: <asp:TextBox ID="In_ScenarioName" runat="server" placeholder="Name the current scenario" ToolTip="Enter the name of the current scenario."></asp:TextBox><br /><br />
					    <h3>College Education</h3>
					    College: <asp:TextBox ID="In_College" runat="server" placeholder="Enter College" ToolTip="Enter the name of the college you wish to attend here."></asp:TextBox><br>
                        Tuition: <asp:TextBox ID="In_Tuition" runat="server" placeholder="Enter tuition cost" ToolTip="Enter the amount in dollars that tuition costs for a single year."></asp:TextBox><br>
                        Scholarships: <asp:TextBox ID="In_Scholarships" runat="server" placeholder="Enter scholarship amount" ToolTip="Enter the total amount in dollars of scholarships recieved per year."></asp:TextBox><br>
                        Part-Time Income: <asp:TextBox ID="In_PartTimeWork" runat="server" placeholder="Enter yearly income during schooling" ToolTip="Enter the total yearly income of part-time work in dollars."></asp:TextBox><br>
                        Gifts: <asp:TextBox ID="In_Gifts" runat="server" placeholder="Enter amount in dollars of gifts recieved" ToolTip="Enter the total amount in dollars of any gifts recieved."></asp:TextBox><br>
                        Career: <asp:TextBox ID="In_ColCareer" runat="server" placeholder="Enter name of after college career" ToolTip="Enter the name of the career you will be working after graduating from college."></asp:TextBox><br>
					    Salary: <asp:TextBox ID="In_ColSalary" runat="server" placeholder="Enter the yearly salary of chosen career" ToolTip="Enter the yearly salary for the career you have chosen."></asp:TextBox><br>
					    <br>

                        <h3>High School Education</h3>
                        Job Title: <asp:TextBox ID="In_HsCareer" runat="server" placeholder="Enter the name of after high school career" ToolTip="Enter the name of the career you would choose to do right after high school."></asp:TextBox><br>
                        Yearly Income: <asp:TextBox ID="In_HsSalary" runat="server" placeholder="Enter the yearly salary of chosen career" ToolTip="Enter the yearly income for the career you have chosen."></asp:TextBox><br>

                        <asp:Button ID="calculate" Text="Calculate" runat="server" OnClick="calculate_Click"/>

                        <h3>Results</h3>
                        Student Loans: <asp:Label ID="Out_StudentLoan" runat="server" Text=""></asp:Label><br>
                        Savings: <asp:Label ID="Out_Savings" runat="server" Text=""></asp:Label><br>
                        Monthly Payment: <asp:Label ID="Out_MonthlyPayment" runat="server" Text=""></asp:Label><br>
                        College Monthly Raw Dollar Value: <asp:Label ID="Out_ColMonthlyRaw" runat="server" Text=""></asp:Label><br>
                        College Initial Monthly Discrectionary: <asp:Label ID="Out_ColInitDisc" runat="server" Text=""></asp:Label><br>
                        College Lifetime Discrectionary: <asp:Label ID="Out_ColLifetimeDisc" runat="server" Text=""></asp:Label><br>
                        College Lifetime Net Present Value: <asp:Label ID="Out_ColLifetimeNPV" runat="server" Text=""></asp:Label><br>
                        Higschool Monthly Raw Dollar Value: <asp:Label ID="Out_HsMonthlyRaw" runat="server" Text=""></asp:Label><br>
                        Higschool Initial Monthly Discrectionary: <asp:Label ID="Out_HsInitDisc" runat="server" Text=""></asp:Label><br>
                        Higschool Lifetime Discrectionary: <asp:Label ID="Out_HsLifetimeDisc" runat="server" Text=""></asp:Label><br>
                        Higschool Lifetime Net Present Value: <asp:Label ID="Out_HsLifetimeNPV" runat="server" Text=""></asp:Label><br>
                        Monthly Raw Dollar Value Difference: <asp:Label ID="Out_DiffMonthlyRaw" runat="server" Text=""></asp:Label><br>
                        Initial Monthly Discrectionary Difference: <asp:Label ID="Out_DiffInitDisc" runat="server" Text=""></asp:Label><br>
                        Lifetime Discrectionary Difference: <asp:Label ID="Out_DiffLifetimeDisc" runat="server" Text=""></asp:Label><br>
                        Lifetime Net Present Value Difference: <asp:Label ID="Out_DiffLifetimeNPV" runat="server" Text=""></asp:Label><br>
                        <br />
                        <asp:Button ID="save_senario" runat="server" Text="Save Scenario" OnClick="save_senario_Click"/>
                        <br />
                    </form>
				</center>
			</div>
			<div id="footer">
					<font size="1">Last updated: March 12, 2018</font>
			</div>
		</div>
	</body>
</html>
