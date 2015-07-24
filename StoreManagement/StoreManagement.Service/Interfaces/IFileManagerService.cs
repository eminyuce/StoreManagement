using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface IFileManagerService : IService
    {
        List<FileManager> GetFilesByStoreIdFromCache(int storeId);
        List<FileManager> GetFilesByStoreId(int storeId);

        FileManager GetFilesByGoogleImageId(String googleImageId);
        List<FileManager> GetFilesByGoogleImageIdArray(String[] googleImageId);
        FileManager GetFilesById(int id);
        List<FileManager> GetStoreCarousels(int storeId);
        StorePagedList<FileManager> GetImagesByStoreId(int storeId, int page, int pageSize);


    }
}
