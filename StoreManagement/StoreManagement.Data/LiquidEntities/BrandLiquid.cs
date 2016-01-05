using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class BrandLiquid : BaseDrop
    {
        public Brand Brand;
        public List<Product> Products { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public BrandLiquid(Brand brand, int imageWidth, int imageHeight)
        {
            this.Brand = brand;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public String DetailLink
        {
            get
            {
                return LinkHelper.GetBrandDetailLink(this.Brand);
            }
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

        public int Id
        {
            get { return Brand.Id; }
        }
        public String Name
        {
            get { return Brand.Name; }
        }
        public String Description
        {
            get { return Brand.Description; }
        }
        public DateTime CreatedDate
        {
            get { return Brand.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Brand.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Brand.State; }
        }

    }
}
