using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class FileManagerRepository : BaseRepository<FileManager, int>, IFileManagerRepository
    {

        static TypedObjectCache<List<FileManager>> CategoryCache
      = new TypedObjectCache<List<FileManager>>("StoreFileManager");


        public FileManagerRepository(IStoreContext dbContext) : base(dbContext)
        {
        }

        public List<FileManager> GetFilesByStoreIdFromCache(int storeId)
        {
            String key = String.Format("StoreFileManager-{0}", storeId);
            List<FileManager> items = null;
            CategoryCache.TryGet(key, out items);
            if (items == null)
            {
                items = GetFilesByStoreId(storeId);
                CategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }
            return items;
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            return FindBy(r => r.StoreId == storeId).ToList();
        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {
            return
                FindBy(r => r.GoogleImageId.Equals(googleImageId, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
        }
    }



}
