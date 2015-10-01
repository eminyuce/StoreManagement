using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface ICategoryService : IService
    {
        List<Category> GetCategoriesByStoreId(int storeId);
        List<Category> GetCategoriesByStoreIdWithContent(int storeId, int? take);
        List<Category> GetCategoriesByStoreId(int storeId, String type, bool? isActive = false);
        List<Category> GetCategoriesByStoreId(int storeId, String type, String search);
        List<Category> GetCategoriesByType(String type);
        List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type);
        Category GetCategory(int id);


        Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId, String type, bool? isActive);
        Task<Category> GetCategoryAsync(int id);
        Task<Category> GetCategoryByContentIdAsync(int storeId, int contentId);
        Task<StorePagedList<Category>> GetCategoriesByStoreIdWithPagingAsync(int storeId, String type, bool? isActive, int page = 1, int pageSize = 25);
    }

}
