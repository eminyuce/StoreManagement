using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Admin.Controllers
{
    public class FileManagerCategoriesController : CategoriesController
    {
        //
        // GET: /NewsCategories/
        public FileManagerCategoriesController()
            : base(StoreConstants.FilesType)
        {

        }

         
	}
}