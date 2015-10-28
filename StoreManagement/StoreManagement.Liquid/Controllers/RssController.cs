using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.ActionResults;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class RssController : BaseController
    {


        public async Task<ActionResult> Products(int take = 15, int description = 300, int imageHeight = 50, int imageWidth = 50)
        {
            var productsTask = ProductService.GetProductsAsync(StoreId, take, true);
            var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            var storeTask = StoreService.GetStoreAsync(StoreId);

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

        public async Task<ActionResult> News(int take = 15, int description = 250, int imageHeight = 50, int imageWidth = 50)
        {
            var contentsTask = ContentService.GetContentByTypeAsync(StoreId, take, true, StoreConstants.NewsType);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.NewsType, true);
            var storeTask = StoreService.GetStoreAsync(StoreId);

            await Task.WhenAll(storeTask, contentsTask, categoriesTask);
            var store = storeTask.Result;
            var content = contentsTask.Result;
            var categories = categoriesTask.Result;

            var feed = ContentHelper.GetContentsRssFeed(store, content, categories, description, StoreConstants.NewsType);
            ProductHelper.ImageWidth = imageWidth;
            ProductHelper.ImageHeight = imageHeight;
            ProductHelper.StoreId = StoreId;
            var comment = new StringBuilder();
            comment.AppendLine("Take=Number of rss item; Default value is 10  ");
            comment.AppendLine("Description=The length of description text.Default value is 300  ");
            return new FeedResult(feed, comment);
        }

        public async Task<ActionResult> Blogs(int take = 15, int description = 250, int imageHeight = 50, int imageWidth = 50)
        {
            var contentsTask = ContentService.GetContentByTypeAsync(StoreId, take, true, StoreConstants.BlogsType);
            var categoriesTask = CategoryService.GetCategoriesByStoreIdAsync(StoreId, StoreConstants.BlogsType, true);
            var storeTask = StoreService.GetStoreAsync(StoreId);

            await Task.WhenAll(storeTask, contentsTask, categoriesTask);
            var store = storeTask.Result;
            var content = contentsTask.Result;
            var categories = categoriesTask.Result;


            var feed = ContentHelper.GetContentsRssFeed(store, content, categories, description, StoreConstants.BlogsType);
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