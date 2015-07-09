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
        public BrandRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<Brand> GetBrandsByStoreId(int storeId, string search)
        {
            var products = this.FindBy(r => r.StoreId == storeId );

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                products = products.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return products.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }
    }
}
