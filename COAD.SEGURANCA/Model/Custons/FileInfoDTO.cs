using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons
{
    public class FileInfoDTO
    {
        public FileInfoDTO()
        {

        }

        public FileInfoDTO(string path)
        {
            this.Path = path;
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
                    var ext = System.IO.Path.GetExtension(path);
                    return ext;
                }
                return null;
            }
            set { } }
    }
}
