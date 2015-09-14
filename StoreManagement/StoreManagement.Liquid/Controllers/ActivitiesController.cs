using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class ActivitiesController : BaseController
    {
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public ActionResult Index()
        {

            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ActivitiesIndex");
                var activitiesTask = ActivityService.GetActivitiesAsync(StoreId, null, true);

                ActivityHelper.StoreSettings = GetStoreSettings();
                ActivityHelper.ImageWidth = GetSettingValueInt("ActivitiesIndex_ImageWidth", 50);
                ActivityHelper.ImageHeight = GetSettingValueInt("ActivitiesIndex_ImageHeight", 50);
                var pageOutput = ActivityHelper.GetActivityIndexPage(pageDesignTask, activitiesTask);


                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
	}
}