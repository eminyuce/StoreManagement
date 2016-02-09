using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IProductCategoryService : IBaseService
    { 
        ProductCategoryViewModel GetProductCategory(string id, int page);

        StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<ProductCategory> categories);
        StoreLiquidResult GetProductCategoriesPartial(List<ProductCategory> categories, PageDesign pageDesign);

        StoreLiquidResult GetCategoryPage(PageDesign pageDesign, ProductCategory categories);
    }
}
