using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class BlogsCategoriesController : CategoriesController
    {
        //
        // GET: /BlogsCategories/
        public BlogsCategoriesController()
            : base(StoreConstants.BlogsType)
        {

        }
        
        
	}
}