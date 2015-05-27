using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class PageDesignService : BaseService, IPageDesignService
    {
        public PageDesignService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        public List<PageDesign> GetPageDesignByStoreId(int storeId)
        {
            throw new NotImplementedException();
        }
    }
}
