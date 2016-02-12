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
                return LinkHelper.GetImageLink("Thumbnail", this.FileManager, this.ImageWidth, this.ImageHeight);
            }
        }
        public String WebContentLink
        {
            get { return FileManager.WebContentLink; }
        }
        public String OriginalFilename
        {
            get { return FileManager.OriginalFilename; }
        }
        public int Id
        {
            get { return FileManager.Id; }
        }
        public String Title
        {
            get { return FileManager.Title; }
        }
        public String Name
        {
            get { return FileManager.Name; }
        }
        public String ContentType
        {
            get { return FileManager.ContentType; }
        }
        public String FileSize
        {
            get { return FileManager.FileSize; }
        }
        public String GoogleImageId
        {
            get { return FileManager.GoogleImageId; }
        }
        public bool IsCarousel
        {
            get { return FileManager.IsCarousel; }
        }
        public DateTime CreatedDate
        {
            get { return FileManager.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return FileManager.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return FileManager.State; }
        }
        public string FileStatus
        {
            get { return FileManager.FileStatus; }
        }
    }
}
