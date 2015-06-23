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

    public class EmailListRepository : BaseRepository<EmailList, int>, IEmailListRepository
    {
        public EmailListRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public List<EmailList> GetStoreEmailList(int storeId)
        {
            return this.FindBy(r => r.StoreId == storeId).ToList();
        }
    }
}
