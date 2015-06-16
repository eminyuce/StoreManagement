using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class LabelsController : BaseController
    {
        private int _labelType = 1;
        protected int LabelType
        {
            get { return _labelType; }
            set { _labelType = value; }
        }

        public ActionResult Index(int storeId = 0, String search = "", int categoryId = 0)
        {
            var resultList = new List<Label>();
            storeId = GetStoreId(storeId);
            if (storeId == 0)
            {
                resultList = LabelRepository.GetLabelsByItemType(_labelType);
            }
            else
            {
                resultList = LabelRepository.GetLabelsByItemType(storeId, _labelType);
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

        public ActionResult CreateOrEdit(int id = 0)
        {
            Label label = new Label();
            if (id != 0)
            {
                label = LabelRepository.GetSingle(id);
            }
            return View(label);
        }

        [HttpPost]
        public ActionResult CreateOrEdit(Label label)
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

        public ActionResult Delete(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            return View(label);
        }

        //
        // POST: /Labels/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Label label = LabelRepository.GetSingle(id);
            LabelRepository.Delete(label);
            LabelRepository.Save();
            return RedirectToAction("Index");
        }
    }
}