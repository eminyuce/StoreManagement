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
    public class ProductCategoriesController : BaseController
    {
        private const String PageDesingName = "ProductCategoriesIndexPage";

       [OutputCache(CacheProfile = "Cache20Minutes")]
        public async  Task<ActionResult> Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingName);
                var pageSize = GetSettingValueInt("ProductCategories_PageSize", StoreConstants.DefaultPageSize);
                var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true, page, pageSize);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();

                await Task.WhenAll(pageDesignTask, categoriesTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;

                var pageOutput = ProductCategoryHelper.GetCategoriesIndexPage(pageDesign, categories);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingHelper.StoreSettings = GetStoreSettings();
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
                Logger.Error(ex, "ProductCategories:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }


        //
        // GET: /Product/
        [OutputCache(CacheProfile = "Cache20Minutes")]
       public async Task<ActionResult> Category(String id = "", int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductCategoryPage");
                var pageSize = GetSettingValueInt("ProductCategoryPage_ItemsNumber", StoreConstants.DefaultPageSize);
                var categoriesTask = ProductCategoryService.GetProductCategoryAsync(categoryId);
                var productsTask = ProductService.GetProductsCategoryIdAsync(StoreId, categoryId, StoreConstants.ProductType, true, page, pageSize);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();
                ProductCategoryHelper.ImageWidth = GetSettingValueInt("ProductCategoryPage_ImageWidth", 50);
                ProductCategoryHelper.ImageHeight = GetSettingValueInt("ProductCategoryPage_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, categoriesTask, productsTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;
                var products = productsTask.Result;

                var pageOutput = ProductCategoryHelper.GetCategoryPage(pageDesign, categories, products);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");

                PagingHelper.StoreSettings = GetStoreSettings();
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;

                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask.Result);


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