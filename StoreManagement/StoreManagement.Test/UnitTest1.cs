using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using GenericRepository.EntityFramework.Enums;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcPaging;
using Newtonsoft.Json;
using RazorEngine;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Controllers;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories;
using Newtonsoft.Json.Linq;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Service.Services;
using File = System.IO.File;

namespace StoreManagement.Test
{
    [TestClass]
    [DeploymentItem("EntityFramework.SqlServer.dll")]
    public class UnitTest1
    {
        private const String ConnectionString = "Stores";
        private StoreContext dbContext;
        [TestInitialize]
        public void MyTestInitialize()
        {
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            //  dbContext = new StoreContext(ConnectionString);

        }

        [TestMethod]
        public void GetContentsCategoryIdAsyncSearch()
        {

            int? categoryId = null;
            int StoreId = 9;
            IContentService rep2 = new ContentRepository(new StoreContext(ConnectionString));

            // (int storeId, int? catId, string type, int page, int pageSize, bool ? isActive, string contentType);
            var blogsTask = rep2.GetContentsByContentKeywordAsync(
                StoreId,
                categoryId,
                StoreConstants.BlogsType,
                1,
                50,
                true,
                "main");

            Task.WhenAll(blogsTask).Wait();

            Console.Write(blogsTask.Result.Count);
        }

        [TestMethod]
        public void DeleteBrandsWithoutProducts()
        {
            IProductRepository rep = new ProductRepository(new StoreContext(ConnectionString));
            IBrandRepository brandService = new BrandRepository(new StoreContext(ConnectionString));
            var brands = brandService.GetBrandsByStoreId(53, "");
            var products = rep.GetProductsByStoreId(53, "");
            List<int> ints = new List<int>();
            foreach (var brand in brands)
            {
                if (products.All(r => r.BrandId != brand.Id))
                {
                    Console.WriteLine(brand.Name);
                    ints.Add(brand.Id);
                }
            }
            Console.WriteLine(String.Join(",", ints));
            //foreach (var brandId in ints)
            //{
            //    var brand = brandService.GetSingle(brandId);
            //    brandService.Delete(brand);
            //    brandService.Save();
            //}
        }

        [TestMethod]
        public void Test33335555()
        {

            int? categoryId = null;
            int StoreId = 9;
            IProductService rep = new ProductRepository(new StoreContext(ConnectionString));
            IContentService rep2 = new ContentRepository(new StoreContext(ConnectionString));
            IPageDesignService rep3 = new PageDesignRepository(new StoreContext(ConnectionString));


            var list = rep.GetProductsByBrandAsync(StoreId, 5, 100, null);
            var pageDesignTask = rep3.GetPageDesignByName(StoreId, "HomePage");
            var blogsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.BlogsType, 5);
            var newsTask = rep2.GetMainPageContentsAsync(StoreId, categoryId, StoreConstants.NewsType, 5);

            Task.WhenAll(list, pageDesignTask, blogsTask, newsTask).Wait();

            Console.WriteLine(list.Result.Count);
            Console.WriteLine(pageDesignTask.Result.Id);
            Console.WriteLine(blogsTask.Result.Count);
            Console.WriteLine(newsTask.Result.Count);

        }


        [TestMethod]
        public void Test333344466()
        {

            StoreRepository rep = new StoreRepository(new StoreContext(ConnectionString));
            var m = rep.GetStoreIdByDomainAsync("login.seatechnologyjobs.com");
            Task.WaitAny(m);
            Console.Write(m.Result.Id);
        }
        [TestMethod]
        public void Test3333444()
        {
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            ProductRepository rep = new ProductRepository(new StoreContext(ConnectionString));
            var m = rep.GetFirstAsync(r => r.StoreId, OrderByType.Descending);
            Task.WaitAll(m);
            Console.Write(m.Result.Id);
        }
        [TestMethod]
        public void Test3333()
        {
            var x = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            ProductRepository rep = new ProductRepository(new StoreContext(ConnectionString));
            var m = rep.CountAsync(r => r.StoreId == 9);
            Task.WaitAll(m);
            Console.Write(m.Result);
        }

