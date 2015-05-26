using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;
using GoogleDriveUploader;
using Ninject;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class FileManagerController : BaseController
    {
        private const String ControllerName = "FileManager";
      

        [Inject]
        public IUploadHelper UploadHelper { set; get; }

        public FileManagerController(IStoreContext dbContext,
            ISettingRepository settingRepository) : base(dbContext, settingRepository)
        {
            
        }

        private string StorageRoot
        {
            get { return Path.Combine(Server.MapPath("~/Files")); }
        }
        public ActionResult DisplayImages(int storeId=1)
        {
            return View(FileManagerRepository.GetFilesByStoreId(storeId));
        }
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ImageGallery()
        {
            return PartialView("_ImageGallery");
        }
        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS

        [HttpPost]
        public ActionResult DeleteAll(List<String> values)
        {
            foreach (var id in values)
            {
                DeleteFile(id);
            }
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            DeleteFile(id);
            return RedirectToAction("DisplayImages");
        }

        private void DeleteFile(string id)
        {
            var f = FileManagerRepository.GetSingle(id.ToInt());
            var filename = f.FileName;
            var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            FileManagerRepository.Delete(f);
            FileManagerRepository.Save();
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public ActionResult Download(string id)
        {
            var f = FileManagerRepository.GetSingle(id.ToInt());
            var filename = f.FileName;
            var filePath = Path.Combine(Server.MapPath("~/Files"), filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
                
            }
            else
            {
                context.Response.StatusCode = 404;   
            }
            return RedirectToAction("Index");
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();


                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }
                 

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }

        private FileManager SaveFiles(HttpPostedFileBase file, int storeId=1)
        {
            var fileManager = ConvertToFileManager(file, storeId);
            FileManagerRepository.Add(fileManager);
            FileManagerRepository.Save();

            return fileManager;
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fileManager = SaveFiles(file);



            var fullName = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }



      
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = String.Format("/{0}/Download/",ControllerName) + fileManager.Id,
                delete_url = String.Format("/{0}/Delete/", ControllerName) + fileManager.Id,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });




        }

        private static FileManager ConvertToFileManager(HttpPostedFileBase file, int storeId=1)
        {
            var fileManager = new FileManager();
            fileManager.ContentType = file.ContentType;
            fileManager.ContentLength = file.ContentLength;
            fileManager.CreatedDate = DateTime.Now;
            fileManager.FileName = file.FileName;
            fileManager.State = true;
            fileManager.StoreId = storeId;
            fileManager.Ordering = 1;
            fileManager.Description = "";
            fileManager.Guid = Guid.NewGuid().ToStr();
            return fileManager;
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Path.Combine(StorageRoot, Path.GetFileName(file.FileName));

                file.SaveAs(fullPath);

                try
                {
                    var fileByte = GeneralHelper.ReadFully(file.InputStream);
                    var googleFile = this.UploadHelper.InsertFile(file.FileName, "File Desc", fileByte);
                    String id = googleFile.Id;
                }
                catch (Exception ewx)
                {
                    Logger.Error("Exception is occured.",ewx);
                }

                var fileManager = SaveFiles(file);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = String.Format("/{0}/Download/", ControllerName) + fileManager.Id,
                    delete_url = String.Format("/{0}/Delete/", ControllerName) + fileManager.Id,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });
            }
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }


    }

}