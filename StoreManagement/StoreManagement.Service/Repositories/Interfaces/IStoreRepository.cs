using System.Collections.Generic;
using System.Web;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreRepository : IEntityRepository<Store, int>, IStoreService
    {
        void DeleteStore(int storeId);
        List<Store> GetAllStores();
        int SaveStore();
    }
}
