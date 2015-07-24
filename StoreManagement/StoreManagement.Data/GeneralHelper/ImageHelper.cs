using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using StoreManagement.Data.Enums;

namespace StoreManagement.Data.GeneralHelper
{
    public class ImageHelper
    {
        public ImageOrientation GetOrientation(int width, int height)
        {
            if (width == 0 || height == 0)
                return ImageOrientation.Unknown;

            float relation = (float)height / (float)width;

            if (relation > .95 && relation < 1.05)
            {
                return ImageOrientation.Square;
            }
            else if (relation > 1.05 && relation < 2)
            {
                return ImageOrientation.Portrate;
            }
            else if (relation >= 2)
            {
                return ImageOrientation.Vertical;
            }
            else if (relation <= .95 && relation > .5)
            {
                return ImageOrientation.Landscape;
            }
            else if (relation <= .5)
            {
                return ImageOrientation.Horizontal;
            }
            else
            {
                return ImageOrientation.Unknown;
            }
        }
        public static byte[] MakeThumbnail(byte[] myImage, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            using (Image thumbnail = Image.FromStream(new MemoryStream(myImage)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        // Create a thumbnail in byte array format from the image encoded in the passed byte array.  
        // (RESIZE an image in a byte[] variable.)  
        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide, int Height, int Width)
        {
            byte[] ReturnedThumbnail;

            using (System.IO.MemoryStream StartMemoryStream = new System.IO.MemoryStream(), NewMemoryStream = new System.IO.MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                System.Drawing.Bitmap startBitmap = new System.Drawing.Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }
                newHeight = Height;
                newWidth = Width;
                // create a new Bitmap with dimensions for the thumbnail.  
                System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }
            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }
 

        public static byte[] ImageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        } 
        public static bool IsImage(string ext)
        {
            return ext == ".gif" || ext == ".jpg" || ext == ".png";
        }
        /// <summary>
        /// Determines if a file is a known image type by checking the extension.
        /// </summary>
        /// <param name="fileName">The file name to check.</param>
        /// <returns>True if the file is an image.</returns>
        public static bool IsImageByFileName(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            string types = "|.png|.gif|.jpg|.jpeg|.bmp|.tiff|";

            return types.IndexOf("|" + ext + "|") >= 0;
        }
        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            //Parsing stream to bitmap
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                //Get new dimensions
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                //Creating new bitmap with valid dimensions
                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                        return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            //Settings to increase quality of the image
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

            //Temporary stream to save the bitmap
            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                //Get image bytes from temporary stream
                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }

        public static byte[] CreateGoogleImage(HttpPostedFileBase file)
        {
            var fileByte = GeneralHelper.ReadFully(file.InputStream);
            //Create image from Bytes array
            System.Drawing.Image img = ByteArrayToImage(fileByte);
            int height = Convert.ToInt32(Convert.ToDouble(img.Height) * .7);
            int width = Convert.ToInt32(Convert.ToDouble(img.Width) * .7);
            //Resize Image - ORIGINAL
            var byteArrayIn = CreateThumbnail(fileByte, 10000, height, width);

            return byteArrayIn;
        }
    }
}
