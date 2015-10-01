using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class CategoryLiquid : BaseDrop
    {

        public Category  Category { get; set; }
        private String Type { get; set; }

        public CategoryLiquid(Category category, String type)
        {
            this.Category = category;
            this.Type = type;
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetCategoryLink(this.Category, this.Type);
            }
        }
        public int Count { get; set; }
    }
}
