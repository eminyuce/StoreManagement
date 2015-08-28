using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IStoreHelper
    {
        Store GetStoreByDomain(IStoreService storeService, HttpContextBase request);
        int GetStoreIdByDomain(IStoreService storeService, HttpContextBase request);
    }

}