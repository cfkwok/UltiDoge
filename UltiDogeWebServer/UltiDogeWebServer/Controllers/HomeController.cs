using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using UltiDogeWebServer.App_Start;
using UltiDogeWebServer.Models;

namespace UltiDogeWebServer.Controllers
{
    public class HomeController : Controller
    {
        private Dictionary<Tuple<string, string>, DateTime> popupTimers = new Dictionary<Tuple<string, string>, DateTime>();
        private MongoContext context;

        public HomeController()
        {
            context = new MongoContext();
        }

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
            userId = "chunk";  //Temp

            List<DealsModel> dealModels = new List<DealsModel>();
            ViewBag.Message = "Your application description page.";

            var userDeal = string.Empty;
            var userMessage = string.Empty;
            var userUrl = string.Empty;

            var collection = context.db.GetCollection<DealsModel>("Benefits");

            var userBenefits = collection.Find(x => x.UserId == userId).ToList();

            foreach (DealsModel userBenefit in userBenefits)
            {
                foreach (string perk in userBenefit.Perks)
                {
                    if (url.ToLower().Contains(perk))
                    {
                        dealModels.Add(userBenefit);

//                        jsonModels.Add(Json(new DealsModel()
//                        {
//                            TypeOfDeal = userBenefit.TypeOfDeal,
//                            Message = userBenefit.Message,
//                            OnClickUrl = userBenefit.OnClickUrl
//                        },
//                        JsonRequestBehavior.AllowGet));
                    }
                }
            }

            foreach (DealsModel deal in dealModels)
            {
                if (userDeal.Equals(string.Empty))
                {
                    userDeal = deal.TypeOfDeal;
                    userMessage = deal.Message;
                    userUrl = deal.OnClickUrl;
                }
                else if (!userDeal.Contains(deal.TypeOfDeal))
                {
                    userDeal += $"{userDeal}, {deal.TypeOfDeal}";
                    userMessage += $"{userMessage}\n{deal.Message} ({deal.OnClickUrl})";
                }

            }
            //            if (url?.ToLower().Contains("amazon") == true)
            //            {
            //                return Json(new DealsModel()
            //                {
            //                    TypeOfDeal = "Discount",
            //                    Message = $"Congragulations!! Your employer offers Discount in this site. Learn More? {num}",
            //                    //IconUrl = "https://pics.me.me/fat-doge-8386016.png",
            //                    OnClickUrl = "http://localhost:10829/home/GetDealPage"
            //                },
            //                JsonRequestBehavior.AllowGet);
            //            }
            //            else if (url?.ToLower().Contains("starbucks") == true)
            //            {
            //                return Json(new DealsModel()
            //                {
            //                    TypeOfDeal = "Gift Card",
            //                    Message = $"Congragulations!! You have Gift Card in this site. Use it? {num}",
            //                    //IconUrl = "https://apprecs.org/ios/images/app-icons/256/4e/851878478.jpg",
            //                    OnClickUrl = "http://localhost:10829/home/GetGiftCardPage"
            //                },
            //                JsonRequestBehavior.AllowGet);
            //            }
            //            else if (url?.ToLower().Contains("redcross") == true)
            //            {
            //                return Json(new DealsModel()
            //                {
            //                    TypeOfDeal = "Charity",
            //                    Message = "Your donation will be matched 1:1 by employer. Donate?",
            //                    //IconUrl = "https://shibe.digital/wishing_well/assets/og_doge.png",
            //                    OnClickUrl = "http://localhost:10829/home/GetCharityPage"
            //                },
            //                JsonRequestBehavior.AllowGet);
            //            }

            Tuple<string, string> userAndMessage =
                new Tuple<string, string>(userDeal, userMessage);
            if (!popupTimers.ContainsKey(userAndMessage))
            {
                popupTimers.Add(userAndMessage, DateTime.Now);
            }
            else
            {
                popupTimers[userAndMessage] = DateTime.Now;
            }

            // Implement pop up blocker logic
            return Json(new DealsModel()
                {
                    TypeOfDeal = userDeal,
                    Message = userMessage,
                    OnClickUrl = userUrl
                },
                JsonRequestBehavior.AllowGet);
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