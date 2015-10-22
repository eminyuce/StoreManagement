using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class ProductsController : BaseController
    {
        private const String IndexPageDesingName = "ProductsIndexPage";
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index(int page = 1, String search = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var pageSize =  GetSettingValueInt("ProductsIndex_PageSize", StoreConstants.DefaultPageSize);
                var productsTask = ProductService.GetProductsCategoryIdAsync(StoreId, null, StoreConstants.ProductType, true, page, pageSize, search);
                var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);

                var settings = GetStoreSettings();
                ProductHelper.StoreSettings = settings;
                ProductHelper.ImageWidth = GetSettingValueInt("ProductsIndex_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("ProductsIndex_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, productsTask, categoriesTask);
                var products = productsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = ProductHelper.GetProductsIndexPage(products, pageDesign, categories);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingHelper.StoreSettings = settings;
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask.Result);


                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }


        public ActionResult Index2()
        {


            return View();
        }

        public async Task<ActionResult> Product(String id = "")
        {

            try
            {

                if (!IsModulActive(StoreConstants.ProductType))
                {
                    Logger.Trace("Navigation Modul is not active:" + StoreConstants.ProductType);
                    return HttpNotFound("Not Found");
                }
                int productId = id.Split("-".ToCharArray()).Last().ToInt();
                var categoryTask = ProductCategoryService.GetProductCategoryAsync(StoreId, productId);
                var productsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductDetailPage");
                var productsTask = ProductService.GetProductsByIdAsync(productId);


                await Task.WhenAll(productsTask, categoryTask, productsPageDesignTask);
                var product = productsTask.Result;
                var pageDesign = productsPageDesignTask.Result;
                var category = categoryTask.Result;



                ProductHelper.ImageWidth = GetSettingValueInt("ProductsDetail_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("ProductsDetail_ImageHeight", 50);
                ProductHelper.StoreSettings = GetStoreSettings();
                var dic = ProductHelper.GetProductsDetailPage(product, pageDesign, category);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Product:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
        public ActionResult Product2()
        {



            return View();
        }
        public ActionResult ProductBuy(int id=0)
        {
            var productsTask = ProductService.GetProductsById(id);
            return Redirect(productsTask.VideoUrl);
        }
    }
}