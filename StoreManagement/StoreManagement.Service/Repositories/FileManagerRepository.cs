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
    public class FileManagerRepository : EntityRepository<FileManager, int>, IFileManagerRepository
    {
        private IStoreContext dbContext;
        public FileManagerRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            return FindBy(r => r.StoreId == storeId).ToList();
        }
    }



}
