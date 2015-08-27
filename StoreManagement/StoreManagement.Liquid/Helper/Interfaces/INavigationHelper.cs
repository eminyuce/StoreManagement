using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface INavigationHelper : IHelper
    {
        StoreLiquidResult GetMainLayoutLink(
            Task<List<Navigation>> navigationsTask,
            Task<PageDesign> pageDesignTask);

        StoreLiquidResult GetMainLayoutFooterLink(Task<List<Navigation>> navigationsTask,
                                                  Task<PageDesign> pageDesignTask);
    }
}