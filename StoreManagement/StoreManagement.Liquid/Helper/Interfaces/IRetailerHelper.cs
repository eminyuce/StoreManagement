using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IRetailerHelper : IHelper
    {
        StoreLiquidResult GetRetailers(
            List<Retailer> labels,
            PageDesign pageDesign);
    }
}