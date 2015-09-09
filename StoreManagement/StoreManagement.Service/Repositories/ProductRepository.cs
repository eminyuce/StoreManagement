﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ProductRepository : BaseRepository<Product, int>, IProductRepository
    {

        private static readonly TypedObjectCache<List<Product>> ProductCache = new TypedObjectCache<List<Product>>("ProductsCache");
        private static readonly TypedObjectCache<StorePagedList<Product>> ProductCacheStorePagedList = new TypedObjectCache<StorePagedList<Product>>("StorePagedListProduct");


        public ProductRepository(IStoreContext dbContext)
            : base(dbContext)
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

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search)
        {
            var products = this.FindBy(
                r => r.StoreId == storeId &&
                     r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));


            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                products = products.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }
            else if (categoryId > 0)
            {
                products = products.Where(r => r.ProductCategoryId == categoryId);
            }



            return products.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("GetProductByTypeAndCategoryIdFromCache-{0}-{1}-{2}", storeId, typeName, categoryId);
            List<Product> items = null;
            ProductCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetProductByTypeAndCategoryId(storeId, typeName, categoryId);
                ProductCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Products_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page,
                                                    int pageSize)
        {
            String key = String.Format("GetContentsCategoryId-{0}-{1}-{2}-{3}-{4}-{5}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "", page, pageSize);
            StorePagedList<Product> items = null;
            ProductCacheStorePagedList.TryGet(key, out items);

            if (items == null)
            {
                var returnList =
                        this.GetAllIncluding(r => r.ProductFiles.Select(r1 => r1.FileManager)).Where(r2 => r2.StoreId == storeId && r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));


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
                ProductCacheStorePagedList.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Products_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Product GetProductWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ProductFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }

        public Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State) && r2.ProductCategoryId == (categoryId.HasValue ? categoryId.Value : r2.ProductCategoryId) && r2.MainPage;
            Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
       
            var items = this.FindAllIncludingAsync(match, page, pageSize, r => r.Ordering, OrderByType.Descending, includeProperties);
            Task.WaitAll(items);
            var cat = items.Result;

            var task = Task.Factory.StartNew(() =>
            {
                StorePagedList<Product> resultItems = new StorePagedList<Product>(cat, page, pageSize, this.Count(match));
                return resultItems;
            });
            return task;

        }

        public async Task<Product> GetProductsByIdAsync(int productId)
        {
            Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
            return await this.GetSingleIncludingAsync(productId, includeProperties);
        }

        public async Task<List<Product>> GetMainPageProductsAsync(int storeId, int? take)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.MainPage;
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.ProductCategoryId == categoryId && r2.Id
                    != (excludedProductId.HasValue ? excludedProductId.Value : r2.Id);
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }

        }

        public async Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId)
        {
            try
            {
                int excludedProductId2 = excludedProductId.HasValue ? excludedProductId.Value : 0;
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.BrandId == brandId && r2.Id != excludedProductId2;
                var items = this.FindAllIncludingAsync(match, take, t => t.Ordering, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));

                var itemsResult = items;
                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetPopularProducts(int storeId, string productType, int page, int pageSize)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State;
                Expression<Func<Product, int>> keySelector = t => t.TotalRating;
                var items = this.FindAllIncludingAsync(match, page, pageSize, keySelector, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetRecentProducts(int storeId, string productType, int page, int pageSize)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State;
                Expression<Func<Product, int>> keySelector = t => t.Id;
                var items = this.FindAllIncludingAsync(match, page, pageSize, keySelector, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }


        public List<Product> GetMainPageProducts(int storeId, int? take)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.MainPage;
                var items =  this.FindAllIncludingAsync(match, take, t => t.Ordering, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));
                Task.WaitAll(items);
                return items.Result;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }

        }
        public List<Product> GetProductsByStoreId(int storeId, String searchKey)
        {
            return BaseEntityRepository.GetActiveBaseEntitiesSearchList(this, storeId, searchKey);
        }
    }
}
