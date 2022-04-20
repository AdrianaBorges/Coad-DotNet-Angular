using COADWEBAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Web.Http;
//using COAD.CORPORATIVO.Service.Aloweb;
using System.Web.Script.Serialization;

namespace COADWEBAPI.Controllers
{
    public class AlowebController : ApiController
    {

        //private AlowebService AlowebService;

        //public AlowebController()
        //{
        //    AlowebService = new AlowebService();
        //}

        [HttpPost]
        public HttpResponseMessage Inicio()
        {
            var JsonObject = JObject.Parse(Request.Content.ReadAsStringAsync().Result);
            string type = JsonObject["type"].ToString();
            /*switch (type)
            {
                case "atendimento_iniciado":
                    AlowebService.RegistraAtendimentoIniciado(JsonObject);
                    break;
                case "atendimento_aceito":
                    AlowebService.RegistraAtendimentoAceito(JsonObject);
                    break;
                case "atendimento_encerrado":
                    AlowebService.RegistraAtendimentoEncerrado(JsonObject);
                    break;
            }*/
            return null ;
        }
    }
}