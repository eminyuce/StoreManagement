using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class NewsCategoriesController : CategoriesController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // GET: /BlogsCategories/
        public NewsCategoriesController()
            : base(StoreConstants.NewsType)
        {
            this.PageDesingCategoryPageName = "NewsCategoryPage";
            this.PageDesingIndexPageName = "NewsCategoriesIndexPage";
        }
	}
}