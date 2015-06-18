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
    public class StoreUserRepository : BaseRepository<StoreUser, int>, IStoreUserRepository
    {

        private static readonly TypedObjectCache<StoreUser> StoreUserCache = new TypedObjectCache<StoreUser>("_storeUserCache");

        public StoreUserRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        
        }
        public StoreUser GetStoreUserByUserId(int userId)
        {
            String key = String.Format("GetStoreUserByUserId-{0}", userId);
            StoreUser item = null;
            StoreUserCache.TryGet(key, out item);

            if (item == null)
            {
                item = this.FindBy(r => r.UserId == userId).FirstOrDefault();
                StoreUserCache.Set(key, item, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("MainMenuNavigation_CacheAbsoluteExpiration_Minute", 10)));
            }

            return item;

        }
    }



}
