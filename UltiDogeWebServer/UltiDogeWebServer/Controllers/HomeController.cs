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
        private static Dictionary<Tuple<string, string>, DateTime> popupTimers = new Dictionary<Tuple<string, string>, DateTime>();
        private MongoContext context = new MongoContext();
        public int minuteTimeout = 5;

        public HomeController()
        {
//            context = new MongoContext();
//            popupTimers = new Dictionary<Tuple<string, string>, DateTime>();
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

            List<ActionResult> jsonModels = new List<ActionResult>();
            ViewBag.Message = "Your application description page.";

            var collection = context.db.GetCollection<DealsModel>("Benefits");

            var userBenefits = collection.Find(x => x.UserId == userId).ToList();

            foreach (DealsModel userBenefit in userBenefits)
            {
                foreach (string perk in userBenefit.Perks)
                {
                    if (url.ToLower().Contains(perk))
                    {
                        Tuple<string, string> userAndMessage =
                            new Tuple<string, string>(userId, userBenefit.Message);

                        if (!popupTimers.ContainsKey(userAndMessage))
                        {
                            popupTimers.Add(userAndMessage, DateTime.Now);
                            jsonModels.Add(Json(new DealsModel()
                                {
                                    TypeOfDeal = userBenefit.TypeOfDeal,
                                    Message = userBenefit.Message,
                                    OnClickUrl = userBenefit.OnClickUrl
                                },
                                JsonRequestBehavior.AllowGet));
                        }
                        else
                        {
                            // If this time has passed, then add the popup in list again
                            if (popupTimers[userAndMessage].AddSeconds(10) < DateTime.Now)
                            {
                                jsonModels.Add(Json(new DealsModel()
                                    {
                                        TypeOfDeal = userBenefit.TypeOfDeal,
                                        Message = userBenefit.Message,
                                        OnClickUrl = userBenefit.OnClickUrl
                                    },
                                    JsonRequestBehavior.AllowGet));

                                popupTimers[userAndMessage] = DateTime.Now;
                            }
                        }
                    }
                }
            }

            return jsonModels.Count == 0
                ? Json(new DealsModel()
                    {
                        TypeOfDeal = string.Empty
                    },
                    JsonRequestBehavior.AllowGet)
                : jsonModels[0];
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