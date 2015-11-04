using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class BlogCategoriesController : CategoriesController
    {
        //
        // GET: /BlogsCategories/
        public BlogCategoriesController()
            : base(StoreConstants.BlogsType)
        {
            this.PageDesingCategoryPageName = "BlogsCategoryPage";
            this.PageDesingIndexPageName = "BlogCategoriesIndexPage";
            this.PageTitle = "Blog Categories";

        }
        
        
	}
}