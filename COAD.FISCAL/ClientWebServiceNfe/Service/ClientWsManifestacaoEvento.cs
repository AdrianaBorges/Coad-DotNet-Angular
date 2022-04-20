using COAD.FISCAL.Model;
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
    class ClientWsManifestacaoEvento
    {
        //private RecepcaoEventoSoapClient recepClient { get; set; }
        //public X509Certificate2 Certificado { get; set; }

        //public ClientWsManifestacaoEvento()
        //{
        //    Init();
        //}

        //public ClientWsManifestacaoEvento(X509Certificate2 cert)
        //{
        //    Certificado = cert;
        //    Init();
        //}

        //private void Init()
        //{
        //    recepClient = new RecepcaoEventoSoapClient();
        //}

        //public XmlNode EnviarEvento(EventoRequest eventoRequest)
        //{
        //    if (Certificado == null)
        //    {

        //        throw new Exception("Nenhum certificado foi encontrado. Para utilizar esse método é necessário um certificado para altenticar o usuário do webService");
        //    }

        //    if (eventoRequest != null)
        //    {
        //        XmlDocument doc = ConvertEventoToXml(eventoRequest);
        //        nfeCabecMsg cabecRecep = new nfeCabecMsg();
        //        cabecRecep.cUF = "91";
        //        cabecRecep.versaoDados = "1.00";

        //        doc = XmlUtil.AssinarXml(doc, Certificado);
        //        var conf = recepClient.ClientCredentials.ClientCertificate;
        //        conf.Certificate = Certificado;

        //        XmlUtil.SerializeXml(doc, @"c:\xmlnfe\request.xml");
        //        var resp = recepClient.nfeRecepcaoEvento(ref cabecRecep, (XmlNode)doc);
        //        return resp;
        //    }

        //    return null;
        //}

        //public XmlNode EnviarEvento(string chave, TipoEvento tipoEvento = null, DateTime? dataEvento = null)
        //{
        //    EventoRequest request = new EventoRequest();
        //    request.idLote = "00001";

        //    if (tipoEvento == null)
        //        tipoEvento = TipoEvento.CienciaDaOperacao;

        //    if (dataEvento == null)
        //    {
        //        dataEvento = DateTime.Now;
        //    }

        //    Evento evento = new Evento()
        //    {

        //        chaveNfe = chave,
        //        CNPJ = "27922913000111",
        //        CodUf = 91,
        //        detEvento = new DetEvento() { DescEvento = TipoEvento.CienciaDaOperacao.Desc },
        //        dhEvento = (DateTime)dataEvento,
        //        tipoEvento = tipoEvento,
        //        tpAmb = 1,
        //        VerEvento = "1.00",
        //    };

        //    request.Eventos.Add(evento);

        //    return EnviarEvento(request);
        //}


        ///**
        // * Exemplo do formato do xml de saida
        // * <?xml version="1.0" encoding="utf-8"?>
        //    <envEvento xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.00">
        //    <idLote>00001</idLote>
        //    <evento  xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.00">
        //      <infEvento Id="ID2102003315030211722700014755008000004780111103103201">
        //        <cOrgao>91</cOrgao>
        //        <tpAmb>1</tpAmb>
        //        <CNPJ>27922913000111</CNPJ>
        //        <chNFe>33150302117227000147550080000047801111031032</chNFe>
        //        <dhEvento>2015-04-15T09:37:15-03:00</dhEvento>
        //        <tpEvento>210200</tpEvento>
        //        <nSeqEvento>1</nSeqEvento>
        //        <verEvento>1.00</verEvento>
        //        <detEvento versao="1.00">
        //          <descEvento>Confirmacao da Operacao</descEvento>
        //        </detEvento>
        //      </infEvento>
        //  </evento>
        //</envEvento>
        // */
        //private XmlDocument ConvertEventoToXml(EventoRequest eventoRequest)
        //{
        //    XmlDocument doc = new XmlDocument();

        //    tagRaiz envEvento
        //    var envEvento = doc.CreateElement("envEvento", "http://www.portalfiscal.inf.br/nfe");
        //    envEvento.SetAttribute("versao", "1.00");
        //    doc.AppendChild(envEvento);

        //    Atributo idLote
        //    var idLote = doc.CreateElement("idLote", "http://www.portalfiscal.inf.br/nfe");
        //    idLote.InnerText = eventoRequest.idLote;
        //    doc.DocumentElement.AppendChild(idLote);

        //    grupo de elementos evento
        //    var elementoEvento = doc.CreateElement("evento", "http://www.portalfiscal.inf.br/nfe");
        //    elementoEvento.SetAttribute("versao", "1.00");

        //    if (eventoRequest.Eventos != null)
        //    {
        //        int seq = 1;
        //        foreach (var evento in eventoRequest.Eventos)
        //        {

        //            var infEvento = doc.CreateElement("infEvento", "http://www.portalfiscal.inf.br/nfe");
        //            var chave = evento.chaveNfe;

        //            O id do elemento é a combinação de "ID" + código do tipo de evento + chave da nfe +nº da sequencia
        //            string id = "ID" + evento.tipoEvento.Codigo + chave + ((seq < 10) ? "0" + seq.ToString() : seq.ToString());
        //            infEvento.SetAttribute("Id", id);

        //            var cOrgao = doc.CreateElement("cOrgao", "http://www.portalfiscal.inf.br/nfe");
        //            cOrgao.InnerText = evento.CodUf.ToString();
        //            infEvento.AppendChild(cOrgao);

        //            var tpAmb = doc.CreateElement("tpAmb", "http://www.portalfiscal.inf.br/nfe");
        //            tpAmb.InnerText = evento.tpAmb.ToString();
        //            infEvento.AppendChild(tpAmb);

        //            var CNPJ = doc.CreateElement("CNPJ", "http://www.portalfiscal.inf.br/nfe");
        //            CNPJ.InnerText = evento.CNPJ.ToString();
        //            infEvento.AppendChild(CNPJ);

        //            var chNFe = doc.CreateElement("chNFe", "http://www.portalfiscal.inf.br/nfe");
        //            chNFe.InnerText = evento.chaveNfe.ToString();
        //            infEvento.AppendChild(chNFe);

        //            var dhEvento = doc.CreateElement("dhEvento", "http://www.portalfiscal.inf.br/nfe");
        //            dhEvento.InnerText = String.Format("{0:yyyy-MM-ddTH:mm:ss-03:00}", evento.dhEvento);
        //            infEvento.AppendChild(dhEvento);

        //            var tpEvento = doc.CreateElement("tpEvento", "http://www.portalfiscal.inf.br/nfe");
        //            tpEvento.InnerText = evento.tipoEvento.Codigo.ToString();
        //            infEvento.AppendChild(tpEvento);

        //            var nSeqEvento = doc.CreateElement("nSeqEvento", "http://www.portalfiscal.inf.br/nfe");
        //            nSeqEvento.InnerText = seq.ToString();
        //            infEvento.AppendChild(nSeqEvento);

        //            var verEvento = doc.CreateElement("verEvento", "http://www.portalfiscal.inf.br/nfe");
        //            verEvento.InnerText = evento.VerEvento;
        //            infEvento.AppendChild(verEvento);

        //            var detEvento = doc.CreateElement("detEvento", "http://www.portalfiscal.inf.br/nfe");
        //            detEvento.SetAttribute("versao", "1.00");
        //            infEvento.AppendChild(detEvento);

        //            var descEvento = doc.CreateElement("descEvento", "http://www.portalfiscal.inf.br/nfe");
        //            descEvento.InnerText = evento.tipoEvento.Desc;
        //            detEvento.AppendChild(descEvento);

        //            elementoEvento.AppendChild(infEvento);
        //            seq++;

        //        }
        //    }

        //    envEvento.AppendChild(elementoEvento);
        //    return doc;
        //}
    }
}

