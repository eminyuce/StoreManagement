using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Models;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Inject]
        public IFileManagerService FileManagerService { get; set; }

        [Inject]
        public IContentFileService ContentFileService { set; get; }

        [Inject]
        public IContentService ContentService { set; get; }

        [Inject]
        public ICategoryService CategoryService { set; get; }

        [Inject]
        public IStoreService StoreService { set; get; }

        [Inject]
        public INavigationService NavigationService { set; get; }

        [Inject]
        public IPageDesignService PageDesignService { set; get; }

        [Inject]
        public IStoreUserService StoreUserService { set; get; }

        [Inject]
        public ISettingService SettingService { set; get; }

        protected Store Store { set; get; }

        

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            try
            {
                this.Store = StoreService.GetStore(requestContext.HttpContext.Request);
            }
            catch (Exception ex)
            {

                this.Store = StoreService.GetSingle(1);
            }
           
            
           
        }

    }
}