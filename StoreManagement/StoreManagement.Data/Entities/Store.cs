using System;
using System.Runtime.Serialization;
using GenericRepository;
using System.Collections.Generic;

namespace StoreManagement.Data.Entities
{
    public class Store : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Layout { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [IgnoreDataMember]
        public bool State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }

        public virtual ICollection<FileManager> FileManagers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreUser> StoreUsers { get; set; }


        public override string ToString()
        {
            return Id + " " + Name;
        }
         
    }

}
