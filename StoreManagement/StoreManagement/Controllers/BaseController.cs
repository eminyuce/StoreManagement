using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Models;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected Store store { set; get; }
        protected IStoreContext dbContext;
        protected ISettingRepository settingRepository;
        protected IStoreRepository storeRepository;
        public BaseController(IStoreContext dbContext, 
            ISettingRepository settingRepository,
            IStoreRepository storeRepository)
        {
            this.dbContext = dbContext;
            this.settingRepository = settingRepository;
            this.storeRepository = storeRepository;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            // this.store = storeRepository.GetStore(requestContext.HttpContext.Request);
            this.store = storeRepository.GetSingle(1);
        }

    }
}