using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
          
    public class Comment : BaseEntity
    {
        public int ParentId { set; get; }
        [Required(ErrorMessage = "Please enter name")]
        public int Name { set; get; }
        [Required(ErrorMessage = "Please enter email")]
        public int Email { set; get; }
        public int CommentType { set; get; }
        [Required(ErrorMessage = "Please enter comment")]
        public int CommentText { set; get; }
    }
}
