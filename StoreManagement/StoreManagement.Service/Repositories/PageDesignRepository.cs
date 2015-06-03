using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using GenericRepository.EntityFramework;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;


namespace StoreManagement.Service.Repositories
{
    public class PageDesignRepository : BaseRepository<PageDesign, int>, IPageDesignRepository
    {

        public PageDesignRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<PageDesign> GetPageDesignByStoreId(int storeId)
        {
            return this.FindBy(r => r.StoreId == storeId).ToList();
        }
    }
}


