using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class EmailListService : BaseService, IEmailListService
    {
        public EmailListService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<EmailList> GetStoreEmailList(int storeId)
        {
            throw new NotImplementedException();
        }
    }
}
