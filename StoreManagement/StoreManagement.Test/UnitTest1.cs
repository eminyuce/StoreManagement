using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcPaging;
using Newtonsoft.Json;
using RazorEngine;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Controllers;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories;
using Newtonsoft.Json.Linq;
using StoreManagement.Service.Services;
using File = System.IO.File;

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
            dbContext = new StoreContext(ConnectionString);

        }

        [TestMethod]
        public void TestGetRelatedContentsPartial()
        {

            var rr = new ContentRepository(new StoreContext(ConnectionString));
            var pds = new PageDesignRepository(new StoreContext(ConnectionString));
            var cat = new CategoryRepository(new StoreContext(ConnectionString));

            int categoryId = 77;
            var contentType = "news";
            var categoryTask = cat.GetCategoryAsync(categoryId);
            int take = 5;
            var relatedContentsTask = rr.GetContentByTypeAndCategoryIdAsync(9, contentType, categoryId, take);
            var pageDesignTask = pds.GetPageDesignByName(9, "RelatedContentsPartial");

          
        }

        [TestMethod]
        public void TestGetProductsCategoryIdAsync()
        {
            int storeId = 9;
            var rr = new ProductRepository(dbContext);

            var productsTask = rr.GetProductsCategoryIdAsync(storeId, null, StoreConstants.ProductType, true, 1, 25);
            Task.WaitAll(productsTask);
            Console.WriteLine(productsTask.Result.totalItemCount);

        }
        [TestMethod]
        public void TestGetPageDesignByName()
        {
            int storeId = 9;
            var pds = new PageDesignRepository(new StoreContext(ConnectionString));
            var pageDesignTask = pds.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
         
        }
        [TestMethod]
        public void TestGetStoreActiveNavigationsAsync()
        {
            int storeId = 9;
            var nav = new NavigationRepository(new StoreContext(ConnectionString));
            var pds = new PageDesignRepository(new StoreContext(ConnectionString));


            var mainMenu = nav.GetStoreActiveNavigationsAsync(storeId);
            var pageDesignTask = pds.GetPageDesignByName(storeId, "MainLayout");


        }

        [TestMethod]
        public void TestDotLiquidEngineOutput335555555()
        {
            int storeId = 9;
            var pds = new PageDesignRepository(new StoreContext(ConnectionString));
            var cat = new ProductCategoryRepository(new StoreContext(ConnectionString));

            var pageDesignTask = pds.GetPageDesignByName(storeId, "ProductCategoriesIndex");
            var pageSize = 25;
            var categories = cat.GetProductCategoriesByStoreIdAsync(storeId, StoreConstants.ProductType, true, 1, pageSize);
            var liquidHelper = new ProductCategoryHelper();
            //   liquidHelper.StoreSettings = StoreSettings;

            var dic = liquidHelper.GetCategoriesIndexPage(pageDesignTask, categories);



        }

        [TestMethod]
        public void TestDotLiquidEngineOutput335555()
        {
            int storeId = 9;
            var rr = new ProductRepository(dbContext);
            var pds = new PageDesignRepository(dbContext);
            var cat = new ProductCategoryRepository(dbContext);
            var controller = new ProductsController();


            var productsTask = rr.GetProductsCategoryIdAsync(storeId, null, StoreConstants.ProductType, true, 1, 25);
            var productsPageDesignTask = pds.GetPageDesignByName(storeId, "ProductsIndex");
            var categories1 = cat.GetProductCategoriesByStoreIdAsync(storeId, StoreConstants.ProductType, true);
            var liquidHelper = new ProductHelper();
         //   liquidHelper.StoreSettings = StoreSettings;
            var dic = liquidHelper.GetProductsIndexPage(productsTask, productsPageDesignTask, categories1);



        }

        [TestMethod]
        public void TestDotLiquidEngineOutput33()
        {
            int storeId = 9;
            var rr = new ContentRepository(dbContext);
            var pds = new PageDesignRepository(dbContext);
            var cat = new CategoryRepository(dbContext);


            var contentsTask = rr.GetContentsCategoryIdAsync(storeId, null, "blog", true, 1, 24);
            var blogsPageDesignTask = pds.GetPageDesignByName(storeId, "BlogsIndex");
            var categories = cat.GetCategoriesByStoreIdAsync(storeId, StoreConstants.BlogsType, true);
            Task.WaitAll(blogsPageDesignTask, contentsTask, categories);

            var contents = contentsTask.Result;
            var blogsPageDesign = blogsPageDesignTask.Result;
            var liquidHelper = new ContentHelper();
            //   liquidHelper.StoreSettings = StoreSettings;
            var dic = liquidHelper.GetContentsIndexPage(contentsTask, blogsPageDesignTask, categories, StoreConstants.BlogsType);

        }

      


        public static string GetImportPath()
        {
            string[] importPaths =
    {
            @"GoogleDrive", @"..\GoogleDrive", @"..\..\GoogleDrive",
    };
            string importPath = importPaths.FirstOrDefault(Directory.Exists);
            return importPath;
        }
        [TestMethod]
        public void TestLog2222()
        {
            var s = new StoreRepository(dbContext);
            var laptopBilgisayar = s.GetSingle(5); var store = laptopBilgisayar;
            // var loginSeaTechnology = s.GetSingle(9); var store = loginSeaTechnology;


            try
            {
                var scopes = new[]
                {
                    DriveService.Scope.Drive,
                    DriveService.Scope.DriveFile 
                };

                var p12File = String.Format("{0}/{1}", @"C:\Users\Yuce\Documents\GitHub\StoreManagement\StoreManagement\StoreManagement.Admin\App_Data\GoogleDrive", store.GoogleDriveCertificateP12FileName);
                //string p12File = Path.Combine(GetImportPath(), store.GoogleDriveCertificateP12FileName);
                X509Certificate2 certificate = new X509Certificate2(p12File,
                    store.GoogleDrivePassword, X509KeyStorageFlags.Exportable);
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(store.GoogleDriveServiceAccountEmail)
                    {
                        Scopes = scopes,
                        User = store.GoogleDriveUserEmail
                    }.FromCertificate(certificate));

                // Create the service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "StoreManagement"
                });

                var listReq = service.Files.List();
                FileList files = listReq.Execute();
                Console.WriteLine(files.Items.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }

        }
        [TestMethod]
        public void TestLog()
        {
            LogRepository ssss = new LogRepository(dbContext);
            var logs = ssss.GetSingle(1);
            Assert.IsNotNull(logs);

        }
        [TestMethod]
        public void GetProductCategoriesByStoreIdFromCache2()
        {
            FileManagerRepository ssss = new FileManagerRepository(dbContext);
            var m = ssss.GetFilesByGoogleImageIdArray(new string[] { "0B9lWnliAZuUdaFRwZmVLZXRYWE0", "0B9lWnliAZuUdb1RaamRITnhkbXc", "0B9lWnliAZuUdNHlKdTVaRzg3OWM" });



            Assert.IsNotNull(m);

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

            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\emin\Desktop\script\script.sql");
            String line = "";// file.ReadToEnd();
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
            var blogs = rep.GetContentByTypeAndCategoryId(5, "news", 55, "proxy");

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
            var m = rep.GetCategoryWithContentsAsync(77, 1);

            Console.WriteLine(m.Result.totalItemCount);

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
