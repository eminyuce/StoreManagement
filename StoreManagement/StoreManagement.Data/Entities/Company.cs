using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using StoreManagement.Data.Paging;

namespace StoreManagement.Data.Entities
{
    public class Company : IEntity, IDto
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public string Sector { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Tel3 { get; set; }
        public string Fax1 { get; set; }
        public string Fax2 { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
    }
}
