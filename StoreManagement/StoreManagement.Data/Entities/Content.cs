using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Content : IEntity
    {

        public Content()
        {

        }

        public int Id { get; set; }
        [IgnoreDataMember]
        public int StoreId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        
        [AllowHtml]
        public string Description { get; set; }
        
        [IgnoreDataMember]
        public string Type { get; set; }
        [IgnoreDataMember]
        public Boolean MainPage { get; set; }
        [IgnoreDataMember]
        public Boolean ImageState { get; set; }
        [IgnoreDataMember]
        public Boolean State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<ContentFile> ContentFiles { get; set; }

 

    }
}
