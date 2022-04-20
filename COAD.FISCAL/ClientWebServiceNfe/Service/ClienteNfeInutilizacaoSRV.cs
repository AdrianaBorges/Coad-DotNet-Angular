using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.NfeInutilizacao;
using COAD.FISCAL.NFeInutilizacao4;
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
    public class ClienteNfeInutilizacaoSRV
    {
        public InutilizacaoRetorno InutilizarNFe(RequisicaoInutilizacao reqInutilizacao, X509Certificate2 certificado)
        {
            if (reqInutilizacao == null)
                throw new ArgumentNullException("O objeto requisição de initilização não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlReqInutilizacao = XmlUtil.AssinarXml(reqInutilizacao, certificado, "infInut", "..");
            // XmlUtil.SerializarObjetoTeste(xmlReqInutilizacao);

            //var cliente = new NfeInutilizacao2SoapClient();
            //var cabecalho = new nfeCabecMsg()
            //{
            //    cUF = "33",
            //    versaoDados = ConstantsNFe.VERSAO,
            //};

            var cliente = new NFeInutilizacao4SoapClient();

            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.nfeInutilizacaoNF((XmlNode)xmlReqInutilizacao);
            InutilizacaoRetorno retornoInutilizacao = XmlUtil.LoadFromXMLDocument<InutilizacaoRetorno>(resposta);
            
            return retornoInutilizacao;
        }

    }
}
