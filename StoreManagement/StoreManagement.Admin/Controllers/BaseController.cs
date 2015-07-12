using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using GoogleDriveUploader;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using WebMatrix.WebData;

namespace StoreManagement.Admin.Controllers
{
    public abstract class BaseController : Controller
    {

        static readonly TypedObjectCache<Store> UserStoreCache = new TypedObjectCache<Store>("UserStoreCache");



        [Inject]
        public IFileManagerRepository FileManagerRepository { get; set; }

        [Inject]
        public IContentFileRepository ContentFileRepository { set; get; }

        [Inject]
        public IContentRepository ContentRepository { set; get; }

        [Inject]
        public ICategoryRepository CategoryRepository { set; get; }

        [Inject]
        public IStoreRepository StoreRepository { set; get; }

        [Inject]
        public INavigationRepository NavigationRepository { set; get; }

        [Inject]
        public IPageDesignRepository PageDesignRepository { set; get; }

        [Inject]
        public IStoreUserRepository StoreUserRepository { set; get; }

        [Inject]
        public ISettingRepository SettingRepository { set; get; }

        [Inject]
        public IStoreContext DbContext { set; get; }

        [Inject]
        public IEmailSender EmailSender { set; get; }

        [Inject]
        public IProductRepository ProductRepository { set; get; }

        [Inject]
        public IProductFileRepository ProductFileRepository { set; get; }

        [Inject]
        public IProductCategoryRepository ProductCategoryRepository { set; get; }

        [Inject]
        public ILabelRepository LabelRepository { set; get; }

        [Inject]
        public ILabelLineRepository LabelLineRepository { set; get; }

        [Inject]
        public IEmailListRepository EmailListRepository { set; get; }



        [Inject]
        public IContactRepository ContactRepository { set; get; }


        [Inject]
        public IUploadHelper UploadHelper { set; get; }


        [Inject]
        public ILocationRepository LocationRepository { set; get; }


        [Inject]
        public IBrandRepository BrandRepository { set; get; }



        private Boolean MySuperAdmin
        {
            get
            {
                var formsIdentity = HttpContext.User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                
                    FormsAuthenticationTicket ticket = formsIdentity.Ticket;
                    string userData = ticket.UserData;

                    return userData.Equals("true", StringComparison.InvariantCultureIgnoreCase);
                }
                else
                {
                    return false;
                }
            }

        }
        protected Boolean IsSuperAdmin
        {
            get { return MySuperAdmin; }
        }

