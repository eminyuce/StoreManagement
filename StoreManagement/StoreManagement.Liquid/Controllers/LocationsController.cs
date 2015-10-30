using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class LocationsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {

            try
            {
                
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "LocationsIndexPage");
                var locationsTask = LocationService.GetLocationsAsync(StoreId, null, true);
                var settings = GetStoreSettings();
                LocationHelper.StoreSettings = settings;
                LocationHelper.ImageWidth = GetSettingValueInt("LocationsIndex_ImageWidth", 50);
                LocationHelper.ImageHeight = GetSettingValueInt("LocationsIndex_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, locationsTask);
                var pageDesign = pageDesignTask.Result;
                var locations = locationsTask.Result;

                var pageOutput = LocationHelper.GetLocationIndexPage(pageDesign, locations);
                pageOutput.StoreSettings = settings;

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