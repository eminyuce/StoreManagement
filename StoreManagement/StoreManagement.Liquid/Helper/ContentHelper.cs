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
            Task<StorePagedList<Content>> contentsTask,
            Task<PageDesign> pageDesignTask,
                 Task<List<Category>> categoriesTask, String type)
        {
            Task.WaitAll(pageDesignTask, contentsTask, categoriesTask);
            var contents = contentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null");
            }



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


        public StoreLiquidResult GetContentDetailPage(Task<Content> contentTask, Task<PageDesign> pageDesignTask, Task<Category> categoryTask, String type)
        {
            Task.WaitAll(pageDesignTask, contentTask, categoryTask);
            var content = contentTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;
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



        public StoreLiquidResult GetRelatedContentsPartial(Task<Category> categoryTask, Task<List<Content>> relatedContentsTask, Task<PageDesign> pageDesignTask, String type)
        {
            Task.WaitAll(pageDesignTask, relatedContentsTask, categoryTask);
            var contents = relatedContentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;

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


        public Rss20FeedFormatter GetContentsRssFeed(Task<Store> storeTask, Task<List<Content>> contentsTask, Task<List<Category>> categoriesTask, int description, string type)
        {
            Task.WaitAll(storeTask, contentsTask, categoriesTask);
            var store = storeTask.Result;
            var content = contentsTask.Result;
            var categories = categoriesTask.Result;
            try
            {
                String url = "http://login.seatechnologyjobs.com/";

                var feed = new SyndicationFeed(store.Name, "", new Uri(url))
                {
                    Language = "en-US"
                };


                var feedItemList = new List<SyndicationItem>();
                foreach (var product in content)
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