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
    public class LocationRepository : BaseRepository<Location, int>, ILocationRepository
    {
        public LocationRepository(IStoreContext dbContext)
            : base(dbContext)
        {
        }

        public List<Location> GetLocationsByStoreId(int storeId, string search)
        {
            var locations = this.FindBy(r => r.StoreId == storeId);

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                locations = locations.Where(r => r.Address.ToLower().Contains(search.ToLower().Trim()));
            }

            return locations.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }

        public async Task<List<Location>> GetLocationsAsync(int storeId, int? take, bool? isActive)
        {
            return await BaseEntityRepository.GetActiveBaseEnitiesAsync(this, storeId, take, isActive);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
