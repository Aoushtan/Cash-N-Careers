<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="history.aspx.cs" Inherits="CashNCareers.history" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta name="description" content="">
	<title>Cash N Careers</title>
	<link rel="stylesheet" type="text/css" href="css/style.css"/>
</head>
<style>
table, th, td {
    border: 1px solid black;
    border-collapse: collapse;
}
th, td {
    padding: 8px;
    text-align: center;
}
</style>
<body>
	<div id="container">
		<div id="header">
		<hr>
		<center><h1 id="title">Cash N Careers</h1></center>
		<hr>
	</div>
	<div id="body">
	    <center>
			<p><b><h1>History</h1></b></p>
            <asp:Label ID="logged_as" runat="server"></asp:Label>
            <p>This page allows you to view or edit your previous career calculations.
            </p>
            <asp:Label ID="info" runat="server" style="color:red"></asp:Label>
            <form runat="server"><asp:Button ID="create_new" Text="Create New" runat="server" OnClick="create_new_Click"></asp:Button>
                <table style="width:100%">
                    <tr>
                    <th>Edit</th>
                    <th>Scenario Name</th>
                    <th>College</th>
                    <th>College Career</th>
                    <th>College Career Salary</th>
                    <th>Highschool Career</th>
                    <th>Highschool Career Salary</th>
	                <th>Date Created</th>
                    </tr>
                    <div id="history_div" runat="server">


                    </div>
                </table>
                <br /><br />
                <asp:Button id="edit_btn" runat="server" Text="View/Modify" OnClick="edit_btn_Click" />
            </form><br /><br />
            <div id="footer">
		    <font size="2">Last updated: April 16, 2018<br />
                    Western Michigan University<br />Haworth College of business<br />CIS 4990 Enterprise Project
                    Tool Designed by Dr. Matthew Ross.  Project for Dr. Muhammad Razi<br />
                    Project leads: Rachel Larson, Kelsey Hood.  Business Analysts: Justin Johnson, Kruti Patel.<br />
                    Systems Analysts: Hanen Alwafi, John Bruestle, Marzouq Albaiji.  Systems Developers: Austin Lemacks, Zack Filary, Steven Freds.<br /><br />
			</font>
		</div>
         </center>
    </div>
</body>
</html>
