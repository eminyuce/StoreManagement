using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class RssController : BaseController
    {

        [Inject]
        public IProductHelper ProductHelper { set; get; }

         
        public async Task<ActionResult> Products(int StoreId,int take = 1, int description = 300, int imageHeight = 50, int imageWidth = 50)
        {
            // var productsTask = ProductService.GetProductsAsync(StoreId, take, true);
            var productsTask = ProductRepository.GetProductsByProductType(StoreId, null, null, null, StoreConstants.ProductType, 1,
                                                             take, true, "random", null);
            var productCategoriesTask = ProductCategoryRepository.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            var storeTask = StoreRepository.GetStoreAsync(StoreId);

            await Task.WhenAll(storeTask, productsTask, productCategoriesTask);
            var store = storeTask.Result;
            var products = productsTask.Result;
            var productCategories = productCategoriesTask.Result;

            var feed = ProductHelper.GetProductsRssFeed(store, products, productCategories, description);
            ProductHelper.ImageWidth = imageWidth;
            ProductHelper.ImageHeight = imageHeight;
            ProductHelper.StoreId = StoreId;
            var comment = new StringBuilder();
            comment.AppendLine("Take=Number of rss item; Default value is 10  ");
            comment.AppendLine("Description=The length of description text.Default value is 300  ");
            return new FeedResult(feed, comment);
        }

	}
}