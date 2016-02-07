using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IProductGeneralRepository : IGeneralRepository
    {
        Product GetProductsById(int productId);
        List<Product> GetProductByType(String typeName);
        List<Product> GetProductByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Product> GetProductByTypeAndCategoryId(int storeId, String typeName, int categoryId, String search, bool? isActive);
        List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize);
        Product GetProductWithFiles(int id);
        Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize, String search, String filters);
        Task<Product> GetProductsByIdAsync(int productId);
        Task<List<Product>> GetMainPageProductsAsync(int storeId, int? take);
        Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive);

        Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId);
        Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId);
        Task<List<Product>> GetProductsByRetailerAsync(int storeId, int retailerId, int? take, int? excludedProductId);
         List<Product> GetProductsByProductType(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int take, int skip, bool? isActive, String functionType, int? excludedProductId);


         Task<List<Product>> GetProductsByProductTypeAsync(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int take, int skip, bool? isActive, String functionType, int? excludedProductId);

        Task<ProductsSearchResult> GetProductsSearchResult(int storeId, string search, String filters,
                                                                        int top, int skip, bool isAdmin,String categoryApiId);

    }
}
