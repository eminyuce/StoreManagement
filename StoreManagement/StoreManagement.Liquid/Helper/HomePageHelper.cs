using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper
{
    public class HomePageHelper : BaseLiquidHelper
    {

        public Dictionary<string, string> GetHomePageDesign(
         Task<List<Product>> productsTask,
         Task<List<Content>> blogsTask,
         Task<List<Content>> newsTask,
         Task<List<FileManager>> sliderTask,
         Task<PageDesign> pageDesignTask,
         Task<List<Category>> categoriesTask,
         Task<List<ProductCategory>> productCategoriesTask)
        {
            Task.WaitAll(productsTask, blogsTask, newsTask, pageDesignTask, sliderTask, categoriesTask, productCategoriesTask);
            var products = productsTask.Result;
            var blogs = blogsTask.Result;
            var news = newsTask.Result;
            var pageDesing = pageDesignTask.Result;
            var sliderImages = sliderTask.Result;
            var categories = categoriesTask.Result;
            var productCategories = productCategoriesTask.Result;


            var home = new HomePageLiquid(pageDesing, sliderImages);
            home.Products = products;
            home.Blogs = blogs;
            home.News = news;
            home.Categories = categories;
            home.ProductCategories = productCategories;

            object anonymousObject = new
            {


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesing.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            return dic;
        }


    }
}