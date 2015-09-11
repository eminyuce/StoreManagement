using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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


        private static readonly TypedObjectCache<List<Store>> AllStoreCache = new TypedObjectCache<List<Store>>("AllStoreCache");

        public StoreRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }
        public int SaveStore()
        {
            // do additional work
            MemoryCacheHelper.ClearCache("GetLoginUserStore");
            MemoryCacheHelper.ClearCache("GetAllStores");
            MemoryCacheHelper.ClearCache("StoreCache");
            return base.Save();
        }

        public void CopyStore(int copyStoreId, string name, string domain, string layout)
        {
            // Create a SQL command to execute the sproc 
            using (SqlCommand cmd = (SqlCommand)this.StoreDbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "dbo.DuplicateStoreData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("StoreId", SqlDbType.Int).Value = copyStoreId;
                cmd.Parameters.Add("StoreName", SqlDbType.NVarChar).Value = name;
                cmd.Parameters.Add("Domain", SqlDbType.NVarChar).Value = domain;
                cmd.Parameters.Add("Layout", SqlDbType.NVarChar).Value = layout;


                try
                {

                    StoreDbContext.Database.Connection.Open();
                    var reader = cmd.ExecuteNonQuery();

                }
                finally
                {
                    StoreDbContext.Database.Connection.Close();
                }
            }
        }

        public List<Store> GetStoresByStoreId(string searchKey)
        {
            var items = this.FindBy(r => r.Name.ToLower().Contains(searchKey.ToLower())).OrderBy(r => r.Name);
            return items.ToList();
        }

        public List<Store> GetAllStores()
        {
            String key = String.Format("GetAllStores");
            List<Store> sites = null;
            AllStoreCache.TryGet(key, out sites);
            if (sites == null)
            {
                sites = GetAll().ToList();
                StoreCache.Set(key, sites, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("TooMuchTime_CacheAbsoluteExpiration_Minute", 100000)));
            }
            return sites;
        }

        public Store GetStoreByDomain(string domainName)
        {
            IQueryable<Store> s = this.FindBy(r => r.Domain.Equals(domainName, StringComparison.InvariantCultureIgnoreCase));
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
                if (site != null)
                {
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
                if (site != null)
                {
                    StoreCache.Set(key, site, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("TooMuchTime_CacheAbsoluteExpiration_Minute", 100000)));
                }
            }
            return site;
        }

        public Store GetStoreByUserName(string userName)
        {
            var res = from s in this.StoreDbContext.Stores
                      join c in this.StoreDbContext.StoreUsers on s.Id equals c.StoreId
                      join u in this.StoreDbContext.UserProfiles on c.UserId equals u.UserId
                      where u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)
                      select s;
            var result = res.ToList();
            return result.FirstOrDefault();
        }

        public bool GetStoreCacheStatus(int id)
        {
            var item = GetSingle(id);
            if (item != null)
            {
                return item.IsCacheEnable;
            }
            else
            {
                return false;
            }

        }

        public int GetStoreIdByDomain(string domainName)
        {
            var item = GetStoreByDomain(domainName);

            if (item != null)
            {
                return item.Id;
            }
            else
            {
                return 0;
            }
        }

        public Task<Store> GetStoreAsync(int storeId)
        {
            return GetSingleAsync(storeId);
        }

        public void DeleteStore(int storeId)
        {

            // Create a SQL command to execute the sproc 
            using (SqlCommand cmd = (SqlCommand)this.StoreDbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "dbo.DeleteStore";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("StoreId", SqlDbType.Int).Value = storeId;


                try
                {

                    StoreDbContext.Database.Connection.Open();
                    var reader = cmd.ExecuteNonQuery();

                }
                finally
                {
                    StoreDbContext.Database.Connection.Close();
                }
            }


        }


    }
}

