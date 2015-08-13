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
            List<FileManager> fileManagers = content.ContentFiles != null && content.ContentFiles.Any() ? content.ContentFiles.Select(r => r.FileManager).ToList() : new List<FileManager>();
            this.ImageLiquid = new ImageLiquid(fileManagers, pageDesign, imageWidth, imageHeight);
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
