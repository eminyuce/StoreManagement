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
        public IStoreRepository StoreRepository { set; get; }

        [Inject]
        public INavigationRepository NavigationRepository { set; get; }

        [Inject]
        public IPageDesignRepository PageDesignRepository { set; get; }
        
        [Inject]
        public IStoreUserRepository StoreUserRepository { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }


        protected bool IsSuperAdmin
        {
            get { return User.IsInRole("SuperAdmin"); }
        }

        protected int LoginStoreId  
        {
            get
            {
                if (Session["LoginStoreId"] != null)
                    return Session["LoginStoreId"].ToInt();
                else
                {
                    return -1;
                };
            }
            set
            {
                Session["LoginStoreId"] = value;
            }
        }

        protected int GetStoreId(int id)
        {
             if (IsSuperAdmin)
             {
                 return id;
             }
             else
             {
                 return LoginStoreId;
             }
            
        }
        protected Store LoginStore
        {
            get
            {
                if (Session["LoginStore"] != null)
                {
                    return (Store)Session["LoginStore"];
                }
                else
                {
                    return null;
                };
            }
            set
            {
                Session["LoginStore"] = value;
            }
        }
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected IStoreContext DbContext;
        protected ISettingRepository SettingRepository;
        protected BaseController(IStoreContext dbContext, ISettingRepository settingRepository)
        {
            this.DbContext = dbContext;
            this.SettingRepository = settingRepository;
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
