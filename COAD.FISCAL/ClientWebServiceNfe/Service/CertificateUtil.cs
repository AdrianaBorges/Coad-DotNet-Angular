using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service
{
    public static class CertificateUtil
    {
        public static string NfeCertificateName = "13934825_ATUALIZACAO_PROFISSIONAL_CONTINUADA_LTDA27922913000111 (2018).p12";
        public static string NfeCertificatePassword = "279229";
        public static string NfeCertificateRelativePath = "certificados";
        private static X509Certificate2 RetornarCertificadoNFeAtual(string basePath)
        {

            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentNullException("O caminho base para o certificado 'certPath' não foi informado");

            string certPath = Path.Combine(basePath, string.Format(@"{0}\{1}", NfeCertificateRelativePath, NfeCertificateName));

            if (!File.Exists(certPath))
                throw new Exception(string.Format("O certificado não pode ser encontrado no caminho especificado '{0}'", certPath));

            X509Certificate2 actualCert = new X509Certificate2(certPath, NfeCertificatePassword, X509KeyStorageFlags.MachineKeySet);

            return actualCert;
        }

        public static X509Certificate2 RetornarCertificado(int? empID)
        {
            try
            {
                var basePath = SysUtils.DefaultPath;

                var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(empID);
                if(empresa != null)
                {
                    if (string.IsNullOrWhiteSpace(empresa.EMP_NOME_CERTIFICADO))
                        throw new Exception("A empresa não possui certificado cadastrado.");
                    if (string.IsNullOrWhiteSpace(empresa.EMP_CERTIFICADO_SENHA))
                        throw new Exception("A empresa não possui a senha do certificado cadastrado.");

                    if (string.IsNullOrWhiteSpace(basePath))
                        throw new ArgumentNullException("O caminho base para o certificado 'certPath' não foi informado");

                    string certPath = Path.Combine(basePath, string.Format(@"{0}\{1}", NfeCertificateRelativePath, empresa.EMP_NOME_CERTIFICADO));

                    if (!File.Exists(certPath))
                        throw new Exception(string.Format("O certificado não pode ser encontrado no caminho especificado '{0}'", certPath));

                    X509Certificate2 actualCert = new X509Certificate2(certPath, empresa.EMP_CERTIFICADO_SENHA, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                    return actualCert;
                }
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Não é possível retornar o certificado da Empresa {0}", empID), e);
            }
            return null;
        }
    }
}
