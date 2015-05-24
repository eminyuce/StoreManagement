using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/

        public class OrderingItem
        {
            public int Id { get; set; }
            public int Ordering { get; set; }
            public bool State { get; set; }

        }
        public AjaxController(IStoreContext dbContext, 
            ISettingRepository settingRepository,
            IStoreRepository storeRepository)
            : base(dbContext, settingRepository)
        {
      
        }

        [HttpPost]
        public ActionResult DeleteContentItem(List<String> values)
        {
            foreach (String id in values)
            {
                var contentId = id.ToInt();
                var content = ContentRepository.GetSingle(contentId);
                ContentRepository.Delete(content);
            }
            ContentRepository.Save();

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeContentGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var content = ContentRepository.GetSingle(item.Id);
                if (item.Ordering > 0 && !String.IsNullOrEmpty(checkbox))
                {
                    content.Ordering = item.Ordering;
                }
                else if (checkbox.Equals("imagestate", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.ImageState = item.State;
                }
                else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.State = item.State;
                }
                else if (checkbox.Equals("mainpage", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.MainPage = item.State;
                }
                

                ContentRepository.Edit(content);
            }
            ContentRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveStyles(int storeId = 0, String styleArray="")
        {
            JObject results = JObject.Parse(styleArray);
            foreach (var result in results["styleArray"])
            {
                string id = (string)result["Id"];
                string style = (string)result["Style"];
                var s = this.settingRepository.GetSingle(id.ToInt());
                s.SettingValue = style;
                settingRepository.Edit(s);
            }
            settingRepository.Save();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRootCategories(int storeId = 0)
        {
            var cat = CategoryRepository.FindBy(r => r.ParentId == 0 && r.StoreId == storeId).ToList();
            var returnJson = from c in cat select new { Text = c.Name, Value = c.Id };
            return Json(returnJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSettingValue(int id = 0, string value = "")
        {
            var s = settingRepository.GetSingle(id);
            s.SettingValue = value;
            settingRepository.Edit(s);
            settingRepository.Save();
            return Content(value);
        }

        public ActionResult GetImages(int storeId)
        {
            var images = FileManagerRepository.GetFilesByStoreId(storeId);
            return Json(images, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHiearchicalNodesInfo()
        {
            var tree = this.CategoryRepository.CreateCategoriesTree(1, "family");

            return Json(tree, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFiles(int contentId)
        {
            var files = ContentFileRepository.GetContentByContentId(contentId);
            return Json(files, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SaveHiearchy(string childId, string parentId)
        //{
        //  // JsTreeDAO.SaveNodeRelationship(childId, parentId);

        //    return null; //you may return any success flag etc
        //}
        //public ActionResult RenameNode(string nodeId, string nodeNewTitle)
        //{
        //   // JsTreeDAO.RenameNode(nodeId, nodeNewTitle);

        //    return null; //you may return any success flag etc
        //}

        //public JsonResult CreateFolder(string folderName, string parentId)
        //{
        //   // int newNodeId = JsTreeDAO.AddSubNode(parentId, folderName);

        //    return Json(new { nodeId = newNodeId });//id of newly created node is required for rename callback.
        //}

        //public ActionResult DeleteSubNode(string folderId)
        //{
        //   // JsTreeDAO.DeleteSubNode(folderId);

        //    return null; //you may return any success flag etc
        //}
       
    }
    
}
