using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ContentFileService : BaseService, IContentFileService
    {
        public ContentFileService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<ContentFile> GetContentByContentId(int contentId)
        {
            throw new NotImplementedException();
        }

        public List<ContentFile> GetContentByFileManagerId(int fileManagerId)
        {
            throw new NotImplementedException();
        }

        public void DeleteContentFileByContentId(int contentId)
        {
            throw new NotImplementedException();
        }

        public void SaveContentFiles(int[] selectedFileId, int contentId)
        {
            throw new NotImplementedException();
        }
    }
}
