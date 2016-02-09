using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class ContentService : BaseService, IContentService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ContentDetailViewModel GetContentDetail(string id, string contentType)
        {
            var resultModel = new ContentDetailViewModel();
            int newsId = id.Split("-".ToCharArray()).Last().ToInt();
            resultModel.SContent = ContentRepository.GetContentsContentId(newsId);

            if (!CheckRequest(resultModel.SContent))
            {
                throw new Exception("Not Found.Site content is wrong");
            }
            resultModel.Type = contentType;
            resultModel.SStore = MyStore;
            resultModel.SCategory = CategoryRepository.GetCategory(resultModel.Content.CategoryId);
            resultModel.SCategories = CategoryRepository.GetCategoriesByStoreId(MyStore.Id, contentType);
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();

            return resultModel;
            

        }

        public ContentsViewModel GetContentIndexPage(int page, String contentType)
        {
            var resultModel = new ContentsViewModel();
            resultModel.SStore = MyStore;
            var m = ContentRepository.GetContentsCategoryId(MyStore.Id, null, contentType, true, page, 24);
            resultModel.SContents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            resultModel.SCategories = CategoryRepository.GetCategoriesByStoreId(MyStore.Id, contentType, true);
            resultModel.Type = contentType;
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();

            return resultModel;
        }

        public FeedResult GetContentRss(int take, int description, int imageHeight, int imageWidth, string contentType)
        {
            var contents = ContentRepository.GetContentByType(StoreId, take, true, contentType);
            var categories = CategoryRepository.GetCategoriesByStoreId(StoreId, contentType, true);


            var rssHelper = new RssHelper();
            var feed = rssHelper.GetContentsRssFeed(MyStore, contents, categories, description, contentType);
            rssHelper.ImageWidth = imageWidth;
            rssHelper.ImageHeight = imageHeight;

            var comment = new StringBuilder();
            comment.AppendLine("Take=Number of rss item; Default value is 10  ");
            comment.AppendLine("Description=The length of description text.Default value is 300  ");
            return new FeedResult(feed, comment);
        }




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
                desc = Data.GeneralHelper.GeneralHelper.GetDescription(product.Description, description);
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
