using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    public class ImagesController : BaseController
    {
        

        public ActionResult FetchImage(String id = "", string size = "", string contentType = "")
        {
            var dic = new Dictionary<String, String>();
            String url = "";
            url = String.Format("https://docs.google.com/uc?id={0}", id);


            byte[] imageData = GeneralHelper.GetImageFromUrl(url, dic);
            //  var cacheControl = dic.Where(pair => pair.Key.Contains("Cache-Control")).Select(pair => pair.Value).FirstOrDefault();
            //  var expired = dic.Where(pair => (pair.Key.Contains("Expired") || pair.Key.Contains("Expires"))).Select(pair => pair.Value).FirstOrDefault();
            //  var contentType = dic.Where(pair => pair.Key.Contains("ContentType")).Select(pair => pair.Value).FirstOrDefault();



            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(30));
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetMaxAge(new TimeSpan(30, 0, 0, 0));

            //if (!String.IsNullOrEmpty(cacheControl))
            //{
            //     Response.Headers.Add("Cache-Control", cacheControl);
            //}
            //if (!String.IsNullOrEmpty(expired))
            //{
            //    Response.Headers.Add("Expired", expired);
            //}

            if (!String.IsNullOrEmpty(contentType))
            {
                return File(imageData, contentType);
            }
            else
            {
                return File(imageData, "image/png");
            }


        }


        public void ThumbnailWithGoogleId(String googleId, int width = 60, int height = 60)
        {
            String url = "";
            var dic = new Dictionary<String, String>();
            url = String.Format("https://docs.google.com/uc?id={0}", googleId);
            byte[] imageData = GeneralHelper.GetImageFromUrl(url, dic);

            new WebImage(imageData)
                    .Resize(width, height, false, true) // Resizing the image to 100x100 px on the fly...
                    .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                    .Write();
        }

        public void Thumbnail(int id, int storeId, int width = 60, int height = 60)
        {
            var dic = new Dictionary<String, String>();
            // Loading photos’ info from database for specific image...
            var file = FileManagerService.GetFilesByStoreIdFromCache(storeId).FirstOrDefault(r => r.Id == id);
            String url = String.Format("https://docs.google.com/uc?id={0}", file.GoogleImageId);
            byte[] imageData = GeneralHelper.GetImageFromUrl(url, dic);

            new WebImage(imageData)
                    .Resize(width, height, false, true) // Resizing the image to 100x100 px on the fly...
                    .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
                    .Write();


        }
        /// <summary>
        /// Reference this in HTML as <img src="/Photo/WatermarkedImage/{ID}" />
        /// Simplistic example supporting only jpeg images.
        /// </summary>
        /// <param name="ID">Photo ID</param>
        public ActionResult WatermarkedImage(int id)
        {
            // Attempt to fetch the photo record from the database using Entity Framework 4.2.
            var file = FileManagerService.GetFilesById(id);

            if (file != null) // Found the indicated photo record.
            {
                var dic = new Dictionary<String, String>();
                // Create WebImage from photo data.
                // Should have 'using System.Web.Helpers' but just to make it clear...
                String url = String.Format("https://docs.google.com/uc?id={0}", file.GoogleImageId);
                byte[] imageData = GeneralHelper.GetImageFromUrl(url, dic);
                var wi = new System.Web.Helpers.WebImage(imageData);

                // Apply the watermark.
                wi.AddTextWatermark("EMIN YUCE");

                // Extract byte array.
                var image = wi.GetBytes("image/jpeg");

                // Return byte array as jpeg.
                return File(image, "image/jpeg");
            }
            else // Did not find a record with passed ID.
            {
                return null; // 'Missing image' icon will display on browser.
            }
        }
        public void SelectPicture()
        {
            WebImage img = new WebImage("~/images/Image1.jpg");
            img.Resize(200, 200);
            img.FileName = "Image1.jpg";
            img.Write();
        }
        public void SelectPicture1()
        {
            WebImage img = new WebImage("~/images/Image2.jpg");
            img.Resize(250, 250);
            img.FileName = "Image2.jpg";
            img.Write();
        }
        public void SelectPicture2()
        {
            WebImage img = new WebImage("~/images/Desert.jpg");
            img.Resize(200, 200);
            img.FileName = "Desert.jpg";
            img.RotateLeft();
            img.Write();
        }


        //controller function to generate image
        //returns image file (through url.action in img src attribute)
        public ActionResult GetCaptcha(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question 
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer 
            Session["Captcha" + prefix] = a + b;

            //image stream 
            FileContentResult img = null;

            try
            {
                img = this.File(CaptchaHelper.GenerateCaptchaImg(captcha, true), "image/Jpeg");
            }
            catch (Exception e)
            {
            }

            return img;
        }
    }
}