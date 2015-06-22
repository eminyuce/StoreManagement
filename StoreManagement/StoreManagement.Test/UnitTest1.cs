using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
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
           // dbContext = new StoreContext(ConnectionString);
        }
        [TestMethod]
        public void GetProductCategoriesByStoreIdFromCache()
        {
            ProductCategoryRepository sss = new ProductCategoryRepository(dbContext);
            var m = sss.GetProductCategoriesByStoreIdFromCache(9, StoreConstants.ProductType);
            Assert.IsNotNull(m);

        } 
        [TestMethod]
        public void InsertALlData()
        {
            ExecuteSP();
        }
        private static void ExecuteSP()
        {
            SqlConnection connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
            connection.Open();

            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\emin\Desktop\11212\11212.sql");
            String line = "";
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    SqlCommand command = new SqlCommand(line, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }
            }

            file.Close();
        }

        [TestMethod]
        public void GetStoreByUserName()
        {
            var s = new StoreRepository(dbContext);
            var m = s.GetStoreByUserName("yuce18@marinelink.com");
            Assert.IsNotNull(m);

        }
        [TestMethod]
        public void GetStoreUserRepository()
        {
            var s = new StoreUserRepository(dbContext);
            var m = s.GetStoreUserByUserId(28);
            Assert.IsNotNull(m);

        }

        [TestMethod]
        public void TestApiCall2()
        {
            var s = new CategoryService("yuce.marinelink.org");
            var m = s.GetCategoriesByStoreId(2);
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
        public void TestJsonText()
        {
            String json = File.ReadAllText(@"C:\Users\Yuce\Desktop\Providers\testJsonText.txt");
            RequestHelper r = new RequestHelper();
            var result = r.GetUrlPagedResults<Content>("http://yuce.marinelink.org/api/Contents/GetContentsCategoryId?storeId=2&categoryId=1&typeName=product&isActive=True&page=4&pageSize=25");
            Console.WriteLine(result.items.Count);
        }
        [TestMethod]
        public void ParseJson()
        {
            String json = File.ReadAllText(@"C:\nLogs\logs\sssss.txt");
            JObject results = JObject.Parse(json);

            foreach (var result in results["styleArray"])
            {
                string id = (string)result["Id"];
                string style = (string)result["Style"];

                Console.WriteLine(id);
            }

        }
        [TestMethod]
        public void TestGetContentsCategoryId()
        {
            ContentRepository rep = new ContentRepository(dbContext);
            //  StorePagedList<Content> m = rep.GetContentsCategoryId(2, 1, "product", true, 1, 25);
            var blogs = rep.GetContentByType("blog");
            var m = rep.GetContentsCategoryId(2, null, "blog", true, 1, 24);

            // var p = m.PageCount2;

            //foreach (var content in m)
            //{
            //    Console.WriteLine(content);
            //}



        }

        [TestMethod]
        public void TestContentService()
        {
            ContentService rep = new ContentService("yuce.marinelink.org");
            StorePagedList<Content> m = rep.GetContentsCategoryId(2, 1, "product", true, 1, 25);
            //var p = m.PageCount2;
            //foreach (var content in m)
            //{
            //    Console.WriteLine(content);
            //}



        }

        [TestMethod]
        public void TestEmailSender()
        {
            IEmailSender m = new EmailSender();
            var emailAccount = new EmailAccount();
            emailAccount.Username = "eminyuce@gmail.com";
            emailAccount.Email = "eminyuce@gmail.com";
            emailAccount.DisplayName = "Store Management";
            emailAccount.Password = "123.q456-*=";
            emailAccount.Host = "smtp.gmail.com";
            emailAccount.Port = 587;
            emailAccount.EnableSsl = true;
            emailAccount.UseDefaultCredentials = false;

            string subject = "Store Management Reset Password";
            string body = "Test";
            string fromAddress = "eminyuce@gmail.com";
            string fromName = "EMIN YUCE";
            string toAddress = "eminyuce@gmail.com";
            string toName = "EMIN YUCE";
            m.SendEmail(emailAccount, subject, body, fromAddress, fromName, toAddress, toName);
        }

        [TestMethod]
        public void TestGetCategoriesByStoreIds()
        {
            CategoryRepository rep = new CategoryRepository(dbContext);
            var m = rep.GetCategoryWithContents(1, 1);


        }
        [TestMethod]
        public void DomainName()
        {
            var storeRep = new StoreRepository(dbContext);
            var store = storeRep.GetStoreByDomain("login.maritimejobs.mobi");

            Assert.IsNotNull(store);

        }
        [TestMethod]
        public void TestNavigationRepositorys()
        {
            NavigationRepository rep = new NavigationRepository(dbContext);
            foreach (var s in rep.GetAll())
            {
                Console.WriteLine(s.StoreId);
            }

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
            Console.WriteLine(GeneralHelper.UrlDencode(mm, false));
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
