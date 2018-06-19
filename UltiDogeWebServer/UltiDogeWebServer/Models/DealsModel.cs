using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UltiDogeWebServer.Models
{
    public class DealsModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("category")]
        public string TypeOfDeal { get; internal set; } //"None", "GiftCard", "Discount", "Charity"

        [BsonElement("url_popup")]
        public string OnClickUrl { get; internal set; }

        [BsonElement("message")]
        public string Message { get; internal set; }

        [BsonElement("perks")]
        public List<String> Perks { get; set; }
    }
}