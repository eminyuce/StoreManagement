using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IContentRepository : IEntityRepository<Content, int>
    {
        List<Content> GetContentByType(int storeId, String typeName);
        Content GetContentByUrl(int storeId, String url);
        List<Content> GetContentByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        List<Content> GetContentsCategoryId(int storeId, int categoryId, bool ? isActive);
    }
}
