using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Service.PortalCoad;
using COADWEBAPI.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Globalization;
using COAD.PORTAL.Utils;
using COAD.COADGED.Service;
using COAD.COADGED.Model.DTO;
using System.Web;

namespace COADWEBAPI.Controllers
{
    public class NoticiasController : ApiController 
    {
        UtilsPortal up = new UtilsPortal();
        private Noticias_BuscaPortalSRV _serviceNoticias = new Noticias_BuscaPortalSRV();
        private Noticias_ConteudoSRV _serviceNotCont = new Noticias_ConteudoSRV();
        private Noticias_GrupoSRV _serviceNotGrupo = new Noticias_GrupoSRV();
        private NoticiasGuardadasSRV _serviceNotGuardadas = new NoticiasGuardadasSRV();
        private LogSimuladorSRV _serviceLogNoticias = new LogSimuladorSRV();
        private LogAcessoPortalSRV _serviceLogAcessoPortal = new LogAcessoPortalSRV();

        /*Recupera últimas notícias*/
        public HttpResponseMessage Get()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                MemoryStream streamObj = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NoticiasMobile));

                var ultimaNoticia = TransformarEmDTODeNoticiaAdequado(_serviceNoticias.UltimaNoticia());

                if (ultimaNoticia != null)
                {
                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, ultimaNoticia);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }

                    jsonobj += "{\"result\": {\"noticia\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Sem notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch
            {
                response.Content = new StringContent("{\"message\":\"Erro ao buscar notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        /*Busca listas de notícias trabalhistas e tributárias*/
        public HttpResponseMessage BuscarNoticias([FromBody]string value)
        {
            int nLinha = 10;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            MemoryStream streamObj = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<NoticiasMobile>));
            List<NoticiasMobile> lstNoticias = new List<NoticiasMobile>();
            var identificador = 0;
            try
            {
                var npagina = Request.Headers.GetValues("pagina").ElementAt(0);
                var tipo = Request.Headers.GetValues("tipo").ElementAt(0);

                try
                {
                    string tipoNot = "recentes";
                    var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                    var ip = Request.Headers.GetValues("ip").ElementAt(0);
                    var email = Request.Headers.GetValues("email").ElementAt(0);
                    var url = "";
                    var id = 0;

                    try
                    {
                        url = Request.Headers.GetValues("url").ElementAt(0);
                        id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;
                    }
                    catch
                    {
                    }

                    if (tipo.Equals("20"))
                        tipoNot = "trabalhistas";
                    else if (tipo.Equals("18"))
                        tipoNot = "tributárias";

                    RegistrarLog("Buscar noticias " + tipoNot, ip, assinatura, email, url, id);
                }
                catch{}

                int parsepagina = 0;
                CultureInfo ci = CultureInfo.InvariantCulture;
                if (int.TryParse(npagina, out parsepagina))
                {
                    if (tipo.Equals("0"))
                        lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.NoticiasEmOrdemDescendente(parsepagina, nLinha));
                    else
                        lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.NoticiasPorTipoEmOrdemDescendente(tipo, parsepagina, nLinha));

                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, lstNoticias);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }
                    jsonobj += "{\"result\": {\"noticias\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch (InvalidOperationException)
            {
                response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\"" + e.Message + "\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            return response;
        }        

        /*Recupera notícia específica*/
        public HttpResponseMessage BuscarNoticiaPorId([FromBody]string value)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            MemoryStream streamObj = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NoticiasMobile));
            var identificador = 0;

            try
            {
                var id = Request.Headers.GetValues("id").ElementAt(0);
                var email = Request.Headers.GetValues("email").ElementAt(0);
                var ip = Request.Headers.GetValues("ip").ElementAt(0);
                var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                var url = Request.Headers.GetValues("url").ElementAt(0);
                var idConvert = int.TryParse(id, out identificador) ? identificador : 0;


                int parseid = 0;
                if (int.TryParse(id, out parseid))
                {
                    NoticiasPortalBuscaDTO noticia = _serviceNoticias.BuscarPorIdNoticia(parseid);
                    NoticiasMobile nm = new NoticiasMobile();
                    if (noticia != null)
                    {
                        nm.id = noticia.id_noticia;
                        nm.id_prod = noticia.id_prod;
                        nm.id_tipo = noticia.id_tipo;
                        nm.texto = noticia.texto_integra;

                        //var conteudo = _serviceNotCont.BuscarPorIdDaNoticia(noticia.id);
                        //if (conteudo != null)
                        //{
                        nm.verbete = noticia.verbete_integra;
                        nm.subverbete = "";
                        //}
                        nm.data_cadastro = noticia.data_cadastro.ToString();

                        string json = "";
                        string jsonobj = "";
                        using (MemoryStream stream = new MemoryStream())
                        {
                            ser.WriteObject(stream, nm);
                            json = Encoding.Default.GetString(stream.ToArray());
                        }
                        jsonobj += "{\"result\": {\"noticia\":";
                        jsonobj += json;
                        jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                        response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");

                        RegistrarLog("Busca noticia por id = " + id, ip, assinatura, email, url, idConvert);
                    }
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Objeto não encontrado.\",\"success\": false}", Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"ID inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch (InvalidOperationException)
            {
                response.Content = new StringContent("{\"message\":\"Parametros inválidos.\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\"" + e.Message + "\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            return response;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage PushNotificationNoticiasCoadNow()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                var titulo = HttpContext.Current.Request.Form["titulo"];
                var idNoticia = HttpContext.Current.Request.Form["idNoticia"];
                var corpo = HttpContext.Current.Request.Form["corpo"];
                var idProduto = HttpContext.Current.Request.Form["idProduto"];

                var firebaseData = "" +
                "{" +
                "\"to\":\"/topics/NEWS\"," +
                "\"priority\" : \"normal\"," +
                "\"notification\": {" +
                "}," +
                "\"data\": {" +
                    "\"title\": " + titulo + "," +
                    "\"text\": " + corpo + "," +
                    "\"click_action\": \"coad.now_TARGET_NOTIFICATION\"" +
                    "\"extra_information\": \"This is some extra information\"," +
                    "\"idNoticia\":" + idNoticia + "," +
                    "\"idProduto\":" + idProduto +
                    "}," +
                    "\"time_to_live\": 20" +
            "}";
                var webRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add("Authorization", "key=AAAAtcUUXIc:APA91bFFJXBNXZI3opSVmSdZM4eYiElAgxGJeOWxOir0yjD3JXyoc4RYHmeno8ZTdMei35SjeZsA1R8aE9ud6j9aZ2GgdqEUT6WxT0njzXSEARbGMk7UHmdhTva1h8Bkt-JmZOfGG21u");
                using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    streamWriter.Write(firebaseData);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    String sResponseFromServer = streamReader.ReadToEnd();
                    string str = sResponseFromServer;
                }
                return response;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return response;
            }
        }

        /*O app envia os ids para recuperar notícias salvas pelo usuário e atualizar na base do celular*/
        public HttpResponseMessage AtualizarMinhasNoticias()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                MemoryStream streamObj = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<NoticiasMobile>));
                var ids = Request.Headers.GetValues("ids").ElementAt(0);
                string[] ids_recuperados = ids.Split('|');
                List<NoticiasMobile> lstNoticias = new List<NoticiasMobile>();

                List<int> lista_ids = new List<int>();

                foreach (string id in ids_recuperados)
                {
                    int numero;
                    bool resultado = Int32.TryParse(id, out numero);
                    if (resultado)
                        lista_ids.Add(numero);
                }

                lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.BuscarNoticiasEmLotePorIDs(lista_ids));

                if (lstNoticias != null)
                {
                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, lstNoticias);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }

                    jsonobj += "{\"result\": {\"noticia\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Sem notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch
            {
                response.Content = new StringContent("{\"message\":\"Erro ao buscar notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        /*Salva notícia do usuário*/
        public HttpResponseMessage SalvarNoticia()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                MemoryStream streamObj = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(NoticiasGuardadasDTO));

                var idCliente = Request.Headers.GetValues("idCliente").ElementAt(0);
                var idNoticia = Request.Headers.GetValues("idNoticia").ElementAt(0);
                var url = "";

                try
                {
                    url = Request.Headers.GetValues("url").ElementAt(0);
                }
                catch
                {
                }
                

                int convertIdCliente;
                int convertIdNoticia;
                bool resultadoCliente = Int32.TryParse(idCliente, out convertIdCliente);
                bool resultadoNoticia = Int32.TryParse(idNoticia, out convertIdNoticia);

                if (resultadoCliente && resultadoNoticia)
                {
                    var noticiaRecuperada = _serviceNotGuardadas.BuscarNoticiaPorIdClienteIdNoticia(convertIdCliente, convertIdNoticia);
                    if (noticiaRecuperada == null)
                    {
                        try
                        {
                            var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                            var ip = Request.Headers.GetValues("ip").ElementAt(0);
                            var email = Request.Headers.GetValues("email").ElementAt(0);

                            RegistrarLog("Salvar noticia " + idNoticia, ip, assinatura, email, url, convertIdNoticia);
                        }
                        catch{}

                        NoticiasGuardadasDTO ngdto = new NoticiasGuardadasDTO();
                        ngdto.ID_CLIENTE = convertIdCliente;
                        ngdto.ID_NOTICIA = convertIdNoticia;
                        _serviceNotGuardadas.Save(ngdto);

                        string json = "";
                        string jsonobj = "";
                        using (MemoryStream stream = new MemoryStream())
                        {
                            ser.WriteObject(stream, ngdto);
                            json = Encoding.Default.GetString(stream.ToArray());
                        }

                        jsonobj += "{\"result\": {\"noticia\":";
                        jsonobj += json;
                        jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                        response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Você já salvou essa notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Parâmetros inválidos.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch
            {
                response.Content = new StringContent("{\"message\":\"Erro ao salvar notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        /*Deleta notícia do usuário*/
        public HttpResponseMessage DeletarNoticia()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                var idCliente = Request.Headers.GetValues("idCliente").ElementAt(0);
                var idNoticia = Request.Headers.GetValues("idNoticia").ElementAt(0);

                var url = "";

                try
                {
                    url = Request.Headers.GetValues("url").ElementAt(0);
                    //id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;
                }
                catch
                {
                }                

                int convertIdCliente;
                int convertIdNoticia;
                bool resultadoCliente = Int32.TryParse(idCliente, out convertIdCliente);
                bool resultadoNoticia = Int32.TryParse(idNoticia, out convertIdNoticia);

                if (resultadoCliente && resultadoNoticia)
                {
                    var noticiaRecuperada = _serviceNotGuardadas.BuscarNoticiaPorIdClienteIdNoticia(convertIdCliente, convertIdNoticia);
                    if (noticiaRecuperada != null)
                    {
                        try
                        {
                            var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                            var ip = Request.Headers.GetValues("ip").ElementAt(0);
                            var email = Request.Headers.GetValues("email").ElementAt(0);

                            RegistrarLog("Excluir noticia " + idNoticia, ip, assinatura, email, url, convertIdNoticia);
                        }
                        catch{}

                        _serviceNotGuardadas.ExcluirNoticia(convertIdCliente, convertIdNoticia);
                        response.Content = new StringContent("{\"message\":\"Notícia excluída.\",\"success\": true}", Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Notícia inexistente.\",\"success\": false}", Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Parâmetros inválidos.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch
            {
                response.Content = new StringContent("{\"message\":\"Erro ao deletar notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        /*Recupera notícias por id de cliente*/
        public HttpResponseMessage RecuperarMinhasNoticias()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            try
            {
                var id = Request.Headers.GetValues("id").ElementAt(0);
                MemoryStream streamObj = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<NoticiasMobile>));
                List<NoticiasMobile> lstNoticias = new List<NoticiasMobile>();
                List<int> listaIds = new List<int>();

                int convertId;
                bool resultadoCliente = Int32.TryParse(id, out convertId);

                if (resultadoCliente)
                {
                    var minhasNoticiasRecuperada = _serviceNotGuardadas.NoticiasSalvas(convertId);

                    foreach (var mn in minhasNoticiasRecuperada)
                        listaIds.Add(mn.ID_NOTICIA);

                    if (listaIds.Count > 0)
                    {
                        lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.BuscarNoticiasEmLotePorIDs(listaIds));

                        if (lstNoticias != null)
                        {
                            string json = "";
                            string jsonobj = "";
                            using (MemoryStream stream = new MemoryStream())
                            {
                                ser.WriteObject(stream, lstNoticias);
                                json = Encoding.Default.GetString(stream.ToArray());
                            }

                            jsonobj += "{\"result\": {\"noticia\":";
                            jsonobj += json;
                            jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                            response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            response.Content = new StringContent("{\"message\":\"Noticías inexistentes.\",\"success\": false}", Encoding.UTF8, "application/json");
                        }
                    }
                    else
                    {
                        response.Content = new StringContent("{\"message\":\"Sem notícia salvas.\",\"success\": false}", Encoding.UTF8, "application/json");
                    }
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Id inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch
            {
                response.Content = new StringContent("{\"message\":\"Erro ao buscar notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        /*Busca listas de notícias trabalhistas e tributárias por palavras específicas*/
        public HttpResponseMessage BuscarNoticiasPorPalavrasChave([FromBody]string value)
        {
            int nLinha = 10;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            MemoryStream streamObj = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<NoticiasMobile>));
            List<NoticiasMobile> lstNoticias = new List<NoticiasMobile>();
            var identificador = 0;
            try
            {
                var npagina = Request.Headers.GetValues("pagina").ElementAt(0);
                var tipo = Request.Headers.GetValues("tipo").ElementAt(0);
                var palavras = Request.Headers.GetValues("palavras").ElementAt(0);
                var url = "";
                var id = 0;

                try
                {
                    url = Request.Headers.GetValues("url").ElementAt(0);
                    id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;
                }
                catch
                {
                }


                try
                {
                    var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                    var ip = Request.Headers.GetValues("ip").ElementAt(0);
                    var email = Request.Headers.GetValues("email").ElementAt(0);

                    RegistrarLog("Pesquisa " + palavras, ip, assinatura, email, url, id);
                }
                catch{}


                int parsepagina = 0;
                CultureInfo ci = CultureInfo.InvariantCulture;
                if (int.TryParse(npagina, out parsepagina))
                {
                    lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.BuscaNoticiasPalavrasChaveVerbeteTexto(up.RetirarAcentos(palavras), parsepagina, nLinha));

                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, lstNoticias);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }
                    jsonobj += "{\"result\": {\"noticias\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch (InvalidOperationException)
            {
                response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\"" + e.Message + "\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            return response;
        }

        /*Busca listas de notícias por grupo*/
        public HttpResponseMessage BuscarNoticiasPorGrupo([FromBody]string value)
        {
            int nLinha = 10;
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            MemoryStream streamObj = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<NoticiasMobile>));
            List<NoticiasMobile> lstNoticias = new List<NoticiasMobile>();
            var identificador = 0;
            try
            {
                var npagina = Request.Headers.GetValues("pagina").ElementAt(0);
                var id_grupo = Request.Headers.GetValues("id_grupo").ElementAt(0);
                var url = Request.Headers.GetValues("url").ElementAt(0);
                var id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;

                try
                {
                    string tipoNot = "cfop 2017";
                    var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                    var ip = Request.Headers.GetValues("ip").ElementAt(0);
                    var email = Request.Headers.GetValues("email").ElementAt(0);

                    RegistrarLog("Buscar noticias " + tipoNot, ip, assinatura, email, url, id);
                }
                catch{}

                int parsepagina = 0;
                CultureInfo ci = CultureInfo.InvariantCulture;
                if (int.TryParse(npagina, out parsepagina))
                {
                    lstNoticias = TransformarEmListaDeDTOsDeNoticiasAdequado(_serviceNoticias.BuscaNoticiasPorGrupo(id_grupo, parsepagina, nLinha));

                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, lstNoticias);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }
                    jsonobj += "{\"result\": {\"noticias\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch (InvalidOperationException)
            {
                response.Content = new StringContent("{\"message\":\"ID página inválido.\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\"" + e.Message + "\",\"success\": false}", Encoding.UTF8, "application/json");
            }
            return response;
        }

        public HttpResponseMessage RetornaNoticiasMaisLidas()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            MemoryStream streamObj = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<int?>));
            List<int?> ids = _serviceLogAcessoPortal.EncontrarMaisLidas();

            //var acao = Request.Headers.GetValues("param").ElementAt(0);
            try
            {
                if (ids != null)
                {
                    string json = "";
                    string jsonobj = "";
                    using (MemoryStream stream = new MemoryStream())
                    {
                        ser.WriteObject(stream, ids);
                        json = Encoding.Default.GetString(stream.ToArray());
                    }

                    jsonobj += "{\"result\": {\"ids\":";
                    jsonobj += json;
                    jsonobj += "},\"message\": null,\"validationMessage\": {},\"page\": null,\"success\": true}";

                    response.Content = new StringContent(jsonobj, Encoding.UTF8, "application/json");
                }
                else
                {
                    response.Content = new StringContent("{\"message\":\"Sem notícia.\",\"success\": false}", Encoding.UTF8, "application/json");
                }
            }
            catch (Exception e)
            {
                response.Content = new StringContent("{\"message\":\""+e.Message+".\",\"success\": false}", Encoding.UTF8, "application/json");
            }

            return response;
        }

        public List<NoticiasMobile> TransformarEmListaDeDTOsDeNoticiasAdequado(IEnumerable<NoticiasPortalBuscaDTO> noticias)
        {
            List<NoticiasMobile> noticiasParaMobile = new List<NoticiasMobile>();
            NoticiasMobile nm = null;

            foreach (var noticia in noticias)
            {
                nm = new NoticiasMobile();
                nm.id = noticia.id_noticia;
                nm.id_prod = noticia.id_prod;
                nm.id_tipo = noticia.id_tipo;

                byte[] bytesTexto = Encoding.Default.GetBytes(noticia.texto_integra == null ? "" : noticia.texto_integra);
                nm.texto = Encoding.UTF8.GetString(bytesTexto);

                byte[] bytesDescricao = Encoding.Default.GetBytes(noticia.descricao == null ? "" : noticia.descricao);
                nm.grupo = Encoding.UTF8.GetString(bytesDescricao);

                byte[] bytesverbete = Encoding.Default.GetBytes(noticia.verbete_integra == null ? "" : noticia.verbete_integra);
                nm.verbete = Encoding.UTF8.GetString(bytesverbete);
                nm.subverbete = "";
                nm.data_cadastro = noticia.data_cadastro.ToString();
                noticiasParaMobile.Add(nm);
            }

            return noticiasParaMobile;
        }

        public NoticiasMobile TransformarEmDTODeNoticiaAdequado(NoticiasPortalBuscaDTO noticia)
        {
            NoticiasMobile nm = new NoticiasMobile();
            nm.id = noticia.id_noticia;
            nm.id_prod = noticia.id_prod;
            nm.id_tipo = noticia.id_tipo;

            byte[] bytesTexto = Encoding.Default.GetBytes(noticia.texto_integra == null ? "" : noticia.texto_integra);
            nm.texto = Encoding.UTF8.GetString(bytesTexto);

            byte[] bytesDescricao = Encoding.Default.GetBytes(noticia.descricao == null ? "" : noticia.descricao);
            nm.grupo = Encoding.UTF8.GetString(bytesDescricao);

            byte[] bytesverbete = Encoding.Default.GetBytes(noticia.verbete_integra == null ? "" : noticia.verbete_integra);
            nm.verbete = Encoding.UTF8.GetString(bytesverbete);

            nm.subverbete = "";
            nm.data_cadastro = noticia.data_cadastro.ToString();

            return nm;
        }

        public HttpResponseMessage RegistraEvento()
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            int identificador = 0;
            try
            {
                var acao = Request.Headers.GetValues("acao").ElementAt(0);
                var ip = Request.Headers.GetValues("ip").ElementAt(0);
                var assinatura = Request.Headers.GetValues("assinatura").ElementAt(0);
                var email = Request.Headers.GetValues("email").ElementAt(0);
                var url = "";
                var id = 0;

                try
                {
                    url = Request.Headers.GetValues("url").ElementAt(0);
                    id = int.TryParse(Request.Headers.GetValues("id").ElementAt(0), out identificador) ? identificador : 0;
                }
                catch
                {
                }

                RegistrarLog(acao, ip, assinatura, email, url, id);
                response.Content = new StringContent("{\"message\":\"Salvo.\",\"success\": true}", Encoding.UTF8, "application/json");
            }
            catch (Exception e)
            {
                response.Content = new StringContent(e.Message, Encoding.UTF8, "application/json");
            }

            return response;
        }

        public void RegistrarLog(string acao, string ip, string assinatura, string email, string url, int id)
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
                lgapdto.LAP_URL_ACESSO = url + '-' +acao;
                lgapdto.NOT_ID = null;
                lgapdto.PUB_ID = null;
                lgapdto.NOT_ID_PORTAL = id;
                lgapdto.PUB_ID_PORTAL = 0;
                //Código 5 referente ao coadnow
                lgapdto.OAC_ID = url.Contains("noticias-detalhe") ? 1 : 5;
            }catch(Exception e)
            {
                lgapdto.LAP_IP_ACESSO = "0.0.0.0";
                lgapdto.LAP_DATA_ACESSO = DateTime.Now;
                lgapdto.LAP_MSG_ERRO = e.Message;
                lgapdto.ASN_NUM_ASSINATURA = assinatura;
                lgapdto.LSI_EMAIL_ACESSO = email;
                lgapdto.LAP_URL_ACESSO = url;
                lgapdto.NOT_ID = null;
                lgapdto.PUB_ID = null;
                lgapdto.NOT_ID_PORTAL = id;
                lgapdto.PUB_ID_PORTAL = 0;
                lgapdto.OAC_ID = 5;
            }
            
            _serviceLogAcessoPortal.Save(lgapdto);
        }
    }
}