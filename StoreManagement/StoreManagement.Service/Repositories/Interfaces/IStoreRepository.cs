using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreRepository : IEntityRepository<Store, int>
    {

    }
}
