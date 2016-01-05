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


        public ContactLiquid(Contact item,  int imageWidth, int imageHeight)
        {
            this.Contact = item;

            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public int Id
        {
            get { return Contact.Id; }
        }
        public String Name
        {
            get { return Contact.Name; }
        }
       
        public DateTime CreatedDate
        {
            get { return Contact.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Contact.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Contact.State; }
        }
    }
}
