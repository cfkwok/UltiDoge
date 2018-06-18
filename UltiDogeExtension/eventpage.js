
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
				   if(result["TypeOfDeal"]!='') {
				   		iconUrl = '';
				   		if (result["TypeOfDeal"] == "Discount") {
				   			iconUrl = 'https://pics.me.me/fat-doge-8386016.png';
				   		}
				   		else if (result["TypeOfDeal"] == "Gift Card") {
				   			iconUrl = 'https://apprecs.org/ios/images/app-icons/256/4e/851878478.jpg';
				   		}
				   		else {
				   			iconUrl = 'https://shibe.digital/wishing_well/assets/og_doge.png';
				   		}

						redirectUrl=result["OnClickUrl"];
						chrome.notifications.create(
							'name-for-notification',{   
							type: 'basic', 
							iconUrl: iconUrl, 
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

