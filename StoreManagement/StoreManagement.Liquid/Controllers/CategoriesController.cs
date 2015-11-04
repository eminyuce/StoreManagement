using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class CategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
                CategoryHelper.StoreSettings = settings;

                await Task.WhenAll(pageDesignTask, categoriesTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingIndexPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = CategoryHelper.GetCategoriesIndexPage(pageDesign, categories, Type);
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
                pagingDic.StoreSettings = settings;
                pagingDic.MyStore = this.MyStore;
                pagingDic.PageTitle = "Categories";
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
                CategoryHelper.StoreSettings = settings;
                CategoryHelper.ImageWidth = GetSettingValueInt(Type + "CategoryPage_ImageWidth", 50);
                CategoryHelper.ImageHeight = GetSettingValueInt(Type + "CategoryPage_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, categoryTask);
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingCategoryPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = CategoryHelper.GetCategoryPage(pageDesign, category,  Type);
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