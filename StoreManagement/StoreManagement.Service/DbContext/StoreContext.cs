using GenericRepository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.DbContext
{
    public class StoreContext : EntitiesContext, IStoreContext
    {

       public StoreContext(String nameOrConnectionString) : base(nameOrConnectionString) { }

       public IDbSet<Product> Products { get; set; }
    


    }
}
