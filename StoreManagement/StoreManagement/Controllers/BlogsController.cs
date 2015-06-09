using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    public class BlogsController : BaseController
    {
        //
        // GET: /Blogs/
        public ActionResult Index(int page = 1)
        {
            int categoryId = 1;
            var newsContents = new ContentsViewModel();
            newsContents.Store = Store;
            var m = ContentService.GetContentsCategoryId(Store.Id, categoryId, "blog", true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(newsContents);
        }
        public ActionResult Blog(String id)
        {
            var returnModel = new ContentDetailViewModel();
            int blogId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(blogId);
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetSingle(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, "blog");
            var c = new List<Content>();
            c.Add(returnModel.Content);
            returnModel.RelatedContents = ContentService.GetContentByTypeAndCategoryId(Store.Id, "blog",
                returnModel.Content.CategoryId).Except(c).Take(5).ToList();


            return View(returnModel);
        }
    }
}