using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IProductFileGeneralRepository : IGeneralRepository
    {
        List<ProductFile> GetProductFilesByProductId(int productId);
        List<ProductFile> GetProductFilesByFileManagerId(int fileManagerId);
    
    }
}
