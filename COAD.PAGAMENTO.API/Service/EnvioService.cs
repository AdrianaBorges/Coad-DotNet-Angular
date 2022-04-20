using COAD.PAGAMENTO.API.Model;
using COAD.PAGAMENTO.API.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace COAD.PAGAMENTO.API.Service
{
    public class EnvioService
    {
        private string url { get; set; }

        public void SetEnvironment(bool homol)
        {
            if (homol)
            {
                url = ServiceUrl.Homologation;
            }
            else
            {
                url = ServiceUrl.Production;
            }
        }

        public RespostaEnvio Enviar(Credentials credentials)
        {
            if (credentials != null)
            {
                if (String.IsNullOrWhiteSpace(credentials.KEY))
                {
                    // erro
                }

                if (String.IsNullOrWhiteSpace(credentials.TOKEN))
                {
                    // TODO: Implementar erros
                }

                string key = credentials.KEY;
                string token = credentials.TOKEN;

                string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(token + ":" + key));

                InstrucaoParser parser = new InstrucaoParser();
                XmlDocument xml = parser.ParseInstrucao();

                if (xml != null)
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("Authorization: Basic " + auth);
                    client.Headers.Add("User-Agent: Mozilla/4.0");
                    string xmlStr = parser.ParseToString(xml);
                    byte[] ResponseArray = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(xmlStr));

                    string xmlResponseStr = Encoding.UTF8.GetString(ResponseArray);
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(xmlResponseStr);
                    
                    var tagIds = xmlResponse.GetElementsByTagName("ID");
                    string id = (tagIds.Count > 0) ? tagIds[0].InnerText : null;
                    
                    var tagStatus = xmlResponse.GetElementsByTagName("Status");
                    string status = (tagStatus.Count > 0) ? tagStatus[0].InnerText : null;

                    var tagToken = xmlResponse.GetElementsByTagName("Token");
                    string tokenResp = (tagToken.Count > 0) ? tagToken[0].InnerText : null;

                    if (!String.IsNullOrWhiteSpace(id) && !String.IsNullOrWhiteSpace(status))
                    {
                        RespostaEnvio resposta = new RespostaEnvio(id, status, tokenResp);
                        return resposta;
                    }
                    
                }

            }

            return null;
        }
    }
}
