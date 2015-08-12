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

        private readonly TypedObjectCache<List<Content>> _contentCache = new TypedObjectCache<List<Content>>("GetContentByTypeAndCategoryIdFromCache");
        private readonly TypedObjectCache<StorePagedList<Content>> _contentCacheStorePagedList = new TypedObjectCache<StorePagedList<Content>>("StorePagedListContent");

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


            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                contents = contents.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }
            else if (categoryId > 0)
            {
                contents = contents.Where(r => r.CategoryId == categoryId);
            }


            return contents.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("Store-{0}-GetContentByTypeAndCategoryIdFromCache-{1}-{2}", storeId, typeName, categoryId);
            List<Content> items = null;

            _contentCache.IsCacheEnable = this.IsCacheEnable;
            _contentCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetContentByTypeAndCategoryId(storeId, typeName, categoryId);
                _contentCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(this.CacheMinute));
            }

            return items;
        }

        public StorePagedList<Content> GetContentsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            String key = String.Format("Store-{0}-GetContentsCategoryId-{1}-{2}-{3}-{4}-{5}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "", page, pageSize);
            StorePagedList<Content> items = null;
            _contentCacheStorePagedList.IsCacheEnable = this.IsCacheEnable;
            _contentCacheStorePagedList.TryGet(key, out items);

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
                _contentCacheStorePagedList.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(this.CacheMinute));
            }

            return items;
        }


        public Content GetContentWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ContentFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }

        public Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            var res = Task.FromResult(GetContentsCategoryId(storeId, categoryId, typeName, isActive, page, pageSize));
            return res;
        }

        public Task<Content> GetContentByIdAsync(int blogId)
        {
            var res = Task.FromResult(this.GetContentWithFiles(blogId));
            return res;
        }

      
        public Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, string typeName, int categoryId, int take, int? excludedContentId)
        {
           
            var task = Task.Factory.StartNew(() =>
            {
                var returnList = this.GetContentByTypeAndCategoryId(storeId, typeName, categoryId);

                if (excludedContentId.HasValue)
                {
                    returnList = returnList.Where(r => r.Id != excludedContentId).ToList();
                }

                return returnList.Take(take).ToList();


            });
            return task;
        }

        public List<Content> GetMainPageContents(int storeId, int? categoryId, string type, int? take)
        {
            try
            {
                IQueryable<Content> returnList =
                  this.GetAllIncluding(r => r.ContentFiles.Select(r1 => r1.FileManager))
                      .Where(r2 => r2.StoreId == storeId &&
                           r2.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
                if (categoryId.HasValue)
                {
                    returnList = returnList.Where(r => r.CategoryId == categoryId.Value);
                }
                returnList = returnList.Where(r => r.State && r.MainPage);

                if (take.HasValue)
                {
                    returnList = returnList.Take(take.Value);
                }

                returnList = returnList.OrderBy(r => r.Ordering);
                return returnList.ToList();
            }
            catch (Exception ex)
            {

                Logger.Error(ex);

                return new List<Content>();
            }

        }

        public Task<List<Content>> GetMainPageContentsAsync(int storeId, int? categoryId, string type, int? take)
        {

            var task = Task.Factory.StartNew(() =>
                {
                    var returnList = GetMainPageContents(storeId, categoryId, type, take);

                    return returnList;

                });
            return task;
        }

        public List<Content> GetContentsByStoreId(int storeId, string searchKey, string typeName)
        {
            var contents = this.FindBy(r => r.StoreId == storeId && r.Type.Equals(typeName));
            if (!String.IsNullOrEmpty(searchKey))
            {
                contents = contents.Where(r => r.Name.ToLower().Contains(searchKey.ToLower())).OrderBy(r => r.Name);
            }
            return contents.ToList();
        }
    }
}
