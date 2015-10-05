using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class CategoriesController : BaseController
    {
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

                CategoryHelper.StoreSettings = GetStoreSettings();

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
                var pageSize = GetSettingValueInt(Type + "CategoryPage_ItemsNumber", StoreConstants.DefaultPageSize);
                var categoriesTask = CategoryService.GetCategoryAsync(categoryId);
                var contentsTask = ContentService.GetContentsCategoryIdAsync(StoreId, categoryId, Type, true, page, pageSize,"");

                CategoryHelper.StoreSettings = GetStoreSettings();
                CategoryHelper.ImageWidth = GetSettingValueInt(Type + "CategoryPage_ImageWidth", 50);
                CategoryHelper.ImageHeight = GetSettingValueInt(Type + "CategoryPage_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, categoriesTask, contentsTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;
                var contents = contentsTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingCategoryPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = CategoryHelper.GetCategoryPage(pageDesign, categories, contents, Type);
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
                Logger.Error(ex, Type + "Category:Index:" + ex.StackTrace, id);
                return new HttpStatusCodeResult(500);
            }

        }




    }
}