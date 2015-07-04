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

        public ViewResult Index()
        {
            return View(ContactRepository.GetAll());
        }

        //
        // GET: /Contacts/Details/5

        public ViewResult Details(int id)
        {
            Contact contact = ContactRepository.GetSingle(id);
            return View(contact);
        }

        //
        // GET: /Contacts/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contacts/Create

        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                ContactRepository.Add(contact);
                ContactRepository.Save();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        //
        // GET: /Contacts/Edit/5

        public ActionResult Edit(int id)
        {
            Contact contact = ContactRepository.GetSingle(id);
            return View(contact);
        }

        //
        // POST: /Contacts/Edit/5

        [HttpPost]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                ContactRepository.Edit(contact);
                ContactRepository.Save();
                return RedirectToAction("Index");
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
            ContactRepository.Delete(contact);
            ContactRepository.Save();
            return RedirectToAction("Index");
        }


    }
}