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
        //TODO: ImageState will manage visibility.
        public bool ImageState { get; set; }

        public ImageLiquid(List<BaseFileEntity> baseFileEntities, PageDesign pageDesign, int width, int height)
        {
            this.BaseFileEntities = baseFileEntities;
            this.FileManagers = baseFileEntities.Select(r => r.FileManager).ToList();
            this.PageDesign = pageDesign;

            this.ImageWidth = width == 0 ? 99 : width;
            this.ImageHeight = height == 0 ? 99 : height;

        }
        public List<FileManagerLiquid> FileManagerLiquids
        {
            get { return this.FileManagers.Select(r => new FileManagerLiquid(r, this.ImageWidth, this.ImageHeight)).ToList(); }
        }
        public String ImageSourceTest
        {
            get { return "Test Image"; }
        }
        public bool MainImageHas
        {
            get
            {
                if (!ImageState)
                {
                    return false;
                }

                var mainImage = BaseFileEntities.Where(r => r.IsMainImage).Select(r => r.FileManager).FirstOrDefault(r => r.State);
                if (mainImage != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public String MainImageSource
        {
            get
            {
                if (!ImageState)
                {
                    return "";
                }
                var mainImage = BaseFileEntities.Where(r => r.IsMainImage).Select(r => r.FileManager).FirstOrDefault(r => r.State);
                if (mainImage != null)
                {

                    return LinkHelper.GetImageLinkHtml("Thumbnail", mainImage.GoogleImageId, this.ImageWidth,
                                                       this.ImageHeight, mainImage.Title, mainImage.Title);

                }
                else
                {
                    return FirstImageSource;
                }

            }
        }
        public String FirstImageSource
        {
            get
            {
                if (!ImageState)
                {
                    return "";
                }
                if (ImageHas)
                {
                    var firstOrDefault = FileManagers.Where(r => r.State).OrderBy(x => x.Id).FirstOrDefault();
                    if (firstOrDefault != null)
                        return LinkHelper.GetImageLinkHtml("Thumbnail", firstOrDefault.GoogleImageId, this.ImageWidth, this.ImageHeight, firstOrDefault.Title, firstOrDefault.Title);
                }
                return "";
            }
        }


        //int width = 60, int height = 60
        public String RandomImageSource
        {
            get
            {
                if (!ImageState)
                {
                    return "";
                }
                if (ImageHas)
                {
                    var firstOrDefault = FileManagers.Where(r => r.State).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                    if (firstOrDefault != null)
                        return LinkHelper.GetImageLinkHtml("Thumbnail", firstOrDefault.GoogleImageId,
                            this.ImageWidth,
                            this.ImageHeight, firstOrDefault.Title, firstOrDefault.Title);
                }
                return "";
            }
        }

        public List<String> ImageLinks
        {

            get
            {
                if (!ImageState)
                {
                    return new List<String>();
                }
                if (ImageHas)
                {
                    var imageList = new List<String>();
                    foreach (var image in this.FileManagers.Where(r => r.State))
                    {
                        var imageLink = LinkHelper.GetImageLinkHtml("Thumbnail", image.GoogleImageId,
                                                                this.ImageWidth,
                                                                this.ImageHeight, image.Title, image.Title);
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
                if (!ImageState)
                {
                    return false;
                }
                return this.FileManagers.Any(r => r.State);
            }
        }
        public int ImageCount
        {
            get
            {
                if (!ImageState)
                {
                    return 0;
                }
                return this.FileManagers.Count;
            }
        }
    }
}
