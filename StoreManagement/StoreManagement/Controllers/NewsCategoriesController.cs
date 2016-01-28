using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class NewsCategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String ContentType = StoreConstants.NewsType;
        //
        // GET: /NewsCategories/
        public ActionResult Index(int page = 1)
        {
            var pageSize = GetSettingValueInt(ContentType + "Categories_PageSize", StoreConstants.DefaultPageSize);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdWithPagingAsync(StoreId, ContentType, true, page, pageSize);
            var settings = GetStoreSettings();
            return View();
        }
        public ActionResult Category(String id, int page = 1)
        {

            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();

            StorePagedList<Content> task2 = ContentService.GetContentsCategoryId(MyStore.Id, categoryId, ContentType, true, page, 600);


            returnModel.SCategories = CategoryService.GetCategoriesByStoreId(MyStore.Id, ContentType, true);
            returnModel.SStore = MyStore;
            returnModel.SCategory = CategoryService.GetCategory(categoryId);
            returnModel.Type = ContentType;
            returnModel.SNavigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            returnModel.SContents = new PagedList<Content>(task2.items, task2.page - 1, task2.pageSize, task2.totalItemCount);

            return View(returnModel);

        }
    }
}