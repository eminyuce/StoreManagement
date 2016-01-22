using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class NewsCategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String ContentType = StoreConstants.BlogsType;
        //
        // GET: /NewsCategories/
        public ActionResult Index(int page=1)
        {
            var pageSize = GetSettingValueInt(ContentType + "Categories_PageSize", StoreConstants.DefaultPageSize);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdWithPagingAsync(StoreId, ContentType, true, page, pageSize);
            var settings = GetStoreSettings();
            return View();
        }
	}
}