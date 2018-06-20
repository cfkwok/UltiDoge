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
            ViewBag.Message = "Your application description page.";
            List<DealsModel> showingList = new List<DealsModel>();
            bool giftCardSatisfied = giftCardOption == 0;
            bool charitySatisfied = charityOption == 0;
            bool dealsSatisfied = dealsOption == 0;

            if (giftCardSatisfied && charitySatisfied && dealsSatisfied)
            {
                showingList.Add(new DealsModel()
                {
                    TypeOfDeal = string.Empty
                });

                return Json(showingList, JsonRequestBehavior.AllowGet);
            }

            var collection = context.db.GetCollection<DealsModel>("Benefits");
            var userBenefits = collection.Find(x => x.UserId == userId).ToList();

            List<DealsModel> mainMatchPerks = GetMainMatchPerks(url, userBenefits);

            if (giftCardOption >= 1)
            {
                var giftCardMatch = mainMatchPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Gift Card"));
                if (giftCardMatch != null)
                {
                    showingList.Add(giftCardMatch);
                    giftCardSatisfied = true;
                }
            }

            if (charityOption >= 1)
            {
                var charityMatch = mainMatchPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Charity"));
                if (charityMatch != null)
                {
                    showingList.Add(charityMatch);
                    charitySatisfied = true;
                }
            }

            if (dealsOption >= 1)
            {
                var dealsMatch = mainMatchPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Discount"));
                if (dealsMatch != null)
                {
                    showingList.Add(dealsMatch);
                    dealsSatisfied = true;
                }
            }

            if (giftCardSatisfied && charitySatisfied && dealsSatisfied)
            {
                if (showingList.Count == 0)
                {
                    showingList.Add(new DealsModel()
                    {
                        TypeOfDeal = string.Empty
                    });
                }

                return Json(showingList, JsonRequestBehavior.AllowGet);
            }

            // One or more category is still not satisfied. Get relevant sites
            List<DealsModel> relevantPerks = GetRelevantPerks(url, userBenefits);

            if (giftCardOption == 2 && !giftCardSatisfied)
            {
                var giftCardMatch = relevantPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Gift Card"));
                if (giftCardMatch != null)
                {
                    showingList.Add(giftCardMatch);
                }
            }

            if (charityOption == 2 && !charitySatisfied)
            {
                var charityMatch = relevantPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Charity"));
                if (charityMatch != null)
                {
                    showingList.Add(charityMatch);
                }
            }

            if (dealsOption == 2 && !dealsSatisfied)
            {
                var dealsMatch = relevantPerks.FirstOrDefault(x => x.TypeOfDeal.Equals("Discount"));
                if (dealsMatch != null)
                {
                    showingList.Add(dealsMatch);
                }
            }

            if (showingList.Count == 0)
            {
                showingList.Add(new DealsModel()
                {
                    TypeOfDeal = string.Empty
                });
            }

            return Json(showingList, JsonRequestBehavior.AllowGet);
        }

        private List<DealsModel> GetRelevantPerks(string url, List<DealsModel> userBenefits)
        {
            List<DealsModel> dealModels = new List<DealsModel>();
            string userId = userBenefits[0]?.UserId;

            var domainAddress = GetDomainOnly(url);
            var similarSites = GetSimilarSites(domainAddress);


            foreach (DealsModel userBenefit in userBenefits)
            {
                foreach (string perk in userBenefit.Perks)
                {
                    if (similarSites.Contains(perk))
                    {
                        var message = $"{userBenefit.TypeOfDeal} found for similar website in your perk list. {perk} ";
                        Tuple<string, string> userAndMessage =
                            new Tuple<string, string>(userId, message);

                        if (!popupTimers.ContainsKey(userAndMessage))
                        {
                            popupTimers.Add(userAndMessage, DateTime.Now);
                            dealModels.Add(new DealsModel()
                                {
                                    TypeOfDeal = userBenefit.TypeOfDeal,
                                    Message = message,
                                    OnClickUrl = userBenefit.OnClickUrl
                                }
                            );
                        }
                        else if (popupTimers[userAndMessage].AddSeconds(10) < DateTime.Now)  // If this time has passed, then add the popup in list again
                        {
                            dealModels.Add(new DealsModel()
                                {
                                    TypeOfDeal = userBenefit.TypeOfDeal,
                                    Message = message,
                                    OnClickUrl = userBenefit.OnClickUrl
                                }
                            );
                            popupTimers[userAndMessage] = DateTime.Now;
                        }
                    }
                }
            }

            return dealModels;
        }

        private List<DealsModel> GetMainMatchPerks(string url, List<DealsModel> userBenefits)
        {
            List<DealsModel> dealModels = new List<DealsModel>();
            string userId = userBenefits[0]?.UserId;

            foreach (DealsModel userBenefit in userBenefits)
            {
                foreach (string perk in userBenefit.Perks)
                {
                    if (url.Contains(perk))
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

            return dealModels;
        }

        [HttpGet]
        public ActionResult GetCompanyDiscounts()
        {
            var collection = context.db.GetCollection<DealsModel>("Benefits");
            var userBenefits = collection.Find(_ => true).ToList().OrderBy(x => x.UserId).ThenBy(x => x.TypeOfDeal);

            ViewBag.Title = "Discounts";

            return View(userBenefits);
        }

        [HttpPost]
        public ActionResult CreateMultiple(string file)
        {
            if(!string.IsNullOrEmpty(file))
            {

            }
            return Redirect("/Home/GetCompanyDiscounts");
        }

        [HttpPost]
        public ActionResult CreateDeal(string userid, string typeOfDeal, string onClickUrl, string message, string perks)
        {
            var newDeal = new DealsModel
            {
                Id = new ObjectId(),
                UserId = userid, 
                TypeOfDeal = typeOfDeal, 
                OnClickUrl = onClickUrl, 
                Message = message
            };

            var dealPerks = perks.Split(',').ToList();
            newDeal.Perks = dealPerks;

            var collection = context.db.GetCollection<DealsModel>("Benefits");

            collection.InsertOne(newDeal);

            return Redirect("/Home/GetCompanyDiscounts");
        }

        [HttpGet]
        [ActionName("DeleteDeal")]
        public ActionResult DeleteDeal(string id)
        {
            try
            {
                var objectId = new ObjectId(id);

                var collection = context.db.GetCollection<DealsModel>("Benefits");
                collection.DeleteOne(x => x.Id == objectId);
            }
            catch { }
            return Redirect("/Home/GetCompanyDiscounts");
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
        public ActionResult GetAmazonPage(string userId, string url)
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetHertzPage(string userId, string url)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStarbucksGiftCardPage(string userId, string url)
        {
            return View();
        }

        public ActionResult GetAmazonGiftCardPage(string userId, string url)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRedCrossPage(string userId, string url)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetTeslaCharityPage(string userId, string url)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetTeslaGiftCardPage(string userId, string url)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetTeslaDeltaPage(string userId, string url)
        {
            return View();
        }

        private List<string> GetSimilarSites(string currentSite)
        {
            var similarSites = new List<string>();
            var url = $"https://alexa.com/find-similar-sites/data?site={currentSite}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    var responseString = responseContent.ReadAsStringAsync().Result;

                    var jsonObject = JObject.Parse(responseString);
                    var amazonSimilarSiteObjects = jsonObject.First.First.Children();

                    foreach (var o in amazonSimilarSiteObjects)
                    {
                        try
                        {
                            var similarSite = o["site2"].ToString();

                            if (similarSite != currentSite)
                            {
                                similarSites.Add(similarSite);
                            }
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