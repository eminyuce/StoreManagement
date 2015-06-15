using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class BlogsController : BaseController
   {
        private const String ContentType = "blog";
        

        public ActionResult Index(int storeId=0, String search="", int categoryId=0)
        {
            List<Content> resultList = new List<Content>();
            storeId = GetStoreId(storeId);
            if (storeId == 0)
            {
                resultList = ContentRepository.GetContentByType(ContentType);
            }
            else
            {
                resultList = ContentRepository.GetContentByType(storeId, ContentType);
            }

            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (categoryId > 0)
            {
                resultList = resultList.Where(r => r.CategoryId == categoryId).ToList();
            }
            var contentsAdminViewModel = new ContentsAdminViewModel();
            contentsAdminViewModel.Contents = resultList;
            contentsAdminViewModel.Categories = CategoryRepository.GetCategoriesByStoreIdFromCache(storeId, ContentType);
            return View(contentsAdminViewModel);
        }

        //
        // GET: /Content/Details/5

        public ActionResult Details(int id = 0)
        {
            Content content = ContentRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        //
        // GET: /Content/Create

        public ActionResult SaveOrEdit(int id=0)
        {
            var content = new Content();
            if (id == 0)
            {
                content.Type = ContentType;
                content.UpdatedDate = DateTime.Now;
            }
            else
            {
                content = ContentRepository.GetSingle(id);
                content.UpdatedDate = DateTime.Now;
            }
            return View(content);
        }

        //
        // POST: /Content/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Content content, int [] selectedFileId = null)
        {
            if (ModelState.IsValid)
            {
                if (content.Id == 0)
                {
                    ContentRepository.Add(content);
                }
                else
                {
                    ContentRepository.Edit(content);
                }
                ContentRepository.Save();
                if (selectedFileId != null)
                {
                    int contentId = content.Id;
                    ContentFileRepository.SaveContentFiles(selectedFileId, contentId);
                }

                return RedirectToAction("Index");
            }

            return View(content);
        }

      

        //
        // GET: /Content/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Content content = ContentRepository.GetSingle(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        //
        // POST: /Content/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = ContentRepository.GetSingle(id);
            ContentRepository.Delete(content);
            ContentRepository.Save();
            return RedirectToAction("Index");
        }

        
    }
}