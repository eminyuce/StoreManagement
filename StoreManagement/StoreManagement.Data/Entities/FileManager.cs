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
        public string FileSize { get; set; }

        public string ThumbnailLink { get; set; }
        
        public string OriginalFilename { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        
        public string IconLink { get; set; }
        
        public int ContentLength { get; set; }
        
        public bool IsCarousel { get; set; }

        public int ? Width { get; set; }
        
        public int ?  Height { get; set; }

         public string ImageSourceType { get; set; }


    }
}
