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
    public class ContentFileRepository : EntityRepository<ContentFile, int>, IContentFileRepository
    {
        private IStoreContext dbContext;
        public ContentFileRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext =  dbContext;
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
        }
    }



}
