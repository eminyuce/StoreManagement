using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.GeneralHelper;

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
    }
}