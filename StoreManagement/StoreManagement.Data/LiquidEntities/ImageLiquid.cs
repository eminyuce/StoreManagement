using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class ImageLiquid : BaseDrop
    {

        private List<FileManager> FileManagers { get; set; }



        public ImageLiquid(List<FileManager>  fileManagers, PageDesign pageDesign, int width, int height)
        {

            this.FileManagers = fileManagers;
            this.PageDesign = pageDesign;

            this.ImageWidth = width;
            this.ImageHeight = height;

        }


        //int width = 60, int height = 60
        public String ImageSource
        {
            get
            {
                if (ImageHas)
                {
                    var firstOrDefault = FileManagers.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    return LinkHelper.GetImageLink("Thumbnail", firstOrDefault.GoogleImageId, this.ImageWidth,this.ImageHeight);
                }
                else
                {

                    return "";
                }
            }
        }

        public List<String> ImageLinks
        {

            get
            {
                if (ImageHas)
                {
                    var imageList = new List<String>();
                    foreach (var image in this.FileManagers)
                    {
                        var imageLink = LinkHelper.GetImageLink("Thumbnail", image.GoogleImageId,
                                                                this.ImageWidth,
                                                                this.ImageHeight);
                        imageList.Add(imageLink);
                    }

                    return imageList;
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        public bool ImageHas
        {
            get
            {
                return this.FileManagers.Any();
            }
        }

    }
}
