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
    public class ActivityRepository : BaseRepository<Activity, int>, IActivityRepository
    {
        public ActivityRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<Activity> GetActivitiesByStoreId(int storeId, string search)
        {
            return BaseEntityRepository.GetActiveBaseEntitiesSearchList(this, storeId, search);
        }

        public Task<List<Activity>> GetActivitiesAsync(int storeId, int? take, bool? isActive)
        {
            return BaseEntityRepository.GetActiveBaseEnitiesAsync(this, storeId, take, isActive);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
