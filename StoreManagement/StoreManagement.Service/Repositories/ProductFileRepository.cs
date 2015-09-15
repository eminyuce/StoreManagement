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

        public List<ProductFile> GetProductFilesByProductId(int productId)
        {
            return this.GetAllIncluding(r => r.FileManager).Where(r => r.ProductId == productId).ToList();
        }

        public List<ProductFile> GetProductFilesByFileManagerId(int fileManagerId)
        {
            return this.FindBy(r => r.FileManagerId == fileManagerId).ToList();
        }

        public void DeleteProductFileByProductId(int productId)
        {
            foreach (var c in this.GetProductFilesByProductId(productId))
            {
                this.Delete(c);
            }
            this.Save();
        }

        public void SaveProductFiles(int[] selectedFileId, int productId)
        {
            var selectedFileIdUniqueFileIds = selectedFileId.Distinct();
            var list = this.FindBy(r => r.ProductId == productId).ToList();
            var fileIdUniqueFileIds = list.Select(r => r.FileManagerId).Distinct();

            // Delete already existing items.
            foreach (var fileManagerId in fileIdUniqueFileIds)
            {
                if (!selectedFileIdUniqueFileIds.Contains(fileManagerId))
                {
                    var item = list.FirstOrDefault(r => r.FileManagerId == fileManagerId && r.ProductId == productId);
                    this.Delete(item);
                }
            }
            this.Save();


            foreach (var i in selectedFileIdUniqueFileIds)
            {
                int fileManagerId = i;
                var item = list.FirstOrDefault(r => r.FileManagerId == fileManagerId);
                if (item == null)
                {
                    var m = new ProductFile();
                    m.ProductId = productId;
                    m.FileManagerId = i;
                    m.IsMainImage = false;
                    Add(m);
                }
            }
            Save();
        }

        public void SetMainImage(int id, int fileId)
        {
            var items = this.FindBy(r => r.ProductId == id).ToList();
            foreach (var productFile in items)
            {
                productFile.IsMainImage = false;
                Edit(productFile);
            }
            Save();
            var item = this.FindBy(r => r.ProductId == id && r.FileManagerId == fileId).FirstOrDefault();
            if (item != null)
            {
                item.IsMainImage = true;
                Edit(item);
            }
            else
            {
                item = new ProductFile();
                item.FileManagerId = fileId;
                item.ProductId = id;
                item.IsMainImage = true;
                Add(item);
            }

            Save();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
