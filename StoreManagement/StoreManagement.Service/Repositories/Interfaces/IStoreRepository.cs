using System;
using System.Collections.Generic;
using System.Web;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreRepository : IBaseRepository<Store, int>, IStoreGeneralRepository, IDisposable 
    {
        void DeleteStore(int storeId);
        List<Store> GetAllStores();
        int SaveStore();
        void CopyStore(int copyStoreId, string name, string domain, string layout);
        List<Store> GetStoresByStoreId(string searchKey);
    }
}
