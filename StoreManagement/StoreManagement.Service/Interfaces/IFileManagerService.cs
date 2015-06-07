using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IFileManagerService : IService
    {
        List<FileManager> GetFilesByStoreIdFromCache(int storeId);
        List<FileManager> GetFilesByStoreId(int storeId);
        FileManager GetFilesByGoogleImageId(String googleImageId);
        List<FileManager> GetStoreCarousels(int storeId);

    }
}
