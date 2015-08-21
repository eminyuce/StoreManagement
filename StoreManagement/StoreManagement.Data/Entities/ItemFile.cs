using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class ItemFile : BaseFileEntity
    {
        public int ItemId { get; set; }
        public String ItemType { get; set; }
    }
}
