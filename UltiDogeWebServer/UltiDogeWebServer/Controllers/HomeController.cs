using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UltiDogeWebServer.Models;

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
            {
                return Json(new DealsModel()
                {
                    TypeOfDeal = "Discount",
                    Message = "Congragulations!! Your employer offers Discount in this site. Learn More?",
                    IconUrl = "https://pics.me.me/fat-doge-8386016.png",
                    OnClickUrl = "http://localhost:10829/home/GetDealPage"
                },
                JsonRequestBehavior.AllowGet);
            }
            else if (url?.ToLower().Contains("starbucks") == true)
            {
                return Json(new DealsModel()
                {
                    TypeOfDeal = "Gift Card",
                    Message = "Congragulations!! You have Gift Card in this site. Use it?",
                    IconUrl = "https://apprecs.org/ios/images/app-icons/256/4e/851878478.jpg",
                    OnClickUrl = "http://localhost:10829/home/GetGiftCardPage"
                },
                JsonRequestBehavior.AllowGet);
            }
            else if (url?.ToLower().Contains("redcross") == true)
            {
                return Json(new DealsModel()
                {
                    TypeOfDeal = "Charity",
                    Message = "Your donation will be matched 1:1 by employer. Donate?",
                    IconUrl = "https://shibe.digital/wishing_well/assets/og_doge.png",
                    OnClickUrl = "http://localhost:10829/home/GetCharityPage"
                },
                JsonRequestBehavior.AllowGet);
            }
            return Json(new DealsModel() { TypeOfDeal = "None" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoginToUltiPro(string userId, string password)
        {
            //T.B.D. From db
            if (userId.ToLower() == "ramesh1263")
                return Json("Ramesh Chander", JsonRequestBehavior.AllowGet);
            return Json("Failure", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDealPage(string userId, string url)
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult GetGiftCardPage(string userId, string url)
        {
            ViewBag.Title = "Home Page";

            return View();
        }


        [HttpGet]
        public ActionResult GetCharityPage(string userId, string url)
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}