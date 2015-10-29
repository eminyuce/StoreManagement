using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    
    public class ProductHelper : BaseLiquidHelper, IProductHelper
    {

        public StoreLiquidResult GetProductsIndexPage(StorePagedList<Product> products,
            PageDesign pageDesign, List<ProductCategory> categories)
        {

            var items = new List<ProductLiquid>();
            var cats = new List<ProductCategoryLiquid>();
            foreach (var item in products.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category,  ImageWidth, ImageHeight);
                    items.Add(blog);
                }
            }
            foreach (var category in categories)
            {
                var catLiquid = new ProductCategoryLiquid(category);
                catLiquid.Count = products.items.Count(r => r.ProductCategoryId == category.Id);
                cats.Add(catLiquid);
            }

            object anonymousObject = new
                {

                    products = LiquidAnonymousObject.GetProductsLiquid(items),
                    categories = LiquidAnonymousObject.GetProductCategories(cats)
                };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, products.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, products.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, products.totalItemCount.ToStr());
            //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            //dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);



            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;

            return result;
        }

        public StoreLiquidResult GetPopularProducts(List<Product> products, List<ProductCategory> productCategories, PageDesign pageDesign)
        {
            
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {


                var items = new List<ProductLiquid>();
                foreach (var item in products)
                {
                    var cat = productCategories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                    var product = new ProductLiquid(item, cat,  this.ImageWidth, this.ImageHeight);
                    items.Add(product);

                }

                object anonymousObject = new
                {
                    products = LiquidAnonymousObject.GetProductsLiquid(items) 
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;
                result.LiquidRenderedResult = dic;

                return result;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPopularProducts");
                dic.Add(StoreConstants.PageOutput, ex.Message);
                return result;
            }
        }


        public StoreLiquidResult GetProductsDetailPage(Product product, PageDesign pageDesign, ProductCategory category)
        {
            if (product == null)
            {
                throw new Exception("Product is NULL");
            }
            if (pageDesign == null)
            {
                throw new Exception("pageDesign is NULL");
            }

            if (category == null)
            {
                throw new Exception("ProductCategory is NULL");
            }
           
           
            var s = new ProductLiquid(product, category,  ImageWidth, ImageHeight);

            var anonymousObject = LiquidAnonymousObject.GetProductAnonymousObject(s);

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            result.DetailLink = s.DetailLink;
            return result;
        }

     

        public StoreLiquidResult  GetRelatedProductsPartialByCategory(ProductCategory category,
            List<Product> products,
           PageDesign pageDesign
          )
        {
           
            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            var dic = new Dictionary<String, String>();
            try
            {


                var items = new List<ProductLiquid>();
                foreach (var item in products)
                {
                    var blog = new ProductLiquid(item, category,  ImageWidth, ImageHeight);
                    items.Add(blog);

                }

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
                {
                    products = LiquidAnonymousObject.GetProductsLiquid(items)
                }
                    );


                dic[StoreConstants.PageOutput] = indexPageOutput;



                result.LiquidRenderedResult = dic;
                return result;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return result;
            }
        }

        public StoreLiquidResult GetRelatedProductsPartialByBrand(Brand brand,
            List<Product> products,
            PageDesign pageDesign,
            List<ProductCategory> productCategories)
        {

         
            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {

                var brandLiquid = new BrandLiquid(brand, ImageWidth, ImageHeight);

                var items = new List<ProductLiquid>();
                foreach (var item in products)
                {
                    var imageWidth = GetSettingValueInt("BrandProduct_ImageWidth", 50);
                    var imageHeight = GetSettingValueInt("BrandProduct_ImageHeight", 50);
                    var cat = productCategories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                    var product = new ProductLiquid(item, cat,  imageWidth, imageHeight);
                    product.Brand = brand;
                    items.Add(product);

                }

                object anonymousObject = new
                    {
                        products = LiquidAnonymousObject.GetProductsLiquid(items),
                        brand = LiquidAnonymousObject.GetBrandLiquid(brandLiquid)
                    };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;
                result.LiquidRenderedResult = dic;

                return result;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return result;
            }
        }

 
        public Rss20FeedFormatter GetProductsRssFeed(Store store , List<Product> products, List<ProductCategory> productCategories, int description)
        {
     
            try
            {
                String url = "http://login.seatechnologyjobs.com/";

                var feed = new SyndicationFeed(store.Name, "", new Uri(url))
                {
                    Language = "en-US"
                };


                var feedItemList = new List<SyndicationItem>();
                foreach (var product in products)
                {
                    try
                    {
                        var feedItem = GetSyndicationItem(store, product,
                                                                             productCategories.FirstOrDefault(r => r.Id == product.ProductCategoryId),
                                                                             description);
                        if (feedItem != null)
                        {
                            feedItemList.Add(feedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);    
                    }

                }
                feed.Items = feedItemList;


                feed.AddNamespace("products", "https://www.maritimejobs.com/jobs");
                var rssFeed = new Rss20FeedFormatter(feed);
 

                return rssFeed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return null;
            }
        }

      

        private SyndicationItem GetSyndicationItem(Store store, Product product, ProductCategory productCategory, int description)
        {
            if (productCategory == null)
                return null;

            if (description == 0)
                description = 300;

            var productDetailLink = LinkHelper.GetProductLink(product, productCategory.Name);
            String detailPage = String.Format("http://{0}{1}", store.Domain, productDetailLink);

            string desc = "";
            if (description > 0)
            {
                desc = GeneralHelper.GetDescription(product.Description,description);
            }

            var si = new SyndicationItem(product.Name, desc, new Uri(detailPage));
            if (product.UpdatedDate != null)
            {
                si.PublishDate = product.UpdatedDate.Value.ToUniversalTime();
            }


            if (!String.IsNullOrEmpty(productCategory.Name))
            {
                si.ElementExtensions.Add("Category", String.Empty, productCategory.Name);
            }

            


            if (product.ProductFiles.Any())
            {
                var mainImage =  product.ProductFiles.FirstOrDefault(r => r.IsMainImage);
                if (mainImage == null)
                {
                    mainImage = product.ProductFiles.FirstOrDefault();
                } 
                
                if (mainImage != null)
                {
                    
            
                    string imageSrc = LinkHelper.GetImageLink("Thumbnail",mainImage.FileManager , this.ImageWidth,this.ImageHeight);
                    if (!string.IsNullOrEmpty(imageSrc))
                    {
                        try
                        {
                            SyndicationLink imageLink =
                                SyndicationLink.CreateMediaEnclosureLink(new Uri(imageSrc), "image/jpeg", 100);
                            si.Links.Add(imageLink);
                        }
                        catch (Exception e)
                        {
                        
                        }

                    }
                }
            }

            return si;

        }


    }
}