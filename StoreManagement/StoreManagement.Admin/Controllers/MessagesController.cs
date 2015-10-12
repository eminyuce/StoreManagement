using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class MessagesController : BaseController
    {

        public ActionResult Index(int storeId = 0, String search = "")
        {
            var resultList = new List<Message>();
            storeId = GetStoreId(storeId);
            if (storeId != 0)
            {
                resultList = MessageRepository.GetMessagesByStoreId(storeId, search);
            }

            return View(resultList);
        }

        //
        // GET: /Labels/Details/5

        public ViewResult Details(int id)
        {
            Message message = MessageRepository.GetSingle(id);
            return View(message);
        }

        public ActionResult SaveOrEdit(int id = 0, int selectedStoreId = 0)
        {
            var message = new Message();
            message.StoreId = GetStoreId(selectedStoreId);
            if (id != 0)
            {
                message = MessageRepository.GetSingle(id);
                message.UpdatedDate = DateTime.Now;
            }
            else
            {
                message.UpdatedDate = DateTime.Now;
                message.CreatedDate = DateTime.Now;
                message.State = true;

            }
            return View(message);
        }

        [HttpPost]
        public ActionResult SaveOrEdit(Message message)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (message.Id == 0)
                    {
                        MessageRepository.Add(message);
                    }
                    else
                    {
                        MessageRepository.Edit(message);
                    }
                    MessageRepository.Save();

                    if (IsSuperAdmin)
                    {
                        return RedirectToAction("Index", new {storeId = message.StoreId});
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to save:" + ex.StackTrace, message);
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("",
                                         "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }


            return View(message);
        }

        //
        // GET: /Labels/Delete/5
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult Delete(int id)
        {
            Message message = MessageRepository.GetSingle(id);
            return View(message);
        }

        //
        // POST: /Labels/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = MessageRepository.GetSingle(id);
            try
            {
                MessageRepository.Delete(message);
                MessageRepository.Save();

                if (IsSuperAdmin)
                {
                    return RedirectToAction("Index", new {storeId = message.StoreId});
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unable to delete it:" + ex.StackTrace, message);
                ModelState.AddModelError("",
                                         "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(message);
        }
    }

}