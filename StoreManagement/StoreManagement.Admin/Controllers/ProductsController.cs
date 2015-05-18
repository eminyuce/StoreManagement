using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    //[Authorize]
    public class ProductsController : BaseController
    {

        [Inject]
        public IContentFileRepository ContentFileRepository { set; get; } 
        
        private IContentRepository contentRepository;
        public ProductsController(IStoreContext dbContext, ISettingRepository settingRepository, IContentRepository contentRepository) : base(dbContext, settingRepository)
        {
            this.contentRepository = contentRepository;
        }

        public ActionResult Index(int storeId=0)
        {
            List<Content> resultList = new List<Content>();
            if (storeId == 0)
            {
                resultList = contentRepository.GetAll().ToList();
            }
            else
            {
                resultList = contentRepository.GetContentByType(storeId,"content");
            }
            return View(resultList);
        }

        //
        // GET: /Content/Details/5

        public ActionResult Details(int id = 0)
        {
            Content content = contentRepository.GetSingle(id);
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
                content.StoreId = 1;
                content.Type = "product";
            }
            else
            {
               content = contentRepository.GetSingle(id);
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
                    contentRepository.Add(content);
                    contentRepository.Save();
                }
                else
                {
                    contentRepository.Edit(content);
                    contentRepository.Save();
                }

                if (selectedFileId != null)
                {
                    ContentFileRepository.DeleteContentFileByContentId(content.Id);
                    foreach (var i in selectedFileId)
                    {
                        var m = new ContentFile();
                        m.ContentId = content.Id;
                        m.FileManagerId = i;
                        ContentFileRepository.Add(m);
                    }
                    ContentFileRepository.Save();
                }

                return RedirectToAction("Index");
            }

            return View(content);
        }

        //
        // GET: /Content/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Content content = contentRepository.GetSingle(id);
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
            Content content = contentRepository.GetSingle(id);
            contentRepository.Delete(content);
            contentRepository.Save();
            return RedirectToAction("Index");
        }

        
    }
}