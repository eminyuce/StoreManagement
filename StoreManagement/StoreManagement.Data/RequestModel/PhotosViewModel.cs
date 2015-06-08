using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.RequestModel
{
    public class PhotosViewModel
    {
        public Store Store { get; set; }
        public PagedList<FileManager> FileManagers { get; set; }
    }
}
