using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneCell { get; set; }
    }
}
