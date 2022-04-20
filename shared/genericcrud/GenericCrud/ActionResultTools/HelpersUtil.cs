using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GenericCrud.ActionResultTools
{
    public static class HelpersUtil
    {
        public static string[] ProcessUrl(params string[] paths)
        {
            if (paths != null)
            {
                string[] newPaths = new string[paths.Length];
                int index = 0;
                foreach (var path in paths)
                {
                    if (SysUtils.UseMinResources())
                    {
                        var filename = path.Split('/').Last();
                        if (filename != null)
                        {
                            var fileNameArray = filename.Split('.');
                            var sufix = fileNameArray.Last();
                            var newSufix = "min." + sufix;
                            var newfilename = filename.Replace(sufix, newSufix);
                            var newPath = path.Replace(filename, newfilename);
                            var prefixPath = "/compilados/";

                            newPaths[index] = newPath.Replace("{path}", prefixPath);
                        }
                        else
                        {
                            newPaths[index] = path.Replace("{path}", "/");
                        }
                    }
                    else
                    {
                        newPaths[index] = path.Replace("{path}", "/");
                    }
                }

                paths = newPaths;
            }

            return paths;
        }
    }
}
