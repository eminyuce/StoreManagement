using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
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
            var contacts = this.FindBy(r => r.StoreId == storeId);

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                contacts = contacts.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }
           
            return contacts.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }

        public Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, bool? isActive)
        {
            var contacts = this.FindBy(r => r.StoreId == storeId);

            if (isActive.HasValue)
            {
                var active = isActive.Value;
                contacts = contacts.Where(r => r.State == active);
            }

            return contacts.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToListAsync();
        }


    }
}
