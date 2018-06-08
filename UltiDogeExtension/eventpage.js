//listen to url changes
chrome.tabs.onUpdated.addListener(function (tabId, changeInfo, tab) {
	
	if(changeInfo.url!=null)
	{
		var actionurl="http://localhost:10829/home/HasDealsInSite?url="+changeInfo.url+"&userId=T.B.D";


	   var currentUrl = changeInfo.url;
	   var currentUserId="";

	   $.ajax(
		   {
			   url: actionurl,
			   //data: JSON.stringify({
			   //userId:currentUserId,						   
			   //	   url:currentUrl,
			   //}),
			   contentType: "application/json; charset=utf-8",
			   dataType: "json",
			   type: "GET",
			   success: function (result) {
				   if(result!='No Discount'){
						chrome.notifications.create(
							'name-for-notification',{   
							type: 'basic', 
							iconUrl: "https://pics.me.me/fat-doge-8386016.png", 
							title: "Discount..Woof Woof", 
							message: result
							},
						function() {} 
						);						
				   }
			   }
		   });

		
	}
});

