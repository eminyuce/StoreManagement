using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.GeneralHelper
{
    public class LinkHelper
    {
        public static String GetProductLink(Product c, String categoryName )
        {
            String productDetailLink = String.Format("/Products/Product/{0}", String.Format("{2}/{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id, GeneralHelper.GetUrlSeoString(categoryName)));
            return productDetailLink.ToLower();
        }
        public static String GetContentLink(Content c, String categoryName)
        {
            String productDetailLink = String.Format("/Products/Product/{0}", String.Format("{2}/{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id, GeneralHelper.GetUrlSeoString(categoryName)));
            return productDetailLink.ToLower();
        }
        public static String GetBlogLink(Content c, String categoryName)
        {
            String productDetailLink = String.Format("/Blogs/Blog/{0}", String.Format("{2}/{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id, GeneralHelper.GetUrlSeoString(categoryName)));
            return productDetailLink.ToLower();
        }
        public static String GetImageLink(String imageActionName, String googleId, int width, int height)
        {
            String imageLink = String.Format("/Images/{0}/{1}?width={2}&height={3}", imageActionName, googleId, width, height);
            return imageLink;
        }

        public static string GetCategoryLink(BaseCategory productCategory)
        {
            String productDetailLink = String.Format("/Categories/{0}", 
                String.Format("{0}-{1}",GeneralHelper.GetUrlSeoString(productCategory.Name), productCategory.Id));

            return productDetailLink.ToLower();
        }
    }
}
