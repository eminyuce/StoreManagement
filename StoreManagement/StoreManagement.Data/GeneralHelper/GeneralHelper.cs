using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.GeneralHelper
{
    public class GeneralHelper
    {
        public static string UrlDencode(string adres, bool encode)
        {
            string[] karakter = { "<", ">", "#", "%", "{", "}", "|", @"\", "^", "~", "[", "]", "`", ";", "/", "?", ":", "@", "=", "&", "$" };
            string[] donustur = { "%3C", "%3E", "%23", "%25", "%7B", "%7D", "%7C", "%5C", "%5E", "%7E", "%5B", "%5D", "%60", "%3B", "%2F", "%3F", "%3A", "%40", "%3D", "%26", "%24" };

            if (encode)
            {
                for (int i = 0; i < karakter.Length; i++)
                {
                    adres = adres.Replace(karakter[i], donustur[i]);
                }
            }
            else
            {
                for (int i = 0; i < donustur.Length; i++)
                {
                    adres = adres.Replace(donustur[i], karakter[i]);
                }
            }
            return adres;
        }
    }
}
