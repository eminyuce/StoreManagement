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

        public ActionResult Index(int page=1)
        {
            if (!IsModulActive(StoreConstants.NewsType))
            {
                return HttpNotFound("Not Found");
            }
            var newsContents = new ContentsViewModel();
            newsContents.Store = MyStore;
            var m = ContentService.GetContentsCategoryId(MyStore.Id, null, StoreConstants.NewsType, true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(newsContents);
        }
        public ActionResult Detail(String id)
        {
            if (!IsModulActive(StoreConstants.NewsType))
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

            returnModel.Store = MyStore;
            returnModel.Category = CategoryService.GetCategory(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(MyStore.Id, StoreConstants.NewsType);

            return View(returnModel);
        }
	}
}