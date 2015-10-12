using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Message : BaseEntity
    {

        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string UserMessage { get; set; }

        public override string ToString()
        {
            return Name + " " + Email + " " + Telefon + " " + Company + " " + UserMessage;
        }
    }
}
