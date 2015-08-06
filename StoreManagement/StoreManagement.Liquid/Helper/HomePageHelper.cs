using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper
{
    public class HomePageHelper
    {

        public static Dictionary<string, string> GetHomePageDesign(
         Task<List<Product>> productsTask,
         Task<List<Content>> blogsTask,
         Task<List<Content>> newsTask,
         Task<List<FileManager>> slider,
         Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask, productsTask);
            var products = productsTask.Result;
            var blogs = blogsTask.Result;
            var news = newsTask.Result;
            var pageDesing = pageDesignTask.Result;
            var sliderImages = slider.Result;

            var home = new HomePageLiquid(pageDesing, sliderImages);
            home.Products = products;
            home.Blogs = blogs;
            home.News = news;


            object anonymousObject = new
            {
                

            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesing.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);


            return dic;
        }


    }
}