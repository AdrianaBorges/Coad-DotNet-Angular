using COAD.FISCAL.DownloadNf;
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
    class ClientWsDownloadNfeSRV
    {
        private NfeDownloadNFSoapClient downloadClient { get; set; }
        public X509Certificate2 Certificado { get; set; }

        public ClientWsDownloadNfeSRV()
        {
            Init();
        }

        public ClientWsDownloadNfeSRV(X509Certificate2 cert)
        {
            Certificado = cert;
            Init();
        }
        
        private void Init()
        {
            downloadClient = new NfeDownloadNFSoapClient();
        }

        public XmlNode BaixarNota(DownloadRequest downloadRequest)
        {
            if(Certificado == null){

                throw new Exception("Nenhum certificado foi encontrado. Para utilizar esse método é necessário um certificado para altenticar o usuário do webService");
            }

            if (downloadRequest != null)
            { 
                XmlDocument doc = ConvertDownloadRequestToXml(downloadRequest);
                //doc = XmlUtil.AssinarXml(doc, Certificado);
                nfeCabecMsg cabecRecep = new nfeCabecMsg();
                cabecRecep.cUF = "91";
                cabecRecep.versaoDados = "1.00";
                               
                var conf = downloadClient.ClientCredentials.ClientCertificate;
                conf.Certificate = Certificado;                
                var resp = downloadClient.nfeDownloadNF(cabecRecep, (XmlNode) doc);
                string downloadPath = string.Format(@"c:\notas_fiscais_baixadas\nfe_({0})_{1:yyyy-MM-ddTH-mm}.xml", downloadRequest.chaveNfe, DateTime.Now);
                XmlUtil.SerializeXml(resp, downloadPath);
                return resp;
            }

            return null;
        }
        
        public XmlNode BaixarNota(string chave, string downloadPath = @"c:\notas_fiscais_baixadas\")
        {
            DownloadRequest request = new DownloadRequest() {
                CNPJ = "27922913000111",
                tpAmb = "1",
                chaveNfe = chave,
                downloadPath = downloadPath
            };

            return BaixarNota(request);
        }


        /**
         * Exemplo do formato do xml de saida
         *  <downloadNFe  xmlns="http://www.portalfiscal.inf.br/nfe" versao="1.00">
	            <tpAmb>1</tpAmb>
	            <xServ>DOWNLOAD NFE</xServ>
	            <CNPJ>27922913000111</CNPJ>
	            <chNFe>35150314314050000662550640000026721496332811</chNFe>
            </downloadNFe>

         */
        private XmlDocument ConvertDownloadRequestToXml(DownloadRequest downloadRequest)
        {
            XmlDocument doc = new XmlDocument();

            var downloadNFe = doc.CreateElement("downloadNFe", "http://www.portalfiscal.inf.br/nfe");
            downloadNFe.SetAttribute("versao", "1.00");
            doc.AppendChild(downloadNFe);


            var tpAmb = doc.CreateElement("tpAmb", "http://www.portalfiscal.inf.br/nfe");
            tpAmb.InnerText = downloadRequest.tpAmb;
            downloadNFe.AppendChild(tpAmb);

            var xServ = doc.CreateElement("xServ", "http://www.portalfiscal.inf.br/nfe");
            xServ.InnerText = "DOWNLOAD NFE";
            downloadNFe.AppendChild(xServ);

            var CNPJ = doc.CreateElement("CNPJ", "http://www.portalfiscal.inf.br/nfe");
            CNPJ.InnerText = downloadRequest.CNPJ;
            downloadNFe.AppendChild(CNPJ);

            var chNFe = doc.CreateElement("chNFe", "http://www.portalfiscal.inf.br/nfe");
            chNFe.InnerText = downloadRequest.chaveNfe;
            downloadNFe.AppendChild(chNFe);

            return doc;
        }
    }
}

