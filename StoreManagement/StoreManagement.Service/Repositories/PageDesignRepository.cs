using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public List<PageDesign> GetPageDesignByStoreId(int storePageDesignId, string search)
        {
            var pages = this.FindBy(item => item.StorePageDesignId == storePageDesignId);
           
            
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                pages = pages.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()) || r.PageTemplate.ToLower().Contains(search.ToLower().Trim()) || r.Type.ToLower().Contains(search.ToLower().Trim()));
            }

            return pages.OrderByDescending(r => r.UpdatedDate).ToList();

        }

        public PageDesign GetPageDesignByNameSync(int storePageDesignId, string name)
        {
            var item1 =  this.FindBy(item => item.StorePageDesignId == storePageDesignId && item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
           
            return item1;
        }

        public async Task<PageDesign> GetPageDesignByName(int storeId, string name)
        {
            IQueryable<PageDesign> item1 = from s in this.StoreDbContext.Stores
                                    join c in this.StoreDbContext.StorePageDesigns on s.StorePageDesignId equals c.Id
                                    join u in this.StoreDbContext.PageDesigns on c.Id equals u.StorePageDesignId
                                    where s.Id == storeId && u.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                                    select u;

            var item = await  item1.Where(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();
            return item;
            // return item1.FirstOrDefault();

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}


