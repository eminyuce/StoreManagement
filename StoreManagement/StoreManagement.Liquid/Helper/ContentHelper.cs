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


    public class ContentHelper : BaseLiquidHelper, IContentHelper
    {




        public StoreLiquidResult GetContentsIndexPage(
           StorePagedList<Content> contents,
           PageDesign pageDesign,
               List<Category> categories, String type)
        {




            var items = new List<ContentLiquid>();
            var cats = new List<CategoryLiquid>();
            foreach (var item in contents.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                if (category != null)
                {
                    var blog = new ContentLiquid(item, category, type, ImageWidth, ImageHeight);
                    items.Add(blog);
                }
            }
            foreach (var category in categories)
            {
                var catLiquid = new CategoryLiquid(category, type);
                catLiquid.Count = contents.items.Count(r => r.CategoryId == category.Id);
                cats.Add(catLiquid);
            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, new
            {
                items = LiquidAnonymousObject.GetContentLiquid(items),
                categories = LiquidAnonymousObject.GetCategoriesLiquid(cats)
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, contents.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, contents.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, contents.totalItemCount.ToStr());
            //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            //dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);

            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }


        public StoreLiquidResult GetContentDetailPage(Content content, PageDesign pageDesign, Category category, String type)
        {

            var items = new List<ContentLiquid>();
            var contentLiquid = new ContentLiquid(content, category, type, ImageWidth, ImageHeight);

            var anonymousObject = LiquidAnonymousObject.GetContentAnonymousObject(contentLiquid);

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }



        public StoreLiquidResult GetRelatedContentsPartial(Category category, List<Content> contents, PageDesign pageDesign, String type)
        {


            var items = new List<ContentLiquid>();
            foreach (var item in contents)
            {
                var blog = new ContentLiquid(item, category, type, ImageWidth, ImageHeight);
                items.Add(blog);

            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, new
            {
                items = LiquidAnonymousObject.GetContentLiquid(items)
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;

        }

        public StoreLiquidResult GetContentsByContentType(List<Content> contents, List<Category> categories, PageDesign pageDesign, string type)
        {
            var items = new List<ContentLiquid>();
            foreach (var item in contents)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                var blog = new ContentLiquid(item, category, type, ImageWidth, ImageHeight);
                items.Add(blog);

            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, new
            {
                items = LiquidAnonymousObject.GetContentLiquid(items)
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
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


                feed.AddNamespace(type, url+"/"+type);
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
                si.ElementExtensions.Add(type+":category", String.Empty, category.Name);
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