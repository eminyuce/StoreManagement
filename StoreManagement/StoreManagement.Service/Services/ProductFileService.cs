using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ProductFileService : BaseService, IProductFileService
    {

        public ProductFileService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<ProductFile> GetProductByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public List<ProductFile> GetProductByFileManagerId(int fileManagerId)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductFileByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public void SaveProductFiles(int[] selectedFileId, int productId)
        {
            throw new NotImplementedException();
        }
    }

    
}