        [TestMethod]
        public void TestGetProductsCategoryIdAsync()
        {
            int storeId = 9;
            var rr = new ProductRepository(new StoreContext(ConnectionString));

            var productsTask = rr.GetProductsCategoryIdAsync(storeId, null, StoreConstants.ProductType, true, 1, 25, "", "");
            Task.WaitAll(productsTask);
            Console.WriteLine(productsTask.Result.totalItemCount);

        }
        /*
         
         
         
SELECT  COunt(Products.Id)
FROM         ProductFiles AS p RIGHT OUTER JOIN
                      Products ON p.ProductId = Products.Id
WHERE     (p.Id IS NULL)
         * 
         */
        [TestMethod]
        public void FileManagerProductImages()
        {
            var db = dbContext = new StoreContext(ConnectionString);
            IFileManagerService rep3 = new FileManagerRepository(db);
            var files = rep3.GetFilesByStoreId(53);
            IProductService rep = new ProductRepository(db);
            IProductFileRepository rep2 = new ProductFileRepository(db);
            IProductCategoryService repCategory = new ProductCategoryRepository(new StoreContext(ConnectionString));
            var categoriesTask = repCategory.GetProductCategoriesByStoreId(53);
            foreach (var c in categoriesTask)
            {
                var products = rep.GetProductByTypeAndCategoryId(53, "product", c.Id);
                foreach (var p in products)
                {
                    try
                    {
                        var productList = p.ProductFiles;
                        if (productList != null && !productList.Any())
                        {
                            try
                            {
                                Product p1 = p;
                                if (!String.IsNullOrEmpty(p1.Name))
                                {
                                    var productFile = files.Where(r => !String.IsNullOrEmpty(r.OriginalFilename) && r.OriginalFilename.Contains(p1.Name));
                                    foreach (var pf in productFile)
                                    {
                                        try
                                        {
                                            var item = new ProductFile();
                                            item.FileManagerId = pf.Id;
                                            item.ProductId = p.Id;
                                            item.IsMainImage = true;
                                            rep2.Add(item);
                                            rep2.Save();

                                        }
                                        catch (Exception ex)
                                        {
                                            String mm = ex.Message;  
                                   
                                        }
                                      

                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                String mm = e.Message;

                            }


                        }

                    }
                    catch (Exception m)
                    {
                        String mm = m.Message;  

                    }
                }
            }
        }





        [TestMethod]
        public void TestProductRepository2()
        {
            IFileManagerService rep3 = new FileManagerRepository(new StoreContext(ConnectionString));
            var fileManagerTask = rep3.GetImagesByStoreIdAsync(9, true);
            IProductCategoryService rep2 = new ProductCategoryRepository(new StoreContext(ConnectionString));
            var categoriesTask = rep2.GetProductCategoriesByStoreIdAsync(9, "products", true);
            IProductService rep = new ProductRepository(new StoreContext(ConnectionString));
            var m = rep.GetMainPageProductsAsync(9, 5);
            Task.WaitAll(m, categoriesTask, fileManagerTask);

            Console.WriteLine(m.Result.Count);
            Console.WriteLine(categoriesTask.Result.Count);
            Console.WriteLine(fileManagerTask.Result.Count);
        }

        [TestMethod]
        public void TestProductRepository()
        {
            var db = dbContext = new StoreContext(ConnectionString);
            IFileManagerService rep3 = new FileManagerRepository(db);
            var fileManagerTask = rep3.GetImagesByStoreIdAsync(9, true);
            IProductCategoryService rep2 = new ProductCategoryRepository(db);
            var categoriesTask = rep2.GetProductCategoriesByStoreIdAsync(9, "products", true);
            IProductService rep = new ProductRepository(db);
            var m = rep.GetMainPageProductsAsync(9, 5);
            Task.WaitAll(m, categoriesTask, fileManagerTask);

            Console.WriteLine(m.Result.Count);
            Console.WriteLine(categoriesTask.Result.Count);
            Console.WriteLine(fileManagerTask.Result.Count);
        }

        [TestMethod]
        public void TestNavigationRepositorys()
        {
            INavigationService rep = new NavigationRepository(new StoreContext(ConnectionString));
            var m = rep.GetStoreActiveNavigationsAsync(9);
            Task.WaitAll(m);

            Console.Write(m.Result.Count);

        }
        [TestMethod]
        public void RemoveNewLines()
        {
            String m = "Bru Joy Travel Pillow Neck Inflatable Rosy Red Best Travel Head Rest Kids Adults - Blow Up Get Desired Support for Airplanes Camping Sleeping Car Driving Home Office Snaps Bath Spa - Premium Quality - Super Soft - Convertible 2 in 1 U Shaped Waterproof Washable with a Deluxe Pouch - No More Neck Pain - SAVE EXTRA 10% + FREE SHIPPING For 3 and More, Blue Or Red";
            Console.Write(m.ToStr(0, 200));
        }


        [TestMethod]
        public void TestSettingRepository2()
        {
            String key = "HomePageMainNewsContents_ItemsNumber \t";
            var log = new SettingRepository(new StoreContext(ConnectionString));
            var item = log.GetStoreSettingsFromCacheAsync(9);
            Task.WaitAll(item);
            var i = item.Result.FirstOrDefault(r => r.SettingKey.RemoveTabNewLines().Equals(key.RemoveTabNewLines(), StringComparison.InvariantCultureIgnoreCase));
            if (i != null)
            {
                Console.Write(i.SettingValue);
            }
        }

        [TestMethod]
        public void TestSettingRepository()
        {
            String key = "HomePageMainNewsContents_ItemsNumber \t";
            var log = new SettingRepository(new StoreContext(ConnectionString));
            var item = log.GetStoreSettingsFromCache(9).FirstOrDefault(r => r.SettingKey.RemoveTabNewLines().Equals(key.RemoveTabNewLines(), StringComparison.InvariantCultureIgnoreCase));
            if (item != null)
            {
                Console.Write(item.SettingValue);
            }
        }


        [TestMethod]
        public void TestGetBrandAsync()
        {
            var log = new BrandRepository(new StoreContext(ConnectionString));
            var mm = log.GetSingleAsync(5);
            Task.WaitAll(mm);
            var resultLabels = mm.Result;
        }

        [TestMethod]
        public void TestProductCategoryRepository()
        {
            var db = new StoreContext(ConnectionString);
            db.Configuration.LazyLoadingEnabled = false;
            var log = new ProductCategoryRepository(db);
            var mm = log.GetCategoriesByRetailerIdAsync(52, 478);
            Task.WaitAll(mm);
            var resultLabels = mm.Result;

            Console.WriteLine(resultLabels.Count);
        }


        [TestMethod]
        public void TestLabelRepository()
        {
            var log = new LabelRepository(new StoreContext(ConnectionString));
            var mm = log.GetLabelsByItemTypeId(9, 225, "product");
            Task.WaitAll(mm);
            var resultLabels = mm.Result;
        }


        [TestMethod]
        public void TestStoreLanguageRepositoryGetSingle()
        {
            var log = new StoreLanguageRepository(new StoreContext(ConnectionString));
            var storeLanguange = new StoreLanguage();
            storeLanguange.LanguageCode = "tr-test";
            storeLanguange.Name = "Turkce";
            storeLanguange.UpdatedDate = DateTime.Now;
            storeLanguange.CreatedDate = DateTime.Now;
            storeLanguange.State = true;
            storeLanguange.StoreId = 9;
            storeLanguange.Ordering = 1;
            log.Add(storeLanguange);
            log.Save();
        }

        [TestMethod]
        public void TestGetFiles()
        {
            String path = @"C:\Users\Yuce\Documents\GitHub\StoreManagement\StoreManagement\StoreManagement.Admin\Views";
            path = @"C:\tttt";
            var allFiles = Directory.GetFiles(path, "*.cshtml", SearchOption.AllDirectories);
            foreach (var allFile in allFiles)
            {

                if (allFile.Replace(path, "").ToLower().Contains("saveoredit"))
                {

                    string readText = File.ReadAllText(allFile);
                    Console.WriteLine(readText);
                }
            }
        }
        [TestMethod]
        public void TestLogRepositoryGetSingle()
        {
            var log = new LogRepository(new StoreContext(ConnectionString));
            log.GetSingle(249059);
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
            var relatedContentsTask = rr.GetContentByTypeAndCategoryIdAsync(9, contentType, categoryId, take, 0);
            var pageDesignTask = pds.GetPageDesignByName(9, "RelatedContentsPartial");


        }


        [TestMethod]
        public void TestSettingRepository1()
        {
            var cat = new SettingRepository(new StoreContext(ConnectionString));
            var m = cat.GetStoreSettingsByType(9, "", "blogs");
            Console.WriteLine(m.Count);
        }

        [TestMethod]
        public void TestGetImageFromUrlFromCache()
        {

            String url = String.Format("https://docs.google.com/uc?id={0}", "0B9lWnliAZuUdaEFkemRzV05yUWs");
            var mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);
            mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);
            mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);
            mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);
            mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);
            mmm = GeneralHelper.GetImageFromUrlFromCache(url, new Dictionary<string, string>(), 100);

        }

        [TestMethod]
        public void TestGetMainPageProductsAsync()
        {
            int storeId = 9;
            var rr = new ProductRepository(new StoreContext(ConnectionString));

            var productsTask = rr.GetMainPageProductsAsync(storeId, 5);
            Task.WaitAll(productsTask);
            Console.WriteLine(productsTask.Result.Count);

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
            var m1 = s.GetCategoriesByStoreIdAsync(9, StoreConstants.NewsType, true);
            Task.WaitAll(m1);
            var m = m1.Result;
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

            String rr = @"\\WEBDEVELOPERS15\Projects\SalesMailTracking\TestData\Inbount\yuce@marinelink.com.IMAP";
            DirectoryInfo di = new DirectoryInfo(rr);
            FileInfo[] smFiles = di.GetFiles("*.msg");
            foreach (FileInfo fi in smFiles.Take(1))
            {
                String lineOfText = "";
                String textFilePath = rr + "\\" + fi.Name;
                var filestream = new FileStream(textFilePath,
                                              FileMode.Open,
                                              FileAccess.Read,
                                              FileShare.ReadWrite);
                var file = new StreamReader(filestream, System.Text.Encoding.UTF8, true, 128);
                StringBuilder builder = new StringBuilder();
                while ((lineOfText = file.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(lineOfText))
                    {
                        break;
                    }
                    builder.AppendLine(lineOfText);

                }
                Console.WriteLine(fi.Name);
                Console.WriteLine(builder);
                var tt = GetSearchLine(builder.ToString(), "To");
                Console.WriteLine(tt);
            }

        }

        [TestMethod]
        public void DownloadMaritimeReporter()
        {
            var myFile = new System.IO.StreamReader(@"\\WEBDEVELOPERS15\Projects\BpaNewsLetter\MaritimePropulsion\MaritimePropulsion.txt");
            string myString = myFile.ReadToEnd();
            using (StringReader reader = new StringReader(myString))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string url = line;
                        var dictionary = new Dictionary<String, String>();
                        var pdfFile = RequestHelper.GetImageFromUrl(line, dictionary);
                        String fileName = "";
                        if (dictionary.ContainsKey("Content-Disposition"))
                        {
                            fileName = dictionary["Content-Disposition"].ToStr().Split(";".ToCharArray())[1].Replace("filename=", "");

                        }
                        File.WriteAllBytes(String.Format(@"\\WEBDEVELOPERS15\Projects\BpaNewsLetter\MaritimePropulsion\{0}", fileName), pdfFile);
                    }
                    catch (Exception ex)
                    {


                    }


                }
            }

        }

        public String GetSearchLine(String sourceString, String key)
        {
            using (StringReader reader = new StringReader(sourceString))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(key))
                    {
                        return line;
                    }
                }
            }

            return "";
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
        public void DovizliAskerlik22()
        {
            var dt = new DateTime(2012, 8, 20);
            var dt2 = new DateTime(2013, 3, 8);
            var u = dt2.Subtract(dt).Days;
            var dt3 = new DateTime(2013, 5, 14);
            var dt4 = DateTime.Now;
            var s = dt4.Subtract(dt3).Days;
            Console.WriteLine("Intersoft:" + u);
            Console.WriteLine("Maritime: " + s);

            //Turkiye ablamin tatili
            var hDt = new DateTime(2014, 9, 11);
            var hDt2 = new DateTime(2014, 9, 28);
            int bahamaHoliday = 1;
            int holiday = hDt2.Subtract(hDt).Days;
            holiday += bahamaHoliday;

            //Aile ziyareti
            hDt = new DateTime(2015, 11, 12);
            hDt2 = new DateTime(2015, 11, 27);
            holiday += hDt2.Subtract(hDt).Days;

            Console.WriteLine("Holiday: " + holiday);

            int total = (s + u) - holiday;
            int left = (1095 - total);
            Console.WriteLine("Grand  Total: " + (s + u));
            Console.WriteLine("Total: " + total);
            Console.WriteLine(String.Format("Left:{0} {1}  {2}", left, left / 30, dt4.AddDays(left).ToShortDateString()));

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
