using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ContentFileRepository : BaseRepository<ContentFile, int>, IContentFileRepository
    {
        public ContentFileRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<ContentFile> GetContentFilesByContentId(int contentId)
        {
            return this.GetAllIncluding(r => r.FileManager).Where(r => r.ContentId == contentId).ToList();
        }

        public List<ContentFile> GetContentFilesByFileManagerId(int fileManagerId)
        {
            return this.FindBy(r => r.FileManagerId == fileManagerId).ToList();
        }

        public void DeleteContentFileByContentId(int contentId)
        {
            foreach (var c in this.GetContentFilesByContentId(contentId))
            {
                this.Delete(c);
            }
            this.Save();
        }
        public void SaveContentFiles(int[] selectedFileId, int contentId)
        {
            var selectedFileIdUniqueFileIds = selectedFileId.Distinct();
            var list = this.FindBy(r => r.ContentId == contentId).ToList();
            var fileIdUniqueFileIds = list.Select(r => r.FileManagerId).Distinct();
            
            // Delete already existing items.
            foreach (var fileManagerId in fileIdUniqueFileIds)
            {
                if (!selectedFileIdUniqueFileIds.Contains(fileManagerId))
                {
                    var item = list.FirstOrDefault(r => r.FileManagerId == fileManagerId && r.ContentId == contentId);
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
                    var m = new ContentFile();
                    m.ContentId = contentId;
                    m.FileManagerId = i;
                    m.IsMainImage = false;
                    Add(m);
                }
            }
            Save();
        }

        public void SetMainImage(int id, int fileId)
        {
            var items = this.FindBy(r => r.ContentId == id).ToList();
            foreach (var productFile in items)
            {
                productFile.IsMainImage = false;
                Edit(productFile);
            }
            Save();
            var item = this.FindBy(r => r.ContentId == id && r.FileManagerId == fileId).FirstOrDefault();
            if (item != null)
            {
                item.IsMainImage = true;
                Edit(item);
            }
            else
            {
                item = new ContentFile();
                item.FileManagerId = fileId;
                item.ContentId = id;
                item.IsMainImage = true;
                Add(item);
            }

            Save();
        }
    }



}
