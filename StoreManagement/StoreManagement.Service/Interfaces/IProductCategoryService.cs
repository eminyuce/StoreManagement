using System;
using System.Collections.Generic;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface IProductCategoryService : IService
    {
        List<ProductCategory> GetProductCategoriesByStoreId(int storeId);
        List<ProductCategory> GetProductCategoriesByStoreIdWithContent(int storeId);
        List<ProductCategory> GetProductCategoriesByStoreId(int storeId, String type);
        List<ProductCategory> GetProductCategoriesByStoreId(int storeId, String type,String search);
        List<ProductCategory> GetProductCategoriesByStoreIdFromCache(int storeId, String type);
        ProductCategory GetProductCategory(int id);
        StorePagedList<ProductCategory> GetProductCategoryWithContents(int categoryId, int page, int pageSize = 25);
    }

}
