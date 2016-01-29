using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class RetailerRepository : BaseRepository<Retailer, int>, IRetailerRepository
    {
        public RetailerRepository(IStoreContext dbContext)
            : base(dbContext)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Retailer> GetRetailersByStoreId(int storeId, string search)
        {
            return BaseEntityRepository.GetBaseEntitiesSearchList(this, storeId, search);
        }

        public Task<List<Retailer>> GetRetailersAsync(int storeId, int? take, bool isActive)
        {
            return BaseEntityRepository.GetActiveBaseEnitiesAsync(this, storeId, take, isActive);
        }

        public Task<Retailer> GetRetailerAsync(int retailerId)
        {
            return this.GetSingleAsync(retailerId);
        }

        public List<Retailer> GetRetailers(int storeId, int? take, bool isActive)
        {
            return BaseEntityRepository.GetActiveBaseEnities(this, storeId, take, isActive);
        }
    }
}
