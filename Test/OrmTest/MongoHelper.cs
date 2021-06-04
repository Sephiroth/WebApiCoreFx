using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrmTest
{
    public class MongoHelper
    {
        private MongoClient client;
        private IMongoDatabase database;

        public MongoHelper(string url)
        {
            client = new MongoClient(url);
            database = client.GetDatabase("lut_home");
        }

    }

    public class UserInfo
    {
        public ObjectId _id;

        public string name;

        public int age;

        public string address;
    }
}