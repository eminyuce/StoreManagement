using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class NewsCategoriesController : CategoriesController
    {
        //
        // GET: /BlogsCategories/
        public NewsCategoriesController()
            : base(StoreConstants.NewsType)
        {
            this.PageDesingCategoryPageName = "NewsCategoryPage";
            this.PageDesingIndexPageName = "NewsCategoriesIndexPage";
        }
	}
}