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

        public ProductCategoryLiquid ProductCategoryLiquid
        {
            get
            {
                return new ProductCategoryLiquid(this.Category);
            }
        }

        public ProductLiquid ProductLiquid
        {
            get
            {
                return new ProductLiquid(this.Product, this.Category);
            }
        }

        public List<ProductCategoryLiquid> ProductCategoryLiquids
        {
            get { return Categories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }

        public List<ProductLiquid> ProductLiquids
        {
            get { return RelatedProducts.Select(r => new ProductLiquid(r, this.Categories.FirstOrDefault(r2 => r2.Id == r.ProductCategoryId))).ToList(); }
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
