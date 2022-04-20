using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace COADWEBAPI.Controllers
{
    public class MateriasController : ApiController
    {
        private LogAcessoPortalSRV _serviceLogAcessoPortal = new LogAcessoPortalSRV();
        public HttpResponseMessage RegistraEvento()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            int identificador = 0;
            int origemAcesso = 0;
            try
            {
                var acao = Request.Headers.GetValues("acao").ElementAt(0);
                var ip = Request.Headers.GetValues("ip").ElementAt(0);
                var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                var email = Request.Headers.GetValues("email").ElementAt(0);
                var url = Request.Headers.GetValues("url").ElementAt(0);
                var id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;
                var origem = int.TryParse(Request.Headers.GetValues("origem").ElementAt(0), out origemAcesso) ? origemAcesso : 0;

                RegistrarLog(acao, ip, assinatura, email, url, id, origemAcesso);
                response.Content = new StringContent("{\"message\":\"Salvo.\",\"success\": true}", Encoding.UTF8, "application/json");
            }
            catch (InvalidOperationException)
            {
                response.Content = new StringContent("{\"message\":\"Campos obrigátorios não enviados.\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\""+ e.Message + ".\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        public string RegistrarLog(string acao, string ip, string assinatura, string email, string url, int id, int origem)
        {
            LogAcessoPortalDTO lgapdto = new LogAcessoPortalDTO();
            lgapdto.NOTICIA = null;
            lgapdto.ORIGEM_ACESSO_REF = null;
            lgapdto.PUBLICACAO = null;

            try
            {
                lgapdto.LAP_IP_ACESSO = ip;
                lgapdto.LAP_DATA_ACESSO = DateTime.Now;
                lgapdto.LAP_MSG_ERRO = "";
                lgapdto.ASN_NUM_ASSINATURA = assinatura;
                lgapdto.LSI_EMAIL_ACESSO = email;
                lgapdto.LAP_URL_ACESSO = url + '-' + acao;
                lgapdto.NOT_ID = null;
                lgapdto.PUB_ID = null;
                lgapdto.NOT_ID_PORTAL = 0;
                lgapdto.PUB_ID_PORTAL = id;
                lgapdto.OAC_ID = origem;
                _serviceLogAcessoPortal.Save(lgapdto);
                return "Item salvo com sucesso!";
            }
            catch (Exception e)
            {
                return "Item não pode ser salvo. Erro: "+e.Message;
            }
        }
    }
}