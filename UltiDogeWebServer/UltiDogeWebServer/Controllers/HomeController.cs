using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using UltiDogeWebServer.App_Start;
using UltiDogeWebServer.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

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
        public ActionResult HasDealsInSite(string userId, string url, int giftCardOption, int charityOption, int dealsOption)
        {
            List<DealsModel> dealModels = new List<DealsModel>();
            ViewBag.Message = "Your application description page.";

            var collection = context.db.GetCollection<DealsModel>("Benefits");
            var userBenefits = collection.Find(x => x.UserId == userId).ToList();

            var domainAddress = GetDomainOnly(url);
            var similarSites = GetSimilarSites(domainAddress);

            foreach (DealsModel userBenefit in userBenefits)
            {
                foreach (string perk in userBenefit.Perks)
                {
                    if (similarSites.Contains(perk))
                    {
                        Tuple<string, string> userAndMessage =
                            new Tuple<string, string>(userId, userBenefit.Message);

                        if (!popupTimers.ContainsKey(userAndMessage))
                        {
                            popupTimers.Add(userAndMessage, DateTime.Now);
                            dealModels.Add(new DealsModel()
                                {
                                    TypeOfDeal = userBenefit.TypeOfDeal,
                                    Message = userBenefit.Message,
                                    OnClickUrl = userBenefit.OnClickUrl
                                }
                            );
                        }
                        else if (popupTimers[userAndMessage].AddSeconds(10) < DateTime.Now)  // If this time has passed, then add the popup in list again
                        {
                            dealModels.Add(new DealsModel()
                                {
                                    TypeOfDeal = userBenefit.TypeOfDeal,
                                    Message = userBenefit.Message,
                                    OnClickUrl = userBenefit.OnClickUrl
                                }
                            );
                            popupTimers[userAndMessage] = DateTime.Now;
                        }
                    }
                }
            }

            if (dealModels.Count == 0)
            {
                dealModels.Add(new DealsModel()
                {
                    TypeOfDeal = string.Empty
                });
            }

            return Json(dealModels, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LoginToUltiPro(string userId, string password)
        {
            var collection = context.db.GetCollection<UserModel>("Users");
            var user = collection.Find(x => x.UserId == userId).ToList();

            if (user.Count > 0)
            {
                return Json(user[0].Name, JsonRequestBehavior.AllowGet);
            }

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

        private List<string> GetSimilarSites(string currentSite)
        {
            var similarSites = new List<string>();
            var url = $"https://alexa.com/find-similar-sites/data?site={currentSite}";
            var responseString = "";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    responseString = responseContent.ReadAsStringAsync().Result;

                    var jsonObject = JObject.Parse(responseString);
                    var amazonSimilarSiteObjects = jsonObject.First.First.Children();

                    foreach (var o in   )
                    {
                        try
                        {
                            var similarSite = o["site2"].ToString();
                            similarSites.Add(similarSite);
                        }
                        catch { }
                    }
                }
            }

            return similarSites;
        }

        private string GetDomainOnly(string url)
        {
            return new Uri(url).Host.ToLower().Remove(0,4);
        }
    }
}