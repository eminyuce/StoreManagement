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



        public StoreLiquidResult GetHomePageDesign(
         Task<List<Product>> productsTask,
         Task<List<Content>> blogsTask,
         Task<List<Content>> newsTask,
         Task<List<FileManager>> sliderTask,
         Task<PageDesign> pageDesignTask,
         Task<List<Category>> categoriesTask,
         Task<List<ProductCategory>> productCategoriesTask)
        {
            Task.WaitAll(productsTask, blogsTask, newsTask, pageDesignTask, sliderTask, categoriesTask, productCategoriesTask);

            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            result.LiquidRenderedResult = dic;
            try
            {
                var products = productsTask.Result;
                var blogs = blogsTask.Result;
                var news = newsTask.Result;
                var pageDesing = pageDesignTask.Result;
                var sliderImages = sliderTask.Result;
                var categories = categoriesTask.Result;
                var productCategories = productCategoriesTask.Result;
                
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
                    blogs = from s in home.BlogsLiquidList
                            select new
                            {
                                s.Content.Name,
                                s.Content.Description,
                                s.DetailLink,
                                images = s.ImageLiquid
                            },
                    products = from s in home.ProductLiquidList
                               select new
                               {
                                   s.Product.Name,
                                   s.Product.Description,
                                   s.DetailLink,
                                   images = s.ImageLiquid
                               },
                    news = from s in home.NewsLiquidList
                           select new
                           {
                               s.Content.Name,
                               s.Content.Description,
                               s.DetailLink,
                               images = s.ImageLiquid
                           },
                    sliders = from s in home.SliderImagesLiquid
                             select new
                             {
                                 imagesource=s.ImageSource,
                                 s.FileManager.Title,
                                 s.FileManager.OriginalFilename,
                                 s.FileManager.GoogleImageId,
                                 s.FileManager.Width,
                                 s.FileManager.Height,
                                 s.ImageHeight,
                                 s.ImageWidth
                             },
                     

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