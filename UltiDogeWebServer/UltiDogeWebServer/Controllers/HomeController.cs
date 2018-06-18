using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UltiDogeWebServer.Models;

namespace UltiDogeWebServer.Controllers
{
    public class HomeController : Controller
    {

        private List<string> companyDiscounts = new List<string> { "amazon.com", "themiamiheatstore.com", "papajohns.com", "starbucks.com", "costco.com", "walmart.com" };
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
            Uri uri = new Uri(url);
            string host = uri.Host.ToLower().Remove(0,4);

            var result = GetSimilarWebsites(host);
            //if(result.FirstOrDefault(s => s.Contains(host)) != null)
            //{

            //}


            ViewBag.Message = "Your application description page.";
            //if (url?.ToLower().Contains("amazon") == true)
            var site = discountWebsite(result);
            if (site != "")
            {
                return Json(new DealsModel()
                {
                    TypeOfDeal = "Discount",
                    Message = $"Congragulations!! Your employer offers Discount in {site}. Learn More?",
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

        private List<string> GetSimilarWebsites(string website)
        {
            var responseString = "";
            var url = $"https://alexa.com/find-similar-sites/data?site={website}";
            var sites = new List<string>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    responseString = responseContent.ReadAsStringAsync().Result;
                }
                var a = JObject.Parse(responseString);
                var list = a.First.First.Children();
                foreach(var i in list)
                {
                    try
                    {
                        var site2 = i["site2"].ToString();
                        sites.Add(site2);
                    }
                    catch { }
                }
                return sites;
            }
        }

        private string discountWebsite(List<string> websites)
        {
            var result = "";

            foreach(var w in websites)
            {
                var res = companyDiscounts.IndexOf(w);
                if (res > 0)
                {
                    result = companyDiscounts[res];
                    break;
                }
            }

            return result;
        }
    }
}