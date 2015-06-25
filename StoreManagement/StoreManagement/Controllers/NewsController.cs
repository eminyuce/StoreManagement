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
    public class NewsController : BaseController
    {
        

        public ActionResult Index(int page=1)
        {
            if (IsModulActive(StoreConstants.NewsType))
            {
                return HttpNotFound("Not Found");
            }
            var newsContents = new ContentsViewModel();
            newsContents.Store = Store;
            var m = ContentService.GetContentsCategoryId(Store.Id, null, StoreConstants.NewsType, true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(newsContents);
        }
        public ActionResult Detail(String id)
        {
            var returnModel = new ContentDetailViewModel();
            int blogId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(blogId);
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetCategory(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, StoreConstants.NewsType);

            return View(returnModel);
        }
	}
}