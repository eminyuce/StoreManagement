using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity, TId> : IEntityRepository<TEntity, TId> where TEntity : class, IEntity<TId> where TId : IComparable
    {

    }
}
