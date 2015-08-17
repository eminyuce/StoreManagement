using System;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
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

        [ScriptIgnore]
        public bool State { get; set; }
        [ScriptIgnore]
        public int Ordering { get; set; }

        public virtual ICollection<FileManager> FileManagers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreUser> StoreUsers { get; set; }

        public int CategoryId { get; set; }

        public String GoogleDriveClientId { set; get; }
        [ScriptIgnore]
        public String GoogleDriveUserEmail { set; get; }
        [ScriptIgnore]
        public String GoogleDriveUserEmailPassword { set; get; }
        [ScriptIgnore]
        public String GoogleDriveFolder { set; get; }
        [ScriptIgnore]
        public String GoogleDriveServiceAccountEmail { set; get; }
        [ScriptIgnore]
        public String GoogleDriveCertificateP12FileName { set; get; }
        [ScriptIgnore]
        public String GoogleDrivePassword { set; get; }
        [ScriptIgnore]
        public byte[] GoogleDriveCertificateP12RawData { set; get; }

        public bool IsCacheEnable { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }

    }

}
