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
        public String Type { get; set; }




        public ContentLiquid(Content content, Category category, String type, int imageWidth=0, int imageHeight=0)
        {
            this.Content = content;
            this.Category = category;

            List<BaseFileEntity> baseFileEntities = content.ContentFiles != null && content.ContentFiles.Any() ? content.ContentFiles.Cast<BaseFileEntity>().ToList() : new List<BaseFileEntity>();
            this.ImageLiquid = new ImageLiquid(baseFileEntities, imageWidth, imageHeight);
            this.ImageLiquid.ImageState = content.ImageState;
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            this.Type = type;
        }

        public CategoryLiquid CategoryLiquid
        {
            get { return new CategoryLiquid(this.Category, Type); }
        }

        public String DetailLink
        {
            get
            {
                return LinkHelper.GetContentLink(this.Content, Category.Name, this.Type);
            }
        }
        public String PlainDescription
        {
            get { return YuceConvert.StripHtml(this.Content.Description); }
        }
        public int Id
        {
            get { return Content.Id; }
        }
        public String Name
        {
            get { return Content.Name; }
        }

        public DateTime CreatedDate
        {
            get { return Content.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Content.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Content.State; }
        }

        public int CategoryId
        {
            get { return Content.CategoryId; }
        }
        public String Author
        {
            get { return Content.Author; }
        }

    }
}
