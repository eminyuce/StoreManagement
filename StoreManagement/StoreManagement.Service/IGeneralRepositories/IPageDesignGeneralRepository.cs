using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IPageDesignGeneralRepository : IGeneralRepository
    {
        Task<PageDesign> GetPageDesignByName(int storeId, string name);
    }
}
