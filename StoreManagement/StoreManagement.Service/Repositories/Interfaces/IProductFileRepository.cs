using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IProductFileRepository : IEntityRepository<ProductFile, int>, IProductFileService
    {
        void DeleteProductFileByProductId(int productId);
        void SaveProductFiles(int[] selectedFileId, int productId);

    }
}
