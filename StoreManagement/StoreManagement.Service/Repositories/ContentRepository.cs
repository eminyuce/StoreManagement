using GenericRepository.EntityFramework;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
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

        private static readonly TypedObjectCache<List<Content>> ContentCache = new TypedObjectCache<List<Content>>("GetContentByTypeAndCategoryIdFromCache");
        private static readonly TypedObjectCache<StorePagedList<Content>> ContentCacheStorePagedList = new TypedObjectCache<StorePagedList<Content>>("StorePagedListContent");

        public ContentRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public Content GetContentsContentId(int contentId)
        {
            return this.GetSingleIncluding(contentId, r => r.ContentFiles.Select(r1 => r1.FileManager));
            //return dbContext.Contents.Include(r => r.ContentFiles.Select(t => t.FileManager)).FirstOrDefault(x => x.Id == contentId);
        }

        public List<Content> GetContentByType(string typeName)
        {
            return
             this.FindBy(r => r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                 .ToList();
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            return
                this.FindBy(
                    r => r.StoreId == storeId && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(r => r.Ordering).ThenByDescending(r => r.Id)
                    .ToList();
        }


        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.GetAllIncluding(r1 => r1.ContentFiles.Select(r2 => r2.FileManager)).Where(
                     r => r.StoreId == storeId &&
                         r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) &&
                         r.CategoryId == categoryId)
                     .ToList();
        }

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search)
        {
            var contents = this.FindBy(
                r => r.StoreId == storeId &&
                     r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));

            if (categoryId > 0)
            {
                contents = contents.Where(r => r.CategoryId == categoryId);
            }
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                contents = contents.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return contents.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("Store-{0}-GetContentByTypeAndCategoryIdFromCache-{1}-{2}", storeId, typeName, categoryId);
            List<Content> items = null;
            ContentCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetContentByTypeAndCategoryId(storeId, typeName, categoryId);
                ContentCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Contents_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public StorePagedList<Content> GetContentsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            String key = String.Format("Store-{0}-GetContentsCategoryId-{1}-{2}-{3}-{4}-{5}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "", page, pageSize);
            StorePagedList<Content> items = null;
            ContentCacheStorePagedList.TryGet(key, out items);

            if (items == null)
            {
                var returnList =
                        this.GetAllIncluding(r => r.ContentFiles.Select(r1 => r1.FileManager))
                            .Where(r2 => r2.StoreId == storeId &&
                                 r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
                if (categoryId.HasValue)
                {
                    returnList = returnList.Where(r => r.CategoryId == categoryId.Value);
                }
                if (isActive.HasValue)
                {
                    returnList = returnList.Where(r => r.State == isActive);
                }

                var cat = returnList.OrderByDescending(r => r.Id).ToList();
                items = new StorePagedList<Content>(cat.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, pageSize, cat.Count());
                //  items = new PagedList<Content>(cat, page, cat.Count());
                //  items = (PagedList<Content>) cat.ToPagedList(page, pageSize);
                ContentCacheStorePagedList.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Contents_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }


        public Content GetContentWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ContentFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }

        public  Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            var res =  Task.FromResult(GetContentsCategoryId(storeId, categoryId, typeName, isActive, page, pageSize));
            return res;
        }

        public List<Content> GetContentsByStoreId(int storeId, string searchKey, string typeName)
        {
            var contents = this.FindBy(r => r.StoreId == storeId &&  r.Type.Equals(typeName));
            if (!String.IsNullOrEmpty(searchKey))
            {
                contents = contents.Where(r => r.Name.ToLower().Contains(searchKey.ToLower())).OrderBy(r => r.Name);
            }
            return contents.ToList();
        }
    }
}
