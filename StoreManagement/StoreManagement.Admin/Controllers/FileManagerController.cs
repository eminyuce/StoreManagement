using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Enums;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;
using GoogleDriveUploader;
using Ninject;
using StoreManagement.Data;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class FileManagerController : BaseController
    {
        private const String ControllerName = "FileManager";


        public int SessionStoreId
        {
            get { return Session["storeId"].ToString().ToInt(); }
            set { Session["storeId"] = value; }
        }


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DisplayImages(int storeId = 1, String search = "")
        {
            storeId = GetStoreId(storeId);
            ViewBag.Store = StoreRepository.GetSingle(storeId);
            var images = FileManagerRepository.GetFilesBySearchKey(storeId, search);



            return View(images);
        }
        public ActionResult UploadImages(int storeId = 1)
        {
            storeId = GetStoreId(storeId);
            SessionStoreId = storeId;
            ViewBag.Store = StoreRepository.GetSingle(storeId);
            return View();
        }
        public PartialViewResult ImageGallery()
        {
            return PartialView("_ImageGallery");
        }

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
            var f = new FileManager();
            if (id.ToInt() > 0)
            {
                f = FileManagerRepository.GetSingle(id.ToInt());
            }
            else
            {
                f = FileManagerRepository.GetFilesByGoogleImageId(id);
            }


            var filename = f.Title;

            try
            {
                ConnectToStoreGoogleDrive(SessionStoreId);
                this.UploadHelper.deleteFile(f.GoogleImageId);
            }
            catch (Exception ewx)
            {
                Logger.Error(ewx, "Exception is occured." + ewx.StackTrace);
            }

            FileManagerRepository.Delete(f);
            FileManagerRepository.Save();
        }

        [HttpGet]
        public ActionResult Download(string id)
        {
            var f = FileManagerRepository.GetFilesByGoogleImageId(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            var labels = Request.Form["labels"].ToStr();
            int storeId = SessionStoreId;

            var r = new List<ViewDataUploadFilesResult>();
            List<String> labelArray = labels.Split(",".ToCharArray()).ToList();

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

                SaveImagesLabels(labelArray.ToArray(), statuses.Select(r1 => r1.Id.ToStr()).ToList(), storeId);

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";


                return result;


            }

            return Json(r);

        }


        private FileManager SaveFiles(HttpPostedFileBase file, int storeId = 1)
        {


            var fileManager = ConvertToFileManager(file, storeId);
            try
            {
                var imageBype = ImageHelper.CreateGoogleImage(file);
                ConnectToStoreGoogleDrive(SessionStoreId);
                var googleFile = this.UploadHelper.InsertFile(file.FileName, "File Desc", imageBype);
                ConvertToFileManager(fileManager, googleFile);
                fileManager.FileStatus = "Success";
            }
            catch (Exception ewx)
            {
                Logger.Error(ewx, "this.UploadHelper.InsertFile Exception is occured." + ewx.StackTrace, storeId);
                fileManager.FileStatus = "Error";
            }

            FileManagerRepository.Add(fileManager);
            FileManagerRepository.Save();

            return fileManager;


        }

        private static void ConvertToFileManager(FileManager fileManager, GoogleDriveFile googleFile)
        {
            fileManager.GoogleImageId = googleFile.Id;
            fileManager.ThumbnailLink = googleFile.ThumbnailLink;
            fileManager.ModifiedDate = googleFile.ModifiedDate;
            fileManager.UpdatedDate = googleFile.ModifiedDate;
            fileManager.OriginalFilename = googleFile.OriginalFilename;
            fileManager.Title = googleFile.Title;
            fileManager.IconLink = googleFile.IconLink;
            fileManager.CreatedDate = googleFile.CreatedDate;
            fileManager.WebContentLink = googleFile.WebContentLink;
            fileManager.Width = googleFile.Width.HasValue ? googleFile.Width.Value : 0;
            fileManager.Height = googleFile.Height.HasValue ? googleFile.Height.Value : 0;
            fileManager.ImageSourceType = "GoogleApi";
            fileManager.FileSize = "";
        }



        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            int storeId = SessionStoreId;
            var fileManager = SaveFiles(file, storeId);


            statuses.Add(new ViewDataUploadFilesResult()
            {
                Id = fileManager.Id,
                GoogleImageId = fileManager.GoogleImageId,
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = String.Format("/{0}/Download/", ControllerName) + fileManager.GoogleImageId,
                delete_url = String.Format("/{0}/Delete/", ControllerName) + fileManager.GoogleImageId,
                //thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                thumbnail_url = String.Format("https://docs.google.com/uc?id={0}", fileManager.GoogleImageId),
                delete_type = "GET",
            });




        }

        private static FileManager ConvertToFileManager(HttpPostedFileBase file, int storeId = 1)
        {
            var fileManager = new FileManager();
            fileManager.ContentType = file.ContentType;
            fileManager.ContentLength = file.ContentLength;
            fileManager.CreatedDate = DateTime.Now;
            fileManager.Title = file.FileName;
            fileManager.State = true;
            fileManager.StoreId = storeId;
            fileManager.Ordering = 1;
            return fileManager;
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses)
        {

            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                // var fullPath = Path.Combine(StorageRoot, Path.GetFileName(file.FileName));

                // file.SaveAs(fullPath);
                int storeId = SessionStoreId;
                var fileManager = SaveFiles(file, storeId);




                statuses.Add(new ViewDataUploadFilesResult()
                {
                    Id = fileManager.Id,
                    GoogleImageId = fileManager.GoogleImageId,
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = String.Format("/{0}/Download/", ControllerName) + fileManager.GoogleImageId,
                    delete_url = String.Format("/{0}/Delete/", ControllerName) + fileManager.GoogleImageId,
                    // thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    thumbnail_url = String.Format("https://docs.google.com/uc?id={0}", fileManager.GoogleImageId),
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
        public string GoogleImageId { get; set; }
        public int Id { get; set; }
    }

}