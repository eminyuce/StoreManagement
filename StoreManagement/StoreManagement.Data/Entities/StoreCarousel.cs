using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class StoreCarousel : IEntity
    {
        public int Id { get; set; }
        public int FileManagerId { get; set; }
        public int StoreId { get; set; }
        public string Type { get; set; }

        public FileManager FileManager { get; set; }
    }
}
