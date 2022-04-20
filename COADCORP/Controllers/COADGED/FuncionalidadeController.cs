using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.COADGED
{

    public class FuncionalidadeController : Controller
    {
        private TabDinamicaConfigSRV _tabDinConfigSRV = new TabDinamicaConfigSRV();

        //                                    <ol class=""carousel-indicators"">
        //                                        <li data-target=""#carousel-example-generic"" data-slide-to=""0"" class=""active"" ng-if=""funcionalidade.FCI_IMG01!=null""></li>
        //                                        <li data-target=""#carousel-example-generic"" data-slide-to=""1"" ng-if=""funcionalidade.FCI_IMG02!=null""></li>
        //                                        <li data-target=""#carousel-example-generic"" data-slide-to=""2"" ng-if=""funcionalidade.FCI_IMG03!=null""></li>
        //                                        <li data-target=""#carousel-example-generic"" data-slide-to=""3"" ng-if=""funcionalidade.FCI_IMG04!=null""></li>
        //                                    </ol>

        public string carousel = @"<div id=""carousel-example-generic"" class=""carousel slide"" data-ride=""carousel"" data-interval=""10000""> 
                                    <div class=""carousel-inner"" role=""listbox"">
                                         {0}
                                    </div>
                                    <a class=""left carousel-control"" href=""#carousel-example-generic"" role=""button"" data-slide=""prev"">
                                        <span class=""glyphicon glyphicon-chevron-left"" aria-hidden=""true""></span>
                                        <span class=""sr-only"">Previous</span>
                                    </a>
                                    <a class=""right carousel-control"" href=""#carousel-example-generic"" role=""button"" data-slide=""next"">
                                        <span class=""glyphicon glyphicon-chevron-right"" aria-hidden=""true""></span>
                                        <span class=""sr-only"">Next</span>
                                    </a>
                                </div>";

//        public string carousel = @"<div class=""owl-carousel buttons-autohide controlls-over"" data-plugin-options='{""singleItem"": true, ""autoPlay"": true, ""navigation"": true, ""pagination"": true, ""transitionStyle"":""fade""}'>
//                                     {0}
//                                   </div>";


        public string novidades = @"<div class=""alert alert-mini alert-primary margin-bottom-30"">
                                        <strong>NOVIDADES:</strong>
                                        <div class=""owl-carousel controlls-over nomargin"" data-plugin-options='{""autoPlay"":3000, ""stopOnHover"":true, ""items"": 1, ""singleItem"": true, ""navigation"": false, ""pagination"": false, ""transitionStyle"":""fadeUp""}'>
                                            <div class=""text-left size-14"">
                                                <a href=""#"">1/3 Potential for the contamination of forensic DNA evidence has been highlighted by the Meredith Kercher murder trial.</a>
                                            </div>
                                            <div class=""text-left size-14"">
                                                <a href=""#"">2/3 Australia thrash England to win T20 series in Melbourne</a>
                                            </div>
                                            <div class=""text-left size-14"">
                                                <a href=""#"">3/3 China's bulldozer mayor kicked out of party, handed to prosecutors</a>
                                            </div>
                                        </div>
                                    </div>";

        //
        // GET: /Funcionalidade/
        [Autorizar(IsAjax = true)]
        public void PreencherGrids()
        {
            IList<TabDinamicaConfigDTO> ListaTabRef = _tabDinConfigSRV.ListarTabDinamica(null, null, 1);
           
            ViewBag.listaproduto = new SelectList(new ProdutosSRV().BuscarPorTipoProduto(9), "PRO_ID", "PRO_SIGLA");
            ViewBag.ListaTabRef = new SelectList(ListaTabRef, "TDC_ID", "TDC_NOME_TABELA");
        }

        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            return View();
        }
        [Autorizar(IsAjax = true)]
        public ActionResult Pesquisar(string _descricao, int _pagina = 1, int _registroPorPagina = 20)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listafuncionalidade = new FuncionalidadeSRV().ListarFuncionalidades(_descricao, _pagina, _registroPorPagina);

                if (listafuncionalidade == null)
                    throw new Exception("Nenhum registro encontrado para a consulta");

                response.AddPage("listafuncionalidade", listafuncionalidade);
                response.success = true;
                response.message = Message.Info("Ok");

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
        [Autorizar(IsAjax = true)]
        public ActionResult ListarFuncionalidades(int? _origem)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var listafuncionalidade = new FuncionalidadeSRV().ListarFuncionalidadesNaoSelect(_origem); 

                if (listafuncionalidade == null)
                    throw new Exception("Nenhum registro encontrado para a consulta");

                response.Add("listafuncionalidade", listafuncionalidade);
                response.success = true;
                response.message = Message.Info("Ok");

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

        [Autorizar(Acesso = "Editar")]
        public ActionResult Editar(int? _fci_id)
        {
            ViewBag.Title = " Funcionalidade (Editar) ";

            FuncionalidadeDTO _func = new FuncionalidadeDTO();

            if (_fci_id == null)
                ViewBag.Title = " Funcionalidade (Novo)";
            else 
                _func = new FuncionalidadeSRV().FindById(_fci_id);

            PreencherGrids();

            ViewBag.fci_id = _fci_id;

            FuncionalidadeDTO func = new FuncionalidadeSRV().FindById(_fci_id);

            return View(func);
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult BuscarFuncionalidade(int? _fci_id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var _funcionalidade = new FuncionalidadeSRV().FindById(_fci_id);

                response.success = true;
                response.Add("funcionalidade", _funcionalidade);
                return Json(response);

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
        [Autorizar(Acesso = "Editar")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Salvar(FuncionalidadeDTO funcatu
                                  ,HttpPostedFileBase uploadFile
                                  ,HttpPostedFileBase FCI_IMG01
                                  ,HttpPostedFileBase FCI_IMG02
                                  ,HttpPostedFileBase FCI_IMG03
                                  ,HttpPostedFileBase FCI_IMG04)
        {
            try
            {
                string _nomearquivo = null;
                string _filePath = null;
                string _conteudo = null;

           //   FuncionalidadeDTO _func = new FuncionalidadeSRV().FindById(funcatu.FCI_ID);
 
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    _nomearquivo = uploadFile.FileName;
                    _filePath = Path.Combine(HttpContext.Server.MapPath("~/Portal/Images/"), Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(_filePath);
                    funcatu.FCI_CONTEUDO = "<img src='http://corp.coad.com.br/Portal/Images/" + _nomearquivo + "' alt='" + funcatu.FCI_DESCRICAO + "' style=width:304px;height:228px;>";
                    funcatu.FCI_URL = "http://corp.coad.com.br/Portal/Images/" + _nomearquivo;
                }
            

                if (FCI_IMG01 != null && FCI_IMG01.ContentLength > 0)
                {
                    _nomearquivo = FCI_IMG01.FileName;
                    _filePath = Path.Combine(HttpContext.Server.MapPath("~/Portal/Images/"), Path.GetFileName(FCI_IMG01.FileName));
                    FCI_IMG01.SaveAs(_filePath);
                    funcatu.FCI_IMG01 = "http://corp.coad.com.br/Portal/Images/" + _nomearquivo;
                }

                if (FCI_IMG02 != null && FCI_IMG02.ContentLength > 0)
                {
                    _nomearquivo = FCI_IMG02.FileName;
                    _filePath = Path.Combine(HttpContext.Server.MapPath("~/Portal/Images/"), Path.GetFileName(FCI_IMG02.FileName));
                    FCI_IMG02.SaveAs(_filePath);
                    funcatu.FCI_IMG02 = "http://corp.coad.com.br/Portal/Images/" + _nomearquivo;
                }

                if (FCI_IMG03 != null && FCI_IMG03.ContentLength > 0)
                {
                    _nomearquivo = FCI_IMG03.FileName;
                    _filePath = Path.Combine(HttpContext.Server.MapPath("~/Portal/Images/"), Path.GetFileName(FCI_IMG03.FileName));
                    FCI_IMG03.SaveAs(_filePath);
                    funcatu.FCI_IMG03 = "http://corp.coad.com.br/Portal/Images/" + _nomearquivo;
                }

                if (FCI_IMG04 != null && FCI_IMG04.ContentLength > 0)
                {
                    _nomearquivo = FCI_IMG04.FileName;
                    _filePath = Path.Combine(HttpContext.Server.MapPath("~/Portal/Images/"), Path.GetFileName(FCI_IMG04.FileName));
                    FCI_IMG04.SaveAs(_filePath);
                    funcatu.FCI_IMG04 = "http://corp.coad.com.br/Portal/Images/" + _nomearquivo;
                }

                if (funcatu.FCI_TIPO == "CAR")
                {
                    if (!String.IsNullOrWhiteSpace(funcatu.FCI_IMG01))
                    //_conteudo += @"<a href=""#""><img class=""img-responsive"" src='" + funcatu.FCI_IMG01 + "'></a>";
                       _conteudo += @"<div class='item active'><img src='" + funcatu.FCI_IMG01 + "' style='margin: auto; height: 300px; width: 200px;' /></div>";
                    if (!String.IsNullOrWhiteSpace(funcatu.FCI_IMG02))
                       _conteudo += @"<div class='item active'><img src='" + funcatu.FCI_IMG02 + "' style='margin: auto; height: 300px; width: 200px;' /></div>";
                    //_conteudo += @"<a href=""#""><img class=""img-responsive"" src='" + funcatu.FCI_IMG02 + "'></a>";
                    if (!String.IsNullOrWhiteSpace(funcatu.FCI_IMG03))
                       _conteudo += @"<div class='item active'><img src='" + funcatu.FCI_IMG03 + "' style='margin: auto; height: 300px; width: 200px;' /></div>";
                        //_conteudo += @"<a href=""#""><img class=""img-responsive"" src='" + funcatu.FCI_IMG03 + "'></a>";
                    if (!String.IsNullOrWhiteSpace(funcatu.FCI_IMG04))
                       _conteudo += @"<div class='item active'><img src='" + funcatu.FCI_IMG04 + "' style='margin: auto; height: 300px; width: 200px;' /></div>";
                    //_conteudo += @"<a href=""#""><img class=""img-responsive"" src='" + funcatu.FCI_IMG04 + "'></a>";

                    funcatu.FCI_CONTEUDO = carousel;
                    funcatu.FCI_CONTEUDO = funcatu.FCI_CONTEUDO.Replace("{0}", _conteudo);
                }

                if (funcatu.FCI_TIPO == "NOV")
                    funcatu.FCI_CONTEUDO = novidades;

                funcatu.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
                funcatu.FCI_DATA_ALTERA = DateTime.Now;
     
                funcatu = new FuncionalidadeSRV().SaveOrUpdate(funcatu);
     
                TempData.Add("Resultado", "Operação realizada com sucesso!!");

                this.PreencherGrids();

                ViewBag.fci_id = funcatu.FCI_ID;

                return View("Editar", funcatu);

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);

                TempData.Add("Resultado", SysException.Show(ex));

                this.PreencherGrids();

                return View("Editar");
            }
        }


        [Autorizar(Acesso = "Excluir")]
        public ActionResult Excluir(FuncionalidadeDTO func)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                new FuncionalidadeSRV().Delete(func, "FCI_ID");

                result.success = true;
                result.message = Message.Info("Operação realizada com sucesso !!");

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                SysException.RegistrarLog(SysException.Show(ex), "", SessionContext.autenticado);

                result.success = false;
                result.SetMessageFromModelState(ModelState);
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

    }
}
