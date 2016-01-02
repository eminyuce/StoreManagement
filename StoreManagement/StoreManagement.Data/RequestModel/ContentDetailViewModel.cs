using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ContentDetailViewModel : BaseDrop
    {
        public Content Content { get; set; }
        public Store Store { get; set; }
        public List<Category> Categories { get; set; }
        public Category Category { get; set; }
        public List<Content> RelatedContents { get; set; }
        public String Type { get; set; }

        public CategoryLiquid CategoryLiquid
        {
            get
            {
                return new CategoryLiquid(this.Category, Type);
            }
        }

        public ContentLiquid ContentLiquid
        {
            get
            {
                return new ContentLiquid(this.Content, this.Category, this.Type);
            }
        }

        public List<CategoryLiquid> CategoryLiquids
        {
            get { return Categories.Select(r => new CategoryLiquid(r, Type)).ToList(); }
        }

        public List<ContentLiquid> ContentLiquids
        {
            get { return RelatedContents.Select(r => new ContentLiquid(r, this.Categories.FirstOrDefault(r2 => r2.Id == r.CategoryId), Type)).ToList(); }
        }


        public StoreLiquid StoreLiquid
        {
            get
            {
                return new StoreLiquid(Store);
            }
        }
    }
}
