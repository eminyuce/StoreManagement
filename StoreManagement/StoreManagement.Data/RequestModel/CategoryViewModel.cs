using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class CategoryViewModel : ViewModel
    {
        public Category SCategory { get; set; }
        public List<Category> SCategories { get; set; }
        public PagedList<Content> SContents { get; set; }
        public String Type { get; set; }

        public CategoryLiquid Category
        {
            get
            {
                return new CategoryLiquid(this.SCategory, Type);
            }
        }
        public List<CategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new CategoryLiquid(r, Type)).ToList(); }
        }

        public List<ContentLiquid> Contents
        {

            get
            {
                var contentsResult = new List<ContentLiquid>();
                foreach (var content in SContents)
                {
                    var cat = this.SCategories.FirstOrDefault(r2 => r2.Id == content.CategoryId);
                    if (cat != null)
                    {
                        contentsResult.Add(new ContentLiquid(content, cat, Type));
                    }
                }
                return contentsResult;

            }
        }



    }
}
