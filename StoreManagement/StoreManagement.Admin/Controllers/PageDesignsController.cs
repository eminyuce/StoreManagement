using System.Threading.Tasks;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class PageDesignsController : BaseController
    {

        //
        // GET: /PageDesigns/
        public ViewResult Index(int storePageDesignId = 0, String search = "")
        {
           
            var resultList = new List<PageDesign>();
            resultList = PageDesignRepository.GetPageDesignByStoreId(storePageDesignId, search);
            return View(resultList);
        }

        //
        // GET: /PageDesigns/Details/5

        public ViewResult Details(int id)
        {
            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            return View(pagedesign);
        }

        public ActionResult ExportExcel(int id = 0)
        {
            int storePageDesignId = id;
            var storePageDesing = StorePageDesignRepository.GetSingle(storePageDesignId);
            var resultList = PageDesignRepository.GetPageDesignByStoreId(storePageDesignId, "");
            var dt = MapToListHelper.ToDataTable(resultList);
            var report = ExcelHelper.GetExcelByteArrayFromDataTable(dt);
            return File(report, "application/vnd.ms-excel",
                         String.Format("PageDesigns-{0}-{1}.xls", storePageDesing.Name, DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.GetCultureInfo("en-US"))));
        }
        [HttpPost]
        public ActionResult ImportExcel(int id, HttpPostedFileBase excelFile=null)
        {
            if (excelFile == null)
            {
                return RedirectToAction("Index", new { storePageDesignId = id });
            }
            var dt = ExcelHelper.PostValues(excelFile);
            int selectedStoreId = id;
            var resultList = PageDesignRepository.GetPageDesignByStoreId(selectedStoreId, "");
            var pageDesingsExcelReport = MapToListHelper.ToList<PageDesign>(dt);
            foreach (var pageDesign in pageDesingsExcelReport.Where(r =>
                !r.Name.Equals("Name", StringComparison.InvariantCultureIgnoreCase) &&
                !r.PageTemplate.Equals("PageTemplate", StringComparison.InvariantCultureIgnoreCase)))
            {

                pageDesign.StorePageDesignId = id;
                var pageDesignTask = resultList.FirstOrDefault(r => r.Name.Equals(pageDesign.Name, StringComparison.InvariantCultureIgnoreCase));
                if (pageDesignTask == null)
                {
                    pageDesign.Id = 0;
                    PageDesignRepository.Add(pageDesign);
                }
                else
                {
                    pageDesignTask.PageTemplate = pageDesign.PageTemplate;
                    PageDesignRepository.Edit(pageDesignTask);
                }
                pageDesign.CreatedDate = DateTime.Now;
                pageDesign.UpdatedDate = DateTime.Now;
            }
            PageDesignRepository.Save();

            return RedirectToAction("Index", new { storePageDesignId = id });
        }
        //
        // GET: /PageDesigns/Edit/5

        public ActionResult SaveOrEdit(int id = 0, int storePageDesignId = 0)
        {
            var pagedesign = new PageDesign();
            
            if (id == 0)
            {
                pagedesign.CreatedDate = DateTime.Now;
                pagedesign.State = true;
                pagedesign.UpdatedDate = DateTime.Now;
                pagedesign.StorePageDesignId = storePageDesignId;
            }
            else
            {
                pagedesign = PageDesignRepository.GetSingle(id);
                pagedesign.State = true;
                pagedesign.UpdatedDate = DateTime.Now;
            }
            var spd = StorePageDesignRepository.GetSingle(pagedesign.StorePageDesignId);
            ViewBag.StorePageDesignName = spd.Name;
            return View(pagedesign);
        }

        //
        // POST: /PageDesigns/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(PageDesign pagedesign)
        {
            var spd = StorePageDesignRepository.GetSingle(pagedesign.StorePageDesignId);
            ViewBag.StorePageDesignName = spd.Name;
            try
            {
                if (ModelState.IsValid)
                {



                    if (pagedesign.Id > 0)
                    {
                        PageDesignRepository.Edit(pagedesign);
                    }
                    else
                    {
                        var isSamePageNameExists = PageDesignRepository.GetPageDesignByStoreId(pagedesign.StorePageDesignId, "")
                                            .Any(r => r.Name.Equals(pagedesign.Name, StringComparison.InvariantCultureIgnoreCase));
                        if (isSamePageNameExists)
                        {
                            ModelState.AddModelError("", "Same Page Desing Name exists, put a different name.");
                            return View(pagedesign);
                        }


                        PageDesignRepository.Add(pagedesign);
                    }

                    PageDesignRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("SaveOrEdit", new { id = pagedesign.Id, storePageDesignId = pagedesign.StorePageDesignId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, pagedesign);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(pagedesign);
        }
        public ActionResult CopyPageDesign(int id)
        {
            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            try
            {
                var guid = Guid.NewGuid().ToString();
                var pagedesignCopy = new PageDesign();
                pagedesignCopy.PageTemplate = pagedesign.PageTemplate;
                pagedesignCopy.Name = pagedesign.Name + "_" + guid;
                pagedesignCopy.Type = pagedesign.Type + "_" + guid;
                pagedesignCopy.Id = 0;
                pagedesignCopy.CreatedDate = DateTime.Now;
                pagedesignCopy.State = true;
                pagedesignCopy.UpdatedDate = DateTime.Now;
                pagedesignCopy.StorePageDesignId = pagedesign.StorePageDesignId;
                PageDesignRepository.Add(pagedesignCopy);
                PageDesignRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, pagedesign);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }



            if (IsSuperAdmin)
            {
                return RedirectToAction("Index", new { storeId = pagedesign.StorePageDesignId });
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        //
        // GET: /PageDesigns/Delete/5

        public ActionResult Delete(int id)
        {
            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            return View(pagedesign);
        }

        //
        // POST: /PageDesigns/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            try
            {
                PageDesignRepository.Delete(pagedesign);
                PageDesignRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storePageDesignId = pagedesign.StorePageDesignId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete:" + ex.StackTrace, pagedesign);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(pagedesign);
        }

    }
}