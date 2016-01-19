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
    public class ContentsViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public PagedList<Content> Contents { get; set; }
        public List<Category> Categories { get; set; }
        public String Type { get; set; }

        public StoreLiquid StoreLiquid
        {
            get
            {
                return new StoreLiquid(Store);
            }
        }

        public List<CategoryLiquid> CategoryLiquids
        {
            get { return Categories.Select(r => new CategoryLiquid(r, Type)).ToList(); }
        }

        public List<ContentLiquid> ContentLiquids
        {
            get
            {
                var contentLiquidList = new List<ContentLiquid>();
                foreach (var c in Contents)
                {
                    var mm = this.Categories.FirstOrDefault(r2 => r2.Id == c.CategoryId);
                    if (mm == null)
                    {
                                      contentLiquidList.Add( new ContentLiquid(c,Categories.First(), Type));
                    }
                    else
                    {
                                      contentLiquidList.Add( new ContentLiquid(c,mm, Type));
                    }
      
                }
                return contentLiquidList;
            }
        }


    }
}
