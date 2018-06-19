
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
						//default values
						localStorage["UltiProDealOption"]=2;
						localStorage["UltiProCharityOption"]=2;
						localStorage["UltiProGiftCardOption"]=2;	
						document.getElementById("DealRange").value=localStorage["UltiProDealOption"];
						document.getElementById("GiftCardRange").value=localStorage["UltiProGiftCardOption"];
						document.getElementById("CharityRange").value=localStorage["UltiProCharityOption"];						
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
	localStorage.removeItem("UltiProDealOption");
	localStorage.removeItem("UltiProCharityOption");
	localStorage.removeItem("UltiProGiftCardOption");
}
function logout()
{
	ResetLogin();
}

function DealChanged()
{
	//alert("Deal Changed to " + this.value);
	localStorage["UltiProDealOption"]=this.value;
}

function GiftCardChanged()
{
	localStorage["UltiProGiftCardOption"]=this.value;
}

function CharityChanged()
{
	localStorage["UltiProCharityOption"]=this.value;
}

$(document).ready(function(){

	var oauth_token = localStorage.getItem("UltiProUserId");
	document.getElementById("ultiProloginButton").addEventListener("click", loginWithUltiPro);
	document.getElementById("ultiPrologoutButton").addEventListener("click", logout);
	document.getElementById("DealRange").addEventListener("change", DealChanged);
	document.getElementById("GiftCardRange").addEventListener("change", GiftCardChanged);
	document.getElementById("CharityRange").addEventListener("change", CharityChanged);
	var dealoption_token = localStorage.getItem("UltiProDealOption");
	//default values
	if(!dealoption_token)
	{
		//alert("Reinitialize");
		localStorage["UltiProDealOption"]=2;
		localStorage["UltiProCharityOption"]=2;
		localStorage["UltiProGiftCardOption"]=2;
	}

	if(!oauth_token)
	{
		ResetLogin();		

	}
	else
	{
		document.getElementById("ultiDogeLogin").style.display = "none";					
		document.getElementById("WelcomeText").innerHTML="Welcome "+localStorage["UltiProUserName"];
		document.getElementById("ultiDogeLogout").style.display = "block";
		
		document.getElementById("DealRange").value=localStorage["UltiProDealOption"];
		document.getElementById("GiftCardRange").value=localStorage["UltiProGiftCardOption"];
		document.getElementById("CharityRange").value=localStorage["UltiProCharityOption"];	
	}
});












