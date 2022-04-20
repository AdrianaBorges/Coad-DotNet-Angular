using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.TextTemplate.Model
{
    public class T4CallBack : ITextTemplatingCallback
    {
        public T4CallBack()
        {
            errorMessages = new List<string>();
            fileExtension = ".tt";
            outputEcoding = Encoding.UTF8;
        }
        public List<string> errorMessages { get; set; }
        public string fileExtension { get; set; }
        public Encoding outputEcoding { get; set; }
        public void ErrorCallback(bool warning, string message, int line, int column)
        {
            var formatedMensagem = string.Format("Alerta: {0} - {1} linha {2} coluna {3}", warning, message, line, column);
            errorMessages.Add(formatedMensagem);
        }

        public void SetFileExtension(string extension)
        {
            fileExtension = extension;
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            outputEcoding = encoding;
        }
    }
}
