using System.Collections.Generic;
using System.Web;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreRepository : IBaseRepository<Store, int>, IStoreService
    {
        void DeleteStore(int storeId);
        List<Store> GetAllStores();
        int SaveStore();
        void CopyStore(int copyStoreId, string name, string domain, string layout);
        List<Store> GetStoresByStoreId(string searchKey);
    }
}
