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


        public ActionResult Index(int page = 1)
        {
            var newsContents = new ContentsViewModel();
            newsContents.Store = MyStore;
            var m = ContentService.GetContentsCategoryId(MyStore.Id, null, StoreConstants.BlogsType, true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);

            return View(newsContents);
        }
        public ActionResult Blog(String id)
        {
            var returnModel = new ContentDetailViewModel();
            int blogId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(blogId);
            returnModel.Store = MyStore;
            returnModel.Category = CategoryService.GetCategory(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(MyStore.Id, StoreConstants.BlogsType, true);

            return View(returnModel);
        }
    }
}