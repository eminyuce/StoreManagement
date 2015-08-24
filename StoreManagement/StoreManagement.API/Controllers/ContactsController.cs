using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class ContactsController : BaseApiController<Contact>, IContactService
    {
        public override IEnumerable<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Contact Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Contact value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Contact value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Contact>> GetContactsByStoreIdAsync(int storeId, bool? isActive)
        {
            return await ContactRepository.GetContactsByStoreIdAsync(storeId, isActive);
        }
    }
}
