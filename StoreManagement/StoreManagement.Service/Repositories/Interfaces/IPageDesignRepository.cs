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
    public interface IPageDesignRepository : IBaseRepository<PageDesign, int>, IPageDesignGeneralRepository, IDisposable 
    {
        List<PageDesign> GetPageDesignByStoreId(int storePageDesignId, string search);
          PageDesign GetPageDesignByNameSync(int storePageDesignId, string name);
    }
}
