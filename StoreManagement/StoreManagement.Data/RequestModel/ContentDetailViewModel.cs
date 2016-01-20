using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ContentDetailViewModel : ViewModel
    {
        public Content SContent { get; set; }
        public List<Category> SCategories { get; set; }
        public Category SCategory { get; set; }
        public List<Content> SRelatedContents { get; set; }
        public String Type { get; set; }

        public CategoryLiquid Category
        {
            get
            {
                return new CategoryLiquid(this.SCategory, Type);
            }
        }

        public ContentLiquid Content
        {
            get
            {
                return new ContentLiquid(this.SContent, this.SCategory, this.Type);
            }
        }

        public List<CategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new CategoryLiquid(r, Type)).ToList(); }
        }

        public List<ContentLiquid> Contents
        {
            get { return SRelatedContents.Select(r => new ContentLiquid(r, this.SCategories.FirstOrDefault(r2 => r2.Id == r.CategoryId), Type)).ToList(); }
        }


        
    }
}
