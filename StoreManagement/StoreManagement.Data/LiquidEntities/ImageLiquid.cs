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

        public ImageLiquid(List<BaseFileEntity> baseFileEntities, int width, int height)
        {
            this.BaseFileEntities = baseFileEntities.Where(r => r.FileManager != null).ToList();
            this.FileManagers = baseFileEntities.Where(r=> r.FileManager != null).Select(r => r.FileManager).ToList();


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

                    return LinkHelper.GetImageLinkHtml("Thumbnail", mainImage, this.ImageWidth,
                                                       this.ImageHeight, mainImage.Title, mainImage.Title);

                }
                else
                {
                    return BestImageSource;
                }

            }
        }
        public String BestImageSource
        {
            get { return GetImageSource("best"); }
        }
        public String PhoneSmallImageSource
        {
            get { return GetImageSource("IPhoneSmall"); }
        }
        public String PhoneImageSource
        {
            get { return GetImageSource("IPhone"); }
        }
        public String OriginalImageSource
        {
            get { return GetImageSource("Original"); }
        }
        public String SmallImageSource
        {
            get { return GetImageSource("Small"); }
        }
        public String XlargeImageSource
        {
            get { return GetImageSource("XLarge"); }
        }
        public String MediumImageSource
        {
            get { return GetImageSource("Medium"); }
        }
        public String LargeImageSource
        {
            get { return GetImageSource("Large"); }
        }


        public String BestImageSourceFix
        {
            get { return GetImageSource("best", false); }
        }
        public String PhoneSmallImageSourceFix
        {
            get { return GetImageSource("IPhoneSmall", false); }
        }
        public String PhoneImageSourceFix
        {
            get { return GetImageSource("IPhone", false); }
        }
        public String OriginalImageSourceFix
        {
            get { return GetImageSource("Original", false); }
        }
        public String SmallImageSourceFix
        {
            get { return GetImageSource("Small", false); }
        }
        public String XlargeImageSourceFix
        {
            get { return GetImageSource("XLarge", false); }
        }
        public String MediumImageSourceFix
        {
            get { return GetImageSource("Medium", false); }
        }
        public String LargeImageSourceFix
        {
            get { return GetImageSource("Large", false); }
        }

        public String GetImageSource(String size, bool isImageSizeActive = true)
        {
            var fileImage = FileManagers.FirstOrDefault(r => r.State && r.FileSize.Equals(size, StringComparison.InvariantCultureIgnoreCase));
            if (fileImage != null)
            {
                return fileImage.WebContentLink;
            }
            else
            {
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
                        return LinkHelper.GetImageLinkHtml("Thumbnail", firstOrDefault,
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
                        var imageLink = LinkHelper.GetImageLinkHtml("Thumbnail", image,
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
