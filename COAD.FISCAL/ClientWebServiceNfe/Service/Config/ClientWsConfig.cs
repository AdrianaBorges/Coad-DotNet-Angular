using COAD.FISCAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service.Config
{
    class ClientWsConfig
    {
        public static void SetCertificate(string certPath, string password)
        {
            if (certPath != null && password != null)
            {
                //string certPath = @"C:\Users\dasilva\Documents\certificados\APC A1.pfx"; "27922913"
                X509Certificate2 actualCert = new X509Certificate2(certPath, password);
                ClientWsFactory.certificate = actualCert;
            }
        }
    }
}
