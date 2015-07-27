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
    [LiquidType]
    public class ProductLiquid
    {

        public Product Product { get; set; }
        public ProductCategory Category { get; set; }
        public PageDesign PageDesign { get; set; }

        private HttpRequestBase HttpRequestBase { get; set; }

        public ProductLiquid(HttpRequestBase httpRequestBase, Product product, ProductCategory category, PageDesign pageDesign)
        {
            this.HttpRequestBase = httpRequestBase;
            this.Product = product;
            this.Category = category;
            this.PageDesign = pageDesign;
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetProductLink(this.Product, Category.Name);
            }
        }
        //int width = 60, int height = 60
        public String ImageSource
        {
            get
            {
                if (ImageHas)
                {
                    var urlHelper = new UrlHelper(HttpRequestBase.RequestContext);
                    var firstOrDefault = this.Product.ProductFiles.FirstOrDefault();
                    return urlHelper.AbsoluteAction("Thumbnail", "Images", new
                            {
                                id = firstOrDefault.FileManager.GoogleImageId,
                                width = this.PageDesign.ImageWidth,
                                height = this.PageDesign.ImageHeight
                            });

                }
                else
                {

                    return "";
                }
            }
        }

        public bool ImageHas
        {
            get
            {
                return this.Product.ProductFiles.Any();
            }
        }
    }
}
