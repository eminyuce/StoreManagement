using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreUserRepository : IEntityRepository<StoreUser, int>, IStoreUserService
    {

    }

}
