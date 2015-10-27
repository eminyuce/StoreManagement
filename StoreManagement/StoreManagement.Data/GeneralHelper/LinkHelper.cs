using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.GeneralHelper
{
    public class LinkHelper
    {
        public static String GetProductLink(Product c, String categoryName)
        {
            String detailLink = String.Format("/Products/Product/{0}", String.Format("{2}/{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id, GeneralHelper.GetUrlSeoString(categoryName)));
            return detailLink.ToLowerInvariant();
        }
        public static String GetContentLink(Content c, String categoryName, String type)
        {
            String url = "/Blogs/Blog/";
            if (type.Equals(StoreConstants.NewsType))
            {
                url = "/News/Detail/";
            }
            else if (type.Equals(StoreConstants.BlogsType))
            {
                url = "/Blogs/Detail/";
            }
            String detailLink = url + String.Format("{0}", String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id));
            return detailLink.ToLowerInvariant();
        }

        public static String GetImageLink(String imageActionName, FileManager fileImage, int width, int height)
        {

            String imageLink = "";
            if (fileImage.ImageSourceType.Equals("ShopStyle", StringComparison.InvariantCultureIgnoreCase))
            {
                imageLink = String.Format("/Images/{0}/{1}?width={2}&height={3}", "ImageUrl", fileImage.Id, width, height);
            }
            else
            {
                imageLink = String.Format("/Images/{0}/{1}?width={2}&height={3}", imageActionName, fileImage.GoogleImageId, width, height);
            }

           
            return imageLink;
        }
        public static String GetImageLinkHtml(String imageActionName, FileManager fileImage, int width, int height, String title, String alt)
        {
            String imageLink = "";

            if (fileImage.ImageSourceType.Equals("ShopStyle", StringComparison.InvariantCultureIgnoreCase))
            {
                imageLink = String.Format("<img class='imageItem' src='/Images/{0}/{1}?width={2}&height={3}' alt='{4}' title='{5}' />",
                    "ImageUrl", fileImage.Id, width, height, alt, title);
            }
            else
            {
                imageLink = String.Format("<img class='imageItem' src='/Images/{0}/{1}?width={2}&height={3}' alt='{4}' title='{5}' />",
                imageActionName, fileImage.GoogleImageId, width, height, alt, title);
            }
            return imageLink;
        }
        public static string GetProductCategoryLink(BaseCategory productCategory)
        {
            String detailLink = String.Format("/ProductCategories/Category/{0}",
                String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(productCategory.Name), productCategory.Id));

            return detailLink.ToLowerInvariant();
        }
        public static string GetCategoryLink(BaseCategory productCategory, String type)
        {
            String detailLink = String.Format("/"+type+"categories/category/{0}",
                String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(productCategory.Name), productCategory.Id));

            return detailLink.ToLowerInvariant();
        }
        public static string GetNavigationLink(Navigation navigation)
        {
            if (!navigation.Static)
            {
                String detailLink = String.Format("/{0}/{1}", navigation.ControllerName, navigation.ActionName);
                return detailLink.ToLowerInvariant();
            }
            else
            {
                if (!String.IsNullOrEmpty(navigation.Link))
                {
                    return navigation.Link.ToLowerInvariant();
                }
                else
                {
                    return "";
                }

            }


        }

        public static string GetBrandDetailLink(Brand brand)
        {
            String link = String.Format("/Brands/Detail/{0}", String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(brand.Name), brand.Id));

            return link.ToLowerInvariant();
        }

        public static string GetLabelLink(Label label)
        {
            String link = String.Format("/Tags/{0}", String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(label.Name), label.Id));

            return link.ToLowerInvariant();
        }

        public static string GetRetailerLink(Retailer retailer)
        {
            String link = String.Format("/Retailers/{0}", String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(retailer.Name), retailer.Id));

            return link.ToLowerInvariant();
        }

        public static string GetRetailerDetailLink(Retailer retailer)
        {
            String link = String.Format("/Retailers/Detail/{0}", String.Format("{0}-{1}", GeneralHelper.GetUrlSeoString(retailer.Name), retailer.Id));

            return link.ToLowerInvariant();
        }
    }
}
