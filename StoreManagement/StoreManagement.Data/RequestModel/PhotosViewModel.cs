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
    public class PhotosViewModel : BaseDrop
    {
        public Store Store { get; set; }
        public PagedList<FileManager> FileManagers { get; set; }

        public StoreLiquid StoreLiquid
        {
            get
            {
                return new StoreLiquid(Store);
            }
        }

        public List<FileManagerLiquid> FileManagerLiquids
        {
            get { return FileManagers.Select(r => new FileManagerLiquid(r)).ToList(); }
        }

    }
}
