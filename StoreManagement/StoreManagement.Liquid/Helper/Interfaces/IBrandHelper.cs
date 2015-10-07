using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IBrandHelper : IHelper
    {
        StoreLiquidResult GetBrandsPartial(List<Brand> brands, PageDesign pageDesign);

        StoreLiquidResult GetBrandDetailPage(Brand brand, List<Product> products,
                                             PageDesign pageDesign,
                                             List<ProductCategory> productCategories);

        StoreLiquidResult GetBrandsIndexPage(PageDesign pageDesign, StorePagedList<Brand> brands);
    }
}
