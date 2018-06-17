using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltiDogeWebServer.Models
{
    public class DealsModel
    {
        public string TypeOfDeal { get; internal set; } //"None", "GiftCard", "Discount", "Charity"
        public string IconUrl { get; internal set; }
        public string OnClickUrl { get; internal set; }
        public string Message { get; internal set; }
    }
}