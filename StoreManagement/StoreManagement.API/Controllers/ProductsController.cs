using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.IGeneralRepositories;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class ProductsController : BaseApiController<Product>, IProductGeneralRepository
    {
        public override IEnumerable<Product> GetAll()
        {
            return ProductRepository.GetAll();
        }

        public override Product Get(int id)
        {
            return ProductRepository.GetSingle(id);
        }

        public override HttpResponseMessage Post(Product value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Product value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetProductsById(int productId)
        {
            return ProductRepository.GetProductsById(productId);
        }

        public List<Product> GetProductByType(string typeName)
        {
            return ProductRepository.GetProductByType(typeName);
        }



        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return ProductRepository.GetProductByTypeAndCategoryId(storeId, typeName, categoryId);
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool? isActive)
        {
            return ProductRepository.GetProductByTypeAndCategoryId(storeId, typeName, categoryId, search, isActive);
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            return ProductRepository.GetProductByTypeAndCategoryIdFromCache(storeId, typeName, categoryId);
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page,
                                                    int pageSize)
        {
            return ProductRepository.GetProductsCategoryId(storeId, categoryId, typeName, isActive, page, pageSize);
        }

        public Product GetProductWithFiles(int id)
        {
            return ProductRepository.GetProductWithFiles(id);
        }

        public async Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize, string search, string filters)
        {
            return await ProductRepository.GetProductsCategoryIdAsync(storeId, categoryId, typeName, isActive, page, pageSize, search, filters);
        }

        public async Task<Product> GetProductsByIdAsync(int productId)
        {
            return await ProductRepository.GetProductsByIdAsync(productId);
        }

        public async Task<List<Product>> GetMainPageProductsAsync(int storeId, int? take)
        {
            return await ProductRepository.GetMainPageProductsAsync(storeId, take);
        }

        public async Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive)
        {
            return await ProductRepository.GetProductsAsync(storeId, take, isActive);
        }

        public async Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId)
        {
            return await ProductRepository.GetProductByTypeAndCategoryIdAsync(storeId, categoryId, take, excludedProductId);
        }

        public async Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId)
        {
            return await ProductRepository.GetProductsByBrandAsync(storeId, brandId, take, excludedProductId);
        }

        public async Task<List<Product>> GetProductsByRetailerAsync(int storeId, int retailerId, int? take, int? excludedProductId)
        {
            return await ProductRepository.GetProductsByBrandAsync(storeId, retailerId, take, excludedProductId);
        }

        public List<Product> GetProductsByProductType(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int page,
                                             int pageSize, bool? isActive, string functionType, int? excludedProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetProductsByProductTypeAsync(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int page, int pageSize, bool? isActive, string functionType, int? excludedProductId)
        {
            return await ProductRepository.GetProductsByProductTypeAsync(storeId, categoryId, brandId, retailerId, productType, page, pageSize, isActive, functionType, excludedProductId);
        }

        public async Task<ProductsSearchResult> GetProductsSearchResult(int storeId, string search, string filters, int top, int skip, bool isAdmin, string categoryApiId)
        {
            return await ProductRepository.GetProductsSearchResult(storeId, search, filters, top, skip, isAdmin, categoryApiId);
        }
    }
}
