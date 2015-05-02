using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class CompanyRepository : EntityRepository<Company, int>, ICompanyRepository
    {
        private IStoreContext dbContext;
        public CompanyRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}


