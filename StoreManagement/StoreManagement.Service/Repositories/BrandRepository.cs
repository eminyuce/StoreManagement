using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive)
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    var items = this.FindBy(r2 => r2.StoreId == storeId);

                    if (isActive.HasValue)
                    {
                        items = items.Where(r => r.State == isActive.Value);
                    }

                    if (take.HasValue)
                    {
                        items = items.Take(take.Value);
                    }

                    return items.OrderBy(r => r.Ordering).ToList();

                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                    return null;
                }
            });
            return task;
        }

        public Task<Brand> GetBrandAsync(int brandId)
        {

            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    return  this.GetSingle(brandId);

                }
                catch (Exception exception)
                {
                    Logger.Error(exception);
                    return null;
                }
            });
            return task;
        }
    }
}
