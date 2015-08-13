using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public abstract class BaseDrop : Drop
    {
        protected int ImageWidth { get; set; }
        protected int ImageHeight { get; set; }
        protected PageDesign PageDesign { get; set; }
    }
}
