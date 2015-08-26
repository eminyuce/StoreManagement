using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Admin.Filters;
using StoreManagement.Data.GeneralHelper;
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
            if (storeId != 0)
            {
                resultList = NavigationRepository.GetStoreNavigations(storeId, search);
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
            try
            {

                // if (ModelState.IsValid)
                {
                    var c = navigation.Modul.Split("-".ToCharArray());
                    navigation.ControllerName = c[0];
                    navigation.ActionName = c[1];
                    if (navigation.Id == 0)
                    {
                        NavigationRepository.Add(navigation);
                    }
                    else
                    {
                        NavigationRepository.Edit(navigation);

                    }
                    NavigationRepository.Save();
                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = navigation.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to save changes:" + navigation, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
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

            try
            {
                NavigationRepository.Delete(navigation);
                NavigationRepository.Save();
                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = navigation.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to delete:" + navigation, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(navigation);

        }
        public ActionResult StoreDetails(int id = 0)
        {
            try
            {
                Navigation item = NavigationRepository.GetSingle(id);
                Store s = StoreRepository.GetSingle(item.StoreId);
                var productDetailLink = LinkHelper.GetNavigationLink(item);
                String detailPage = String.Format("http://{0}{1}", s.Domain, productDetailLink);

                return Redirect(detailPage);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new EmptyResult();
            }


        }


    }
}