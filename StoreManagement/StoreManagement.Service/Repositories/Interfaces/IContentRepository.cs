using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IContentRepository : IEntityRepository<Content, int>
    {

    }
}
