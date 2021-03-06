﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class EmailListsController : BaseController
    {

        //
        // GET: /EmailLists/

        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<EmailList>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = EmailListRepository.GetStoreEmailList(storeId, search);
            }

            return View(resultList);
        }

        //
        // GET: /EmailLists/Details/5

        public ViewResult Details(int id)
        {
            EmailList emaillist = EmailListRepository.GetSingle(id);
            return View(emaillist);
        }



        //
        // GET: /EmailLists/Edit/5

        public ActionResult SaveOrEdit(int id = 0)
        {
            EmailList emaillist = new EmailList();
            if (id != 0)
            {
                emaillist = EmailListRepository.GetSingle(id);
                emaillist.UpdatedDate = DateTime.Now;
            }
            else
            {
                emaillist.UpdatedDate = DateTime.Now;
                emaillist.CreatedDate = DateTime.Now;
                emaillist.State = true;
            }
            return View(emaillist);
        }

        //
        // POST: /EmailLists/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(EmailList emaillist)
        {
            if (ModelState.IsValid)
            {
                if (emaillist.Id == 0)
                {
                    EmailListRepository.Add(emaillist);
                }
                else
                {
                    EmailListRepository.Edit(emaillist);
                }
                EmailListRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = emaillist.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(emaillist);
        }

        //
        // GET: /EmailLists/Delete/5

        public ActionResult Delete(int id)
        {
            EmailList emaillist = EmailListRepository.GetSingle(id);
            return View(emaillist);
        }

        //
        // POST: /EmailLists/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailList emaillist = EmailListRepository.GetSingle(id);
            if (emaillist == null)
            {
                return HttpNotFound();
            }
            try
            {
                EmailListRepository.Delete(emaillist);
                EmailListRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = emaillist.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex,"Unable to delete:" + emaillist, ex);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(emaillist);
        }


    }
}