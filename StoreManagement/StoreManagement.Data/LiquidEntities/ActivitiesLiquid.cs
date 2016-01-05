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

        public ActivitiesLiquid(Activity item, int imageWidth, int imageHeight)
        {
            this.Activity = item;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public String Name
        {
            get { return Activity.Name; }
        }
        public int Id
        {
            get { return Activity.Id; }
        }
        public String Description
        {
            get { return Activity.Description; }
        }

        public DateTime BeginDate
        {
            get { return Activity.BeginDate; }
        }
        public DateTime FinishDate
        {
            get { return Activity.FinishDate; }
        }
       


    }
}
