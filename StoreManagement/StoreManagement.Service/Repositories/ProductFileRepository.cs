using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ProductFileRepository : BaseRepository<ProductFile, int>, IProductFileRepository
    {
        public ProductFileRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<ProductFile> GetProductByProductId(int productId)
        {
            return this.GetAllIncluding(r => r.FileManager).Where(r => r.ProductId == productId).ToList();
        }

        public List<ProductFile> GetProductByFileManagerId(int fileManagerId)
        {
            return this.FindBy(r => r.FileManagerId == fileManagerId).ToList();
        }

        public void DeleteProductFileByProductId(int productId)
        {
            foreach (var c in this.GetProductByProductId(productId))
            {
                this.Delete(c);
            }
            this.Save();
        }

        public void SaveProductFiles(int[] selectedFileId, int productId)
        {
            DeleteProductFileByProductId(productId);
            var uniqueFileIds = selectedFileId.Distinct();
            foreach (var i in uniqueFileIds)
            {
                var m = new ProductFile();
                m.ProductId = productId;
                m.FileManagerId = i;
                Add(m);
            }
            Save();
        }
    }
}
