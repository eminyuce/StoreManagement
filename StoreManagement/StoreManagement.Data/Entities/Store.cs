using System;
using System.Runtime.Serialization;
using GenericRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Data.Entities
{
    public class Store : IEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter domain")]
        public string Domain { get; set; }
        [Required(ErrorMessage = "Please enter layout")]
        public string Layout { get; set; }
        public  DateTime ? CreatedDate { get; set; }
        public DateTime ? UpdatedDate { get; set; }

        [IgnoreDataMember]
        public bool State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }

        public virtual ICollection<FileManager> FileManagers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreUser> StoreUsers { get; set; }

        public int CategoryId { get; set; }

        public String GoogleDriveClientId { set; get; }
        public String GoogleDriveUserEmail { set; get; }        
        public String GoogleDriveUserEmailPassword { set; get; }
        public String GoogleDriveFolder { set; get; }
        public String GoogleDriveServiceAccountEmail { set; get; }
        public String GoogleDriveCertificateP12FileName { set; get; }
        public String GoogleDrivePassword { set; get; }

        public override string ToString()
        {
            return Id + " " + Name;
        }

    }

}
