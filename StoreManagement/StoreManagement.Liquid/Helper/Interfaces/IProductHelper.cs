using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IProductHelper : IHelper
    {
        StoreLiquidResult GetProductsIndexPage(StorePagedList<Product> products,
                                                               PageDesign pageDesign, List<ProductCategory> categories);

        StoreLiquidResult GetProductsDetailPage(Product products, PageDesign productsPageDesignTask, ProductCategory category);

        StoreLiquidResult GetRelatedProductsPartialByCategory(ProductCategory category,
                                                                              List<Product> relatedProducts,
                                                                              PageDesign pageDesign);

        StoreLiquidResult GetRelatedProductsPartialByBrand(Brand brandTask, List<Product> relatedProducts, PageDesign pageDesignTask,
                                                                           List<ProductCategory> productCategories);

        Rss20FeedFormatter GetProductsRssFeed(Store store,List<Product> products,    List<ProductCategory>  productCategories, int description);

        StoreLiquidResult GetPopularProducts(List<Product> products, List<ProductCategory> productCategories, PageDesign pageDesign);

        StoreLiquidResult GetProductsSearchPage(Controller productController,
            ProductsSearchResult productSearchResult, 
            PageDesign pageDesign, 
            List<ProductCategory> categories, 
            String search,
            String filters, String headerText);
    }
}