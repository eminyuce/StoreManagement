using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{
    public abstract class BaseController : Controller
    {

        static readonly TypedObjectCache<Store> UserStoreCache = new TypedObjectCache<Store>("UserStoreCache");



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
        public ISettingRepository SettingRepository { set; get; }

        [Inject]
        public IStoreContext DbContext { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }

        [Inject]
        public IProductRepository ProductRepository { set; get; }

        [Inject]
        public IProductFileRepository ProductFileRepository { set; get; }

        [Inject]
        public IProductCategoryRepository ProductCategoryRepository { set; get; }

        [Inject]
        public ILabelRepository LabelRepository { set; get; }

        [Inject]
        public ILabelLineRepository LabelLineRepository { set; get; }

        [Inject]
        public IEmailListRepository EmailListRepository { set; get; }

        private Boolean MySuperAdmin
        {
            get
            {
                if (Session["MySuperAdmin"] != null)
                    return Session["MySuperAdmin"].ToBool();
                else
                {
                    return false;
                };
            }
            set
            {
                Session["MySuperAdmin"] = value;
            }
        }
        protected Boolean IsSuperAdmin
        {
            get { return MySuperAdmin; }
            set
            {
                MySuperAdmin = value;
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
                if (LoginStore.Id > 0)
                {
                    return LoginStore.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
        private Store _store = new Store();
        private Store MyLoginStore
        {
            get
            {
                if (Session["MyLoginStore"] != null)
                    return Session["MyLoginStore"] as Store;
                else
                {
                    return new Store();
                };
            }
            set
            {
                Session["MyLoginStore"] = value;
            }
        }

        protected Store LoginStore
        {
            get { return MyLoginStore; }
            set { MyLoginStore = value; }
        }
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected BaseController()
        {

        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            try
            {
                if (requestContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    SetStoreValues(requestContext.HttpContext.User.Identity.Name);
                }
            }
            catch (Exception ex)
            {

            }
            return base.BeginExecute(requestContext, callback, state);
        }


        protected bool SetStoreValues(String userName)
        {
           
                String key = String.Format("SetStoreValues-{0}", userName);
                Store site = null;
                UserStoreCache.TryGet(key, out site);
                if (site == null)
                {
                    var s = StoreRepository.GetStoreByUserName(userName);
                    if (s == null)
                    {
                        return false;
                    }
                    else
                    {
                        UserStoreCache.Set(key, s, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("TooMuchTime_CacheAbsoluteExpiration_Minute", 100000)));
                    }
                }
                LoginStore = site;
            

            return true;
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
        protected bool CheckRequest(BaseEntity entity)
        {
            if (IsSuperAdmin)
            {
                return true;
            }
            else
            {
                return entity.StoreId == LoginStore.Id;
            }
        }

    }
}
