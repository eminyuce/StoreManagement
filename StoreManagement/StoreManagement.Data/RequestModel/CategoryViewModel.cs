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
    public class CategoryViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public PagedList<Content> Contents { get; set; }
    }
}
