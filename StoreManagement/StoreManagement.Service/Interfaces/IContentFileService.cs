using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IContentFileService
    {
        List<ContentFile> GetContentByContentId(int contentId);
        List<ContentFile> GetContentByFileManagerId(int fileManagerId);
        void DeleteContentFileByContentId(int contentId);
        void SaveContentFiles(int[] selectedFileId, int contentId);
    }
}
