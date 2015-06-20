using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    }
}
