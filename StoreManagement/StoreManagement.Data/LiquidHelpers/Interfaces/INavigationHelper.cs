using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace  StoreManagement.Data.LiquidHelpers.Interfaces
{
    public interface INavigationHelper : IHelper
    {
        StoreLiquidResult GetMainLayoutLink(
           List<Navigation> navigations,
           PageDesign pageDesign);

        StoreLiquidResult GetMainLayoutFooterLink(List<Navigation> navigations,
                                                  PageDesign pageDesign);
    }
}