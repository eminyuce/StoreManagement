using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class ContactsController : BaseController
    {

        //
        // GET: /Contacts/
        public ActionResult Index(int storeId = 0, String search = "")
        {
            List<Contact> resultList = new List<Contact>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = ContactRepository.GetContactsByStoreId(storeId, search);
            }
            return View(resultList);
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(int id)
        {
            Contact contact = ContactRepository.GetSingle(id);
            return View(contact);
        }


        //
        // GET: /Contacts/Edit/5

        public ActionResult SaveOrEdit(int id = 0)
        {
            Contact contact = new Contact();
            if (id != 0)
            {
                contact = ContactRepository.GetSingle(id);
            }
            return View(contact);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult SaveOrEdit(Contact contact)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (contact.Id == 0)
                    {
                        contact.State = true;
                        contact.UpdatedDate = DateTime.Now;
                        contact.CreatedDate = DateTime.Now;
                        ContactRepository.Add(contact);
                    }
                    else
                    {
                        contact.UpdatedDate = DateTime.Now;
                        ContactRepository.Edit(contact);
                    }

                    ContactRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new { storeId = contact.StoreId });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to save changes:" + contact, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(contact);
        }

        //
        // GET: /Contacts/Delete/5

        public ActionResult Delete(int id)
        {
            Contact contact = ContactRepository.GetSingle(id);
            return View(contact);
        }

        //
        // POST: /Contacts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = ContactRepository.GetSingle(id);

            try
            {
                ContactRepository.Delete(contact);
                ContactRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new { storeId = contact.StoreId });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to delete:" + contact, ex);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(contact);
        }


    }
}