using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace StoreManagement.Data.GeneralHelper
{
    public static class FileHelper
    {
        public static void SaveFile(byte[] content, string path)
        {
            string filePath = GetFileFullPath(path);
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            //Save file
            using (FileStream str = File.Create(filePath))
            {
                str.Write(content, 0, content.Length);
            }
        }

        public static string GetFileFullPath(string path)
        {
            string relName = path.StartsWith("~") ? path : path.StartsWith("/") ? string.Concat("~", path) : path;

            string filePath = relName.StartsWith("~") ? HostingEnvironment.MapPath(relName) : relName;

            return filePath;
        }
    }
}
