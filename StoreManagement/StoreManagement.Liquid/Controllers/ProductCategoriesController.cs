using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class ProductCategoriesController : BaseController
    {
        private const String PageDesingName = "ProductCategoriesIndex";

       [OutputCache(CacheProfile = "Cache20Minutes")]
        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingName);
                var pageSize = GetSettingValueInt(PageDesingName, StoreConstants.DefaultPageSize);
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true, page, pageSize);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();
                var pageOutput = ProductCategoryHelper.GetCategoriesIndexPage(pageDesignTask, categories);



                PagingHelper.StoreSettings = GetStoreSettings();
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask);


                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductCategories:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }


        //
        // GET: /Product/
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public ActionResult Category(String id = "", int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
                int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductCategoryPage");
                var pageSize = GetSettingValueInt("ProductCategoryPage_ItemsNumber", StoreConstants.DefaultPageSize);
                var categories = ProductCategoryService.GetProductCategoryAsync(categoryId);
                var productsTask = ProductService.GetProductsCategoryIdAsync(StoreId, categoryId, StoreConstants.ProductType, true, page, pageSize);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();
                ProductCategoryHelper.ImageWidth = GetSettingValueInt("ProductCategoryPage_ImageWidth", 50);
                ProductCategoryHelper.ImageHeight = GetSettingValueInt("ProductCategoryPage_ImageHeight", 50);

                var pageOutput = ProductCategoryHelper.GetCategoryPage(pageDesignTask, categories, productsTask);



                PagingHelper.StoreSettings = GetStoreSettings();
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask);


                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Category:Index:" + ex.StackTrace, id);
                return new HttpStatusCodeResult(500);
            }

        }



    }
}