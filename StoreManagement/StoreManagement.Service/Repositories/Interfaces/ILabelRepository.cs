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
    public interface ILabelRepository : IBaseRepository<Label, int>, ILabelService, IDisposable 
    {
        List<Label> GetActiveLabels(int storeId);
        Label GetLabelByName(string label, int storeId);
        List<Label> GetStoreLabels(int storeId);
        List<Label> GetLabelsByStoreId(int storeId, string searchKey);
    }
}
