using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.TextTemplate
{
    public class T4TemplateCompiler
    {
        public ITextTemplating T4 { get; set; }
        public ITextTemplatingSessionHost sessionHost { get; set; }

        public T4TemplateCompiler(ITextTemplating T4)
        {
            this.T4 = T4;
            this.sessionHost = T4 as ITextTemplatingSessionHost;
        }
    }
}
