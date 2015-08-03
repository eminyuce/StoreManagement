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
            string url = string.Format("http://{0}/api/{1}/GetFilesByStoreIdFromCache?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<FileManager>(url);

        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetFilesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<FileManager>(url);

        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {

            string url = string.Format("http://{0}/api/{1}/GetFilesByGoogleImageId?storeId={2}", WebServiceAddress, ApiControllerName, googleImageId);
            return HttpRequestHelper.GetUrlResult<FileManager>(url);

        }

        public List<FileManager> GetFilesByGoogleImageIdArray(string[] googleImageId)
        {
            throw new NotImplementedException();
        }

        public FileManager GetFilesById(int id)
        {

            string url = string.Format("http://{0}/api/{1}/GetFilesById?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResult<FileManager>(url);

        }

        public List<FileManager> GetStoreCarousels(int storeId)
        {

            string url = string.Format("http://{0}/api/{1}/GetStoreCarousels?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<FileManager>(url);

        }

        public StorePagedList<FileManager> GetImagesByStoreId(int storeId, int page, int pageSize)
        {

            String parameters = string.Format("GetImagesByStoreId?storeId={0}&page={1}&pageSize={2}", storeId, page, pageSize);
            string url = string.Format("http://{0}/api/{1}/{2}", WebServiceAddress, ApiControllerName, parameters);
            return HttpRequestHelper.GetUrlPagedResults<FileManager>(url);

        }

        public Task<List<FileManager>> GetImagesByStoreIdAsync(int storeId, bool? isActive)
        {
            string url = string.Format("http://{0}/api/{1}/GetImagesByStoreIdAsync?storeId={2}&isActive={3}", WebServiceAddress, ApiControllerName, storeId, isActive);
            return HttpRequestHelper.GetUrlResultsAsync<FileManager>(url);
        }
    }
}
