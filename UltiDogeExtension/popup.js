
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
						document.getElementById("ultiDogeLogin").style.display = "none";					
						document.getElementById("ultiDogeLogout").style.display = "block";
						localStorage["UltiProUserId"]=document.getElementById('ultiProUserName').value;
						localStorage["UltiProUserName"]=result;
				   }
				   else
				   {
					   document.getElementById('ultiProUserName').value="";
					   document.getElementById('ultiProPassword').value="";
				   }

			   }
		   });	
	
/* 							chrome.windows.create({
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
						}); */
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

function ResetLogin()
{
	document.getElementById("ultiDogeLogin").style.display = "bock";					
	document.getElementById("ultiDogeLogout").style.display = "none";	
	localStorage.removeItem("UltiProUserName");
	localStorage.removeItem("UltiProUserid");

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
		document.getElementById("ultiDogeLogout").style.display = "block";
		//alert(localStorage["UltiProUserName"]);
		//alert(localStorage["UltiProUserId"]);
	
	}
});












