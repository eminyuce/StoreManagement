using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IProductFileService : IService
    {
        List<ProductFile> GetProductByProductId(int productId);
        List<ProductFile> GetProductByFileManagerId(int fileManagerId);
        void DeleteProductFileByProductId(int productId);
        void SaveProductFiles(int[] selectedFileId, int productId);
    }
}
