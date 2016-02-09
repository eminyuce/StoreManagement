using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Services.IServices
{
    public interface IBrandService : IBaseService
    {
        StoreLiquidResult GetBrandsPartial(List<Brand> brands, PageDesign pageDesign);

        StoreLiquidResult GetBrandDetailPage(Brand brand, List<Product> products,
                                             PageDesign pageDesign,
                                             List<ProductCategory> productCategories);

        StoreLiquidResult GetBrandsIndexPage(PageDesign pageDesign, StorePagedList<Brand> brands);
    }
}
