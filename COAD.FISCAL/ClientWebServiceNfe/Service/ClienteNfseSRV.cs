using COAD.FISCAL.Model;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Model.NFSe.Retornos;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.NfeAutorizacao;
using COAD.FISCAL.NFeAutorizacao4;
using COAD.FISCAL.Nfse;
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

namespace COAD.FISCAL.Service
{
    public class ClienteNfseSRV
    {
        public EnviarLoteRpsResposta EnviarLoteNotaFiscal(EnviarLoteRpsEnvio lote, X509Certificate2 certificado)
        {
            if (lote == null)
                throw new ArgumentNullException("O objeto de lote não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.AssinarXml(lote, certificado, "InfRps", "..", "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd");
            xmlDoLote = XmlUtil.AssinarXml(xmlDoLote, certificado, "LoteRps",  "..");

            XmlUtil.SerializarObjetoTeste(xmlDoLote);
            var xmlString = XmlUtil.SerializeAsXml(xmlDoLote, false, true);
            
            var cliente = new NfseSoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.RecepcionarLoteRps(xmlString);
            EnviarLoteRpsResposta retornoLote = XmlUtil.LoadFromXMLString<EnviarLoteRpsResposta>(resposta);
            return retornoLote;
        }

        public ConsultarSituacaoLoteRpsResposta ChecarSituacaoDoLote(ConsultarSituacaoLoteRpsEnvio consulta, X509Certificate2 certificado)
        {
            if (consulta == null)
                throw new ArgumentNullException("O objeto de consulta não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.SerializeAsXmlDocument(consulta);
            //XmlUtil.SerializarObjetoTeste(xmlDoLote);
            var xmlString = XmlUtil.SerializeAsXml(xmlDoLote, false, true);

            var cliente = new NfseSoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.ConsultarSituacaoLoteRps(xmlString);
            ConsultarSituacaoLoteRpsResposta retornoLote = XmlUtil.LoadFromXMLString<ConsultarSituacaoLoteRpsResposta>(resposta);

            return retornoLote;
        }

        public ConsultarLoteRpsResposta ConsultarLoteRps(ConsultarLoteRpsEnvio consulta, X509Certificate2 certificado)
        {
            if (consulta == null)
                throw new ArgumentNullException("O objeto de consulta não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.SerializeAsXmlDocument(consulta);
            //XmlUtil.SerializarObjetoTeste(xmlDoLote);
            var xmlString = XmlUtil.SerializeAsXml(xmlDoLote, false, true);

            var cliente = new NfseSoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.ConsultarLoteRps(xmlString);
            ConsultarLoteRpsResposta retornoLote = XmlUtil.LoadFromXMLString<ConsultarLoteRpsResposta>(resposta);

            return retornoLote;
        }

        public ConsultarNfseRpsResposta ConsultarNfsePorRps(ConsultarNfseRpsEnvio consulta, X509Certificate2 certificado)
        {
            if (consulta == null)
                throw new ArgumentNullException("O objeto de consulta não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.SerializeAsXmlDocument(consulta);
            //XmlUtil.SerializarObjetoTeste(xmlDoLote);
            var xmlString = XmlUtil.SerializeAsXml(xmlDoLote, false, true);

            var cliente = new NfseSoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.ConsultarNfsePorRps(xmlString);
            ConsultarNfseRpsResposta retornoLote = XmlUtil.LoadFromXMLString<ConsultarNfseRpsResposta>(resposta);

            return retornoLote;
        }

        public CancelarNfseResposta CancelarNotaFiscalServico(CancelarNfseEnvio requisicaoEnvio, X509Certificate2 certificado)
        {
            if (requisicaoEnvio == null)
                throw new ArgumentNullException("O objeto de consulta não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlDoLote = XmlUtil.AssinarXml(requisicaoEnvio, certificado, "InfPedidoCancelamento", "..", "http://www.abrasf.org.br/ABRASF/arquivos/nfse.xsd");
            XmlUtil.SerializarObjetoTeste(xmlDoLote);
            var xmlString = XmlUtil.SerializeAsXml(xmlDoLote, false, true);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            var cliente = new NfseSoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.CancelarNfse(xmlString);
            CancelarNfseResposta retornoLote = XmlUtil.LoadFromXMLString<CancelarNfseResposta>(resposta);

            return retornoLote;
        }
    }
}
