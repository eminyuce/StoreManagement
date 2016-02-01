using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.ActionResults;


namespace StoreManagement.Controllers
{
    public class RssController : BaseController
    {


        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Products(int take = 1, int description = 300, int imageHeight = 50, int imageWidth = 50, int isDetailLink = 0)
        {
         
            return ProductService2.GetProductRss(take, description, imageHeight, imageWidth, isDetailLink);
        }

        public ActionResult News(int take = 15, int description = 250, int imageHeight = 50, int imageWidth = 50)
        {
             
            return ContentService2.GetContentRss(take, description, imageHeight, imageWidth, StoreConstants.NewsType);
        }

        public ActionResult Blogs(int take = 15, int description = 250, int imageHeight = 50, int imageWidth = 50)
        {
            
            return ContentService2.GetContentRss(take, description, imageHeight, imageWidth, StoreConstants.BlogsType);
        }
	}
}