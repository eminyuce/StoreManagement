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
            foreach (var item in contents.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                if (category != null)
                {
                    var blog = new ContentLiquid(item, category, pageDesign, type, ImageWidth, ImageHeight);
                    items.Add(blog);
                }
            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
            {
                items = LiquidAnonymousObject.GetContentLiquid(items)
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
            return result;
        }


        public StoreLiquidResult GetContentDetailPage(Content content, PageDesign pageDesign, Category category, String type)
        {
          
            var items = new List<ContentLiquid>();
            var contentLiquid = new ContentLiquid(content, category, pageDesign, type, ImageWidth, ImageHeight);

            var anonymousObject = LiquidAnonymousObject.GetContentAnonymousObject(contentLiquid);

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }



        public StoreLiquidResult GetRelatedContentsPartial(Category category, List<Content> contents,PageDesign pageDesign, String type)
        {
       

            var items = new List<ContentLiquid>();
            foreach (var item in contents)
            {
                var blog = new ContentLiquid(item, category, pageDesign, type, ImageWidth, ImageHeight);
                items.Add(blog);

            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
            {
                items = LiquidAnonymousObject.GetContentLiquid(items)
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;

        }


        public Rss20FeedFormatter GetContentsRssFeed(Store store, List<Content> contents, List<Category> categories, int description, string type)
        {
          
            try
            {
                String url = "http://login.seatechnologyjobs.com/";

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


                feed.AddNamespace(type, "https://www.maritimejobs.com/jobs");
                var rssFeed = new Rss20FeedFormatter(feed);


                return rssFeed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProductsPartial");
                return null;
            }
        }
        private SyndicationItem GetSyndicationItem(Store store, Content product, Category productCategory, int description, String type)
        {
            if (productCategory == null)
                return null;

            if (description == 0)
                description = 300;

            var productDetailLink = LinkHelper.GetContentLink(product, productCategory.Name, type);
            String detailPage = String.Format("http://{0}{1}", store.Domain, productDetailLink);

            string desc = "";
            if (description > 0)
            {
                desc = GeneralHelper.GetDescription(product.Description, description);
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




            if (product.ContentFiles.Any())
            {
                var mainImage = product.ContentFiles.FirstOrDefault(r => r.IsMainImage);
                if (mainImage == null)
                {
                    mainImage = product.ContentFiles.FirstOrDefault();
                }

                if (mainImage != null)
                {


                    string imageSrc = LinkHelper.GetImageLink("Thumbnail", mainImage.FileManager.GoogleImageId, this.ImageWidth, this.ImageHeight);
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