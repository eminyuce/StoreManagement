using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
         
    public class FileManager : BaseEntity
    {
        public string ContentType { get; set; }
        public string GoogleImageId { get; set; }
        public string Title { get; set; }
        public string WebContentLink { get; set; }       
        public string FileStatus { get; set; }

        [ScriptIgnore]
        public string ThumbnailLink { get; set; }
        
        [ScriptIgnore]
        public string OriginalFilename { get; set; }
        
        [ScriptIgnore]
        public DateTime? ModifiedDate { get; set; }
        
        [ScriptIgnore]
        public string IconLink { get; set; }
        
        [ScriptIgnore]
        public int ContentLength { get; set; }
        
        [ScriptIgnore]
        public bool IsCarousel { get; set; }

        [ScriptIgnore]
        public int ? Width { get; set; }
        
        [ScriptIgnore]
        public int ?  Height { get; set; }


    }
}
