using System.Linq.Expressions;
using GenericRepository.EntityFramework;
using GenericRepository.EntityFramework.Enums;
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

 

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool ? isActive)
        {
            Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId
                && r2.State == (isActive.HasValue ? isActive.Value : r2.State)
                && r2.CategoryId == (categoryId > 0 ? categoryId : r2.CategoryId) &&
                     r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase);


            var predicate = PredicateBuilder.Create<Content>(match);
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                predicate = predicate.And(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            var contents = this.FindBy(predicate);

            return contents.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
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
 

        public async Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize,
                                               string search)
        {



            Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId
                                                          && r2.State == (isActive.HasValue ? isActive.Value : r2.State)
                                                          && typeName.Equals(r2.Type,StringComparison.InvariantCultureIgnoreCase)
                                                          && r2.CategoryId ==
                                                          (categoryId.HasValue ? categoryId.Value : r2.CategoryId);


            var predicate = PredicateBuilder.Create<Content>(match);

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                predicate = predicate.And(r => r.Name.Contains(search.ToLower()));
            }

             Expression<Func<Content, object>> includeProperties = r => r.ContentFiles.Select(r1 => r1.FileManager);
             Expression<Func<Content, int>> keySelector = t => t.Ordering;
             var items = await this.FindAllIncludingAsync(predicate, page, pageSize, keySelector, OrderByType.Descending, includeProperties);
             var totalItemNumber = await this.CountAsync(predicate);


            var task = Task.Factory.StartNew(() =>
                {

                    StorePagedList<Content> resultItems = new StorePagedList<Content>(items, page, pageSize, totalItemNumber);
                    return resultItems;
                });
            var result = await task;

            return result;
        }

        public async Task<Content> GetContentByIdAsync(int blogId)
        {
            return await this.GetAllIncluding(r2 => r2.ContentFiles.Select(r3 => r3.FileManager)).FirstOrDefaultAsync(r1 => r1.Id == blogId);
        }


        public async Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, string typeName, int categoryId, int take, int? excludedContentId)
        {
            try
            {
                Expression<Func<Content, bool>> match = r => r.StoreId == storeId
                    && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase)
                    && r.CategoryId == categoryId
                    && r.Id != (excludedContentId.HasValue ? excludedContentId.Value : r.Id);
                return await this.FindAllIncludingAsync(match, take, null, r1 => r1.Ordering, OrderByType.Descending, r1 => r1.ContentFiles.Select(r2 => r2.FileManager));
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
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

        public async Task<List<Content>> GetMainPageContentsAsync(int storeId, int? categoryId, string type, int? take)
        {
            try
            {
                Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId 
                    && r2.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase)
                    && r2.CategoryId == (categoryId ?? r2.CategoryId) && r2.State && r2.MainPage;

                var items =  this.FindAllAsync(match, r => r.Ordering, OrderByType.Descending, take, null);

                return await items;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<Content>();
            }
        }

        public async Task<List<Content>> GetContentByTypeAsync(int storeId, int? take, bool? isActive, string typeName)
        {
            try
            {
                Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId && r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase)
                    && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                Expression<Func<Content, object>> includeProperties = r => r.ContentFiles.Select(r1 => r1.FileManager);

                var items = await this.FindAllIncludingAsync(match, take, null, r => r.Ordering, OrderByType.Descending, includeProperties);

                return items;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<Content>();
            }
        }

        public List<Content> GetContentByType(int storeId, int? take, bool? isActive, string typeName)
        {
         try
            {
                Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId && r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase)
                    && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                Expression<Func<Content, object>> includeProperties = r => r.ContentFiles.Select(r1 => r1.FileManager);

                var items = this.FindAllIncluding(match, take, null, r => r.Ordering, OrderByType.Descending, includeProperties);

                return items;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<Content>();
            }
        }

        public async Task<List<Content>> GetContentsByContentKeywordAsync(int storeId, int? catId, string type, int page, int pageSize, bool? isActive,
                                                     string contentType)
        {
            try
            {
                Expression<Func<Content, bool>> match = r2 => r2.StoreId == storeId
                    &&  r2.State == (isActive ?? r2.State)
                    && r2.CategoryId == (catId ?? r2.CategoryId) && r2.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase);

                var predicate = PredicateBuilder.Create<Content>(match);

                Expression<Func<Content, object>> includeProperties = r => r.ContentFiles.Select(r1 => r1.FileManager);

                Expression<Func<Content, int>> keySelector = t => t.Id;
                if (contentType.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                {
                    Expression<Func<Content, Guid>> keySelector2 = t => Guid.NewGuid();
                    var itemsRandom = this.FindAllIncludingAsync(predicate, page, pageSize, keySelector2, OrderByType.Descending, includeProperties);
                    return await itemsRandom;
                }
                else if (contentType.Equals("normal", StringComparison.InvariantCultureIgnoreCase))
                {
                    keySelector = t => t.Ordering;
                }
                else if (contentType.Equals("popular"))
                {
                    keySelector = t => t.TotalRating;
                }
                else if (contentType.Equals("recent"))
                {
                    keySelector = t => t.Id;
                }
                else if (contentType.Equals("main"))
                {
                    keySelector = t => t.Ordering;
                    predicate = predicate.And(r => r.MainPage);
                }


                var items = this.FindAllIncludingAsync(predicate, page, pageSize, keySelector, OrderByType.Descending, includeProperties);
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
