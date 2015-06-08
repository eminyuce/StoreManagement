using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class FileManagerService : BaseService, IFileManagerService
    {
        private const String ApiControllerName = "FileManagers";
        public FileManagerService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<FileManager> GetFilesByStoreIdFromCache(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetFilesByStoreIdFromCache?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return HttpRequestHelper.GetUrlResults<FileManager>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<FileManager>();
            }
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetFilesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return HttpRequestHelper.GetUrlResults<FileManager>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<FileManager>();
            }
        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetFilesByGoogleImageId?storeId={2}", WebServiceAddress, ApiControllerName, googleImageId);
                return HttpRequestHelper.GetUrlResult<FileManager>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new FileManager();
            }
        }

        public List<FileManager> GetStoreCarousels(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetStoreCarousels?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return HttpRequestHelper.GetUrlResults<FileManager>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<FileManager>();
            }
        }

        public StorePagedList<FileManager> GetImagesByStoreId(int storeId, int page, int pageSize)
        {
            try
            {
                String parameters = string.Format("GetImagesByStoreId?storeId={0}&page={1}&pageSize={2}", storeId, page, pageSize);
                string url = string.Format("http://{0}/api/{1}/{2}", WebServiceAddress, ApiControllerName, parameters);
                return HttpRequestHelper.GetUrlPagedResults<FileManager>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new StorePagedList<FileManager>();
            }
        }
    }
}
