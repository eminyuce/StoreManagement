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

        public CommentLiquid(Comment comment, int imageWidth, int imageHeight)
        {
            this.Comment = comment;

            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;
        }

        public int Id
        {
            get { return Comment.Id; }
        }
        public String Name
        {
            get { return Comment.Name; }
        }
        public String CommentText
        {
            get { return Comment.CommentText; }
        }
        public String CommentType
        {
            get { return Comment.CommentType; }
        }
        public String Email
        {
            get { return Comment.Email; }
        }
        public DateTime CreatedDate
        {
            get { return Comment.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Comment.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Comment.State; }
        }

    }
}
