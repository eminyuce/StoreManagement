using GenericRepository.EntityFramework;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories
{
    public class ContentRepository : BaseRepository<Content, int>, IContentRepository
    {

        static TypedObjectCache<List<Content>> contentCache = new TypedObjectCache<List<Content>>("GetContentByTypeAndCategoryIdFromCache");


        public ContentRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public Content GetContentsContentId(int contentId)
        {
            return this.GetSingleIncluding(contentId, r => r.ContentFiles.Select(r1 => r1.FileManager));
            //return dbContext.Contents.Include(r => r.ContentFiles.Select(t => t.FileManager)).FirstOrDefault(x => x.Id == contentId);
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            return
                this.FindBy(
                    r => r.StoreId == storeId && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
        }

        public Content GetContentByUrl(int storeId, string url)
        {
            return
                this.FindBy(
                    r => r.StoreId == storeId && r.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
        }

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.GetAllIncluding(r1 => r1.ContentFiles.Select(r2 => r2.FileManager)).Where(
                     r => r.StoreId == storeId &&
                         r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) &&
                         r.CategoryId == categoryId)
                     .ToList();
        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("GetContentByTypeAndCategoryIdFromCache-{0}-{1}-{2}", storeId, typeName, categoryId);
            List<Content> items = null;
            contentCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetContentByTypeAndCategoryId(storeId, typeName, categoryId);
                contentCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }

        public List<Content> GetContentsCategoryId(int storeId, int categoryId, String typeName, bool? isActive)
        {

            String key = String.Format("GetContentsCategoryId-{0}-{1}-{2}-{3}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "");
            List<Content> items = null;
            contentCache.TryGet(key, out items);

            if (items == null)
            {
                var returnList =
                        this.GetAllIncluding(r => r.ContentFiles.Select(r1 => r1.FileManager))
                            .Where(r2 => r2.StoreId == storeId &&
                                 r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) && r2.CategoryId == categoryId);

                if (isActive.HasValue)
                {
                    returnList = returnList.Where(r => r.State == isActive);
                }

                items = returnList.OrderByDescending(r => r.Id).ToList();
                contentCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }
        public Content GetContentWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ContentFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }
    }
}
