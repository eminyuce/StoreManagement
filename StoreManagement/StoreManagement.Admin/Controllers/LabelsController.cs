using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Admin.Controllers
{
    public abstract class LabelsController : BaseController
    {

        private String LabelType { set; get; }

        protected LabelsController(String labelType)
        {
            this.LabelType = labelType;
        }

        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            var resultList = new List<Label>();
            storeId = GetStoreId(storeId);
            if (storeId == 0)
            {
                resultList = LabelRepository.GetLabelsByLabelType(LabelType);
            }
            else
            {
                resultList = LabelRepository.GetLabelsByLabelType(storeId, LabelType);
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

            return View(resultList);
        }

        //
        // GET: /Labels/Details/5

        public ViewResult Details(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            return View(label);
        }

        public ActionResult SaveOrEdit(int id = 0)
        {
            Label label = new Label();
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
            label.LabelType = LabelType;
            return View(label);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(Label label)
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
                return RedirectToAction("Index");
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
            LabelRepository.Delete(label);
            LabelRepository.Save();
            return RedirectToAction("Index");
        }


    
    }
}