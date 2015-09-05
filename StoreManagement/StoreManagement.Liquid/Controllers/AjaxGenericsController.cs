using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class AjaxGenericsController : BaseController
    {
        public async Task<JsonResult> MainLayout()
        {
            int storeId = StoreId;

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainNavigation");

                    NavigationHelper.StoreSettings = GetStoreSettings();
                    var pageOutput = NavigationHelper.GetMainLayoutLink(mainMenu, pageDesignTask);
                    String html = pageOutput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "MainLayout", storeId);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);


        }
        public async Task<JsonResult> MainLayoutFooter()
        {
            int storeId = StoreId;

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var mainMenu = NavigationService.GetStoreActiveNavigationsAsync(storeId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "Footer");
                    var pageOutput = NavigationHelper.GetMainLayoutFooterLink(mainMenu, pageDesignTask);
                    String html = pageOutput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "MainLayout", storeId);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);

        }
	}
}