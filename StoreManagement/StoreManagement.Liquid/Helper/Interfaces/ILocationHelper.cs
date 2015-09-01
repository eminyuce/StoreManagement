using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface ILocationHelper : IHelper
    {

        StoreLiquidResult GetLocationIndexPage(Task<PageDesign> pageDesignTask, Task<List<Location>> locationsTask);
    }
}