using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductsViewModel : ViewModel
    {

        public List<ProductCategory> SCategories { get; set; }

        public List<ProductCategoryLiquid> Categories
        {
            get { return SCategories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }
        public List<ProductLiquid> Products
        {
            get
            {
                var returnList = new List<ProductLiquid>();
                var relatedProducts = SCategories.Select(r => r.Products).ToList();
                foreach (var relatedProduct in relatedProducts)
                {
                    foreach (var product in relatedProduct)
                    {
                        returnList.Add(new ProductLiquid(product, SCategories.FirstOrDefault(r => r.Id == product.ProductCategoryId)));
                    }
                }
                return returnList;
            }
        }

       
    }
}
