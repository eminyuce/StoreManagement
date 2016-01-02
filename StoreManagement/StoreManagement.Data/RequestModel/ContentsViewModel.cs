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
            get { return Contents.Select(r => new ContentLiquid(r, this.Categories.FirstOrDefault(r2 => r2.Id == r.CategoryId), Type)).ToList(); }
        }


    }
}
