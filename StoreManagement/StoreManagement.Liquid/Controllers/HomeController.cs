using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult About()
        {
            var item = GetSettingValue(StoreConstants.AboutUs);
            return View(item);
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

        public ActionResult MainLayout()
        {
            int storeId = Store.Id;

            var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayout");
            var dic = NavigationHelper.GetMainLayoutLink(mainMenu, pageDesignTask);
            return View(dic);

        }
        public ActionResult MainLayoutFooter()
        {
            int storeId = Store.Id;

            var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutFooter");
            var dic = NavigationHelper.GetMainLayoutFooterLink(mainMenu, pageDesignTask);
            return View(dic);

        }
        public ActionResult MainLayoutJavaScriptFiles()
        {
            int storeId = Store.Id;
            var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
            Task.WaitAll(pageDesignTask);
            return View(pageDesignTask.Result);

        }
    }
}