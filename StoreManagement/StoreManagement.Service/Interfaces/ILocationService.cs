using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Services;

namespace StoreManagement.Service.Interfaces
{
    public interface ILocationService : IService
    {
        Task<List<Location>> GetLocationsAsync(int storeId, int ? take, bool ? isActive);
    }
}
