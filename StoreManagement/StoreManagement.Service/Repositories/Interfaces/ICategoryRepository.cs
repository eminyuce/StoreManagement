using System;
using System.Collections.Generic;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.JsTree;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICategoryRepository : IEntityRepository<Category, int>
    {
        List<Category> GetCategoriesByStoreId(int storeId);
        List<Category> GetCategoriesByStoreId(int storeId, String type);
        List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type);
        List<JsTreeNode> CreateCategoriesTree(int storeId, String type);

    }

}
