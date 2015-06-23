using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IEmailListService : IService
    {
        List<EmailList> GetStoreEmailList(int storeId);

    }
}
