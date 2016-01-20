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
    public class ProductCategoryViewModel : ViewModel
    {
        public ProductCategory SCategory { get; set; }
        public List<ProductCategory> SCategories { get; set; }
        public PagedList<Product> SProducts { get; set; }

        public ProductCategoryLiquid Category
        {
            get { return new ProductCategoryLiquid(SCategory);  }
        }

        public List<ProductCategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }
        public List<ProductLiquid> Products
        {
            get
            {
                return SProducts.Select(product => new ProductLiquid(product, SCategories.FirstOrDefault(r => r.Id == product.ProductCategoryId))).ToList();
            }
        }

      

    }
}
