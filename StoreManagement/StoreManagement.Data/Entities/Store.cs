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
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [IgnoreDataMember]
        public bool State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }

        public virtual ICollection<FileManager> FileManagers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreUser> StoreUsers { get; set; }

        public int CategoryId { get; set; }

        public String GoogleDriveClientId { set; get; }
        [IgnoreDataMember]
        public String GoogleDriveUserEmail { set; get; }
        [IgnoreDataMember]
        public String GoogleDriveUserEmailPassword { set; get; }
        [IgnoreDataMember]
        public String GoogleDriveFolder { set; get; }
        [IgnoreDataMember]
        public String GoogleDriveServiceAccountEmail { set; get; }
        [IgnoreDataMember]
        public String GoogleDriveCertificateP12FileName { set; get; }
        [IgnoreDataMember]
        public String GoogleDrivePassword { set; get; }
        [IgnoreDataMember]
        public byte[] GoogleDriveCertificateP12RawData { set; get; }

        public bool IsCacheEnable { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }

    }

}
