
var sharesnipTabId=0;
							
function loginWithWebpop()
{
							chrome.windows.create({
							url:'https://secure.sharefile.com/oauth/authorize?response_type=code&client_id=CwSkWnXRlhU4wFRTVy0IwY11S&redirect_uri=https%3A%2F%2Fsecure.sharefile.com%2Foauth%2Foauthcomplete.aspx&state=&requirev3=true',
							type: 'popup',
							focused: true,
							width:500,
							height:500,
							left: screen.availWidth - 506,
							// incognito, top, left, ...
						}, function(window)
						{ 
							sharesnipTabId=window.tabs[0].id;
							chrome.tabs.onUpdated.addListener(function(tabId, changeInfo, tab) {
								   if(tabId==sharesnipTabId && changeInfo.url!=null)
								   {
										var str=tab.url;
										if(str.startsWith('https://secure.sharefile.com/oauth/oauthcomplete.aspx'))
										{
											var pos2=str.indexOf('?');
											var query=str.substring(pos2+1);
											var vars = query.split('&');
											for (var i = 0; i < vars.length; i++) {
												var pair = vars[i].split('=');
												//alert(pair[0]+'-->'+pair[1]);
												if (pair[0] == 'code') {
													localStorage["access_token"] = pair[1];
													localStorage["userid"] = pair[1];
												}
												if (pair[0] == 'subdomain') {
													localStorage["subDomain"] = pair[1];
												}												
												//if(pair[0])
												//localStorage["subDomain"] = subDomain;												
											}  
											document.getElementById("shareSnipLoginHeader").style.display = "none";											
											document.getElementById("shareSniploginFormId").style.display = "none";	
											document.getElementById("shareSniperrormessageId").style.display = "none";
											chrome.tabs.remove(tabId);
									
											RetriveAndDisplayColTable(true);

										}
								   }
								}); 
						});
}


function RetriveAndDisplayColTable(refresh)
{
	var xhrFiles = new XMLHttpRequest();
	var colListurl="http://localhost:62988/home/FindAllColaborationFiles?userId="+localStorage["access_token"]+"&subdomain="+localStorage["subDomain"]+"&refresh="+refresh;
	xhrFiles.open("GET", colListurl);
	xhrFiles.send();
	xhrFiles.onreadystatechange = function()	{
					
		if (xhrFiles.readyState == 4)
		{
			//alert("state is "+xhrFiles.status);	
			if(xhrFiles.status ==0)		
			{
				document.getElementById("shareSnipLoginHeader").style.display = "none";
				document.getElementById("shareSniploginFormId").style.display = "none";	
				document.getElementById("shareSniperrormessageId").style.display = "Block";					
				document.getElementById("shareSniphomeFormId").style.display = "none";				
			}	
			else if(xhrFiles.status == 200)
			{
				document.getElementById("shareSnipLoginHeader").style.display = "none";
				document.getElementById("shareSniploginFormId").style.display = "none";	
				document.getElementById("shareSniperrormessageId").style.display = "none";					
				document.getElementById("shareSniphomeFormId").style.display = "block";

				var table=document.getElementById("shareSnipColaborationTable");

				
				var respArr = JSON.parse(xhrFiles.responseText)	;	
				//create table
				for (i = 0; i < respArr.length; i++) {
					var row=table.insertRow();
					var cell1=row.insertCell(0);
					cell1.id="TableDoc"+i;
					var cell2=row.insertCell(1);
					cell2.id="TableKeywords"+i;					
				}	
				localStorage["NumberOfColaborations"]=respArr.length;
				//insert data
				for (i = 0; i < respArr.length; i++) {
		
					var row = respArr[i];
					var tabletitleId="TableDoc"+i;
					var tablekeywordId="TableKeywords"+i;
					var buttonId="ShareSnipDocTitle"+i;
					localStorage[tabletitleId]=row["Title"];
					localStorage[tablekeywordId]=row["Keywords"];

					var buttonRow = document.createElement("button");
					buttonRow.innerHTML = row["Title"];
					buttonRow.style.background='none';
					buttonRow.style.border='none';
					buttonRow.style.font='inherit';
					buttonRow.style.margin='0';
					buttonRow.style.padding='2';
					buttonRow.style.outline='none';
					buttonRow.style.color= 'blue';
					buttonRow.style.cursor='pointer';
					buttonRow.style.textDecoration = "underline";
					buttonRow.style.outlineoffset='0'					
					document.getElementById(tabletitleId).appendChild(buttonRow);
					buttonRow.addEventListener("click", function()
					{
						var xhrDocumentFetcherUrl="http://localhost:62988/home/GetDocument?userId="+localStorage["access_token"]+"&subdomain="+localStorage["subDomain"]+"&documentName="+this.innerHTML;						
						chrome.downloads.download({
						  url: xhrDocumentFetcherUrl,
						  saveAs:true,
						});
					
						
					});					
					document.getElementById(tablekeywordId).innerHTML=row["Keywords"];
				}
			}
			else if(xhrFiles.status == 500)
			{
				ResetLogin();
			}
		}

	}	
}

