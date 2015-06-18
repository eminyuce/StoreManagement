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
    public class StoreRepository : BaseRepository<Store, int>, IStoreRepository
    {
  
        private static string _defaultlayout = "~/Views/Shared/Layouts/{0}.cshtml";

        private static readonly TypedObjectCache<Store> StoreCache = new TypedObjectCache<Store>("StoreCache");



        public StoreRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }


        

        public Store GetStoreByDomain(string domainName)
        {
            IQueryable<Store> s =  this.FindBy(r => r.Domain.Equals(domainName, StringComparison.InvariantCultureIgnoreCase));
            return s.FirstOrDefault();
        }
        public Store GetStore(String domainName)
        {
          
            String key = String.Format("GetStoreDomain-{0}", domainName);
            Store site = null;
            StoreCache.TryGet(key, out site);
            if (site == null)
            {
                site = this.GetStoreByDomain(domainName);
                string layout = String.Format("~/Views/Shared/Layouts/{0}.cshtml", !String.IsNullOrEmpty((String)site.Layout) ? (String)site.Layout : "_Layout1");
                var isFileExist = File.Exists(System.Web.HttpContext.Current.Server.MapPath(layout));
                _defaultlayout = String.Format(_defaultlayout, ProjectAppSettings.GetWebConfigString("DefaultLayout", "_Layout1"));
                if (!isFileExist)
                {
                    Logger.Info(String.Format("Layout is not found.Default Layout {0} is used.Site Domain is {1} ", _defaultlayout, site.Domain));
                }
                String selectedLayout = isFileExist ? layout : _defaultlayout;

                site.Layout = selectedLayout;
                StoreCache.Set(key, site, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("TooMuchTime_CacheAbsoluteExpiration_Minute", 100000)));
            }
            return site;
        }

        public Store GetStore(int id)
        {
            String key = String.Format("GetStore-{0}", id);
            Store site = null;
            StoreCache.TryGet(key, out site);
            if (site == null)
            {
                site = GetSingle(id);
                StoreCache.Set(key, site, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("TooMuchTime_CacheAbsoluteExpiration_Minute", 100000)));
            }
            return site;
        }

        public Store GetStoreByUserName(string userName)
        {
            var res = from s in this.StoreDbContext.Stores
                      join c in this.StoreDbContext.StoreUsers on s.Id equals c.StoreId
                      join u in this.StoreDbContext.UserProfiles on c.UserId equals u.UserId
                      where u.UserName.Equals(userName,StringComparison.InvariantCultureIgnoreCase)
                      select s;
            var result = res.ToList();
            return result.FirstOrDefault();
        }
    }
}

