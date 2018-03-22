$(document).ready(function() {
	//Sign up/register handling
	$('#sign_up').click(function()
	{
		$('#sign_up_div').show();
		$('#login_div').hide();
	});
	//Sign up div area
	$('#register').click(function(){
		var email = $('#user_email_reg').val();
		var pass = $('#user_pass_reg').val();
		if ((email != "") && (pass != ""))
		{
			if (validateEmail(email))
			{
				//$('#error_message_reg
				//Code to add user to database and sign them in, then move to the next screen
			}
			else{
				$('#error_message_reg').html("Please enter a valid email address.");
			}
		}
		else{
			$('#error_message_reg').html("Please enter a user name and password.");
		}
	});
	//Login handling
	$('#login').click(function()
	{
		$('#sign_up_div').hide();
		$('#login_div').show();
	});
	$('#login_button').click(function()
	{
		var email = $('#user_email_login').val();
		var pass = $('#user_pass_login').val();
		if ((email != "") && (pass != ""))
		{
			if (checkLogin(email,pass))
			{
				//Code to sign the user in and move to the next screen
			}
			else{
				$('#error_message_log').html("Email or password incorrect.");
			}
		}
		else{
			$('#error_message_log').html("Please enter a user name and password.");
		}
	});
});
//Regex to check if an email is valid, returns true or false
function validateEmail(email)
{
	var reg = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return reg.test(String(email).toLowerCase());
}
function checkLogin(email,pass)
{
	//Function that checks against the server for this email.
	//Returns true if login is correct, else false
}