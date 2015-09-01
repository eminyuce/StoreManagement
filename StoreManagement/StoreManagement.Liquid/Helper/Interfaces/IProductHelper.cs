using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IProductHelper : IHelper
    {
        StoreLiquidResult GetProductsIndexPage(Task<StorePagedList<Product>> productsTask,
                                                               Task<PageDesign> pageDesignTask, Task<List<ProductCategory>> categoriesTask);

        StoreLiquidResult GetProductsDetailPage(Task<Product> productsTask, Task<PageDesign> productsPageDesignTask, Task<ProductCategory> categoryTask);

        StoreLiquidResult GetRelatedProductsPartialByCategory(Task<ProductCategory> categoryTask,
                                                                               Task<List<Product>> relatedProductsTask,
                                                                               Task<PageDesign> pageDesignTask
            );

        StoreLiquidResult GetRelatedProductsPartialByBrand(Task<Brand> brandTask,
                                                                           Task<List<Product>> relatedProductsTask,
                                                                           Task<PageDesign> pageDesignTask,
                                                                           Task<List<ProductCategory>> productCategoriesTask);

        Rss20FeedFormatter GetProductsRssFeed(Task<Store> storeTask,Task<List<Product>> productsTask,    Task<List<ProductCategory>>  productCategoriesTask, int description);
    }
}