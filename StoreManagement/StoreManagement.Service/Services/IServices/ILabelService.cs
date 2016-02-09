using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Service.Services.IServices
{
    public interface ILabelService : IBaseService
    {
        StoreLiquidResult GetProductLabels(
            List<Label> labels,
            PageDesign pageDesign);
    }
}
