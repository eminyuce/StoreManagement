using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class StoreLanguageRepository : BaseRepository<StoreLanguage, int>, IStoreLanguageRepository
    {
        public StoreLanguageRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<StoreLanguage> GetStoreLanguages(int storeId, string search)
        {
            var items = this.FindBy(r => r.StoreId == storeId);
            if (!String.IsNullOrEmpty(search))
            {
                items = items.Where(r => r.Name.ToLower().Contains(search.ToLower())).OrderBy(r => r.Name);
            }
            return items.ToList();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
