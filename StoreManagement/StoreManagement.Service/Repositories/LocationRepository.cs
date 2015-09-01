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
    public class LocationRepository : BaseRepository<Location,int>,ILocationRepository
    {
        public LocationRepository(IStoreContext dbContext) : base(dbContext)
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

        public Task<List<Location>> GetLocationsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                Expression<Func<Location, bool>> match =
                    r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                var items = this.FindAllAsync(match, take);

                var itemsResult = items;

                return itemsResult;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

       
    }
}
