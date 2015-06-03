using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;

namespace StoreManagement.Service.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {

        static TypedObjectCache<List<Category>> CategoryCache
            = new TypedObjectCache<List<Category>>("categoryCache");

        static TypedObjectCache<PagedList<Category>> PagingCategoryCache
         = new TypedObjectCache<PagedList<Category>>("PagingCategoryCache");


        public CategoryRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            // return this.StoreDbContext.Categories.Where(r => r.StoreId == storeId).ToList();
            var categories = from entry in this.FindBy(r => r.StoreId == storeId) select entry;
            return categories.ToList();
        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId)
        {

            //return this.GetAllIncluding(IncludeProperties()).Where(r => r.StoreId == storeId && r.Contents.Any()).OrderByDescending(r => r.Id).Take(10).ToList();
            //IQueryable<Category> mmm = (from z in dbContext.Categories
            //                            join f in dbContext.Contents on z.Id equals f.CategoryId
            //                            join t in dbContext.ContentFiles on f.Id equals t.ContentId
            //                            join y in dbContext.FileManagers on t.FileManagerId equals y.Id
            //                            where z.StoreId == storeId && z.Contents.Any()
            //                            select z).
            //     Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)))
            //    .Distinct()
            //    .OrderByDescending(r => r.Ordering)
            //    .Take(10);

            //return mmm.ToList();
            return StoreDbContext.Categories.Where(r => r.StoreId == storeId && r.Contents.Any())
                .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)))
                .OrderByDescending(r => r.Ordering).Take(10).ToList();
        }

        //public IQueryable<T> Include<T>(params Expression<Func<T, object>>[] paths) where T : class
        //{
        //    IQueryable<T> query = dbContext.Set<T>();
        //    foreach (var path in paths)
        //        query = query.Include(path);
        //    return query;
        //}
        private static Expression<Func<Category, object>> IncludeProperties()
        {
            var param = Expression.Parameter(typeof(Category));
            Expression<Func<Category, object>> exp = r => r.Contents.Select(m => m.ContentFiles.Select(p => p.FileManager));

            return exp;
        }

        public List<Category> GetCategoriesByStoreId(int storeId, String type)
        {
            return this.FindBy(r => r.StoreId == storeId &&
                r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type)
        {
            String key = String.Format("GetCategoriesByStoreIdFromCache-{0}-{1}", storeId, type);
            List<Category> items = null;
            CategoryCache.TryGet(key, out items);

            if (items == null && IsCacheActive)
            {
                items = GetCategoriesByStoreId(storeId, type);
                CategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Categories_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }

        public IPagedList<Category> GetCategoryWithContents(int categoryId, int page = 1)
        {
            String key = String.Format("GetCategoryWithContents-{0}-{1}", categoryId, page);
            PagedList<Category> items = null;
            PagingCategoryCache.TryGet(key, out items);

            if (items == null && !IsCacheActive)
            {
                IQueryable<Category> cats = StoreDbContext.Categories.Where(r => r.Id == categoryId && r.Contents.Any())
                                                          .Include(
                                                              r =>
                                                              r.Contents.Select(
                                                                  r1 => r1.ContentFiles.Select(m => m.FileManager)))
                                                          .OrderByDescending(r => r.Ordering);

                items = new PagedList<Category>(cats, page, cats.Count());
       
                PagingCategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Categories_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }


    }
}
