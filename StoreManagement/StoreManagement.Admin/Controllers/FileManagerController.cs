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


        [Inject]
        public IUploadHelper UploadHelper { set; get; }

        private String GoogleDriveClientId { set; get; }
        private String GoogleDriveUserEmail { set; get; }
        private String GoogleDriveFolder { set; get; }
        private String GoogleDriveServiceAccountEmail { set; get; }
        private X509Certificate2 Certificate { set; get; }
        private String GoogleDrivePassword { set; get; }


        public void ConnectToStoreGoogleDrive()
        {
            //GoogleDriveClientId = ProjectAppSettings.GetWebConfigString("GoogleDriveClientId");
            //GoogleDriveUserEmail = ProjectAppSettings.GetWebConfigString("GoogleDriveUserEmail");
            //GoogleDriveServiceAccountEmail = ProjectAppSettings.GetWebConfigString("GoogleDriveServiceAccountEmail");
            //String googleDriveCertificateP12FileName = ProjectAppSettings.GetWebConfigString("GoogleDriveCertificateP12FileName");
            //Certificate = GeneralHelper.CreateCert(
            //    HostingEnvironment.MapPath(
            //    String.Format(@"~\App_Data\GoogleDrive\{0}", googleDriveCertificateP12FileName)),
            //    ProjectAppSettings.GetWebConfigString("GoogleDrivePassword"));
            //GoogleDriveFolder = ProjectAppSettings.GetWebConfigString("GoogleDriveFolder");
            //GoogleDrivePassword = ProjectAppSettings.GetWebConfigString("GoogleDrivePassword");


            if (IsSuperAdmin)
            {
                int storeId = Session["storeId"].ToString().ToInt();
                var selectedStore = StoreRepository.GetStore(storeId);
                GoogleDriveClientId = selectedStore.GoogleDriveClientId;
                GoogleDriveUserEmail = selectedStore.GoogleDriveUserEmail;
                GoogleDriveFolder = selectedStore.GoogleDriveFolder;
                GoogleDriveServiceAccountEmail = selectedStore.GoogleDriveServiceAccountEmail;
                GoogleDrivePassword = selectedStore.GoogleDrivePassword;
                Certificate = GeneralHelper.CreateCert(
                    HostingEnvironment.MapPath(
                    String.Format(@"~\App_Data\GoogleDrive\{0}", selectedStore.GoogleDriveCertificateP12FileName)),
                    ProjectAppSettings.GetWebConfigString("GoogleDrivePassword"));
            }
            else
            {
                GoogleDriveClientId = LoginStore.GoogleDriveClientId;
                GoogleDriveUserEmail = LoginStore.GoogleDriveUserEmail;
                GoogleDriveFolder = LoginStore.GoogleDriveFolder;
                GoogleDriveServiceAccountEmail = LoginStore.GoogleDriveServiceAccountEmail;
                GoogleDrivePassword = LoginStore.GoogleDrivePassword;
                Certificate = GeneralHelper.CreateCert(
                    HostingEnvironment.MapPath(
                    String.Format(@"~\App_Data\GoogleDrive\{0}", LoginStore.GoogleDriveCertificateP12FileName)),
                    ProjectAppSettings.GetWebConfigString("GoogleDrivePassword"));
            }
            this.UploadHelper.Connect(GoogleDriveClientId,
                   GoogleDriveUserEmail,
                   GoogleDriveServiceAccountEmail,
                   Certificate,
                   GoogleDriveFolder, GoogleDrivePassword);


        }


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DisplayImages(int storeId = 1, String search = "")
        {
            storeId = GetStoreId(storeId);
            ViewBag.StoreId = storeId;
            var images = FileManagerRepository.GetFilesByStoreId(storeId);

            if (!String.IsNullOrEmpty(search))
            {
                images = images.Where(r => r.Title.ToLower().Contains(search.ToLower())).ToList();
            }




            return View(images);
        }
        public ActionResult UploadImages(int storeId = 1)
        {
            storeId = GetStoreId(storeId);
            Session["storeId"] = storeId;
            ViewBag.StoreId = storeId;
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
                ConnectToStoreGoogleDrive();
                this.UploadHelper.deleteFile(f.GoogleImageId);
            }
            catch (Exception ewx)
            {
                Logger.Error("Exception is occured.", ewx);
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

            var r = new List<ViewDataUploadFilesResult>();
            var labels = Request.Form["labels"].ToStr();
            List<String> labelArray = labels.Split(",".ToCharArray()).ToList();
            int storeId = Session["storeId"].ToString().ToInt();

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
                var fileByte = GeneralHelper.ReadFully(file.InputStream);
                ConnectToStoreGoogleDrive();
                var googleFile = this.UploadHelper.InsertFile(file.FileName, "File Desc", fileByte);
                ConvertToFileManager(fileManager, googleFile);
            }
            catch (Exception ewx)
            {
                Logger.Error("this.UploadHelper.InsertFile Exception is occured." + ewx.StackTrace, ewx);
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
            fileManager.OriginalFilename = googleFile.OriginalFilename;
            fileManager.Title = googleFile.Title;
            fileManager.IconLink = googleFile.IconLink;
            fileManager.CreatedDate = googleFile.CreatedDate;
            fileManager.WebContentLink = googleFile.WebContentLink;
            fileManager.Width = googleFile.Width.HasValue ? googleFile.Width.Value : 0;
            fileManager.Height = googleFile.Height.HasValue ? googleFile.Height.Value : 0;
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
            int storeId = Session["storeId"].ToString().ToInt();
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
                int storeId = Session["storeId"].ToString().ToInt();
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