using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface ILabelService : IService
    {
        List<Label> GetLabelsByItemType(int itemType);
        List<Label> GetLabelsByItemType(int storeId, int itemType);

    }
}
