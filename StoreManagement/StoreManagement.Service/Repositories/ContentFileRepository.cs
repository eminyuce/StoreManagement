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
        public ContentFileRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<ContentFile> GetContentByContentId(int contentId)
        {
            return this.GetAllIncluding(r => r.FileManager).Where(r => r.ContentId == contentId).ToList();
        }
        public List<ContentFile> GetContentByFileManagerId(int fileManagerId)
        {
            return this.FindBy(r => r.FileManagerId == fileManagerId).ToList();
        }

        public void DeleteContentFileByContentId(int contentId)
        {
            foreach (var c in this.GetContentByContentId(contentId))
            {
                this.Delete(c);
            }
            this.Save();
        }
        public void SaveContentFiles(int[] selectedFileId, int contentId)
        {
            DeleteContentFileByContentId(contentId);
            var uniqueFileIds = selectedFileId.Distinct();
            foreach (var i in uniqueFileIds)
            {
                var m = new ContentFile();
                m.ContentId = contentId;
                m.FileManagerId = i;
                Add(m);
            }
            Save();
        }
    }



}
