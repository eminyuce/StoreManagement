using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Service.Services.IServices
{
    public interface IRetailerService : IBaseService
    {
        StoreLiquidResult GetRetailers(
            List<Retailer> labels,
            PageDesign pageDesign);

        StoreLiquidResult GetRetailerDetailPage(Retailer retailer,
                                                List<Product> products, PageDesign pageDesign,
                                                List<ProductCategory> productCategories);
    }
}
