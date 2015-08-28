using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
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
            var products = this.FindBy(r => r.StoreId == storeId);

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                products = products.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return products.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }

        public async Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                Expression<Func<Brand, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                var items = this.FindAllAsync(match, take);

                var itemsResult = await items;

                return itemsResult.OrderBy(r => r.Ordering).ToList();

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public Task<Brand> GetBrandAsync(int brandId)
        {
            return this.GetSingleAsync(brandId);
        }
    }
}
