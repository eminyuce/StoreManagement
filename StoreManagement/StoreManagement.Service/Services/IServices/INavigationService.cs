using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Service.Services.IServices
{
    public interface INavigationService : IBaseService
    {
        bool IsModulActive(string controllerName);

        StoreLiquidResult GetMainLayoutLink(
       List<Navigation> navigations,
       PageDesign pageDesign);

        StoreLiquidResult GetMainLayoutFooterLink(List<Navigation> navigations,
                                                  PageDesign pageDesign);
    }
}
