using MongoDB.Driver;
using System;
using System.Text;

namespace OrmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            MongoClient client = new MongoClient("mongodb://192.168.52.128:27017");
            IMongoDatabase database = client.GetDatabase("lut_home");
            IMongoCollection<UserInfo> collection = database.GetCollection<UserInfo>("lut_home");
            var s = collection.Find(s => s.name.Equals("lut"));
            UserInfo user = s.First();
            if (user != null)
            {
                Console.WriteLine(user.name);
            }
            #endregion

            Console.Read();
        }
    }
}
