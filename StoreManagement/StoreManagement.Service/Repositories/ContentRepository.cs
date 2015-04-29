using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Helpers;
using StoreManagement.Helpers.CacheHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories
{
    public class ContentRepository : EntityRepository<Content, int>, IContentRepository
    {

        static TypedObjectCache<List<Content>> contentCache = new TypedObjectCache<List<Content>>("GetContentByTypeAndCategoryIdFromCache");

        private IStoreContext dbContext;
        public ContentRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            return
                this.FindBy(
                    r => r.StoreId == storeId && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
        }

        public  Content GetContentByUrl(int storeId, string url)
        {
            return
                this.FindBy(
                    r => r.StoreId == storeId && r.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
        }

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.FindBy(
                     r => r.StoreId == storeId && 
                         r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) &&
                         r.CategoryId == categoryId) 
                     .ToList();
        }
        
        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("Content-{0}-{1}-{2}", storeId, typeName, categoryId);
            List<Content> items = null;
            contentCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetContentByTypeAndCategoryId(storeId, typeName, categoryId);
                contentCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration",10)));
            }

            return items;
        }
    }
}
