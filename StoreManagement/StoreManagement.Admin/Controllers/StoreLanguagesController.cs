using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class StoreLanguagesController : BaseController
    {

        //
        // GET: /StoreLanguages/


        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<StoreLanguage>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = StoreLanguageRepository.GetStoreLanguages(storeId, search);
            }

            return View(resultList);
        }
        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var item = new StoreLanguage();
            if (id != 0)
            {
                item = StoreLanguageRepository.GetSingle(id);
                item.UpdatedDate = DateTime.Now;
            }
            else
            {
                item.UpdatedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;
                item.State = true;
                item.StoreId = selectedStoreId;

            }
            return View(item);
        }


        [HttpPost]
        public ActionResult SaveOrEdit(StoreLanguage model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (model.Id == 0)
                    {
                        StoreLanguageRepository.Add(model);
                    }
                    else
                    {
                        StoreLanguageRepository.Edit(model);
                    }
                    StoreLanguageRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = model.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
              

            }
            catch (DbEntityValidationException ex)
            {
                var message = GetDbEntityValidationExceptionDetail(ex);
                Logger.Error(ex, "DbEntityValidationException:" + message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Unable to save changes:" + model);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(model);
        }
        //
        // GET: /StoreLanguages/Delete/5

        public ActionResult Delete(int id)
        {
            StoreLanguage storelanguage = StoreLanguageRepository.GetSingle(id);
            return View(storelanguage);
        }

        //
        // POST: /StoreLanguages/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreLanguage storelanguage = StoreLanguageRepository.GetSingle(id);

            try
            {
                StoreLanguageRepository.Delete(storelanguage);
                StoreLanguageRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = storelanguage.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Unable to delete it:" + storelanguage);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(storelanguage);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
	}
}