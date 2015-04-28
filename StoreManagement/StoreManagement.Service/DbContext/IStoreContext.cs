using GenericRepository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.DbContext
{
    public interface IStoreContext : IDisposable, IEntitiesContext 
    {


    }
}
