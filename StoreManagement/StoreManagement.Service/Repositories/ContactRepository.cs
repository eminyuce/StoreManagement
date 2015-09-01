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

        public Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                Expression<Func<Contact, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                var items = this.FindAllAsync(match, take);

                var itemsResult = items;

                return itemsResult;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
        
       
    }
}
