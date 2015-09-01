using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class ActivitiesLiquid : BaseDrop
    {
        public Activity Activity;

        public ActivitiesLiquid(Activity item, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
            this.Activity = item;
            this.PageDesign = pageDesign;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }
    }
}
