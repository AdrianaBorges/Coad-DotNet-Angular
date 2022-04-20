using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.ServiceModel;
using COAD.FISCAL.DownloadNf;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Xml.Serialization;
using COAD.FISCAL.Model;
using COAD.FISCAL.Service;
using COAD.FISCAL.Service.Config;
using COAD.FISCAL.XmlUtils;
using COAD.FISCAL.Service.Integracoes;

namespace COAD.FISCAL
{
    class Program
    {
        static void Main(string[] args)
        {
            //33150302117227000147550080000047801111031032
            //NfeDownloadNFSoapClient client = new NfeDownloadNFSoapClient();

            //ClientWsConfig.SetCertificate(@"C:\Users\dasilva\Documents\certificados\13623007_ATUALIZACAO_PROFISSIONAL_CONTINUADA_LTDA27922913000111matriz.p12", "279229"); // nova 279229

            //string chaveNFE = null;
            //NfeRecepcao2SoapClient recepcao = new NfeRecepcao2SoapClient();


            //Console.WriteLine("Digite a chave da nota fiscal:");
            //chaveNFE = Console.ReadLine();

            //if (string.IsNullOrWhiteSpace(chaveNFE))
            //{
            //    chaveNFE = @"33160911127042000104550010000002141000180366";
            //}

            //var client1 = ClientWsFactory.CriarClientWsManifestacaoEvento();
            //var clientDownLoad = ClientWsFactory.CriarClientWsDownload();
            //var data = new DateTime(2016,10,1,10,0,0);
            //var resp = client1.EnviarEvento(chaveNFE,null, data);
            //var resp2 = clientDownLoad.BaixarNota(chaveNFE);

            //XmlUtil.SerializeXml(resp2, @"C:\xmlnfe\download_response.xml");
            /*
            var status = resp["cStat"].InnerText;
            var mensagem = resp["xMotivo"].InnerText;
            var InfEvento = resp["retEvento"]["infEvento"];
            
            var statusEvento = InfEvento["cStat"].InnerText;
            var mensagemEvento = InfEvento["xMotivo"].InnerText;
             */

            //Console.WriteLine(String.Format("Status Geral: {0}  Motivo Geral: {1}", status, mensagem));
           // Console.WriteLine(String.Format("Status Evento: {0} Motivo Evento: {1}", statusEvento, mensagemEvento));

            IntegrNfeSRV service = new IntegrNfeSRV();
            service.TestarCriacaoDeNfe(5102);
            Console.ReadLine();

        }

    }
}
