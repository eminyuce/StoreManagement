using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStoreCarouselRepository : IEntityRepository<StoreCarousel, int>, IStoreCarouselService  
    {

    }
}
