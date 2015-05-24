using Ninject;
using StoreManagement.Data.Entities;
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
        // GET: /Setting/
          public PageDesignsController(IStoreContext dbContext, ISettingRepository settingRepository)
            : base(dbContext, settingRepository)
        {

        }
        //
        // GET: /PageDesigns/

        public ViewResult Index(int storeId=0)
        {
            List<PageDesign> resultList = new List<PageDesign>();
            if (storeId == 0)
            {
                resultList = PageDesignRepository.GetAll().ToList();
            }
            else
            {
                resultList = PageDesignRepository.GetPageDesignByStoreId(storeId);
            }
            return View(resultList);
        }

        //
        // GET: /PageDesigns/Details/5

        public ViewResult Details(int id)
        {
            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            return View(pagedesign);
        }

        //
        // GET: /PageDesigns/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PageDesigns/Create

        [HttpPost]
        public ActionResult Create(PageDesign pagedesign)
        {
            if (ModelState.IsValid)
            {
                PageDesignRepository.Add(pagedesign);
                PageDesignRepository.Save();
               
                return RedirectToAction("Index");
            }

            return View(pagedesign);
        }

        //
        // GET: /PageDesigns/Edit/5

        public ActionResult Edit(int id)
        {
            PageDesign pagedesign = PageDesignRepository.GetSingle(id);
            return View(pagedesign);
        }

        //
        // POST: /PageDesigns/Edit/5

        [HttpPost]
        public ActionResult Edit(PageDesign pagedesign)
        {
            if (ModelState.IsValid)
            {
                PageDesignRepository.Edit(pagedesign);
                PageDesignRepository.Save();
                return RedirectToAction("Index");
            }
            return View(pagedesign);
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
            PageDesignRepository.Delete(pagedesign);
            PageDesignRepository.Save();
            return RedirectToAction("Index");
        }

	}
}