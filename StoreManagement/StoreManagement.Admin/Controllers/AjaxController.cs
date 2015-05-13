using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/

        [Inject]
        public IFileManagerRepository FileManagerRepository { get; set; }
      
        private IStoreRepository storeRepository;
        private ICategoryRepository categoryRepository;
        public AjaxController(IStoreContext dbContext, 
            ISettingRepository settingRepository,
            IStoreRepository storeRepository,
            ICategoryRepository categoryRepository)
            : base(dbContext, settingRepository)
        {
            this.storeRepository = storeRepository;
            this.categoryRepository = categoryRepository;
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
            var tree = this.categoryRepository.CreateCategoriesTree(1, "family");

            return Json(tree, JsonRequestBehavior.AllowGet);
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
