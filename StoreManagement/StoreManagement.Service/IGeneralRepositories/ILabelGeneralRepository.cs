using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface ILabelGeneralRepository : IGeneralRepository
    {
        Task<List<Label>> GetLabelsByItemTypeId(int storeId, int itemId, string itemType);

    }
}
