using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IContactGeneralRepository : IGeneralRepository
    {
        Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, int ? take,  bool ? isActive);
 
    }
}
