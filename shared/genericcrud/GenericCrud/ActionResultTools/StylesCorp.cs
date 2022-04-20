using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace GenericCrud.ActionResultTools
{
    public static class StylesCorp
    {
        
        public static IHtmlString Render(params string[] paths)
        {
            paths = HelpersUtil.ProcessUrl(paths);
            return Styles.Render(paths);
        }
    }
}
