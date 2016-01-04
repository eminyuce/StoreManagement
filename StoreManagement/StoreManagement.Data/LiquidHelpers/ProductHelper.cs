using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Data.LiquidHelpers.Interfaces;

namespace StoreManagement.Data.LiquidHelpers
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
                    var blog = new ProductLiquid(item, category, ImageWidth, ImageHeight);
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

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, products.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, products.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, products.totalItemCount.ToStr());



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
                    var product = new ProductLiquid(item, cat, this.ImageWidth, this.ImageHeight);
                    items.Add(product);

                }

                object anonymousObject = new
                {
                    products = LiquidAnonymousObject.GetProductsLiquid(items)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


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

        public StoreLiquidResult GetProductsSearchPage(Controller controller, 
            ProductsSearchResult productSearchResult, 
            PageDesign pageDesign,
            List<ProductCategory> categories, 
            String search,
            String filters,
            String headerText, 
            String categoryApiId)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            var items = new List<ProductLiquid>();
            var cats = new List<ProductCategoryLiquid>();
            var products = productSearchResult.Products;
            var filterGroups = productSearchResult.FiltersGroups;
            var httpContextRequest = controller.Request;
            foreach (FilterGroup filterGroup in filterGroups)
            {
                foreach (Data.HelpersModel.Filter filter in filterGroup.FiltersHidden)
                {
                    if (string.IsNullOrEmpty(filter.Text))
                    {
                        continue;
                    }

                    filter.FilterLink = filter.Link(httpContextRequest);
                }
            }

            var filtersList = FilterHelper.GetFiltersFromContextRequest(httpContextRequest);
            foreach (var filter in filtersList)
            {
                filter.FilterLink =  filter.LinkExclude(httpContextRequest, productSearchResult.Stats.OwnerType);
            }

            foreach (var item in products)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category, ImageWidth, ImageHeight);
                    items.Add(blog);
                }
            }

            var selectedCategory = categories.FirstOrDefault(r => r.ApiCategoryId.Equals(categoryApiId,StringComparison.InvariantCultureIgnoreCase));
            ProductCategoryLiquid selectedCategoryLiquid = null;
            if (selectedCategory == null)
            {
                selectedCategory=categories.FirstOrDefault();
            }
            selectedCategoryLiquid = new ProductCategoryLiquid(selectedCategory);


            var categoryTree = controller.RenderPartialToStringCache(
                        "pCreateCategoryTree",
                        new ViewDataDictionary(categories));



            object anonymousObject = new
            {
                filterExcluded = LiquidAnonymousObject.GetFilters(filtersList),
                filterGroup = LiquidAnonymousObject.GetFilterGroup(filterGroups),
                products = LiquidAnonymousObject.GetProductsLiquid(items),
                selectedCategory = LiquidAnonymousObject.GetProductCategory(selectedCategoryLiquid),
                categoryTree = categoryTree,
                search = search,
                filters=filters,
                headerText=headerText,
                recordsTotal = productSearchResult.Stats.RecordsTotal,
                isCleanButton = !String.IsNullOrEmpty(search) || !String.IsNullOrEmpty(filters)
            };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);
            dic[StoreConstants.PageOutput] = indexPageOutput;
            dic.Add(StoreConstants.PageSize, productSearchResult.PageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, productSearchResult.Stats.PageCurrent.ToStr());
            dic.Add(StoreConstants.TotalItemCount, productSearchResult.Stats.RecordsTotal.ToStr());

            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            if (selectedCategory != null)
            {
                result.PageTitle = selectedCategory.Name + " Products";
            }
            else
            {
                result.PageTitle = "Products";
            }
            return result;
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


            var s = new ProductLiquid(product, category, ImageWidth, ImageHeight);

            var anonymousObject = LiquidAnonymousObject.GetProductAnonymousObject(s);

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            result.DetailLink = s.DetailLink;
            return result;
        }



        public StoreLiquidResult GetRelatedProductsPartialByCategory(ProductCategory category,
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
                    var blog = new ProductLiquid(item, category, ImageWidth, ImageHeight);
                    items.Add(blog);

                }

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, new
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
                    var product = new ProductLiquid(item, cat, imageWidth, imageHeight);
                    product.Brand = brand;
                    items.Add(product);

                }

                object anonymousObject = new
                    {
                        products = LiquidAnonymousObject.GetProductsLiquid(items),
                        brand = LiquidAnonymousObject.GetBrandLiquid(brandLiquid)
                    };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


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


        public Rss20FeedFormatter GetProductsRssFeed(Store store, List<Product> products, List<ProductCategory> productCategories, int description, int isDetailLink)
        {

            try
            {
                String url = "http://www." + store.Domain.ToLower();

                var feed = new SyndicationFeed(store.Name, store.Description, new Uri(url))
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
                                                                             description, isDetailLink);
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


                feed.AddNamespace("products", url + "/products");
                var rssFeed = new Rss20FeedFormatter(feed);


                return rssFeed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductsRssFeed");
                return null;
            }
        }



        private SyndicationItem GetSyndicationItem(Store store, Product product, ProductCategory productCategory, int description, int isDetailLink)
        {
            if (productCategory == null)
                return null;

            if (description == 0)
                description = 300;

            var productDetailLink = LinkHelper.GetProductLink(product, productCategory.Name);
            String detailPage = String.Format("http://{0}{1}", store.Domain.ToLower(), productDetailLink);
            if (isDetailLink == 1)
            {
                detailPage = String.Format("http://{0}{1}", store.Domain.ToLower(), "/products/productbuy/"+product.Id);
            }
            string desc = "";
            if (description > 0)
            {
                desc = GeneralHelper.GeneralHelper.GetDescription(product.Description, description);
            }
            var uri = new Uri(detailPage);
            var si = new SyndicationItem(product.Name, desc, uri);
            if (product.UpdatedDate != null)
            {
                si.PublishDate = product.UpdatedDate.Value.ToUniversalTime();
            }


            if (!String.IsNullOrEmpty(productCategory.Name))
            {
                si.ElementExtensions.Add("products:category", String.Empty, productCategory.Name);
            }
            si.ElementExtensions.Add("guid", String.Empty, uri);



            if (product.ProductFiles.Any())
            {

                List<BaseFileEntity> baseFileEntities = product.ProductFiles != null && product.ProductFiles.Any() ? product.ProductFiles.Cast<BaseFileEntity>().ToList() : new List<BaseFileEntity>();
                var imageLiquid = new ImageLiquid(baseFileEntities, this.ImageWidth, this.ImageHeight);
                imageLiquid.ImageState = true;
                String imageSrcHtml = String.Format("http://{0}{1}", store.Domain.ToLower(), imageLiquid.SmallImageSource);
                try
                {
                    SyndicationLink imageLink =
                        SyndicationLink.CreateMediaEnclosureLink(new Uri(imageSrcHtml), "image/jpeg", 100);
                    si.Links.Add(imageLink);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetSyndicationItem");
                }

            }

            return si;

        }


    }
}