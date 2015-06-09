using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class StoreUserService : BaseService, IStoreUserService
    {
        public StoreUserService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public StoreUser GetStoreUserByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
