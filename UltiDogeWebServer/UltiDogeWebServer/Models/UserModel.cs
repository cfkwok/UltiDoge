using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UltiDogeWebServer.Models
{
    public class UserModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}