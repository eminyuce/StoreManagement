using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class LabelLiquid : BaseDrop
    {
        public Label Label { get; set; }

        public LabelLiquid(Label label)
        {
            this.Label = label;

        }


        public string Link
        {
            get { return LinkHelper.GetLabelLink(this.Label); }
        }

        public int Id
        {
            get { return Label.Id; }
        }
        public String Name
        {
            get { return Label.Name; }
        }

        public DateTime CreatedDate
        {
            get { return Label.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Label.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Label.State; }
        }


    }
}
