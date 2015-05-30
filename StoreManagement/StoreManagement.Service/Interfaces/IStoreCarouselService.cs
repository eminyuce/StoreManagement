using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Interfaces
{
    public interface IStoreCarouselService
    {
        List<StoreCarousel> GetStoreCarousels(int storeId); 
    }
}
