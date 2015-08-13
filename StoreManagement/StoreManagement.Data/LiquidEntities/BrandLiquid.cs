using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class BrandLiquid : BaseDrop
    {
        public Brand Brand;
        public BrandLiquid(Brand brand, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
            this.Brand = brand;
            this.PageDesign = pageDesign;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public String DetailLink
        {
            get
            {
                return LinkHelper.GetBrandDetailLink(this.Brand);
            }
        }
    }
}
