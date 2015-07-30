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
    public class CategoryLiquid : Drop
    {
        public ProductCategory Category { get; set; }
        public PageDesign PageDesign { get; set; }


        public CategoryLiquid(ProductCategory category, PageDesign pageDesign)
        {
            this.Category = category;
            this.PageDesign = pageDesign;
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetCategoryLink(this.Category);
            }
        }

    }
}
