using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IStoreContext dbContext;
        protected BaseController(IStoreContext dbContext)
        {
            this.dbContext = dbContext;
        }

         
    }
}
