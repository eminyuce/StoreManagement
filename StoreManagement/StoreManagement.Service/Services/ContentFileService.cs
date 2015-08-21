using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ContentFileService : BaseService, IContentFileService
    {
        protected override string ApiControllerName { get { return "ContentFiles"; } }


        public ContentFileService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<ContentFile> GetContentByContentId(int contentId)
        {
            
                string url = string.Format("http://{0}/api/{1}/GetContentByContentId?contentId={2}", WebServiceAddress, ApiControllerName, contentId);
                return HttpRequestHelper.GetUrlResults<ContentFile>(url);
           
        }

        public List<ContentFile> GetContentByFileManagerId(int fileManagerId)
        {
             
                string url = string.Format("http://{0}/api/{1}/GetContentByFileManagerId?fileManagerId={2}", WebServiceAddress, ApiControllerName, fileManagerId);
                return HttpRequestHelper.GetUrlResults<ContentFile>(url);
            
        }

        public void DeleteContentFileByContentId(int contentId)
        {
            throw new NotImplementedException();
        }

        public void SaveContentFiles(int[] selectedFileId, int contentId)
        {
            throw new NotImplementedException();
        }

 

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
