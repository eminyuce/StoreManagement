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
        StoreLiquidResult GetCategoriesIndexPage(Task<PageDesign> pageDesignTask, Task<StorePagedList<ProductCategory>> categoriesTask);
        StoreLiquidResult GetProductCategoriesPartial(Task<List<ProductCategory>> categoriesTask, Task<PageDesign> pageDesignTask);
    }
}