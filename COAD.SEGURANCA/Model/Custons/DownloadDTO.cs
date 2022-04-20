using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons
{
    public class DownloadDTO
    {
        public DownloadDTO()
        {

        }

        public DownloadDTO(string path)
        {
            Init(path);
        }

        private void Init(string path)
        {
            if (File.Exists(path))
            {
                this.Bytes = File.ReadAllBytes(path);
            }
        }

        private string path;
        public string Path { get
            {
                return path;
            }
            set {
                path = value;
                Init(value);
            }

        }
        public byte[] Bytes { get; set; }

        public string FileName
        {
            get {
                if (!string.IsNullOrWhiteSpace(Path))
                {
                    var fileName = Path.Split('\\').Last();
                    return fileName;
                }
                return null;
            }
            set { }
        }

        public string Extension {
            get {
                var fileName = FileName;

                if(fileName != null)
                {
                    var ext = fileName.Split('.').Last();
                    return ext;
                }
                return null;
            }
            set { } }
    }
}
