using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;

using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{

    //[OutputCache(CacheProfile = "Cache1Days")]
    public class HomeController : BaseController
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            StoreHomePage resultModel = ProductService2.GetHomePage();
            return View(resultModel);
        }
        public ActionResult About()
        {
            var item = GetSettingValue(StoreConstants.AboutUs);
            return View(item);
        }
        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Locations()
        {

            return View();
        }
        public ActionResult TermsAndCondition()
        {
            var item = GetSettingValue(StoreConstants.TermsAndCondition);
            return View(item);
        }
        public ActionResult Footer()
        {
            var item = GetSettingValue(StoreConstants.Footer);
            return View(item);
        }
        public ActionResult RecentUpdates()
        {
            return View();
        }

        //public ActionResult MainMenu()
        //{
        //    var navigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
        //    return View(navigations);
        //}

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult PageDesignTest()
        {


            return View();
        }


    }
}
