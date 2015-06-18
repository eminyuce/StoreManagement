using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductCategoryViewModel
    {
        public Store Store { get; set; }
        public ProductCategory Category { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public PagedList<Product> Products { get; set; }
    }
}
