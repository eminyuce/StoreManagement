using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;

namespace StoreManagement.Service.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {

        private static readonly TypedObjectCache<List<Category>> CategoryCache
            = new TypedObjectCache<List<Category>>("categoryCache");

        private static readonly TypedObjectCache<StorePagedList<Category>> PagingCategoryCache
         = new TypedObjectCache<StorePagedList<Category>>("PagingCategoryCache");


        public CategoryRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId, int? take)
        {
            var query = StoreDbContext.Categories.Where(r => r.StoreId == storeId && r.Contents.Any()).Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)));

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.OrderByDescending(r => r.Ordering).ToList();
        }


        private static Expression<Func<Category, object>> IncludeProperties()
        {
            var param = Expression.Parameter(typeof(Category));
            Expression<Func<Category, object>> exp = r => r.Contents.Select(m => m.ContentFiles.Select(p => p.FileManager));

            return exp;
        }

        public List<Category> GetCategoriesByStoreId(int storeId, String type, bool? isActive)
        {
            return BaseCategoryRepository.GetCategoriesByStoreId(this, storeId, type, isActive);
        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type, string search)
        {
            return BaseCategoryRepository.GetBaseCategoriesSearchList(this, storeId, search, type);
        }

        public List<Category> GetCategoriesByType(string type)
        {
            return this.FindBy(r => r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type)
        {
            String key = String.Format("GetCategoriesByStoreIdFromCache-{0}-{1}", storeId, type);
            List<Category> items = null;
            CategoryCache.TryGet(key, out items);
            if (items == null)
            {
                //var ttt = from cus in StoreDbContext.Categories
                //    join ord in StoreDbContext.Contents on cus.Id equals ord.CategoryId
                //    join cf in StoreDbContext.ContentFiles on ord.Id equals cf.ContentId
                //    where cus.StoreId == storeId
                //          && cus.Contents.Any()
                //          && cus.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)
                //    orderby cus.Ordering, ord.Ordering
                //    select cus;

                //var mmmm = ttt
                //    .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)))
                //    .ToList();

                var cats = this.FindBy(r => r.StoreId == storeId &&
                         r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase))
                         .OrderBy(r => r.Ordering)
                         .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)));

                items = cats.ToList();

                foreach (var category in items)
                {
                    foreach (var ccc in category.Contents)
                    {
                        ccc.Description = ""; // GeneralHelper.GetDescription(ccc.Description, 200);
                    }
                }

                //var mmmm = itemss.ToList();

                //  items = GetCategoriesByStoreId(storeId, type);


                //var mmmm = StoreDbContext.Categories
                //    .Where(r => r.StoreId == storeId && r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase))
                //    .OrderBy(b => b.Ordering)
                //    .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)));

                CategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Categories_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Category GetCategory(int id)
        {
            return this.GetSingle(id);
        }

        public StorePagedList<Category> GetCategoryWithContents(int categoryId, int page = 1, int pageSize = 25)
        {
            String key = String.Format("GetCategoryWithContents-{0}-{1}", categoryId, page);
            StorePagedList<Category> items = null;
            PagingCategoryCache.TryGet(key, out items);

            if (items == null)
            {
                IQueryable<Category> cats = StoreDbContext.Categories.Where(r => r.Id == categoryId && r.Contents.Any())
                                                          .Include(
                                                              r =>
                                                              r.Contents.Select(
                                                                  r1 => r1.ContentFiles.Select(m => m.FileManager)))
                                                          .OrderByDescending(r => r.Ordering);


                var c = cats.ToList();
                items = new StorePagedList<Category>(c.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, c.Count());
                // items = new PagedList<Category>(cats, page, cats.Count());
                // items = new StorePagedList<Category>(paging.FindAll(), page, paging.Capacity);
                PagingCategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Categories_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Task<StorePagedList<Category>> GetCategoryWithContentsAsync(int categoryId, int page, int pageSize = 25)
        {
            var res = Task.FromResult(GetCategoryWithContents(categoryId, page, pageSize));
            return res;
        }

        public Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId)
        {
            // var res = Task.FromResult(GetCategoriesByStoreId(storeId));
            // return res;
            return BaseEntityRepository.GetBaseEnitiesAsync(this, storeId, null);
        }


        public Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            return BaseCategoryRepository.GetCategoriesByStoreIdAsync(this, storeId, type, isActive, null);
        }

        public Task<Category> GetCategoryAsync(int id)
        {
            return this.GetSingleAsync(id);
        }

        public Task<Category> GetCategoryByContentIdAsync(int storeId, int contentId)
        {
            Content category = this.FindBy(r => r.StoreId == storeId).Select(r => r.Contents.FirstOrDefault(t => t.Id == contentId)).FirstOrDefault();
            if (category != null)
            {
                return this.GetSingleAsync(category.CategoryId);
            }
            else
            {
                return null;
            }
        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            var categories = from entry in this.FindBy(r => r.StoreId == storeId) select entry;
            return categories.Where(r => r.State).OrderBy(r => r.Ordering).ToList();


        }

    }
}
