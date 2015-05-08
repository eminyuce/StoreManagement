using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IStoreContext dbContext;
        protected ISettingRepository settingRepository;
        public BaseController(IStoreContext dbContext, ISettingRepository settingRepository)
        {
            this.dbContext = dbContext;
            this.settingRepository = settingRepository;
        }
        
     
         
    }
}
