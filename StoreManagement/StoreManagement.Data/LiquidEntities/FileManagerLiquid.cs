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
        private FileManager item;
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public FileManagerLiquid(FileManager fileManager)
        {
            this.FileManager = fileManager;

        }

        public FileManagerLiquid(Entities.FileManager item, int imageWidth, int imageHeight)
        {
            this.item = item;
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
