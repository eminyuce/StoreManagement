using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{

    public class ProductLiquid : BaseDrop
    {

        public Product Product { get; set; }
        public ProductCategory Category { get; set; }
        public ImageLiquid ImageLiquid { get; set; }
        public Brand Brand { get; set; }


        public ProductLiquid(Product product, ProductCategory category, int imageWidth=0, int imageHeight=0)
        {

            this.Product = product;
            this.Category = category;

            List<BaseFileEntity> baseFileEntities = product.ProductFiles != null && product.ProductFiles.Any() ? product.ProductFiles.Cast<BaseFileEntity>().ToList() : new List<BaseFileEntity>();
            this.ImageLiquid = new ImageLiquid(baseFileEntities, imageWidth, imageHeight);
            this.ImageLiquid.ImageState = product.ImageState;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

        }
       

        public String DetailLink
        {
            get
            {
                String catName = Category == null ? "category" : Category.Name;
                return LinkHelper.GetProductLink(this.Product, catName);
            }
        }


        public String PlainDescription
        {
            get
            {
                try
                {
                    var item = YuceConvert.StripHtml(this.Product.Description);

                    return item;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }


        public int Id
        {
            get { return Product.Id; }
        }
        public String Name
        {
            get { return Product.Name; }
        }
        public int UnitsInStock
        {
            get { return Product.UnitsInStock; }
        }
        public double Price
        {
            get { return Product.Price; }
        }
        public String Description
        {
            get { return Product.Description; }
        }
        public String VideoUrl
        {
            get { return Product.VideoUrl; }
        }
        public DateTime CreatedDate
        {
            get { return Product.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Product.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Product.State; }
        }
    }
}
