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


        public CategoryLiquid(Category category)
        {
            this.Category = category;
     
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetCategoryLink(this.Category);
            }
        }
        public int Count { get; set; }
    }
}
