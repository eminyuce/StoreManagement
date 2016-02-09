using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.SEO;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache10Days")]
    public class SitemapsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //
        // GET: /Sitemap/
        public ActionResult Index()
        {
            return SiteMapService.GetNavigationSiteMap(this);
        }
        public ActionResult Products()
        {
           
            return SiteMapService.GetProductsSiteMap(this);
        }
        public ActionResult ProductCategories()
        {
            return SiteMapService.GetProductCategoriesSiteMap(this);
        }
        public ActionResult Brands()
        {
            return SiteMapService.GetBrandsSiteMap(this);
        }
        public ActionResult Retailers()
        {

            return SiteMapService.RetailersSitemapResult(this);
        }
    }
}