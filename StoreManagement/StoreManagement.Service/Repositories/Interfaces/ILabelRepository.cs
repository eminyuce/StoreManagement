using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ILabelRepository : IEntityRepository<Label, int>, ILabelService
    {
        List<Label> GetLabelsCategoryAndSearch(int storeId, string search);
    }
}
