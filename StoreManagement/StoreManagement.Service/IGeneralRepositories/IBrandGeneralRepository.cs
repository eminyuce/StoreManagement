using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IBrandGeneralRepository : IGeneralRepository
    {
        Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive);
        Task<Brand> GetBrandAsync(int brandId);
        Task<StorePagedList<Brand>> GetBrandsByStoreIdWithPagingAsync(int storeId, bool? isActive, int page = 1, int pageSize = 25);
    }
}
