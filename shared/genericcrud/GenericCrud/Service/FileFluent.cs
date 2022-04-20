using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GenericCrud.Util
{
    public class FileFluent
    {
        public FileFluent(HttpPostedFileBase file)
        {
            this.file = file;

            if (this.file != null)
            {
                this.fileName = this.file.FileName;
            }
        }

        public HttpPostedFileBase file { get; set; }
        private string[] aceptedExtensions { get; set; }
        private int minSize { get; set; }
        public string ServerPath { get; set; }
        public string path { get; set; }
        public string fileName { get; set; }

        public FileFluent CheckExtensions(params string[] extension)
        {
            this.aceptedExtensions = extension;
            return this;
        }

        public FileFluent CheckValidations()
        {
            return this;
        }


        public FileFluent SetLocations(string serverPath)
        {
            this.ServerPath = serverPath;
            return this;
        }
        
        public FileFluent SetLocations(string serverPath, string path)
        {
            this.ServerPath = serverPath;
            this.path = path;
            return this;
        }

        public FileFluent SetFileName(string fileName)
        {
            this.fileName = fileName;
            return this;
        }

        public string TrySave()
        {
            string location = Path.Combine(ServerPath, path, fileName);
            file.SaveAs(location);

            return location;
        }
    }
}
