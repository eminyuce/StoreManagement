using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IContentService : IBaseService
    {
        ContentDetailViewModel GetContentDetail(string id, string contentType);

        ContentsViewModel GetContentIndexPage(int page, String contentType);

        FeedResult GetContentRss(int take, int description, int imageHeight, int imageWidth, string contentType);



        StoreLiquidResult GetContentsIndexPage(
           StorePagedList<Content> contents,
           PageDesign pageDesign,
           List<Category> categories, String type);

        StoreLiquidResult GetContentDetailPage(Content content, PageDesign pageDesign, Category category, String type);
        StoreLiquidResult GetRelatedContentsPartial(Category category, List<Content> relatedContentsTask, PageDesign pageDesignTask, String type);

        Rss20FeedFormatter GetContentsRssFeed(Store store, List<Content> contents, List<Category> categories, int description, string type);

        StoreLiquidResult GetContentsByContentType(List<Content> contents, List<Category> categories, PageDesign pageDesign, String type);
    }
}
