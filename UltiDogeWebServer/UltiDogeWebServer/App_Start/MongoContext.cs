using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace UltiDogeWebServer.App_Start
{
    public class MongoContext
    {
        public IMongoDatabase db;
        public MongoContext()   
        {
            var MongoDatabaseName = ConfigurationManager.AppSettings["MongoDBName"]; 

            var _client = new MongoClient();
            db = _client.GetDatabase(MongoDatabaseName);
        }
    }
}