using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class PhotosViewModel : ViewModel
    {
        public PagedList<FileManager> SFileManagers { get; set; }

        public List<FileManagerLiquid> FileManagers
        {
            get { return SFileManagers.Select(r => new FileManagerLiquid(r)).ToList(); }
        }

    }
}
