using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
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

        public List<EmailList> GetStoreEmailList(int storeId, string search)
        {
            var emailList = this.FindBy(r => r.StoreId == storeId);
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                emailList = emailList.Where(r => r.Email.ToLower().Contains(search.ToLower().Trim()) 
                    || r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return emailList.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }
    }
}
