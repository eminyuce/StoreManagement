using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class LabelLine : IEntity
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ItemType { get; set; }
        public int LabelId { get; set; }

    }
}
