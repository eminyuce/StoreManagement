using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.ActionResults;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    public class RssController : BaseController
    {


        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task<ActionResult> Products(int take = 1, int description = 300, int imageHeight = 50, int imageWidth = 50, int isDetailLink = 0)
        {
            // var productsTask = ProductService.GetProductsAsync(StoreId, take, true);
            var productsTask = ProductService.GetProductsByProductType(StoreId, null, null, null, StoreConstants.ProductType, 1,
                                                             take, true, "random", null);
            var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            var storeTask = StoreService.GetStoreAsync(StoreId);

            await Task.WhenAll(storeTask, productsTask, productCategoriesTask);
            var store = storeTask.Result;
            var products = productsTask.Result;
            var productCategories = productCategoriesTask.Result;
            var rssHelper = new RssHelper();
            var feed = rssHelper.GetProductsRssFeed(store, products, productCategories, description, isDetailLink);
            rssHelper.ImageWidth = imageWidth;
            rssHelper.ImageHeight = imageHeight;
    
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
            var rssHelper = new RssHelper();
            var feed = rssHelper.GetContentsRssFeed(store, content, categories, description, StoreConstants.NewsType);
            rssHelper.ImageWidth = imageWidth;
            rssHelper.ImageHeight = imageHeight;
 
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

            var rssHelper = new RssHelper();
            var feed = rssHelper.GetContentsRssFeed(store, content, categories, description, StoreConstants.BlogsType);
            rssHelper.ImageWidth = imageWidth;
            rssHelper.ImageHeight = imageHeight;
 
            var comment = new StringBuilder();
            comment.AppendLine("Take=Number of rss item; Default value is 10  ");
            comment.AppendLine("Description=The length of description text.Default value is 300  ");
            return new FeedResult(feed, comment);
        }
	}
}