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
    [LiquidType]
    public class ContentLiquid
    {
        public Content Content { get; set; }
        public Category Category { get; set; }
        public PageDesign PageDesign { get; set; }

        public ContentLiquid(Content content, Category category, PageDesign pageDesign)
        {
            this.Content = content;
            this.Category = category;
            this.PageDesign = pageDesign;
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetBlogLink(this.Content, Category.Name);
            }
        }
        //int width = 60, int height = 60
        public String ImageSource
        {
            get
            {
                if (ImageHas)
                {
                    var firstOrDefault = this.Content.ContentFiles.FirstOrDefault();
                    return LinkHelper.GetImageLink("Thumbnail", firstOrDefault.FileManager.GoogleImageId, this.PageDesign.ImageWidth,this.PageDesign.ImageHeight);
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
                return this.Content.ContentFiles.Any();
            }
        }
    }
}
