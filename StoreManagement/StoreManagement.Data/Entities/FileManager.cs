using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GenericRepository;
using Newtonsoft.Json;

namespace StoreManagement.Data.Entities
{
         
    public class FileManager : BaseEntity
    {
        public string ContentType { get; set; }
        public string GoogleImageId { get; set; }
        public string Title { get; set; }
        public string WebContentLink { get; set; }       
        public string FileStatus { get; set; }

         [JsonIgnore]
        public string ThumbnailLink { get; set; }
        
         [JsonIgnore]
        public string OriginalFilename { get; set; }
        
         [JsonIgnore]
        public DateTime? ModifiedDate { get; set; }
        
         [JsonIgnore]
        public string IconLink { get; set; }
        
         [JsonIgnore]
        public int ContentLength { get; set; }
        
         [JsonIgnore]
        public bool IsCarousel { get; set; }

         [JsonIgnore]
        public int ? Width { get; set; }
        
         [JsonIgnore]
        public int ?  Height { get; set; }

         public string ImageSourceType { get; set; }


    }
}
