using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreLanguageRepository : IBaseRepository<StoreLanguage, int>, IStoreLanguageGeneralRepository, IDisposable 
    {
        List<StoreLanguage> GetStoreLanguages(int storeId, string search);
    }
}
