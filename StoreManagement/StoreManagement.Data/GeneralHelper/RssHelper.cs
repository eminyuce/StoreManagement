using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.GeneralHelper
{
            
    public class RssHelper
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
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
                detailPage = String.Format("http://{0}{1}", store.Domain.ToLower(), "/products/productbuy/" + product.Id);
            }
            string desc = "";
            if (description > 0)
            {
                desc = GeneralHelper.GetDescription(product.Description, description);
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


        public Rss20FeedFormatter GetContentsRssFeed(Store store, List<Content> contents, List<Category> categories, int description, string type)
        {

            try
            {
                String url = "http://www." + store.Domain.ToLower();

                var feed = new SyndicationFeed(store.Name, "", new Uri(url))
                {
                    Language = "en-US"
                };


                var feedItemList = new List<SyndicationItem>();
                foreach (var product in contents)
                {
                    try
                    {
                        var feedItem = GetSyndicationItem(store,
                            product,
                            categories.FirstOrDefault(r => r.Id == product.CategoryId),
                             description,
                             type);


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


                feed.AddNamespace(type, url + "/" + type);
                var rssFeed = new Rss20FeedFormatter(feed);


                return rssFeed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return null;
            }
        }


        private SyndicationItem GetSyndicationItem(Store store, Content product, Category category, int description, String type)
        {
            if (category == null)
                return null;

            if (description == 0)
                description = 300;

            var productDetailLink = LinkHelper.GetContentLink(product, category.Name, type);
            String detailPage = String.Format("http://{0}{1}", store.Domain.ToLower(), productDetailLink);

            string desc = "";
            if (description > 0)
            {
                desc = GeneralHelper.GetDescription(product.Description, description);
            }
            var uri = new Uri(detailPage);
            var si = new SyndicationItem(product.Name, desc, uri);
            si.ElementExtensions.Add("guid", String.Empty, uri);
            if (product.UpdatedDate != null)
            {
                si.PublishDate = product.UpdatedDate.Value.ToUniversalTime();
            }


            if (!String.IsNullOrEmpty(category.Name))
            {
                si.ElementExtensions.Add(type + ":category", String.Empty, category.Name);
            }




            if (product.ContentFiles.Any())
            {
                var mainImage = product.ContentFiles.FirstOrDefault(r => r.IsMainImage);
                if (mainImage == null)
                {
                    mainImage = product.ContentFiles.FirstOrDefault();
                }

                if (mainImage != null)
                {


                    string imageSrc = LinkHelper.GetImageLink("Thumbnail", mainImage.FileManager, this.ImageWidth, this.ImageHeight);
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
