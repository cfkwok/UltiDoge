
var sharesnipTabId=0;
							
function loginWithUltiPro()
{
		var actionurl="http://localhost:10829/home/LoginToUltiPro?userId="+document.getElementById('ultiProUserName').value;

	   $.ajax(
		   {
			   url: actionurl,
			   contentType: "application/json;",
			   dataType: "json",
			   type: "GET",
			   success: function (result) {
				   if(result!="Failure")
				   {
					    //alert("Success");
						document.getElementById("ultiDogeLogin").style.display = "none";					
						document.getElementById("ultiDogeLogout").style.display = "block";
						localStorage["UltiProUserId"]=document.getElementById('ultiProUserName').value;
						localStorage["UltiProUserName"]=result;
						document.getElementById("WelcomeText").innerHTML="Welcome "+result;
				   }
				   else
				   {
					   //alert("Failure");
					   document.getElementById('ultiProUserName').value="";
					   document.getElementById('ultiProPassword').value="";
				   }

			   }
		   });	
	
}
function ResetLogin()
{
	//alert("Resetting login");
	document.getElementById("ultiDogeLogin").style.display = "bock";					
	document.getElementById("ultiDogeLogout").style.display = "none";	
	localStorage.removeItem("UltiProUserName");
	localStorage.removeItem("UltiProUserId");

}
function logout()
{
	ResetLogin();
}

$(document).ready(function(){

	var oauth_token = localStorage.getItem("UltiProUserId");
	document.getElementById("ultiProloginButton").addEventListener("click", loginWithUltiPro);
	document.getElementById("ultiPrologoutButton").addEventListener("click", logout);

	if(!oauth_token)
	{
		ResetLogin();		

	}
	else
	{
		document.getElementById("ultiDogeLogin").style.display = "none";					
		document.getElementById("WelcomeText").innerHTML="Welcome "+localStorage["UltiProUserName"];
		document.getElementById("ultiDogeLogout").style.display = "block";

	
	}
});












