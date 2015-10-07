using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface IContentService : IService
    {

        Content GetContentsContentId(int contentId);
        List<Content> GetContentByType(String typeName);
        List<Content> GetContentByType(int storeId, String typeName);
        List<Content> GetContentByTypeAndCategoryId(int storeId, String typeName, int categoryId, String search, bool? isActive);
        StorePagedList<Content> GetContentsCategoryId(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize);
        Content GetContentWithFiles(int id);
        Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, String typeName, bool? isActive, int page, int pageSize, String search);
        Task<Content> GetContentByIdAsync(int id);
        Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, String typeName, int categoryId, int take, int? excludedContentId);
        Task<List<Content>> GetMainPageContentsAsync(int storeId, int? categoryId, string type, int? take);
        Task<List<Content>> GetContentByTypeAsync(int storeId, int? take, bool? isActive, String typeName);
        Task<List<Content>> GetContentsByContentKeywordAsync(int storeId, int? catId, string type, int page, int pageSize, bool ? isActive, string contentType);
    }
}
