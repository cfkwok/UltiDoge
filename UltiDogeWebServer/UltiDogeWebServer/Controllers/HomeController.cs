using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UltiDogeWebServer.Controllers
{
    public class HomeController : Controller
    {
        //Tools -> Options -> Debugging -> General and turn off the setting Enable JavaScript Debugging for ASP.NET
        //Otherwise server is not starting
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult HasDealsInSite(string userId, string url)
        {
            ViewBag.Message = "Your application description page.";
            if (url?.ToLower().Contains("amazon") == true)
                return Json("Congragulations!! You have Discount in this site", JsonRequestBehavior.AllowGet);
            return Json("No Discount", JsonRequestBehavior.AllowGet);
        }

    }
}