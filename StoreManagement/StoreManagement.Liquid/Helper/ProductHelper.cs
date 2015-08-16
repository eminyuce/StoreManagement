using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ProductHelper : BaseLiquidHelper
    {

        public Dictionary<string, string> GetProductsIndexPage(Task<StorePagedList<Product>> productsTask,
            Task<PageDesign> pageDesignTask, Task<List<ProductCategory>> categoriesTask)
        {

            Task.WaitAll(pageDesignTask, productsTask, categoriesTask);
            var products = productsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;
            var items = new List<ProductLiquid>();
            var cats = new List<ProductCategoryLiquid>();
            foreach (var item in products.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category, pageDesign, ImageWidth, ImageHeight);
                    items.Add(blog);
                }
                var catLiquid = new ProductCategoryLiquid(category, pageDesign);
                cats.Add(catLiquid);

            }

            object anonymousObject = new
                {
                    items = from s in items
                            select new
                                {
                                    CategoryName = s.Category.Name,
                                    ProductCategoryId = s.Product.ProductCategoryId,
                                    BrandId = s.Product.BrandId,
                                    CategoryDescription = s.Category.Description,
                                    ProductId = s.Product.Id,
                                    s.Product.Name,
                                    s.Product.Description,
                                    s.Product.ProductCode,
                                    s.Product.Price,
                                    s.Product.Discount,
                                    s.Product.UpdatedDate,
                                    s.Product.CreatedDate,
                                    s.Product.TotalRating,
                                    s.Product.UnitsInStock,
                                    s.DetailLink,
                                    images = s.ImageLiquid
                                },
                    categories = from s in cats
                                 select new
                                     {
                                         s.ProductCategory.Name,
                                         s.ProductCategory.Description,
                                         s.DetailLink,
                                     }
                };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, products.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, products.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, products.totalItemCount.ToStr());
            dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);



            return dic;
        }

        public Dictionary<String, String> GetProductsDetailPage(Task<Product> productsTask, Task<PageDesign> productsPageDesignTask, Task<ProductCategory> categoryTask)
        {
            Task.WaitAll(productsPageDesignTask, productsTask, categoryTask);
            var product = productsTask.Result;
            var pageDesign = productsPageDesignTask.Result;
            var category = categoryTask.Result;

            var s = new ProductLiquid(product, category, pageDesign, ImageWidth, ImageHeight);

            object anonymousObject = new
            {
                CategoryName = s.Category.Name,
                ProductCategoryId = s.Product.ProductCategoryId,
                BrandId = s.Product.BrandId,
                CategoryDescription = s.Category.Description,
                ProductId = s.Product.Id,
                s.Product.Name,
                s.Product.Description,
                s.Product.ProductCode,
                s.Product.Price,
                s.Product.Discount,
                s.Product.UpdatedDate,
                s.Product.CreatedDate,
                s.Product.TotalRating,
                s.Product.UnitsInStock,
                images = s.ImageLiquid
            };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            return dic;
        }

        public Dictionary<string, string> GetRelatedProductsPartialByCategory(Task<ProductCategory> categoryTask,
            Task<List<Product>> relatedProductsTask,
            Task<PageDesign> pageDesignTask
          )
        {
            Task.WaitAll(pageDesignTask, relatedProductsTask, categoryTask);
            var products = relatedProductsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {


                var items = new List<ProductLiquid>();
                foreach (var item in products)
                {
                    var blog = new ProductLiquid(item, category, pageDesign, ImageWidth, ImageHeight);
                    items.Add(blog);

                }

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
                {
                    items = from s in items
                            select new
                            {
                                ProductId = s.Product.Id,
                                s.Product.Name,
                                s.Product.Description,
                                s.Product.ProductCode,
                                s.Product.Price,
                                s.Product.Discount,
                                s.Product.UpdatedDate,
                                s.Product.CreatedDate,
                                s.Product.TotalRating,
                                s.Product.UnitsInStock,
                                s.DetailLink,
                                CategoryName = s.Category.Name,
                                ProductCategoryId = s.Product.ProductCategoryId,
                                BrandId = s.Product.BrandId,
                                CategoryDescription = s.Category.Description,
                                ImageWidth,
                                ImageHeight,
                                images = s.ImageLiquid
                            }
                }
                    );


                dic[StoreConstants.PageOutput] = indexPageOutput;


                return dic;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return dic;
            }
        }

        public Dictionary<string, string> GetRelatedProductsPartialByBrand(Task<Brand> brandTask,
            Task<List<Product>> relatedProductsTask,
            Task<PageDesign> pageDesignTask,
            Task<List<ProductCategory>> productCategoriesTask)
        {

            Task.WaitAll(pageDesignTask, relatedProductsTask, brandTask, productCategoriesTask);
            var products = relatedProductsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var brand = brandTask.Result;
            var productCategories = productCategoriesTask.Result;

            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {

                var brandLiquid = new BrandLiquid(brand, pageDesign, ImageWidth, ImageHeight);

                var items = new List<ProductLiquid>();
                foreach (var item in products)
                {
                    var imageWidth = GetSettingValueInt("BrandProduct_ImageWidth", 50);
                    var imageHeight = GetSettingValueInt("BrandProduct_ImageHeight", 50);
                    var cat = productCategories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                    var product = new ProductLiquid(item, cat, pageDesign, imageWidth, imageHeight);
                    product.Brand = brand;
                    items.Add(product);

                }

                object anonymousObject = new
                    {
                        items = from s in items
                                select new
                                    {
                                        s.Product.Name,
                                        s.Product.Description,
                                        s.Product.ProductCode,
                                        s.Product.Price,
                                        s.Product.Discount,
                                        s.Product.UpdatedDate,
                                        s.Product.CreatedDate,
                                        s.Product.TotalRating,
                                        s.Product.UnitsInStock,
                                        s.DetailLink,
                                        CategoryName = s.Category.Name,
                                        ProductCategoryId = s.Product.ProductCategoryId,
                                        BrandId = s.Product.BrandId,
                                        CategoryDescription = s.Category.Description,
                                        ImageWidth,
                                        ImageHeight,
                                        images = s.ImageLiquid
                                    },
                        brand = new
                            {
                                brandLiquid,
                                Name = brandLiquid.Brand.Name,
                                Description = brandLiquid.Brand.Description
                            }
                    };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;


                return dic;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return dic;
            }
        }
    }
}