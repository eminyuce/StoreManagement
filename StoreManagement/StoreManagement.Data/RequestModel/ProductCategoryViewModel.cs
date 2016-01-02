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
    public class ProductCategoryViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public ProductCategory Category { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public PagedList<Product> Products { get; set; }

        public ProductCategoryLiquid ProductCategoryLiquid
        {
            get { return new ProductCategoryLiquid(Category);  }
        }

        public List<ProductCategoryLiquid> ProductCategoryLiquids
        {
            get { return Categories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }
        public List<ProductLiquid> ProductLiquids
        {
            get
            {
                return Products.Select(product => new ProductLiquid(product, Categories.FirstOrDefault(r => r.Id == product.ProductCategoryId))).ToList();
            }
        }

        public StoreLiquid StoreLiquid
        {
            get
            {
                return new StoreLiquid(Store);
            }
        }

    }
}
