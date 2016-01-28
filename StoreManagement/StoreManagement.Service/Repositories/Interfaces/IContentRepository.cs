using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IContentRepository : IBaseRepository<Content, int>, IContentGeneralRepository, IDisposable 
    {
        List<Content> GetContentsByStoreId(int storeId, string searchKey, string typeName);
        List<Content> GetMainPageContents(int storeId, int? categoryId, string type, int? take);
    }
}
