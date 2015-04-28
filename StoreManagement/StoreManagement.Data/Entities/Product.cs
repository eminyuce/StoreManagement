using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Product : IEntity 
    {
        public int Id { get; set; }
        public String Name { get; set; }


    }
}
