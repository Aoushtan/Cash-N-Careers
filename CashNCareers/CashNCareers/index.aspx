<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CashNCareers.cs.WebForm1" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="author" content="Austin Lemacks">
	<meta name="description" content="">
	<title>Cash N Careers</title>
	<link rel="stylesheet" type="text/css" href="css/style.css"/>
    <script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
    <script type = "text/javascript" src="js/index_logic.js"></script>
</head>
<body>
    <div id="container">
		<div id="header">
			<hr/>	
			<center><h1 id="title">Cash N Careers</h1></center>
			<hr/>
		</div>
		<div id="body">
            <center>
            <p>Sign up or Login below!
			</p>
			<button id="sign_up">Sign Up</button>
			<button id="login">Login</button>
            <form id="log_reg_form" runat="server">
		        <div id="sign_up_div" runat="server">
                    <h3>Sign up</h3><br/>
                    Email: <asp:TextBox Id="register_user_email" runat="server"></asp:TextBox><br/>
			        Password: <asp:TextBox ID="register_user_pass" runat="server" TextMode="Password"></asp:TextBox><br />
			        <br/><br/>
			        <asp:Button id="register" runat="server" Text="Register" OnClick="register_Click"></asp:Button>
		        </div>
		        <div id="login_div" runat="server">
                    <h3>Log in</h3><br/>
			        Email: <asp:TextBox Id="login_user_email" runat="server"></asp:TextBox><br/>
			        Password: <asp:TextBox ID="login_user_pass" runat="server" TextMode="Password"></asp:TextBox><br />
			        <br/><br/>
			        <asp:Button id="login_button" runat="server" Text="Login" OnClick="login_button_Click"></asp:Button>
		        </div>
		        <br/><br />
            </form>
            <asp:Label ID="err_message" Text="" runat="server"></asp:Label>
            </center>
        </div>
		<div id="footer">
				<font size="1">Last updated: March 12, 2018</font>
		</div>
	</div>
</body>
</html>
