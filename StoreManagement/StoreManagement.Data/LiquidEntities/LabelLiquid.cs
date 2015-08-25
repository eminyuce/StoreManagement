using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class LabelLiquid : BaseDrop
    {
        public Label Label { get; set; }

        public LabelLiquid(Label label, PageDesign pageDesign)
        {
            this.Label = label;
            this.PageDesign = pageDesign;
        }

    }
}
