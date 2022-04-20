using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GenericCrud.ActionResultTools
{
    public static class UrlCorp
    {
        public static string Content(string url)
        {
            var paths = HelpersUtil.ProcessUrl(url);
            if (paths != null && paths.Count() > 0)
                return UrlHelper.GenerateContentUrl(paths[0], new HttpContextWrapper(HttpContext.Current));
            return string.Empty;
        }
    }
}
