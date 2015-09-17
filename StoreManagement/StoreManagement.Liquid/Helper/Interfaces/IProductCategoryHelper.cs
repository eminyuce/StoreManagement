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
    public interface IProductCategoryHelper : IHelper
    {
        StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<ProductCategory> categories);
        StoreLiquidResult GetProductCategoriesPartial(List<ProductCategory> categories, PageDesign pageDesign);

        StoreLiquidResult GetCategoryPage(PageDesign pageDesign, ProductCategory categories, StorePagedList<Product> products);
    }
}