using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class NewsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String ContentType = StoreConstants.BlogsType;

        public ActionResult Index(int page = 1)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }
            var newsContents = new ContentsViewModel();
            newsContents.Store = MyStore;
            var m = ContentService.GetContentsCategoryId(MyStore.Id, null, ContentType, true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            newsContents.Categories = CategoryService.GetCategoriesByStoreId(MyStore.Id, ContentType, true);
            newsContents.Type = ContentType;
            return View(newsContents);
        }
        public ActionResult Detail(String id)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }
            var returnModel = new ContentDetailViewModel();
            int newsId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(newsId);

            if (!CheckRequest(returnModel.Content))
            {
                return HttpNotFound("Not Found");
            }
            returnModel.Type = ContentType;
            returnModel.Store = MyStore;
            returnModel.Category = CategoryService.GetCategory(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(MyStore.Id, ContentType);

            return View(returnModel);
        }
    }
}