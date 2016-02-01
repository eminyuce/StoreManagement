using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
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
    }
}
