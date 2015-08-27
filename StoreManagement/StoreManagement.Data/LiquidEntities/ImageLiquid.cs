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
        private List<BaseFileEntity> BaseFileEntities { get; set; }


        public ImageLiquid(List<BaseFileEntity> baseFileEntities, PageDesign pageDesign, int width, int height)
        {
            this.BaseFileEntities = baseFileEntities;
            this.FileManagers = baseFileEntities.Select(r => r.FileManager).ToList();
            this.PageDesign = pageDesign;

            this.ImageWidth = width == 0 ? 99 : width;
            this.ImageHeight = height == 0 ? 99 : height;

        }

        public String ImageSourceTest
        {
            get { return "Test Image"; }
        }

        public String mainimagesource
        {
            get
            {
                var mainImage = BaseFileEntities.Where(r => r.IsMainImage).Select(r => r.FileManager).FirstOrDefault(r => r.State);
                if (mainImage == null)
                {
                    return LinkHelper.GetImageLink("Thumbnail", mainImage.GoogleImageId, this.ImageWidth,
                                                   this.ImageHeight);
                }
                else
                {
                    return "";
                }

            }
        }

        //int width = 60, int height = 60
        public String imagesource
        {
            get
            {
                if (imagehas)
                {
                    var firstOrDefault = FileManagers.Where(r => r.State).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    return LinkHelper.GetImageLink("Thumbnail", firstOrDefault.GoogleImageId, this.ImageWidth, this.ImageHeight);
                }
                else
                {

                    return "";
                }
            }
        }

        public List<String> imagelinks
        {

            get
            {
                if (imagehas)
                {
                    var imageList = new List<String>();
                    foreach (var image in this.FileManagers.Where(r => r.State))
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

        public bool imagehas
        {
            get
            {
                return this.FileManagers.Any(r => r.State);
            }
        }
        public int imagecount
        {
            get
            {
                return this.FileManagers.Count;
            }
        }
    }
}
