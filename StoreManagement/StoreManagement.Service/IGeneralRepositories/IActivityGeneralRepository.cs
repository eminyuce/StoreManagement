using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IActivityGeneralRepository : IGeneralRepository
    {
        Task<List<Activity>> GetActivitiesAsync(int storeId, int ? take, bool ? isActive);
    }
}
