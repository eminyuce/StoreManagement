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
    public class ContactRepository : BaseRepository<Contact, int>, IContactRepository
    {
        public ContactRepository(IStoreContext dbContext) : base(dbContext)
        {

        }
    }
}
