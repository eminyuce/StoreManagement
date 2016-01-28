using System;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreUserRepository : IBaseRepository<StoreUser, int>, IStoreUserGeneralRepository, IDisposable 
    {

    }

}
