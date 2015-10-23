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
    public class StorePageDesignRepository : BaseRepository<StorePageDesign, int>, IStorePageDesignRepository
    {
        public StorePageDesignRepository(IStoreContext dbContext) : base(dbContext)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<StorePageDesign> GetStorePageDesignsByStoreId(string search)
        {
            try
            {
                if (!String.IsNullOrEmpty(search))
                {
                    var returnList = this.FindBy(r => r.Name.Equals(search, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    return returnList;
                }
                else
                {
                    var returnList = this.GetAll().ToList();
                    return returnList;
                }


            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public List<StorePageDesign> GetActiveStoreDesings()
        {
            return this.FindBy(r => r.State).OrderBy(r => r.Ordering).ThenBy(r => r.Name).ToList();
        }
    }
}
