using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class NavigationRepository : BaseRepository<Navigation, int>, INavigationRepository
    {

        private static readonly TypedObjectCache<List<Navigation>> NavigationsCache = new TypedObjectCache<List<Navigation>>("NavigationsCache");


        public NavigationRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<Navigation> GetStoreNavigations(int storeId)
        {
            return this.FindBy(r => r.StoreId == storeId).OrderBy(r=>r.Ordering).ToList();
        }

        public List<Navigation> GetStoreActiveNavigations(int storeId)
        {
            String key = String.Format("Navigation-{0}", storeId);
            List<Navigation> items = null;
            NavigationsCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetStoreNavigations(storeId).Where(r => r.State).ToList();
                NavigationsCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("MainMenuNavigation_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }
    }



}
