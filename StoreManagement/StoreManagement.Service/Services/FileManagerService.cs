using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class FileManagerService : BaseService, IFileManagerService
    {
        public FileManagerService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            throw new NotImplementedException();
        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {
            throw new NotImplementedException();
        }
    }
}
