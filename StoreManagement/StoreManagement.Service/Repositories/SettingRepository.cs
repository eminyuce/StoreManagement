using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class SettingRepository : EntityRepository<Setting, int>, ISettingRepository
    {
        private IStoreContext dbContext;
        public SettingRepository(IStoreContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<Setting> GetStoreSettings(int storeid)
        {
            var items =  dbContext.Settings.Where(r => r.StoreId == storeid).ToList();

            return items;
        }

        public string GetStoreSetting(int storeId)
        {
            String result = "";

            var settings = GetStoreSettings(storeId);
            var builder = new StringBuilder();
            foreach (var s in settings)
            {
                builder.Append(GeneralHelper.SettingSpan(s.SettingKey, s.SettingValue));
            }
            result = builder.ToString();


            return result;

        }
       
    }


}
