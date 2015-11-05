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

        public StoreLiquidResult GetHomePageDesign(PageDesign pageDesing, List<FileManager> sliderImages, List<Product> products, List<Content> blogs, List<Content> news,
                                         List<Category> categories, List<ProductCategory> productCategories)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesing.Name;
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


                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesing, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;
            }
            catch (Exception ex)
            {
                dic[StoreConstants.PageOutput] = ex.StackTrace;
                Logger.Error(ex, ex.StackTrace);
            }

            return result;
        }

        public StoreLiquidResult GetHomePageDesign(PageDesign pageDesing, List<FileManager> sliderImages)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesing.Name;
            try
            {
                if (pageDesing == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var home = new HomePageLiquid(pageDesing, sliderImages);

                home.ImageWidthSlider = GetSettingValueInt("SliderHomePage_ImageWidth", 500);
                home.ImageHeightSlider = GetSettingValueInt("SliderHomePage_ImageHeight", 500);



                object anonymousObject = new
                {
                    sliders = LiquidAnonymousObject.GetSliderImagesLiquidList(home.SliderImagesLiquid)
                };


                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesing, anonymousObject);
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