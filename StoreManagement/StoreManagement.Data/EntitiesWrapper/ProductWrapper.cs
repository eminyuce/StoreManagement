using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.EntitiesWrapper
{
    public class ProductWrapper : BaseWrapper
    {
        public Category Category { set; get; }
        public Product Product { set; get; }
        public ProductWrapper(Product product, Category categoryName)
        {
            this.Product = product;
            this.Category = categoryName;

            Name = Product.Name;
            Description = Product.Description;
            HasImage = Product.ProductFiles != null && Product.ProductFiles.Any();
            DetailLink = LinkHelper.GetProductLink(Product, Category.Name);
        }


    }
}
