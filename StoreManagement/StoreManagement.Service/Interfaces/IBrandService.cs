using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IBrandService : IService
    {
        Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive);
        Task<Brand> GetBrandAsync(int brandId);
    }
}
