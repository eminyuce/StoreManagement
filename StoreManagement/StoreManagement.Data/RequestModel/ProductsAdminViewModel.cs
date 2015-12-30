using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductsAdminViewModel : BaseDrop
    {
        public List<Product> Products { get; set; }

        public List<ProductCategory> Categories { get; set; }
    }
}
