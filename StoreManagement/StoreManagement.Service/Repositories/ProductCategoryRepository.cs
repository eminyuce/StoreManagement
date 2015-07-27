using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ProductCategoryRepository : BaseRepository<ProductCategory, int>, IProductCategoryRepository
    {


        private static readonly TypedObjectCache<List<ProductCategory>> ProductCategoryCache
        = new TypedObjectCache<List<ProductCategory>>("ProductCategoryCache");

        private static readonly TypedObjectCache<StorePagedList<ProductCategory>> PagingProductCategoryCache
         = new TypedObjectCache<StorePagedList<ProductCategory>>("PagingProductCategoryCache");

        public ProductCategoryRepository(IStoreContext dbContext)
            : base(dbContext)
        {
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId)
        {
            // return this.StoreDbContext.Categories.Where(r => r.StoreId == storeId).ToList();
            var categories = from entry in this.FindBy(r => r.StoreId == storeId) select entry;
            return categories.ToList();
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdWithContent(int storeId)
        {
            return StoreDbContext.ProductCategories.Where(r => r.StoreId == storeId && r.Products.Any())
             .Include(r => r.Products.Select(r1 => r1.ProductFiles.Select(m => m.FileManager)))
             .OrderByDescending(r => r.Ordering).Take(10).ToList();
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type)
        {
            return this.FindBy(r => r.StoreId == storeId &&
           r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();


        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type, string search)
        {
            var items = this.FindBy(r => r.StoreId == storeId &&
                                         r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                items = items.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdFromCache(int storeId, string type)
        {
            String key = String.Format("GetProductCategoriesByStoreIdFromCache-{0}-{1}", storeId, type);
            List<ProductCategory> items = null;
            ProductCategoryCache.TryGet(key, out items);

            if (items == null)
            {

                var cats = this.FindBy(r => r.StoreId == storeId &&
                         r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase))
                         .OrderBy(r => r.Ordering)
                         .Include(r => r.Products.Select(r1 => r1.ProductFiles.Select(m => m.FileManager)));

                items = cats.ToList();

                foreach (var category in items)
                {
                    foreach (var ccc in category.Products)
                    {
                        ccc.Description = ""; // GeneralHelper.GetDescription(ccc.Description, 200);
                    }
                }


                ProductCategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("ProductCategories_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public ProductCategory GetProductCategory(int id)
        {
            return GetSingle(id);
        }

        public StorePagedList<ProductCategory> GetProductCategoryWithContents(int categoryId, int page, int pageSize = 25)
        {
            String key = String.Format("GetProductCategoryWithContents-{0}-{1}", categoryId, page);
            StorePagedList<ProductCategory> items = null;
            PagingProductCategoryCache.TryGet(key, out items);

            if (items == null)
            {
                IQueryable<ProductCategory> cats = StoreDbContext.ProductCategories.Where(r => r.Id == categoryId && r.Products.Any())
                                                          .Include(
                                                              r =>
                                                              r.Products.Select(
                                                                  r1 => r1.ProductFiles.Select(m => m.FileManager)))
                                                          .OrderByDescending(r => r.Ordering);


                var c = cats.ToList();
                items = new StorePagedList<ProductCategory>(c.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, c.Count());
                PagingProductCategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("ProductCategories_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Task<List<ProductCategory>> GetProductCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            var res = Task.Factory.StartNew(() =>
            {
                var items = this.FindBy(r => r.StoreId == storeId && r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase));

                if (isActive.HasValue)
                {
                    items = items.Where(r => r.State == isActive.Value);
                }

                return items.OrderBy(r => r.Ordering).ToList();

            });

            return res;
        }
    }
}
