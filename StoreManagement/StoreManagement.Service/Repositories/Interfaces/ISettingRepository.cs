using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ISettingRepository : IBaseRepository<Setting, int>, ISettingGeneralRepository, IDisposable 
    {
       

        List<Setting> GetStoreSettingsByType(int storeid, string type, String search);
        void SaveSetting(int storeid, string key, String value, String type);
        void SaveSetting();
 
    }


}
