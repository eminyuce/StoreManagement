using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductDetailViewModel : ViewModel
    {
        public ProductCategory SCategory { get; set; }
        public Product SProduct { get; set; }
        public List<ProductCategory> SCategories { get; set; }
        public List<Product> SRelatedProducts { get; set; }

        public ProductCategoryLiquid ProductCategoryLiquid
        {
            get
            {
                return new ProductCategoryLiquid(this.SCategory);
            }
        }

        public ProductLiquid Product
        {
            get
            {
                return new ProductLiquid(this.SProduct, this.SCategory);
            }
        }

        public List<ProductCategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }

        public List<ProductLiquid> Products
        {
            get { return SRelatedProducts.Select(r => new ProductLiquid(r, this.SCategories.FirstOrDefault(r2 => r2.Id == r.ProductCategoryId))).ToList(); }
        }

       

    }
}
