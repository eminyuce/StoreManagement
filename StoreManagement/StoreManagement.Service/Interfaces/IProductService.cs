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
        List<Product> GetProductByType(int storeId, String typeName);
        List<Product> GetProductByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        StorePagedList<Product> GetProductsCategoryId(int storeId, int ? categoryId, String typeName, bool? isActive, int page, int pageSize);
        Product GetProductWithFiles(int id);
    }
}
