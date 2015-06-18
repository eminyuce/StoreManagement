using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface ILabelLineService : IService
    {
        List<LabelLine> GetLabelLinesByItem(int itemId, String itemType);
    }
}
