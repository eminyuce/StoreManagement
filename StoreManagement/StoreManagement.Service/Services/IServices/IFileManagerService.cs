using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IFileManagerService
    {

       PhotosViewModel GetPhotos(int page);
       List<FileManager> GetFilesByStoreId(int storeId);
    }
}
