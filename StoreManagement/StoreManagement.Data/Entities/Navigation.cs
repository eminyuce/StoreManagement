using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Navigation : IEntity
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ParentId { get; set; }
        public Boolean Static { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int Ordering { get; set; }
        public Boolean ImageState { get; set; }
        public string Lang { get; set; }
        public string Link { get; set; }
        public Boolean LinkState { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PageMetaKeys { get; set; }
        public Boolean IsController { get; set; }
        public Boolean IsAction { get; set; }
        public Boolean IsMainMenu { get; set; }
        public Boolean State { get; set; }
    }
}
