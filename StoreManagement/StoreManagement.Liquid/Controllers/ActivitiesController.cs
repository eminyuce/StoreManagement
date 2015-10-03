using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class ActivitiesController : BaseController
    {
        private const String IndexPageDesingName = "ActivitiesIndexPage";

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {

            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ActivitiesIndexPage");
                var activitiesTask = ActivityService.GetActivitiesAsync(StoreId, null, true);

                ActivityHelper.StoreSettings = GetStoreSettings();
                ActivityHelper.ImageWidth = GetSettingValueInt("ActivitiesIndex_ImageWidth", 50);
                ActivityHelper.ImageHeight = GetSettingValueInt("ActivitiesIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, activitiesTask);
                var pageDesign = pageDesignTask.Result;
                var activities = activitiesTask.Result;

                if (pageDesign == null)
                {
                  
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = ActivityHelper.GetActivityIndexPage(pageDesign, activities);


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