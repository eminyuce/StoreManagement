using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;


namespace StoreManagement.Liquid.Controllers
{
    public class ActivitiesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String IndexPageDesingName = "ActivitiesIndexPage";

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {

            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var activitiesTask = ActivityService.GetActivitiesAsync(StoreId, null, true);

                var settings = GetStoreSettings();
                ActivityService2.ImageWidth = GetSettingValueInt("ActivitiesIndex_ImageWidth", 50);
                ActivityService2.ImageHeight = GetSettingValueInt("ActivitiesIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, activitiesTask);
                var pageDesign = pageDesignTask.Result;
                var activities = activitiesTask.Result;

                if (pageDesign == null)
                {
                  
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = ActivityService2.GetActivityIndexPage(pageDesign, activities);
                pageOutput.StoreSettings = settings;
                pageOutput.MyStore = this.MyStore;
                pageOutput.PageTitle = "Activities";
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