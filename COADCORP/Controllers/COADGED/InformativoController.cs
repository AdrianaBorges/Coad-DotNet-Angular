using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Repositorios.Contexto;
using System.Globalization;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Service;
using System.Net;
using System.IO;
using System.Text;
using COAD.UTIL.Ferramentas;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.Threading;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class InformativoController : Controller
    {
        private InformativoSRV _service = new InformativoSRV();
        public AssinaturaSRV _assinatura { get; set; }
        private ClienteEnderecoSRV _endSRV = new ClienteEnderecoSRV();
        public InformativoSemanalEnvioSRV _infSem = new InformativoSemanalEnvioSRV();
        public ParametrosSRV _paramSRV = new ParametrosSRV();

        public void PreencherCombos()
        {
         
            bool MDP = false;
            ViewBag.cbProdutos = new ProdutosSRV().ObterProdutosInformativoSemanal(MDP, 2).Select(c => new SelectListItem() { Text = c.PRO_SIGLA, Value = c.PRO_ID.ToString() });
            ViewBag.ultimaRemessaEnviada = _paramSRV.BuscarUltimaRemessa();
            ViewBag.numultRemessa = _paramSRV.BuscarProximaRemessa();

        }

        [Autorizar]
        public ActionResult Index()
        {
            this.PreencherCombos();

            return View();
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult PostarInformativoSemanal()
        {
            this.PreencherCombos();

            return View();
        }
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult SalvarProtocolado(string _asn_num_assinatura, bool _protocolado)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _srvAssinatura = new AssinaturaSRV();

                var _assinatura = _srvAssinatura.FindById(_asn_num_assinatura);

                _assinatura.ASN_PROTOCOLADA = _protocolado;
                
                _srvAssinatura.SalvarAssinatura(_assinatura);
                
                response.success = true;
                response.message = Message.Info("Operação realizada com sucesso!!");
                
                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult SalvarMateriaAdicional(string _asn_num_assinatura, string _adicional)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _srvAssinatura = new AssinaturaSRV();

                var _assinatura = _srvAssinatura.FindById(_asn_num_assinatura);

                _assinatura.ASN_MATERIA_ADICIONAL = _adicional;

                _srvAssinatura.SalvarAssinatura(_assinatura);

                response.success = true;
                response.message = Message.Info("Operação realizada com sucesso!!");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult ListarMateriaAdicional(string _asn_num_assinatura, int _pagina = 1)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _srvAssinatura = new AssinaturaSRV();

                var _lstMateriaAdicional = _srvAssinatura.ListarMateriaAdicional(_asn_num_assinatura, _pagina);
                
                response.success = true;
                response.AddPage("lstMateriaAdicional", _lstMateriaAdicional);
                response.message = Message.Info("Operação realizada com sucesso!!");

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }


        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BuscarAssinaturasProtocoladas(int _pagina = 1, int _registroPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            try
            {

                var _lstProtocoladas = new AssinaturaSRV().BuscarAssinaturasProtocoladas(_pagina, _registroPorPagina);

                response.success = true;
                response.message = Message.Info("Consulta realizada com sucesso!!");
                response.AddPage("lstProtocoladas", _lstProtocoladas);

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }

        
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult ListarPostagens(int _tipo, DateTime _dataini, DateTime _datafim, int pagina = 1, int linhasPorPagina = 10)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                
                var _lstinformativo = new InformativoSemanalSRV().Buscar(_tipo, _dataini, _datafim, pagina, linhasPorPagina);

                response.success = true;
                response.message = Message.Info("Consulta realizada com sucesso!!");
                response.AddPage("lstinformativo", _lstinformativo);

                return Json(response, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }


        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        [HttpPost]
        public ActionResult BuscarAssinatura(string ano, string remessa, int envio, string assinatura)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _infSem = new InformativoSemanalAnaliticoSRV();
                var _buscarAssinatura = _infSem.BuscarAssinatura(assinatura); // buscar assinatura

                if (_buscarAssinatura.Count() == 0)
                    result.message = Message.Warning("Assinatura não localizada em qualquer remessa de informativos emitidas neste sistema!");

                result.Add("assinatura", _buscarAssinatura);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                result.Add("assinatura", "Erro ao localizar assinatura!");
            }

            return Json(result);
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult GerarEstatistica(string ano, string remessa, int envio, int nivel)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _infSem = new InformativoSemanalAnaliticoSRV();
                var _gerarEstatistica = _infSem.Estatisticas(ano, remessa, envio, nivel);

                if (_gerarEstatistica.Count() == 0)
                    result.message = Message.Warning("Sem registros para estatísticas!");

                result.Add("estatistica", _gerarEstatistica.ToList());
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Fail(ex);
                result.Add("estatistica", "Sem registros para estatísticas!");
            }

            return Json(result);
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult EnviarArquivo(string ano, string remessa, bool MDP = false, int? envio = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (envio == 1)
                {
                    this.PostarCartas(ano,remessa,MDP,null);

                }
                else if (envio == 2)
                {
                    this.PostarEntregaDireta(ano, remessa, MDP, null);
                }
                    

                response.success = true;
                response.message = Message.Info("Arquivos enviados com sucesso");
                
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }
        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BuscarCliente(string _asn_id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var _ass = _assinatura.FindById(_asn_id);

                if (_ass != null)
                {

                    var _cli = new ClienteSRV().FindById(_ass.CLI_ID);

                    if (_cli != null)
                    {
                        _ass.CLIENTES = _cli;

                        var _end =  _endSRV.FindEnderecoCliente((int)_cli.CLI_ID, 1);

                        if (_end == null)
                            throw new Exception("Cliente com endereço inválido!! Verifique!!");

                        if (_end.END_CEP == "" ||
                            _end.END_CEP == null ||
                            _end.END_LOGRADOURO == "" ||
                            _end.END_LOGRADOURO == null)
                            throw new Exception("Cliente com endereço inválido!! Verifique!!");

                        response.success = true;
                        response.Add("ASN_NUM_ASSINATURA", _ass.ASN_NUM_ASSINATURA);
                        response.Add("CLI_NOME", _cli.CLI_NOME);
                        response.Add("ASN_MATERIA_ADICIONAL", _ass.ASN_MATERIA_ADICIONAL);
                        response.Add("ASN_PROTOCOLADA", _ass.ASN_PROTOCOLADA);
                        return Json(response, JsonRequestBehavior.AllowGet);

                    }
                    else
                        throw new Exception("Cliente não encontrado");
                }
                else
                {
                    throw new Exception("Assinatura não encontrada");
                }
                
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult BaixarArquivo(string ano, string remessa, int? envio = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {            
                var _infSem = new InformativoSemanalEnvioSRV();

                var _pathRetorno = _infSem.BaixarArquivos(ano, remessa, envio, System.Web.HttpContext.Current);

                response.success = true;
                response.message = Message.Info("Arquivos gerados com sucesso");
                response.Add("pathRetorno", _pathRetorno);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult MontarRemessa(string ano, string remessa, bool MDP = false, DateTime? dtEntrega = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                _infSem.GerarRemessaFull(ano, remessa, MDP, dtEntrega);
               

                response.success = true;
                response.message = Message.Success("Arquivos gerados com Sucesso !!");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult PostarEntregaDireta(string ano, string remessa, bool MDP, DateTime? dtEntrega = null)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _infSem = new InformativoSemanalEnvioSRV();

                var _nomeArquivo = "";
                var _conteudoArquivo = "";
                var _remessaAenviar = _infSem.RemessaAenviar(ano, remessa, 2); 

                Ftp FTP = new Ftp(); 

                foreach (var aEnviar in _remessaAenviar)
                {
                    if (_nomeArquivo != aEnviar.INF_ARQUIVO) 
                    {
                        if (_nomeArquivo != "" && _conteudoArquivo != "") 
                        {
                            FTP.Postar(_nomeArquivo, _conteudoArquivo); 
                            _conteudoArquivo = "";
                        }

                        _nomeArquivo = aEnviar.INF_ARQUIVO; 
                    }

                    _conteudoArquivo += aEnviar.INF_TEXTO + Environment.NewLine; 

                }

                if (_nomeArquivo != "" && _conteudoArquivo != "")
                {
                    FTP.Postar(_nomeArquivo, _conteudoArquivo); 
                }

                response.message = Message.Success("Postagem da Entrega Direta enviada com sucesso!");
                response.Add("informativo", "Postagem da Entrega Direta enviada com sucesso!");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult PostarCartas(string ano, string remessa, bool MDP, DateTime? dtEntrega = null)
        {
    
            JSONResponse response = new JSONResponse();
            try
            {
                
                var _infSem = new InformativoSemanalEnvioSRV();
                var _nomeArquivo = "";
                var _conteudoArquivo = "";
                var _pasta = "/Folha de Rosto/" + ano + "-" + remessa; 

                var _remessaAenviar = _infSem.RemessaAenviar(ano, remessa, 1); 

                DropBox dbx = new DropBox();

                if (!dbx.PastaExiste(_pasta))
                {
                    if (!dbx.CriarPasta(_pasta))
                    {
                        throw new Exception(string.Format("Não foi possível criar a pasta {0} no Dropbox.", _pasta));
                    }
                }

                foreach (var aEnviar in _remessaAenviar)
                {
                    if (_nomeArquivo != aEnviar.INF_ARQUIVO) 
                    {
                        if (_nomeArquivo != "" && _conteudoArquivo != "")
                        {
                            var db = dbx.DropboxUpload(_pasta, _nomeArquivo, _conteudoArquivo);

                            Thread.Sleep(1000);

                            _conteudoArquivo = "";
                        }
                        _nomeArquivo = aEnviar.INF_ARQUIVO; 
                    }
                    _conteudoArquivo += aEnviar.INF_TEXTO + Environment.NewLine; 
                }

                if (_nomeArquivo != "" && _conteudoArquivo != "")
                {
                    var db = dbx.DropboxUpload(_pasta, _nomeArquivo, _conteudoArquivo); 
                }

                response.message = Message.Success("Postagem no Dropbox da Gráfica COAD enviada com sucesso!");
                response.Add("informativo", "Postagem no Dropbox da Gráfica COAD enviada com sucesso!");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

         
    }
}