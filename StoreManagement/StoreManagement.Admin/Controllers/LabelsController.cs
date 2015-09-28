using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Admin.Controllers
{
    public class LabelsController : BaseController
    {

        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<Label>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = LabelRepository.GetLabelsByStoreId(storeId, search);
            }

            return View(resultList);
        }

        //
        // GET: /Labels/Details/5

        public ViewResult Details(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            return View(label);
        }

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var label = new Label();
            label.StoreId = GetStoreId(selectedStoreId);
            if (id != 0)
            {
                label = LabelRepository.GetSingle(id);
                label.UpdatedDate = DateTime.Now;
            }
            else
            {
                label.UpdatedDate = DateTime.Now;
                label.CreatedDate = DateTime.Now;
                label.State = true;

            }
            return View(label);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(Label label)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (label.Id == 0)
                    {
                        LabelRepository.Add(label);
                    }
                    else
                    {
                        LabelRepository.Edit(label);
                    }
                    LabelRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = label.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, label);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(label);
        }

        //
        // GET: /Labels/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            return View(label);
        }

        //
        // POST: /Labels/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            try
            {
                LabelRepository.Delete(label);
                LabelRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = label.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, label);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(label);
        }



    }
}