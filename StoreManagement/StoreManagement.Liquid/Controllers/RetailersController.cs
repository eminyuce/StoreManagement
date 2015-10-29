using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Services;

namespace StoreManagement.Liquid.Controllers
{
    public class RetailersController : BaseController
    {
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
                RetailerHelper.StoreSettings = settings;
                RetailerHelper.ImageWidth = GetSettingValueInt("RetailersIndex_ImageWidth", 50);
                RetailerHelper.ImageHeight = GetSettingValueInt("RetailersIndex_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, retailersTask);
                var pageDesign = pageDesignTask.Result;
                var retailers = retailersTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = RetailerHelper.GetRetailers(retailers, pageDesign);
                pageOutput.StoreSettings = settings;

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
                var productsTask = ProductService.GetProductsByProductType(StoreId, null, null, retailerId, StoreConstants.ProductType, 1,
                                                                 take, true, "normal", null);
                var productCategoriesTask = ProductCategoryService.GetCategoriesByRetailerIdAsync(StoreId, retailerId);

                var settings = GetStoreSettings();
                RetailerHelper.StoreSettings = settings;
                RetailerHelper.ImageWidth = GetSettingValueInt("RetailerDetail_ImageWidth", 50);
                RetailerHelper.ImageHeight = GetSettingValueInt("RetailerDetail_ImageHeight", 50);

                await Task.WhenAll(retailerTask, pageDesignTask, pageDesignTask, productCategoriesTask);
                var pageDesign = pageDesignTask.Result;
                var products = productsTask.Result;
                var productCategories = productCategoriesTask.Result;
                var retailer = retailerTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + RetailerDetailPageDesignName);
                }

                var dic = RetailerHelper.GetRetailerDetailPage(retailer, products, pageDesign, productCategories);
                dic.StoreSettings = settings;

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