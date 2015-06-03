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
        public string Name { get; set; }
        public string Modul { get; set; }
        public string ControllerName { get; set; }
        public int Ordering { get; set; }
        public Boolean Static { get; set; }
        public string Link { get; set; }
        public Boolean LinkState { get; set; }
        public DateTime CreatedDate { get; set; }
        public Boolean State { get; set; }
    }
}
