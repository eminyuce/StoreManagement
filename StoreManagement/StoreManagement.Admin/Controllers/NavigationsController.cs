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
    [Authorize]
    public class NavigationsController : BaseController
    {

        //
        // GET: /Navigations/

        public ViewResult Index(int storeId = 0, String search = "")
        {
            storeId = GetStoreId(storeId);
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
                item.State = true;
            }
            else
            {
                item = NavigationRepository.GetSingle(id);
                item.UpdatedDate = DateTime.Now;
            }


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


    
    }
}