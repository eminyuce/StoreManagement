using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductDetailViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public ProductCategory Category { get; set; }
        public Product Product { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