        protected int GetStoreId(int id)
        {
            if (IsSuperAdmin)
            {
                return id;
            }
            else
            {
                if (LoginStore.Id > 0)
                {
                    return LoginStore.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
        private Store MyLoginStore
        {
            get
            {

                var formsIdentity = HttpContext.User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    FormsAuthenticationTicket ticket = formsIdentity.Ticket;
                    string userData = ticket.UserData;
                    if (!string.IsNullOrEmpty(userData))
                    {
                        var serializer = new JavaScriptSerializer();
                        var s = serializer.Deserialize<Store>(userData);

                        return s;
                    }
                    else
                    {
                        return null;
                    }
               
                }
                else
                {
                    return null;
                }
            }

        }
        protected Store LoginStore
        {
            get { return MyLoginStore; }
        }
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected BaseController()
        {

        }

        protected new HttpNotFoundResult HttpNotFound(string statusDescription = null)
        {
            try
            {
                if (IsSuperAdmin)
                {
                    Logger.Trace("HttpNotFoundResult exception for Store " + statusDescription);
                }
                else
                {
                    Logger.Trace("HttpNotFoundResult exception for Store " + statusDescription + " Store:" + LoginStore);
                }

            }
            catch (Exception ex)
            {


            }
            return new HttpNotFoundResult(statusDescription);
        }

        protected string GetCleanHtml(String source)
        {
            if (String.IsNullOrEmpty(source))
                return String.Empty;

            source = HttpUtility.HtmlDecode(source);

            string path = HttpContext.Server.MapPath("~/tags.config");
            var myFile = new System.IO.StreamReader(path);
            string myTags = myFile.ReadToEnd();
            var returnHtml = HtmlCleanHelper.SanitizeHtmlSoft(myTags, source);
            returnHtml = GeneralHelper.NofollowExternalLinks(returnHtml);
            return returnHtml;
        }
        protected bool CheckRequest(BaseEntity entity)
        {
            if (IsSuperAdmin)
            {
                return true;
            }
            else
            {
                return entity.StoreId == LoginStore.Id;
            }
        }

        protected String GoogleDriveClientId { set; get; }
        protected String GoogleDriveUserEmail { set; get; }
        protected String GoogleDriveFolder { set; get; }
        protected String GoogleDriveServiceAccountEmail { set; get; }
        protected X509Certificate2 Certificate { set; get; }
        protected String GoogleDrivePassword { set; get; }
        protected void ConnectToStoreGoogleDrive(int storeId)
        {

            if (IsSuperAdmin)
            {

                var selectedStore = StoreRepository.GetStore(storeId);
                GoogleDriveClientId = selectedStore.GoogleDriveClientId;
                GoogleDriveUserEmail = selectedStore.GoogleDriveUserEmail;
                GoogleDriveFolder = selectedStore.GoogleDriveFolder;
                GoogleDriveServiceAccountEmail = selectedStore.GoogleDriveServiceAccountEmail;
                GoogleDrivePassword = selectedStore.GoogleDrivePassword;

                // String serviceAccountPkCs12FilePath = HostingEnvironment.MapPath(String.Format(@"~\App_Data\GoogleDrive\{0}", selectedStore.GoogleDriveCertificateP12FileName));
                // var rawData = System.IO.File.ReadAllBytes(serviceAccountPkCs12FilePath);
                //X509Certificate2 Certificate2 = GeneralHelper.CreateCert(serviceAccountPkCs12FilePath, selectedStore.GoogleDrivePassword);

                Certificate = GeneralHelper.ImportCert(selectedStore.GoogleDriveCertificateP12RawData,
                                                       selectedStore.GoogleDrivePassword);

                //  Certificate = GeneralHelper.CreateCert(rawData, selectedStore.GoogleDrivePassword);


            }
            else
            {
                GoogleDriveClientId = LoginStore.GoogleDriveClientId;
                GoogleDriveUserEmail = LoginStore.GoogleDriveUserEmail;
                GoogleDriveFolder = LoginStore.GoogleDriveFolder;
                GoogleDriveServiceAccountEmail = LoginStore.GoogleDriveServiceAccountEmail;
                GoogleDrivePassword = LoginStore.GoogleDrivePassword;
                Certificate = GeneralHelper.ImportCert(LoginStore.GoogleDriveCertificateP12RawData,
                                                       LoginStore.GoogleDrivePassword);
            }
            this.UploadHelper.Connect(GoogleDriveClientId,
                   GoogleDriveUserEmail,
                   GoogleDriveServiceAccountEmail,
                   Certificate,
                   GoogleDriveFolder, GoogleDrivePassword);


        }

        protected bool SaveImagesLabels(string[] labels, String selectedFile, int storeId)
        {
            var isNewLabelExists = SaveImagesLabels(labels, new List<string>() { selectedFile }, storeId);
            return isNewLabelExists;
        }
        protected bool SaveImagesLabels(string[] labels, List<string> selectedFiles, int storeId)
        {
            Boolean isNewLabelExists = false;
            foreach (var label in labels)
            {
                if (label.ToInt() > 0)
                {
                    foreach (var m in selectedFiles)
                    {
                        LabelLine labelLine = new LabelLine();
                        labelLine.ItemId = m.ToInt();
                        labelLine.ItemType = StoreConstants.Files;
                        labelLine.LabelId = label.ToInt();
                        LabelLineRepository.Add(labelLine);
                    }
                }
                else
                {
                    var existingLabel = LabelRepository.GetLabelByName(label, storeId);
                    int labelId = 0;
                    if (existingLabel == null)
                    {
                        if (!String.IsNullOrEmpty(label))
                        {
                            var newLabel = new Label();
                            newLabel.StoreId = storeId;
                            newLabel.Name = label;
                            newLabel.ParentId = 1;
                            newLabel.State = true;
                            newLabel.Ordering = 1;
                            newLabel.CreatedDate = DateTime.Now;
                            newLabel.UpdatedDate = DateTime.Now;
                            LabelRepository.Add(newLabel);
                            LabelRepository.Save();
                            labelId = newLabel.Id;
                        }
                    }
                    else
                    {
                        labelId = existingLabel.Id;
                    }

                    if (labelId > 0)
                    {
                        isNewLabelExists = true;
                        foreach (var m in selectedFiles)
                        {
                            var labelLine = new LabelLine();
                            labelLine.ItemId = m.ToInt();
                            labelLine.ItemType = StoreConstants.Files;
                            labelLine.LabelId = labelId;
                            LabelLineRepository.Add(labelLine);
                        }
                    }
                }
            }

            try
            {
                LabelLineRepository.Save();
            }
            catch (Exception ex)
            {
                Logger.Error("Error is " + ex.Message, ex);
            }
            return isNewLabelExists;
        }

    }
}
