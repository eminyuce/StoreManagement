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
    public class Blog  
    {
        public Content Content { get; set; }
        public Category Category { get; set; }
        private HttpRequestBase HttpRequestBase  { get; set; }

        public Blog(HttpRequestBase httpRequestBase,Content content, Category category)
        {
            this.HttpRequestBase = httpRequestBase;
            this.Content = content;
            this.Category = category;
        }


        public String DetailLink
        {
            get
            {
                return LinkHelper.GetBlogLink(this.Content, Category.Name);
            }
        }

        public String ImageSource
        {
            get
            {
                if (ImageHas)
                {
                    var urlHelper = new UrlHelper(HttpRequestBase.RequestContext);
                    var firstOrDefault = this.Content.ContentFiles.FirstOrDefault();
                    return urlHelper.Action("FetchImage", "Images", new
                        {
                            id = firstOrDefault.FileManager.GoogleImageId,
                            size = "",
                            contentType = firstOrDefault.FileManager.ContentType
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
               return this.Content.ContentFiles.Any();
            }
        }
    }
}
