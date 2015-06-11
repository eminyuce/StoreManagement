using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.GeneralHelper
{
    public class LinkHelper
    {
        public static String GetContentLink(Content c, String categoryName )
        {
            String productDetailLink = String.Format("/Products/Product/{0}", String.Format("{2}/{0}-{1}", GeneralHelper.GetUrlSeoString(c.Name), c.Id, GeneralHelper.GetUrlSeoString(categoryName)));
            return productDetailLink;
        }
    }
}
