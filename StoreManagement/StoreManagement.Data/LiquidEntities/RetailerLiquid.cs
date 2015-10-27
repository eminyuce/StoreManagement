using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class RetailerLiquid : BaseDrop
    {
        public Retailer Retailer { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public RetailerLiquid(Retailer retailer)
        {
            Retailer = retailer;
        }
        public RetailerLiquid(Retailer retailer, int imageWidth, int imageHeight)
        {
            this.Retailer = retailer;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }
        public String DetailLink
        {
            get
            {
                return LinkHelper.GetRetailerDetailLink(this.Retailer);
            }
        }


        public string Link
        {
            get { return LinkHelper.GetRetailerLink(this.Retailer); }
        }

        public int ImageHeightProduct { get; set; }
        public int ImageWidthProduct { get; set; }
        public List<ProductLiquid> ProductLiquidList
        {
            get
            {
                var list = new List<ProductLiquid>();

                foreach (var item in Products)
                {
                    var category = ProductCategories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                    if (category != null)
                    {
                        var productLiquid = new ProductLiquid(item, category, ImageWidthProduct, ImageHeightProduct);
                        list.Add(productLiquid);
                    }
                }

                return list;
            }
        }


        public List<ProductCategoryLiquid> ProductCategoriesLiquids
        {
            get
            {

                var cats = new List<ProductCategoryLiquid>();
                foreach (var item in ProductCategories)
                {
                    cats.Add(new ProductCategoryLiquid(item));
                }

                return cats;
            }
        }
    }
}
