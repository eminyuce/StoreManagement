using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleDriveUploader;
using NLog;
using Ninject;
using Quartz;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.ScheduledTasks.Jobs
{
    public class FilesDeleteJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [Inject]
        public IFileManagerRepository FileManagerRepository { get; set; }

        [Inject]
        public IUploadHelper UploadHelper { set; get; }

        [Inject]
        public IStoreRepository StoreRepository { set; get; }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
               // DeleteFiles("deleted");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


            try
            {
             //   DeleteFiles("error");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


        } 

        private void DeleteFiles(string fileStatus)
        {
            var errorFiles = FileManagerRepository.GetFilesByFileStatus(fileStatus);
            var stores = errorFiles.GroupBy(r => r.StoreId);
            var errorList = new List<int>();
            foreach (var store in stores)
            {
                try
                {
                    int storeId = store.Key;
                    ConnectGoogleDrive(storeId);
                    errorList.AddRange(DeleteGoogleDriveFiles(errorFiles));
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "DeleteFiles:" + ex.Message, fileStatus);
                }
            }

            errorFiles = errorFiles.Where(r => !errorList.Contains(r.Id)).ToList();
            DeleteFileFromDb(fileStatus, errorFiles);
        }

        private void DeleteFileFromDb(string fileStatus, List<FileManager> files)
        {
            try
            {
                if (files.Count > 0)
                {
                    foreach (var fileManager in files)
                    {
                         
                            FileManagerRepository.Delete(fileManager);
                       
                    }
                    FileManagerRepository.Save();
                    Logger.Info(String.Format("Deleted " + fileStatus + " files:" + files.Count));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeleteFiles From Db:" + ex.Message, fileStatus);
            }
        }

        private List<int> DeleteGoogleDriveFiles(List<FileManager> files)
        {
            var errorList = new List<int>();
            foreach (var file in files)
            {
                try
                {
                    this.UploadHelper.deleteFile(file.GoogleImageId);
                }
                catch (Exception ex)
                {
                    errorList.Add(file.Id);
                    Logger.Error(ex, "DeleteFiles From google Drive:" + ex.Message);
                }
            }
            return errorList;
        }

        private void ConnectGoogleDrive(int storeId)
        {
            Store selectedStore = StoreRepository.GetStore(storeId);

            var GoogleDriveClientId = selectedStore.GoogleDriveClientId;
            var GoogleDriveUserEmail = selectedStore.GoogleDriveUserEmail;
            var GoogleDriveFolder = selectedStore.GoogleDriveFolder;
            var GoogleDriveServiceAccountEmail = selectedStore.GoogleDriveServiceAccountEmail;
            var GoogleDrivePassword = selectedStore.GoogleDrivePassword;
            var Certificate = GeneralHelper.ImportCert(selectedStore.GoogleDriveCertificateP12RawData,
                                                       selectedStore.GoogleDrivePassword);

            this.UploadHelper.Connect(GoogleDriveClientId,
                                      GoogleDriveUserEmail,
                                      GoogleDriveServiceAccountEmail,
                                      Certificate,
                                      GoogleDriveFolder, GoogleDrivePassword);
        }
    }
}