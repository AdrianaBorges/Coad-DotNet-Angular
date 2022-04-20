using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.NfeRetAutorizacao;
using COAD.FISCAL.NFeRetAutorizacao4;
using COAD.FISCAL.XmlUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace COAD.FISCAL.Service
{
    public class ClienteRetornoLoteSRV
    {

        public ConsultaLoteRetorno ProcessarRetornoNotaFiscal(ConsultaLote consultaLote, X509Certificate2 certificate)
        {
            if (consultaLote == null)
                throw new ArgumentNullException("O objeto de lote não pode ser nulo.");

            var xmlConsultaLote = XmlUtil.SerializeAsXmlDocument(consultaLote);
            //var cliente = new NfeRetAutorizacaoSoapClient();
            //var cabecalho = new nfeCabecMsg()
            //{
            //    cUF = "33",
            //    versaoDados = "3.10",
            //};

            var cliente = new NFeRetAutorizacao4SoapClient();

            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificate ?? throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var resposta = cliente.nfeRetAutorizacaoLote((XmlNode)xmlConsultaLote);
            ConsultaLoteRetorno retorno = XmlUtil.LoadFromXMLDocument<ConsultaLoteRetorno>(resposta);

            return retorno;
        }
    }
}
