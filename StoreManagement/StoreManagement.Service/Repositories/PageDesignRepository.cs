using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using GenericRepository.EntityFramework;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;


namespace StoreManagement.Service.Repositories
{
    public class PageDesignRepository : BaseRepository<PageDesign, int>, IPageDesignRepository
    {

        public PageDesignRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<PageDesign> GetPageDesignByStoreId(int storeId, string search)
        {
            var pages = this.FindBy(r => r.StoreId == storeId);


            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                pages = pages.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()) || r.PageTemplate.ToLower().Contains(search.ToLower().Trim()) || r.Type.ToLower().Contains(search.ToLower().Trim()));
            }

            return pages.OrderByDescending(r => r.UpdatedDate).ToList();

        }

        public Task<PageDesign> GetPageDesignByName(int storeId, string name)
        {
            var res = Task.Factory.StartNew(() =>
                {
                    return this.FindBy(r => r.StoreId == storeId &&
                          r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                          .FirstOrDefault();

                });
            return res;

        }
    }
}


