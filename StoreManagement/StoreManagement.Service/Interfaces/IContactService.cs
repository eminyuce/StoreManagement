using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IContactService : IService
    {
        Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, int ? take,  bool ? isActive);
 
    }
}
