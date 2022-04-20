using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Util
{
    public static class MimeTypeMapperUtil
    {
       public static string GetMimeType(string ext)
        {
            string mimeType = "application/unknown";

            if (!string.IsNullOrWhiteSpace(ext))
            {
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString(); 
            }
            return mimeType;
        }
    }
}
