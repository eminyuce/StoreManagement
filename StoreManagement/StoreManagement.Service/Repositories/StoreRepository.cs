using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using GenericRepository.EntityFramework;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;


namespace StoreManagement.Service.Repositories
{
    public class StoreRepository : EntityRepository<Store, int>, IStoreRepository
    {
        private IStoreContext dbContext;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string defaultlayout = "~/Views/Shared/Layouts/{0}.cshtml";

        static TypedObjectCache<Store> StoreCache = new TypedObjectCache<Store>("StoreCache");


        public StoreRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public Store GetStoreByDomain(string domainName)
        {
            IQueryable<Store> s = this.FindBy(r => r.Domain.Equals(domainName, StringComparison.InvariantCultureIgnoreCase));
            return s.FirstOrDefault();
        }
        public Store GetStore(HttpRequestBase request)
        {
            String domainName = "FUELTECHNOLOGYAGE.COM";
            domainName = request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host + (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port);
            domainName = GeneralHelper.GetDomainPart(domainName);
            String key = String.Format("GetStore-{0}", domainName);
            Store site = null;
            StoreCache.TryGet(key, out site);
            if (site == null)
            {
                site = this.GetStoreByDomain(domainName);
                string layout = String.Format("~/Views/Shared/Layouts/{0}.cshtml", !String.IsNullOrEmpty((String)site.Layout) ? (String)site.Layout : "_Layout1");
                var isFileExist = File.Exists(System.Web.HttpContext.Current.Server.MapPath(layout));
                defaultlayout = String.Format(defaultlayout, ProjectAppSettings.GetWebConfigString("DefaultLayout", "_Layout1"));
                if (!isFileExist)
                {
                    Logger.Info(String.Format("Layout is not found.Default Layout {0} is used.Site Domain is {1} ", defaultlayout, site.Domain));
                }
                String selectedLayout = isFileExist ? layout : defaultlayout;

                site.Layout = selectedLayout;
                StoreCache.Set(key, site, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 100000)));
            }
            return site;
        }
    }
}

