using System;
using System.Collections.Generic;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;

namespace StoreManagement.Service.Interfaces
{
    public interface ICategoryService : IService
    {
        List<Category> GetCategoriesByStoreId(int storeId);
        List<Category> GetCategoriesByStoreIdWithContent(int storeId);
        List<Category> GetCategoriesByStoreId(int storeId, String type);
        List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type);
        Category GetSingle(int id);
        IPagedList<Category> GetCategoryWithContents(int categoryId, int page);
    }

}
