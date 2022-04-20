using COAD.FISCAL.Model;
using COAD.FISCAL.XmlUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.RecepcaoEvento;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.NFeRecepcaoEvento4;

namespace COAD.FISCAL.Service
{
    public class ClientWsNfeRecepcaoSRV
    {
        public RecepcaoEventoRetorno EnviarEvento(RequisicaoRecepcaoEvento reqInutilizacao, X509Certificate2 certificado)
        {
            if (reqInutilizacao == null)
                throw new ArgumentNullException("O objeto requisição de initilização não pode ser nulo.");

            if (certificado == null)
                throw new ArgumentNullException("O certificado 'certificate' não foi informado");

            var xmlReqRecepcaoEvento = XmlUtil.AssinarXml(reqInutilizacao, certificado, "infEvento", "..");
            //XmlUtil.SerializarObjetoTeste(xmlReqRecepcaoEvento);

            var eventos = xmlReqRecepcaoEvento.GetElementsByTagName("evento");
            if(eventos != null && eventos.Count > 0)
            {
                reqInutilizacao.evento.Clear();

                foreach(XmlNode node in eventos)
                {
                    var evento = XmlUtil.LoadFromXMLDocument<Evento>(node);
                    reqInutilizacao.evento.Add(evento);
                }
            }

            //var cliente = new RecepcaoEventoSoapClient();
            //var cabecalho = new nfeCabecMsg()
            //{
            //    cUF = "33",
            //    versaoDados = "1.00",
            //};

            var cliente = new NFeRecepcaoEvento4SoapClient();
            var config = cliente.ClientCredentials.ClientCertificate;
            config.Certificate = certificado;

            var resposta = cliente.nfeRecepcaoEvento((XmlNode)xmlReqRecepcaoEvento);
            RecepcaoEventoRetorno retornoEvento = XmlUtil.LoadFromXMLDocument<RecepcaoEventoRetorno>(resposta);

            return retornoEvento;
        }
    }
}

