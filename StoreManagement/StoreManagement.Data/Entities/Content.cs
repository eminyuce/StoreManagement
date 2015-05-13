using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Content : IEntity 
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public Boolean MainPage { get; set; }
        public Boolean State { get; set; }
        public Boolean ImageState { get; set; }
        public int Ordering { get; set; }
        public string Lang { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Url { get; set; }

    }
}
