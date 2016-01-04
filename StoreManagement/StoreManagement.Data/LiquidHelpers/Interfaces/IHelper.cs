using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace  StoreManagement.Data.LiquidHelpers.Interfaces
{
    public interface IHelper
    {
        int StoreId { get; set; }
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        List<Setting> StoreSettings { get; set; }
    }
}