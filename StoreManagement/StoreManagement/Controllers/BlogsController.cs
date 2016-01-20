using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{

    [OutputCache(CacheProfile = "Cache1Days")]
    public class BlogsController : BaseController
    {
        private const String ContentType = StoreConstants.BlogsType;

        public ActionResult Index(int page = 1)
        {
            var newsContents = new ContentsViewModel();
            newsContents.SStore = MyStore;
            var m = ContentService.GetContentsCategoryId(MyStore.Id, null, ContentType, true, page, 24);
            newsContents.SContents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            newsContents.SCategories = CategoryService.GetCategoriesByStoreId(MyStore.Id, ContentType, true);
            newsContents.Type = ContentType;
            return View(newsContents);
        }
        public ActionResult Blog(String id)
        {
            var returnModel = new ContentDetailViewModel();
            int blogId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.SContent = ContentService.GetContentsContentId(blogId);
            returnModel.SStore = MyStore;
            returnModel.SCategory = CategoryService.GetCategory(returnModel.Content.CategoryId);
            returnModel.SCategories = CategoryService.GetCategoriesByStoreId(MyStore.Id, ContentType, true);
            returnModel.Type = ContentType;
            return View(returnModel);
        }
    }
}