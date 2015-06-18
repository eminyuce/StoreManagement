using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ProductRepository : BaseRepository<Product, int>, IProductRepository
    {

        static TypedObjectCache<List<Product>> _productCache = new TypedObjectCache<List<Product>>("ProductsCache");
        static TypedObjectCache<StorePagedList<Product>> _productCacheStorePagedList = new TypedObjectCache<StorePagedList<Product>>("StorePagedListProduct");


        public ProductRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public Product GetProductsById(int productId)
        {
            return this.GetSingleIncluding(productId, r => r.ProductFiles.Select(r1 => r1.FileManager));
        }

        public List<Product> GetProductByType(string typeName)
        {
            return
               this.FindBy(r => r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                   .ToList();
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            return
               this.FindBy(
                   r => r.StoreId == storeId && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                   .ToList();
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.GetAllIncluding(r1 => r1.ProductFiles.Select(r2 => r2.FileManager)).Where(
                       r => r.StoreId == storeId &&
                           r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) &&
                           r.ProductCategoryId == categoryId)
                       .ToList();
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("GetProductByTypeAndCategoryIdFromCache-{0}-{1}-{2}", storeId, typeName, categoryId);
            List<Product> items = null;
            _productCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetProductByTypeAndCategoryId(storeId, typeName, categoryId);
                _productCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page,
                                                    int pageSize)
        {
            String key = String.Format("GetContentsCategoryId-{0}-{1}-{2}-{3}-{4}-{5}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "", page, pageSize);
            StorePagedList<Product> items = null;
            _productCacheStorePagedList.TryGet(key, out items);

            if (items == null)
            {
                var returnList =
                        this.GetAllIncluding(r => r.ProductFiles.Select(r1 => r1.FileManager))
                            .Where(r2 => r2.StoreId == storeId &&
                                 r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
                if (categoryId.HasValue)
                {
                    returnList = returnList.Where(r => r.ProductCategoryId == categoryId.Value);
                }
                if (isActive.HasValue)
                {
                    returnList = returnList.Where(r => r.State == isActive);
                }

                var cat = returnList.OrderByDescending(r => r.Id).ToList();
                items = new StorePagedList<Product>(cat.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, pageSize, cat.Count());
                //  items = new PagedList<Content>(cat, page, cat.Count());
                //  items = (PagedList<Content>) cat.ToPagedList(page, pageSize);
                _productCacheStorePagedList.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }

        public Product GetProductWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ProductFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }
    }
}
