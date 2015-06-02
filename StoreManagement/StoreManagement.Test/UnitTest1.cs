using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;
using Newtonsoft.Json.Linq;
using StoreManagement.Service.Services;

namespace StoreManagement.Test
{
    [TestClass]
    public class UnitTest1
    {
        private const String ConnectionString = "Stores";
        private StoreContext dbContext;
        [TestInitialize]
        public void MyTestInitialize()
        {
            //dbContext = new StoreContext(ConnectionString);
        }
        [TestMethod]
        public void TestApiCall2()
        {
            var s = new CategoryService("yuce.marinelink.org");
            var m = s.GetCategoriesByStoreId(1);
            foreach (var q in m)
            {
                Console.WriteLine(q.Id);
            }
        }


        [TestMethod]
        public void TestApiCall()
        {
            var s = new NavigationService("yuce.marinelink.org");
            var m = s.GetStoreNavigations(1);
            foreach (var q in m)
            {
                Console.WriteLine(q.Id);
            }
        }

        [TestMethod]
        public void ParseJson()
        {
            String json = File.ReadAllText(@"C:\nLogs\logs\sssss.txt");
            JObject results = JObject.Parse(json);
 
            foreach (var result in results["styleArray"])
            {
                string id = (string) result["Id"];
                string style = (string)result["Style"];

                Console.WriteLine(id);
            }

        }

        [TestMethod]
        public void DomainName()
        {
            var storeRep = new StoreRepository(dbContext);
            var store = storeRep.GetStoreByDomain("login.maritimejobs.mobi");

            Assert.IsNotNull(store);

        }
        [TestMethod]
        public void TestMethod2()
        {
            ContentRepository rep = new ContentRepository(dbContext);
            foreach (var s in rep.GetAll())
            {
                Console.WriteLine(s.StoreId);
            }

        }
        [TestMethod]
        public void TestMethod4()
        {
            ContentRepository rep = new ContentRepository(dbContext);
            var c = rep.GetSingle(1);

            Assert.IsNotNull(c);
        }
        [TestMethod]
        public void TestMethod3()
        {
            String mm = "%25c3%2587ek_cumhur%25c4%25b0yet%25c4%25b0";
            String mm1 = "OTOMOTİV";
            String pp = mm1.UrlEncode();
            Console.WriteLine(GeneralHelper.UrlDencode(mm,false));
            Console.WriteLine(mm.UrlDecode());
        }

        [TestMethod]
        public void TestMethod1()
        {
            StoreUserRepository storeRepository = new StoreUserRepository(dbContext);
            foreach (var s in storeRepository.GetAll())
            {
                Console.WriteLine(s.StoreId);
            }

        }
    }
}
