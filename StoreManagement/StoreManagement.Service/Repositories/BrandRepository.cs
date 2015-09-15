using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class BrandRepository : BaseRepository<Brand, int>, IBrandRepository
    {
        public BrandRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Brand> GetBrandsByStoreId(int storeId, string search)
        {
            return BaseEntityRepository.GetBaseEntitiesSearchList(this, storeId, search);
        }
        public async Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive)
        {
            return await BaseEntityRepository.GetActiveBaseEnitiesAsync(this, storeId, take, isActive);
        }

        public Task<Brand> GetBrandAsync(int brandId)
        {
            return this.GetSingleAsync(brandId);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
