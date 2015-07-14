using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;

namespace StoreManagement.Service.Repositories.Interfaces
{
     
    public interface IBaseRepository<T, TId> : IEntityRepository<T, TId> where T : class, IEntity<TId> where TId : IComparable
    {

    }
}
