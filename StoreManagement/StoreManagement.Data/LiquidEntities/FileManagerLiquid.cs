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
    public class FileManagerLiquid : BaseDrop
    {
        public FileManager FileManager;
        public String Link { get; set; }

        public FileManagerLiquid(FileManager fileManager)
        {
            this.FileManager = fileManager;

        }

        public FileManagerLiquid(FileManager fileManager, int imageWidth, int imageHeight)
        {
            this.FileManager = fileManager;
            this.ImageWidth = imageWidth;
            this.ImageHeight = imageHeight;

        }
        public String ImageSource
        {
            get
            {
                return LinkHelper.GetImageLink("Thumbnail", this.FileManager.GoogleImageId, this.ImageWidth, this.ImageHeight);
            }
        }

    }
}
