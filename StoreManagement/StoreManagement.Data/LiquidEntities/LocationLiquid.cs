﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class LocationLiquid : BaseDrop
    {
        public Location Location;

        public LocationLiquid(Location location, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
            this.Location = location;
            this.PageDesign = pageDesign;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }


    }
}