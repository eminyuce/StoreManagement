using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Admin.Controllers
{
    public abstract class ContentsController : BaseController
    {

        private String ContentType { set; get; }

        public ContentsController(String contentType)
        {
            this.ContentType = contentType;
        }


        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            List<Content> resultList = new List<Content>();
            storeId = GetStoreId(storeId);
            if (storeId != 0 && (categoryId != 0 || !String.IsNullOrEmpty(search)))
            {
                resultList = ContentRepository.GetContentByTypeAndCategoryId(storeId, ContentType, categoryId, search, null);
            }



            var contentsAdminViewModel = new ContentsAdminViewModel();
            contentsAdminViewModel.Contents = resultList;
            contentsAdminViewModel.Type = this.ContentType;
            contentsAdminViewModel.Categories = CategoryRepository.GetCategoriesByStoreId(storeId, ContentType, isActive: null);
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

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0, int selectedCategoryId = 0)
        {
            var content = new Content();
            content.CategoryId = selectedCategoryId;
            content.StoreId = GetStoreId(selectedStoreId);

            var labels = new List<LabelLine>();
            var fileManagers = new List<FileManager>();
            int mainImageFileManagerId = 0;
            if (id == 0)
            {
                content.Type = ContentType;
                content.UpdatedDate = DateTime.Now;
                content.CreatedDate = DateTime.Now;
                content.State = true;

            }
            else
            {
                content = ContentRepository.GetContentWithFiles(id);
                content.UpdatedDate = DateTime.Now;
                labels = LabelLineRepository.GetLabelLinesByItem(id, ContentType);
                fileManagers = content.ContentFiles.Select(r => r.FileManager).ToList();
                if (!CheckRequest(content)) //security for right store and its item.
                {
                    return HttpNotFound("Not Found");
                }
                var mainImage = content.ContentFiles.FirstOrDefault(r => r.IsMainImage);
                if (mainImage != null)
                {
                    mainImageFileManagerId = mainImage.FileManagerId;
                }
            }

            ViewBag.SelectedLabels = labels.Select(r => r.LabelId).ToArray();
            ViewBag.FileManagers = fileManagers;
            ViewBag.MainImageId = mainImageFileManagerId;

            return View(content);
        }

        //
        // POST: /Content/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrEdit(Content content, int[] selectedFileId = null, int[] selectedLabelId = null)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    if (content.CategoryId == 0)
                    {

                        var labels = new List<LabelLine>();
                        var fileManagers = new List<FileManager>();
                        int mainImageFileManagerId = 0;
                        if (content.Id > 0)
                        {
                            content = ContentRepository.GetContentWithFiles(content.Id);
                            labels = LabelLineRepository.GetLabelLinesByItem(content.Id, ContentType);
                            fileManagers = content.ContentFiles.Select(r => r.FileManager).ToList();
                            var mainImage = content.ContentFiles.FirstOrDefault(r => r.IsMainImage);
                            if (mainImage != null)
                            {
                                mainImageFileManagerId = mainImage.FileManagerId;
                            }
                        }
                        ViewBag.MainImageId = mainImageFileManagerId;
                        ViewBag.SelectedLabels = labels.Select(r => r.LabelId).ToArray();
                        ViewBag.FileManagers = fileManagers;

                        ModelState.AddModelError("CategoryId", "You should select category from category tree.");
                        return View(content);
                    }

                    if (content.Id == 0)
                    {
                        ContentRepository.Add(content);
                        ClearCache(content.StoreId);
                    }
                    else
                    {
                        ContentRepository.Edit(content);
                    }
                    ContentRepository.Save();
                    int contentId = content.Id;
                    if (selectedFileId != null)
                    {
                        ContentFileRepository.SaveContentFiles(selectedFileId, contentId);
                    }
                    LabelLineRepository.SaveLabelLines(selectedLabelId, contentId, ContentType);

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = content.StoreId, categoryId = content.CategoryId });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { categoryId = content.CategoryId });
                    }


                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save changes:" + ex.StackTrace, content);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(content);
        }




        //
        // GET: /Content/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
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
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = ContentRepository.GetSingle(id);

            try
            {

                LabelLineRepository.DeleteLabelLinesByItem(id, ContentType);
                ContentRepository.Delete(content);
                ContentRepository.Save();
                ClearCache(content.StoreId);


                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = content.StoreId, categoryId = content.CategoryId });
                }
                else
                {
                    return RedirectToAction("Index", new { categoryId = content.CategoryId });
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, content);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(content);
        }

        public ActionResult StoreDetails(int id = 0)
        {
            try
            {
                Content item = ContentRepository.GetSingle(id);
                Store s = StoreRepository.GetSingle(item.StoreId);
                Category cat = CategoryRepository.GetSingle(item.CategoryId);
                var productDetailLink = LinkHelper.GetContentLink(item, cat.Name, this.ContentType);
                String detailPage = String.Format("http://{0}{1}", s.Domain, productDetailLink);

                return Redirect(detailPage);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new EmptyResult();
            }


        }

        private void ClearCache(int storeId)
        {
            String key = String.Format("GetCategoriesRelatedItemsCount-StoreId-{0}-Category-{1}", storeId, this.ContentType);
            MemoryCacheHelper.ClearCache(key);

        }
    }
}