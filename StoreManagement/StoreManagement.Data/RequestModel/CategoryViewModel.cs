using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;

namespace StoreManagement.Data.RequestModel
{
    public class CategoryViewModel
    {
        public Store Store { get; set; }
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public PagedList<Content> Contents { get; set; }
    }
}
