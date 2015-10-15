﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;


namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IPageDesignRepository : IBaseRepository<PageDesign, int>, IPageDesignService, IDisposable 
    {
          List<PageDesign> GetPageDesignByStoreId(int storeId, string search);
          PageDesign GetPageDesignByNameSync(int storeId, string name);
    }
}
