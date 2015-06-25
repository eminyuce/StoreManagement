using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;

namespace StoreManagement.Admin.Controllers
{
    public class StoreCategoriesController : CategoriesController
    {
        public StoreCategoriesController() : base(StoreConstants.StoreType)
        {
            
        }
    }
}