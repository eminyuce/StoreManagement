using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class ProductAttributeRelation : IEntity
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public int ProductId { get; set; }
        public String ProductAttributeValue { get; set; }

    }
}
