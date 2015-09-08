using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class CommentLiquid : BaseDrop
    {
        public Comment Comment;

        public CommentLiquid(Comment comment, PageDesign pageDesign, int imageWidth, int imageHeight)
        {
            this.Comment = comment;
            this.PageDesign = pageDesign;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }
    }
}
