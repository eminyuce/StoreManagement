using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface IProductService : IService
    {
        Product GetProductsById(int productId);
        List<Product> GetProductByType(String typeName);
        List<Product> GetProductByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Product> GetProductByTypeAndCategoryId(int storeId, String typeName, int categoryId, String search);
        List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize);
        Product GetProductWithFiles(int id);
        Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize);
        Task<Product> GetProductsByIdAsync(int productId);
        Task<List<Product>> GetMainPageProductsAsync(int storeId,  int? take);
        Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive);
 
        Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId);
        Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId);
        Task<List<Product>> GetPopularProducts(int storeId, int? categoryId, int? brandId, string productType, int page, int pageSize, bool? isActive);
        Task<List<Product>> GetRecentProducts(int storeId, int? categoryId, int? brandId, string productType, int page, int pageSize, bool? isActive);

        Task<List<Product>> GetMainPageProductsAsync(int storeId, int? categoryId, int? brandId, string productType, int page, int pageSize, bool ? isActive);

 
    }
}
