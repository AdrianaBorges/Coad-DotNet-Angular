using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class EmailRequestAnexoDTO : FileInfoDTO
    {
        public EmailRequestAnexoDTO()
        {

        }

        public EmailRequestAnexoDTO(string path) : base(path)
        {
            
        }


        private string _nomeArquivo;
        public string NomeArquivo {
            get {

                if (!string.IsNullOrWhiteSpace(_nomeArquivo))
                    return _nomeArquivo;
                else
                    return FileName;
            }
            set
            {
                _nomeArquivo = value;
            }
        }

        public string MimeType {
            get {
                if (!string.IsNullOrWhiteSpace(Extensao))
                {
                    return MimeTypeMapperUtil.GetMimeType(Extensao);
                }
                return null;
            }
        }

        private string _extensao;
        public string Extensao {
            get {

                if (!string.IsNullOrWhiteSpace(_extensao))
                    return _extensao;
                return null;
            }
            set {
                _extensao = value;
            }
        }

    }
}
