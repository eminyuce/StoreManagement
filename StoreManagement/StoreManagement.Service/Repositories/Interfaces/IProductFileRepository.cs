using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IProductFileRepository : IBaseRepository<ProductFile, int>, IProductFileGeneralRepository, IDisposable 
    {
        void DeleteProductFileByProductId(int productId);
        void SaveProductFiles(int[] selectedFileId, int productId);
        void SetMainImage(int id, int fileId);
    }
}
