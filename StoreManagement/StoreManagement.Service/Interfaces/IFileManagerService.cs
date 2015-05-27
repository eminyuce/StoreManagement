using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IFileManagerService
    {

        List<FileManager> GetFilesByStoreId(int storeId);
        FileManager GetFilesByGoogleImageId(String googleImageId);


    }
}
