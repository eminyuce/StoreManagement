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
        public PageDesign PageDesign { get; set; }
        public ImageLiquid ImageLiquid { get; set; }

        public ProductLiquid(Product product, ProductCategory category, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
     
            this.Product = product;
            this.Category = category;
            this.PageDesign = pageDesign;
            List<FileManager> fileManagers = product.ProductFiles !=null && product.ProductFiles.Any() ? product.ProductFiles.Select(r => r.FileManager).ToList() : new List<FileManager>();
            this.ImageLiquid = new ImageLiquid(fileManagers, pageDesign, imageWidth, imageHeight);

            ImageWidth = imageWidth;
            ImageHeight = imageHeight;



        }
       
        public String DetailLink
        {
            get
            {
                return LinkHelper.GetProductLink(this.Product, Category.Name);
            }
        }




    }
}
