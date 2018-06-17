
var redirectUrl;

//listen to url changes
chrome.tabs.onUpdated.addListener(function (tabId, changeInfo, tab) {
	if(changeInfo.url!=null)
	{
		var actionurl="http://localhost:10829/home/HasDealsInSite?url="+changeInfo.url+"&userId=T.B.D";


	   var currentUrl = changeInfo.url;
	   var currentUserId="T.B.D";
	   $.ajax(
		   {
			   url: actionurl,
			   //data: urlData,
			   //userId:currentUserId,						   
			   //	   url:currentUrl,
			   //}),
			   contentType: "application/json;",
			   dataType: "json",
			   type: "GET",
			   success: function (result) {
				   //alert("success->"+JSON.stringify(result));
				   if(result["TypeOfDeal"]!='None'){
						redirectUrl=result["OnClickUrl"];
						chrome.notifications.create(
							'name-for-notification',{   
							type: 'basic', 
							iconUrl:result["IconUrl"], 
							title: result["TypeOfDeal"]+"..Woof Woof", 
							message: result["Message"]
							},
						function() {} 
						);						
				   }
			   }
		   });

		
	}
});

chrome.notifications.onClicked.addListener(function(notificationId)
	{
		chrome.notifications.clear(notificationId);
		chrome.tabs.create({url: redirectUrl},function(){})
	}
);

