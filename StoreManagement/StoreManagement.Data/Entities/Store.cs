using System;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using GenericRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

         [JsonIgnore]
        public bool State { get; set; }
         [JsonIgnore]
        public int Ordering { get; set; }

        public virtual ICollection<FileManager> FileManagers { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<StoreUser> StoreUsers { get; set; }

        public int CategoryId { get; set; }
        public int StorePageDesignId { get; set; }
        public String GoogleDriveClientId { set; get; }
         [JsonIgnore]
        public String GoogleDriveUserEmail { set; get; }
         [JsonIgnore]
        public String GoogleDriveUserEmailPassword { set; get; }
         [JsonIgnore]
        public String GoogleDriveFolder { set; get; }
         [JsonIgnore]
        public String GoogleDriveServiceAccountEmail { set; get; }
         [JsonIgnore]
        public String GoogleDriveCertificateP12FileName { set; get; }
         [JsonIgnore]
        public String GoogleDrivePassword { set; get; }
         [JsonIgnore]
        public byte[] GoogleDriveCertificateP12RawData { set; get; }

        public bool IsCacheEnable { get; set; }

        public override string ToString()
        {
            return Id + " " + Name;
        }


        public string Description { get; set; }
    }

}
