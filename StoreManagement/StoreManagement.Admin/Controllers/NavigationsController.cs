using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Admin.Filters;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
   // [Authorize(Roles = "StoreAdmin")]
   // [InitializeSimpleMembership]
    public class NavigationsController : BaseController
    {


 
        //
        // GET: /Navigations/

        public ViewResult Index(int storeId = 0, String search = "")
        {
            List<Navigation> resultList = new List<Navigation>();
            if (storeId == 0)
            {
                resultList = NavigationRepository.GetAll().ToList();
            }
            else
            {
                resultList = NavigationRepository.GetStoreNavigations(storeId);
            }
            if (!String.IsNullOrEmpty(search))
            {
                resultList =
                    resultList.Where(r => r.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }
            ViewBag.Search = search;
            return View(resultList);
        }

        //
        // GET: /Navigations/Details/5

        public ViewResult Details(int id)
        {
            Navigation navigation = NavigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // GET: /Navigations/Create

        public ActionResult SaveOrEdit(int id = 0)
        {
            var item = new Navigation();
            if (id == 0)
            {
                item.ParentId = 0;
                item.CreatedDate = DateTime.Now;
            }
            else
            {
                item = NavigationRepository.GetSingle(id);
            }

            ViewBag.Moduls = GetModuls();
            return View(item);
        }

        //
        // POST: /Navigations/Create

        [HttpPost]
        public ActionResult SaveOrEdit(Navigation navigation)
        {
            // if (ModelState.IsValid)
            {
                navigation.ControllerName = navigation.Modul;
                if (navigation.Id == 0)
                {
                    NavigationRepository.Add(navigation);
                    NavigationRepository.Save();
                }
                else
                {
                    NavigationRepository.Edit(navigation);
                    NavigationRepository.Save();
                }

                return RedirectToAction("Index");
            }
            ViewBag.Moduls = GetModuls();
            return View(navigation);
        }



        //
        // GET: /Navigations/Delete/5

        public ActionResult Delete(int id)
        {
            Navigation navigation = NavigationRepository.GetSingle(id);
            return View(navigation);
        }

        //
        // POST: /Navigations/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Navigation navigation = NavigationRepository.GetSingle(id);
            NavigationRepository.Delete(navigation);
            NavigationRepository.Save();
            return RedirectToAction("Index");
        }


        private SelectList GetModuls()
        {
            var moduls = new List<SelectListItem>();
            var m = new SelectListItem();
            m.Value = "Home";
            m.Text = "Home";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "News";
            m.Text = "News";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Products";
            m.Text = "Products";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Pages";
            m.Text = "Pages";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Blogs";
            m.Text = "Blogs";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Events";
            m.Text = "Events";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Contact";
            m.Text = "Contact";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Photos";
            m.Text = "Photo Gallery";
            moduls.Add(m);
            var sList = new SelectList(moduls, "Value", "Text");


            return sList;

        }
    }
}