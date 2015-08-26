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
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public PageDesign PageDesign { get; set; }
    }
}
