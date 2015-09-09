using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{

    public class HomePageHelper : BaseLiquidHelper, IHomePageHelper
    {



        public StoreLiquidResult GetHomePageDesign(
         Task<List<Product>> productsTask,
         Task<List<Content>> blogsTask,
         Task<List<Content>> newsTask,
         Task<List<FileManager>> sliderTask,
         Task<PageDesign> pageDesignTask,
         Task<List<Category>> categoriesTask,
         Task<List<ProductCategory>> productCategoriesTask)
        {
            var isApi = ProjectAppSettings.GetWebConfigBool("IsApiService");


            Task.WaitAll(productsTask, blogsTask, newsTask, pageDesignTask, sliderTask, categoriesTask,
                         productCategoriesTask);

            List<Product> products = null;
            List<Content> blogs = null;
            List<Content> news = null;
            PageDesign pageDesing = null;
            List<FileManager> sliderImages = null;
            List<Category> categories = null;
            List<ProductCategory> productCategories = null;



            products = productsTask.Result;
            blogs = blogsTask.Result;
            news = newsTask.Result;
            pageDesing = pageDesignTask.Result;
            sliderImages = sliderTask.Result;
            categories = categoriesTask.Result;
            productCategories = productCategoriesTask.Result;




            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            result.LiquidRenderedResult = dic;
            try
            {


                if (pageDesing == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var home = new HomePageLiquid(pageDesing, sliderImages);
                home.Products = products;
                home.ImageWidthProduct = GetSettingValueInt("ProductsHomePage_ImageWidth", 50);
                home.ImageHeightProduct = GetSettingValueInt("ProductsHomePage_ImageHeight", 50);

                home.Blogs = blogs;
                home.ImageWidthBlog = GetSettingValueInt("BlogsHomePage_ImageWidth", 50);
                home.ImageHeightBlog = GetSettingValueInt("BlogsHomePage_ImageHeight", 50);


                home.News = news;
                home.ImageWidthNews = GetSettingValueInt("NewsHomePage_ImageWidth", 50);
                home.ImageHeightNews = GetSettingValueInt("NewsHomePage_ImageHeight", 50);



                home.ImageWidthSlider = GetSettingValueInt("SliderHomePage_ImageWidth", 500);
                home.ImageHeightSlider = GetSettingValueInt("SliderHomePage_ImageHeight", 500);


                home.Categories = categories;
                home.ProductCategories = productCategories;

                object anonymousObject = new
                {
                    blogs = LiquidAnonymousObject.GetContentLiquid(home.BlogsLiquidList),
                    products = LiquidAnonymousObject.GetProductsLiquid(home.ProductLiquidList),
                    news = LiquidAnonymousObject.GetContentLiquid(home.NewsLiquidList),
                    sliders = LiquidAnonymousObject.GetSliderImagesLiquidList(home.SliderImagesLiquid),


                };


                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesing.PageTemplate, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                dic[StoreConstants.PageOutput] = ex.StackTrace;
                Logger.Error(ex, ex.StackTrace);
            }

            return result;
        }

    }
}