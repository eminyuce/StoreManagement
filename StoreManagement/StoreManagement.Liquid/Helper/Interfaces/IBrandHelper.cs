using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IBrandHelper : IHelper
    {
        StoreLiquidResult GetBrandsPartial(Task<List<Brand>> brandsTask, Task<PageDesign> pageDesignTask);

        StoreLiquidResult GetBrandDetailPage(Task<Brand> brandTask, Task<List<Product>> productsTask,
                                             Task<PageDesign> pageDesignTask,
                                             Task<List<ProductCategory>> productCategoriesTask);

    }
}