function RowButtonClick(name)
{
	alert('clicked'+name);
}

function ResetLogin()
{
	document.getElementById("shareSnipLoginHeader").style.display = "block";
	document.getElementById("shareSniploginFormId").style.display = "block";
	document.getElementById("shareSniperrormessageId").style.display = "none";	
	document.getElementById("shareSniphomeFormId").style.display = "none";	
	localStorage.removeItem("access_token");
	localStorage.removeItem("userid");
	localStorage.removeItem("subDomain");
		
	for (i = 0; i < localStorage["NumberOfColaborations"]; i++) {
		var tabletitleId="TableDoc"+i;
		var tablekeywordId="TableKeywords"+i;
		localStorage.removeItem(tabletitleId);
		localStorage.removeItem(tablekeywordId);	
	}	
	localStorage.removeItem("NumberOfColaborations");		
}
function logout()
{
	console.log("logout called");
	var xhrLogout = new XMLHttpRequest();
	var logoutUrl="http://localhost:62988/home/Logout?userId="+localStorage["access_token"]+"&subdomain="+localStorage["subDomain"];
	xhrLogout.open("GET", logoutUrl);
	xhrLogout.send();
	
	//remove all rows of table except header
	var table = document.getElementById("shareSnipColaborationTable");
	while(table.rows.length > 1) {
		  table.deleteRow(1);
	}
	ResetLogin();
}

$(document).ready(function(){

	var oauth_token = localStorage.getItem("access_token");
	console.log("typeof "+ typeof oauth_token);
	console.log("Values of oauth_token is: " + oauth_token);
	document.getElementById("shareSniploginButton").addEventListener("click", loginWithWebpop);
	document.getElementById("shareSniplogoutButton").addEventListener("click", logout);

	if(!oauth_token)
	{
		ResetLogin();		

	}
	else
	{
		console.log("User already logged in. Showing log out form.");

		document.getElementById("shareSniphomeFormId").style.display = "block";
		RetriveAndDisplayColTable(false);
	
		
	}
});


// function login()/*function to check userid & password*/
// {
	// console.log("Inside login()");

	// document.getElementById("shareSnipLoginHeader").style.display = "none";
	// document.getElementById("shareSniploginFormId").style.display = "none";
	// document.getElementById("shareSniperrormessageId").style.display = "none";

	// var form = document.forms["loginForm"];
	// try
	// {
		// var access_token = null;
		// var xhr = new XMLHttpRequest();
		// var userId = form.userid.value;
		// var password = form.pswrd.value;
		// var subDomain = form.subDomain.value;
		// var url = "https://" + subDomain + ".sharefile.com/oauth/token?grant_type=password&client_id=qhRBpcI7yj931hV2wzGlmsi6b&client_secret=Nu8JDCC9EK598e4PmA2NBbF09oYBS8&username=" + userId + "&password=" + password;

		// console.log("ShareFile Url is: " + url);
		
		// xhr.open("GET", url);
		// xhr.send();

		// xhr.onerror = function(e) {
    		// console.log("Error while sending OAuth request. Status: " + e.target.status);
		// };

		// console.log(xhr.status);
		// console.log(xhr.statusText);

		// xhr.onreadystatechange = function() {
	    	// if (xhr.readyState == 4 && xhr.status == 200) {
					// console.log("readyState == 4 && status == 200")
					// var myArr = JSON.parse(xhr.responseText);
					// access_token = myArr["access_token"];
					// console.log("AccessToken: " + access_token);
					// localStorage["access_token"] = access_token;
				
					// localStorage["userid"] = userId;
					// localStorage["subDomain"] = subDomain;
					// console.log("Added accessToken to localStorage: " + localStorage["access_token"]);
					// RetriveAndDisplayColTable(true);
					


	    	// }
			// else if (xhr.readyState == 4 && xhr.status == 400)
			// {
				// console.log('Login unsuccesful. Please enter correct username/password');
				// document.getElementById("shareSnipLoginHeader").style.display = "block";
				// document.getElementById("shareSniploginFormId").style.display = "block";	
				// document.getElementById("shareSniperrormessageId").style.display = "block";				
			// }
		// }

	// }
	// catch (e)
	// {
		// console.log("Excpetion in login(): ", e);
	// }
// }











