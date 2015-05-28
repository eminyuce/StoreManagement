using System;
using System.Collections.Generic;
using StoreManagement.Data.Entities;
using StoreManagement.Data.JsTree;

namespace StoreManagement.Service.Interfaces
{
    public interface ICategoryService 
    {
        List<Category> GetCategoriesByStoreId(int storeId);
        List<Category> GetCategoriesByStoreIdWithContent(int storeId);
        List<Category> GetCategoriesByStoreId(int storeId, String type);
        List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type);
        List<JsTreeNode> CreateCategoriesTree(int storeId, String type);
        Category GetSingle(int id);
    }

}
