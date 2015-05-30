using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;

namespace StoreManagement.Service.Repositories
{
    public class StoreCarouselRepository : EntityRepository<StoreCarousel, int>, IStoreCarouselRepository
    {

        static TypedObjectCache<List<StoreCarousel>> StoreCarousels
        = new TypedObjectCache<List<StoreCarousel>>("StoreCarouselsCache");


        private IStoreContext dbContext;
        public StoreCarouselRepository(IStoreContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<StoreCarousel> GetStoreCarousels(int storeId)
        {
            String key = String.Format("StoreCarousel-{0}", storeId);
            List<StoreCarousel> items = null;
            StoreCarousels.TryGet(key, out items);
            if (items == null)
            {
                items = this.GetAllIncluding(r=>r.FileManager).Where(r => r.StoreId == storeId).ToList();
                StoreCarousels.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }
            return items;

        }
    }
}
