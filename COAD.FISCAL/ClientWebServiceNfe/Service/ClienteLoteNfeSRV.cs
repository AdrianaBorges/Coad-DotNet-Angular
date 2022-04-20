using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.NfeAutorizacao;
using COAD.FISCAL.NFeAutorizacao4;
using COAD.FISCAL.XmlUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace COAD.FISCAL.Service
{
    public class ClienteLoteNfeSRV
    {
        public LoteRetorno EnviarLoteNotaFiscal(LoteNFE lote, X509Certificate2 certificado, String uf = "RJ")
        {
            if (lote == null)
                throw new ArgumentNullException("O objeto de lote não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.AssinarXml(lote, certificado, "infNFe", "..");
            XmlUtil.SerializarObjetoTeste(xmlDoLote);

            //var cliente = new NfeAutorizacaoSoapClient(); // 3.10
            //var cabecalho = new nfeCabecMsg()
            //{
            //    cUF = "33",
            //    versaoDados = ConstantsNFe.VERSAO,
            //};

            //var cliente = (uf == "RJ" ? new NFeAutorizacao4SoapClient() : new NFeAutorizacao4SoapClientMG());
            //var config = cliente.ClientCredentials.ClientCertificate;
            //config.Certificate = certificado;

            LoteRetorno retornoLote = null;

            if (uf == "RJ")
            {

                var cliente = new NFeAutorizacao4SoapClient();
                var config = cliente.ClientCredentials.ClientCertificate;
                config.Certificate = certificado;

                var resposta = cliente.nfeAutorizacaoLote(xmlDoLote);
                retornoLote = XmlUtil.LoadFromXMLDocument<LoteRetorno>(resposta);

            }
            else
            {

                var cliente = new NFeAutorizacao4SoapClient();
                var config = cliente.ClientCredentials.ClientCertificate;
                config.Certificate = certificado;

                var resposta = cliente.nfeAutorizacaoLote(xmlDoLote);
                retornoLote = XmlUtil.LoadFromXMLDocument<LoteRetorno>(resposta);

            }
           
            
            if(retornoLote != null)
            {
                retornoLote.XmlLote = xmlDoLote;
            }

            return retornoLote;
        }

    }
}
