using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IHelper
    {
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        List<Setting> StoreSettings { get; set; }
    }
}