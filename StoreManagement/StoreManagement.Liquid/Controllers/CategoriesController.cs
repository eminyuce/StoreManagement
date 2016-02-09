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
    public abstract class CategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected String PageTitle { get; set; }
        protected String PageDesingIndexPageName { get; set; }
        protected String PageDesingCategoryPageName { get; set; }

        private String Type { get; set; }
        protected CategoriesController(String type)
        {
            this.Type = type;
        }

        public virtual async Task<ActionResult> Index(int page = 1)
        {
            try
            {


                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingIndexPageName);
                var pageSize = GetSettingValueInt(Type + "Categories_PageSize", StoreConstants.DefaultPageSize);
                var categoriesTask = CategoryService.GetCategoriesByStoreIdWithPagingAsync(StoreId, Type, true, page, pageSize);
                var settings = GetStoreSettings();

                await Task.WhenAll(pageDesignTask, categoriesTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingIndexPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = CategoryService2.GetCategoriesIndexPage(pageDesign, categories, Type);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingService2.PageOutput = pageOutput;
                PagingService2.HttpRequestBase = this.Request;
                PagingService2.RouteData = this.RouteData;
                PagingService2.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingService2.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingService2.GetPaging(pagingPageDesignTask.Result);
                pagingDic.StoreSettings = settings;
                pagingDic.MyStore = this.MyStore;
                pagingDic.PageTitle = this.PageTitle;

                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, Type + "Categories:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }



        public virtual async Task<ActionResult> Category(String id = "", int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingCategoryPageName);
                var categoryTask = CategoryService.GetCategoryAsync(categoryId);

                var settings = GetStoreSettings();
                CategoryService2.ImageWidth = GetSettingValueInt(Type + "CategoryPage_ImageWidth", 50);
                CategoryService2.ImageHeight = GetSettingValueInt(Type + "CategoryPage_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, categoryTask);
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingCategoryPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = CategoryService2.GetCategoryPage(pageDesign, category,  Type);
                pageOutput.StoreSettings = settings;
                pageOutput.MyStore = this.MyStore;
                pageOutput.PageTitle = category.Name;
                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, Type + "Category:Index:" + ex.StackTrace, id);
                return new HttpStatusCodeResult(500);
            }

        }




    }
}