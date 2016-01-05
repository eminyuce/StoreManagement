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
        public String Type { get; set; }

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

        public int Id
        {
            get { return Category.Id; }
        }
        public String Name
        {
            get { return Category.Name; }
        }
        public String Description
        {
            get { return Category.Description; }
        }
        public DateTime CreatedDate
        {
            get { return Category.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Category.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Category.State; }
        }

    }
}
