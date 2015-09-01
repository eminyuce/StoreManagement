using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class ContactLiquid : BaseDrop
    {
        public Contact Contact;


        public ContactLiquid(Contact item, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
            this.Contact = item;
            this.PageDesign = pageDesign;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }


    }
}
