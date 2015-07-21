using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.EntitiesWrapper
{
    public class ContentWrapper : BaseWrapper
    {

        public Category Category { set; get; }
        public Content Content { set; get; }

        public ContentWrapper(Content content, Category category)
        {
            Content = content;
            Category = category;

            Name = Content.Name;
            Description = Content.Description;
            HasImage = Content.ContentFiles != null && Content.ContentFiles.Any();
            DetailLink = LinkHelper.GetContentLink(Content, Category.Name);
        }
    }
}
