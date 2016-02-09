using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
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

        public async Task<StorePagedList<Brand>> GetBrandsByStoreIdWithPagingAsync(int storeId, bool? isActive, int page = 1, int pageSize = 25)
        {
            Expression<Func<Brand, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
            var predicate = PredicateBuilder.Create<Brand>(match);

            var items = await this.FindAllAsync(predicate, r => r.Ordering, OrderByType.Descending, page, pageSize);
            var totalItemNumber = await this.CountAsync(predicate);

            var task = Task.Factory.StartNew(() =>
            {
                var resultItems = new StorePagedList<Brand>(items, page, pageSize, totalItemNumber);
                return resultItems;
            });
            var result = await task;

            return result;
        }

        public List<Brand> GetBrands(int storeId, int? take, bool? isActive)
        {
            return BaseEntityRepository.GetActiveBaseEnities(this, storeId, take, isActive);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
