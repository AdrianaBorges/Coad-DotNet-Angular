using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service
{
    class ClientWsFactory
    {
        public static X509Certificate2 certificate {get; set;}

        public static ClientWsManifestacaoEvento CriarClientWsManifestacaoEvento()
        {
            return null;
        }

        public static ClientWsDownloadNfeSRV CriarClientWsDownload()
        {
            if (certificate == null)
                return new ClientWsDownloadNfeSRV();
            else return new ClientWsDownloadNfeSRV(certificate);
        }
       
    }
}
