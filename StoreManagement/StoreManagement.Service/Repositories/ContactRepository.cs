using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ContactRepository : BaseRepository<Contact, int>, IContactRepository
    {


        public ContactRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Contact> GetContactsByStoreId(int storeId, string search)
        {
            return BaseEntityRepository.GetBaseEntitiesSearchList(this, storeId, search);
        }

        public Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, int? take, bool? isActive)
        {
            return BaseEntityRepository.GetActiveBaseEnitiesAsync(this, storeId, take, isActive);
        }


    }
}
