using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.Services;

namespace StoreManagement.Liquid.Controllers
{
    public class RetailersController : BaseController
    {
        private const String IndexPageDesingName = "RetailersIndexPage";

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {

            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);

                var retailersTask = RetailerService.GetRetailersAsync(StoreId, null, true);

                ActivityHelper.StoreSettings = GetStoreSettings();
                ActivityHelper.ImageWidth = GetSettingValueInt("RetailersIndex_ImageWidth", 50);
                ActivityHelper.ImageHeight = GetSettingValueInt("RetailersIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, retailersTask);
                var pageDesign = pageDesignTask.Result;
                var retailers = retailersTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = RetailerHelper.GetRetailers(retailers, pageDesign);


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