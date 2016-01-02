using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductsViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public List<ProductCategory> Categories { get; set; }

        public List<ProductCategoryLiquid> ProductCategoryLiquids
        {
            get { return Categories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }
        public List<ProductLiquid> ProductLiquids
        {
            get
            {
                var returnList = new List<ProductLiquid>();
                var relatedProducts = Categories.Select(r => r.Products).ToList();
                foreach (var relatedProduct in relatedProducts)
                {
                    foreach (var product in relatedProduct)
                    {
                        returnList.Add(new ProductLiquid(product, Categories.FirstOrDefault(r => r.Id == product.ProductCategoryId)));
                    }
                }

                return returnList;
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
