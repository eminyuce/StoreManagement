using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using Ninject;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Models;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;
using StoreManagement.Data.GeneralHelper;

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

        [Inject]
        public IEmailSender EmailSender { set; get; }


        [Inject]
        public IProductService ProductService { set; get; }

        [Inject]
        public IProductFileService ProductFileService { set; get; }
        
        protected Store Store { set; get; }



        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");

            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                var request = requestContext.HttpContext.Request;
                String domainName = "FUELTECHNOLOGYAGE.COM";
                domainName = request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host + (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port);
                domainName = GeneralHelper.GetDomainPart(domainName);
                this.Store = StoreService.GetStore(domainName);

            }
            else
            {
                this.Store = StoreService.GetStoreByDomain("login.seatechnologyjobs.com");

            }
 

            if (Store == null)
            {
                throw new Exception("Store cannot be NULL");
            }
        }
     

    }
}