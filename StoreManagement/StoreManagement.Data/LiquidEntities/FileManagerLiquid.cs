using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class FileManagerLiquid : Drop
    {
        public FileManager FileManager;
        public PageDesign PageDesign;

        public FileManagerLiquid(FileManager fileManager, PageDesign pageDesign)
        {
            this.FileManager = fileManager;
            this.PageDesign = pageDesign;

        }
        public String ImageSource
        {
            get
            {
                return LinkHelper.GetImageLink("Thumbnail", this.FileManager.GoogleImageId, this.PageDesign.ImageWidth, this.PageDesign.ImageHeight);
            }
        }

    }
}
