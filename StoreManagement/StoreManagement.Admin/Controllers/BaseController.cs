using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public abstract class BaseController : Controller
    {

        [Inject]
        public IFileManagerRepository FileManagerRepository { get; set; }

        [Inject]
        public IContentFileRepository ContentFileRepository { set; get; }

        [Inject]
        public IContentRepository ContentRepository { set; get; }

        [Inject]
        public ICategoryRepository CategoryRepository { set; get; }

        [Inject]
        public IStoreRepository storeRepository { set; get; }

        [Inject]
        public INavigationRepository navigationRepository { set; get; }

        [Inject]
        public IPageDesignRepository PageDesignRepository { set; get; }
        
        [Inject]
        public IStoreUserRepository StoreUserRepository { set; get; }

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected IStoreContext dbContext;
        protected ISettingRepository settingRepository;
        public BaseController(IStoreContext dbContext, ISettingRepository settingRepository)
        {
            this.dbContext = dbContext;
            this.settingRepository = settingRepository;
        }
        protected string GetCleanHtml(String source)
        {
            if (String.IsNullOrEmpty(source))
                return String.Empty;

            source = HttpUtility.HtmlDecode(source);

            string path = HttpContext.Server.MapPath("~/tags.config");
            var myFile = new System.IO.StreamReader(path);
            string myTags = myFile.ReadToEnd();
            var returnHtml = HtmlCleanHelper.SanitizeHtmlSoft(myTags, source);
            returnHtml = GeneralHelper.NofollowExternalLinks(returnHtml);
            return returnHtml;
        }

       
    }
}
