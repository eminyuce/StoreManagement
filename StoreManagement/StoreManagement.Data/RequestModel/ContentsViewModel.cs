using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ContentsViewModel : ViewModel
    {
        public PagedList<Content> SContents { get; set; }
        public List<Category> SCategories { get; set; }
        public String Type { get; set; }


        public List<CategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new CategoryLiquid(r, Type)).ToList(); }
        }

        public List<ContentLiquid> Contents
        {
            get
            {
                var contentLiquidList = new List<ContentLiquid>();
                foreach (var c in SContents)
                {
                    var mm = this.SCategories.FirstOrDefault(r2 => r2.Id == c.CategoryId);
                    if (mm == null)
                    {
                        contentLiquidList.Add(new ContentLiquid(c, SCategories.First(), Type));
                    }
                    else
                    {
                        contentLiquidList.Add(new ContentLiquid(c, mm, Type));
                    }

                }
                return contentLiquidList;
            }
        }


    }
}
