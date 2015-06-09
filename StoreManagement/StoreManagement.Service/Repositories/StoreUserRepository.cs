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
    public class StoreUserRepository : BaseRepository<StoreUser, int>, IStoreUserRepository
    {
        public StoreUserRepository(IStoreContext dbContext) : base(dbContext)
        {

        }

        public StoreUser GetStoreUserByUserId(int userId)
        {
            return this.FindBy(r => r.UserId == userId).FirstOrDefault();
        }
    }



}
