using COAD.FISCAL.CadConsultaCadastro4SVRS;
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
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace COAD.FISCAL.Service
{
    public class ClienteConsultaCadastroSRV
    {
        public ConsultaCadastroRetorno ConsultarCadastro(RequisicaoConsultaCadastro requisicao, X509Certificate2 certificado)
        {
            if (requisicao == null)
                throw new ArgumentNullException("O objeto de requisicao não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            //var xmlDoLote = XmlUtil.AssinarXml(lote, certificado, "infNFe", "..");

            //var cliente = new NfeAutorizacaoSoapClient(); // 3.10
            //var cabecalho = new nfeCabecMsg()
            //{
            //    cUF = "33",
            //    versaoDados = ConstantsNFe.VERSAO,
            //};
            var xmlRequisicao = XmlUtil.SerializeAsXElement(requisicao);
            var xml = XmlUtil.SerializeAsXmlDocument(requisicao);
            XmlUtil.SerializarObjetoTeste(xml);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var cliente = new CadConsultaCadastro4SoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.consultaCadastro(xmlRequisicao);
            ConsultaCadastroRetorno retornoLote = XmlUtil.LoadAsXElement<ConsultaCadastroRetorno>(resposta);
            return retornoLote;

            
        }

    }
}
