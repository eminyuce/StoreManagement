using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class FileManager : BaseEntity
    {
        public string ContentType { get; set; }
        public string GoogleImageId { get; set; }
        public string Title { get; set; }
        public string WebContentLink { get; set; }

        [IgnoreDataMember]
        public string ThumbnailLink { get; set; }
        
        [IgnoreDataMember]
        public string OriginalFilename { get; set; }
        
        [IgnoreDataMember]
        public DateTime? ModifiedDate { get; set; }
        
        [IgnoreDataMember]
        public string IconLink { get; set; }
        
        [IgnoreDataMember]
        public int ContentLength { get; set; }
        
        [IgnoreDataMember]
        public bool IsCarousel { get; set; }

        [IgnoreDataMember]
        public int ? Width { get; set; }
        
        [IgnoreDataMember]
        public int ?  Height { get; set; }


    }
}
