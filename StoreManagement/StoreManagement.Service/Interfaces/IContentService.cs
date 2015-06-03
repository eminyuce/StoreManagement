using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;

namespace StoreManagement.Service.Interfaces
{
    public interface IContentService : IService
    {

        Content GetContentsContentId(int contentId);
        List<Content> GetContentByType(int storeId, String typeName);
        Content GetContentByUrl(int storeId, String url);
        List<Content> GetContentByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        IPagedList<Content> GetContentsCategoryId(int storeId, int categoryId, String typeName, bool? isActive, int page, int pageSize);
        Content GetContentWithFiles(int id);
 
    }
}
