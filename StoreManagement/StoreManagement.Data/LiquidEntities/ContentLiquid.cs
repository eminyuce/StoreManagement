using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
    public class ContentLiquid : BaseDrop
    {
        public Content Content { get; set; }
        public Category Category { get; set; }
        public ImageLiquid ImageLiquid { get; set; }
        private String Type { get; set; }




        public ContentLiquid(Content content, Category category, PageDesign pageDesign, String type, int imageWidth, int imageHeight)
        {
            this.Content = content;
            this.Category = category;
            this.PageDesign = pageDesign;
            List<BaseFileEntity> baseFileEntities = content.ContentFiles != null && content.ContentFiles.Any() ? content.ContentFiles.Cast<BaseFileEntity>().ToList() : new List<BaseFileEntity>();
            this.ImageLiquid = new ImageLiquid(baseFileEntities, pageDesign, imageWidth, imageHeight);
            this.ImageLiquid.ImageState = content.ImageState;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            this.Type = type;
        }




        public String DetailLink
        {
            get
            {
                return LinkHelper.GetContentLink(this.Content, Category.Name, this.Type);
            }
        }
       
    }
}
