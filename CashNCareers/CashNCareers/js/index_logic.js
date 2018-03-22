$(document).ready(function() {
	//Sign up/register handling
	$('#sign_up').click(function()
	{
		$('#sign_up_div').show();
		$('#login_div').hide();
	});
	//Login handling
	$('#login').click(function()
	{
		$('#sign_up_div').hide();
		$('#login_div').show();
	});
});