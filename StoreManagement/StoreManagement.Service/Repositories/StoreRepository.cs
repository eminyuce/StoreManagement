using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;


namespace StoreManagement.Service.Repositories
{
    public class StoreRepository : EntityRepository<Store, int>, IStoreRepository
    {
        private IStoreContext dbContext;

        public StoreRepository(IStoreContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

