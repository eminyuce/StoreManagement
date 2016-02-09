using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Liquid.Controllers
{
    public class RetailersController : BaseController
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const String IndexPageDesingName = "RetailersIndexPage";
        private const String RetailerDetailPageDesignName = "RetailerDetailPage";

        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {

            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);

                var retailersTask = RetailerService.GetRetailersAsync(StoreId, null, true);
                var settings = GetStoreSettings();
                RetailerService2.ImageWidth = GetSettingValueInt("RetailersIndex_ImageWidth", 50);
                RetailerService2.ImageHeight = GetSettingValueInt("RetailersIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, retailersTask);
                var pageDesign = pageDesignTask.Result;
                var retailers = retailersTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = RetailerService2.GetRetailers(retailers, pageDesign);
                pageOutput.StoreSettings = settings;
                pageOutput.PageTitle = "Retailers";
                pageOutput.MyStore = this.MyStore;
                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
        public async Task<ActionResult> Detail(String id = "")
        {
            try
            {

                int retailerId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, RetailerDetailPageDesignName);
                var retailerTask = RetailerService.GetRetailerAsync(retailerId);
                var take = GetSettingValueInt("RetailerProducts_ItemNumber", 20);
                var productsTask = ProductService.GetProductsByProductTypeAsync(StoreId, null, null, retailerId, StoreConstants.ProductType, 1,
                                                                 take, true, "normal", null);
                var productCategoriesTask = ProductCategoryService.GetCategoriesByRetailerIdAsync(StoreId, retailerId);

                var settings = GetStoreSettings();
                RetailerService2.ImageWidth = GetSettingValueInt("RetailerDetail_ImageWidth", 50);
                RetailerService2.ImageHeight = GetSettingValueInt("RetailerDetail_ImageHeight", 50);

                await Task.WhenAll(retailerTask, pageDesignTask,productsTask, productCategoriesTask);
                var pageDesign = pageDesignTask.Result;
                var products = productsTask.Result;
                var productCategories = productCategoriesTask.Result;
                var retailer = retailerTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + RetailerDetailPageDesignName);
                }

                var dic = RetailerService2.GetRetailerDetailPage(retailer, products, pageDesign, productCategories);
                dic.StoreSettings = settings;
                dic.MyStore = this.MyStore;
                dic.PageTitle = retailer.Name;
                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Stack Trace:" + ex.StackTrace, id);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}