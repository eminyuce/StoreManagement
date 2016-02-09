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
    public class ProductCategoriesController : BaseController
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        private const String IndexPageDesingName = "ProductCategoriesIndexPage";
        private const String CategoryPageDesingName = "ProductCategoriesCategoryPage";

       [OutputCache(CacheProfile = "Cache20Minutes")]
        public async  Task<ActionResult> Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    Logger.Trace("Navigation Modul is not active:" + StoreConstants.ProductType);
                    return HttpNotFound("Not Found");
                }

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var pageSize = GetSettingValueInt("ProductCategories_PageSize", StoreConstants.DefaultPageSize);
                var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true, page, pageSize);

                await Task.WhenAll(pageDesignTask, categoriesTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = ProductCategoryService2.GetCategoriesIndexPage(pageDesign, categories);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingService2.PageOutput = pageOutput;
                PagingService2.HttpRequestBase = this.Request;
                PagingService2.RouteData = this.RouteData;
                PagingService2.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingService2.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingService2.GetPaging(pagingPageDesignTask.Result);
                pagingDic.MyStore = this.MyStore;
                pageOutput.PageTitle = "Product Categories";
                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductCategories:Index:" + ex.StackTrace, page);
                throw ex;
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
                    Logger.Trace("Navigation Modul is not active:" + StoreConstants.ProductType);
                    return HttpNotFound("Not Found");
                }
        
                int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, CategoryPageDesingName);
                var categoryTask = ProductCategoryService.GetProductCategoryAsync(categoryId);

                var settings = GetStoreSettings();
                ProductCategoryService2.ImageWidth = GetSettingValueInt("ProductCategoryPage_ImageWidth", 50);
                ProductCategoryService2.ImageHeight = GetSettingValueInt("ProductCategoryPage_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, categoryTask);
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;
                
                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null" + CategoryPageDesingName);
                }


                var pageOutput = ProductCategoryService2.GetCategoryPage(pageDesign, category);
                pageOutput.StoreSettings = settings;
                pageOutput.MyStore = this.MyStore;
                pageOutput.PageTitle = category.Name;
                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Category:Index:" + ex.StackTrace, id);
                throw ex;
            }

        }



    }
}