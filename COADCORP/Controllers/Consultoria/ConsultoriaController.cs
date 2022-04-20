using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Service;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Service.PortalConsultoria;
using COAD.PORTAL.Utils;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Web.Mvc;

namespace COADCORP.Controllers.Consultoria
{
    [Autorizar(IsAjax = true)]
    public class ConsultoriaController : Controller
    {
        public ConsultoriaController() : base() { }
        public FonteDadosDescricaoSRV _fonteDadosDescricaoSRV { get; set; }
        public TemplateHTMLSRV _templateHTMLSRV { get; set; }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public ActionResult ConsultoriaEmail()
        {
            UtilsPortal uc = new UtilsPortal();
            var ufs = uc.UFs();
            //Limpa para o Angular encarar o primeiro item como selecionado
            ufs[0].Value = "";
            ViewBag.UFs = ufs;
            return View();
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public ActionResult Pesquisar(int _id = 0, string _codigo = "", string _status = "",
                string _perini = "", string _perfim = "", string _uf = "", int _pagina = 0)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                ConsultaEmailPerfilColecSRV ccep = new ConsultaEmailPerfilColecSRV();
                //Alterar para buscar na base de consultores pelo login
                var colec = ccep.BuscarColecionadorPorPerfil(SessionContext.autenticado.PER_ID);
                //Alterar para buscar na base de consultores pelo login
                ConsultoriaPortalSRV consultsrv = new ConsultoriaPortalSRV();
                if (colec != null)
                {
                    Pagina<ConsultoriaPortalDTO> _listaconsultas = consultsrv.BuscarConsultasPorPerfil(_id, _codigo, _status,
                        _perini, _perfim, _uf, colec.COLEC_ID, _pagina);
                    result.AddPage("listaconsulta", _listaconsultas);
                    result.success = true;
                    result.message = Message.Info("ok");
                }
                else
                {
                    result.success = false;
                    result.message = Message.Fail("Seu usuário não está devidamente cadastrado para este tipo de consulta.");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
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

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public ActionResult Editar(int _id)
        {
            ViewBag.IdConsulta = _id;
            return View();
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult BuscarConsulta(int _id)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                ConsultoriaPortalSRV consultsrv = new ConsultoriaPortalSRV();
                ConsultoresPortalSRV consultoressrv = new ConsultoresPortalSRV();
                ConsultaEmailColecionadorSRV cecsrv = new ConsultaEmailColecionadorSRV();
                var consulta = consultsrv.FindById(_id);
                var consultor = consultoressrv.FindById(consulta.cod_supervisor);
                int idUltimoAcesso = 0;
                if(int.TryParse(consulta.codFuncUltimoAcesso, out idUltimoAcesso))
                {

                }
                var consultorUltimoAcesso = consultoressrv.FindById(idUltimoAcesso);
                int colecionador = int.Parse(consulta.colec);
                var colec = cecsrv.FindById(colecionador);

                var consultoratual = consultoressrv.BuscarConsultorPorLoginEColecionador(SessionContext.autenticado.USU_LOGIN, colecionador.ToString());

                var exibirBtnSalvar = (consultoratual != null && consulta != null && !consulta.status.Equals("5") && (consultor == null || (consultor != null && consultor.usuario.Equals(SessionContext.autenticado.USU_LOGIN)))) ? true : false;
                response.Add("consulta", consulta);
                if (consultor != null)
                    response.Add("consultor", consultor.usuario);
                if (consultorUltimoAcesso != null)
                    response.Add("consultorUltimoAcesso", consultorUltimoAcesso.usuario);
                response.Add("dataCadastro", consulta.dataCadastro.ToString());
                response.Add("dataResposta", consulta.dataRespSupervisor.ToString());
                response.Add("dataUltimoAcesso", consulta.dataUltimoAcesso.ToString());
                response.Add("descricaocolecionador", colec.COLEC_DESCRICAO);
                response.Add("exibirBtnSalvar", exibirBtnSalvar);
                response.success = true;

                consulta.dataUltimoAcesso = DateTime.Now;
                consulta.codFuncUltimoAcesso = consultoratual.id.ToString();
                consultsrv.Merge(consulta);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult SalvarResposta(int _id, string _resposta)
        {
            ConsultoriaPortalSRV consultsrv = new ConsultoriaPortalSRV();
            ConsultoresPortalSRV consultoressrv = new ConsultoresPortalSRV();
            var consulta = consultsrv.FindById(_id);
            var consultor = consultoressrv.BuscarConsultorPorLogin(SessionContext.autenticado.USU_LOGIN);

            JSONResponse response = new JSONResponse();
            try
            {
                if (consultor != null)
                {
                    consulta.status = "4";
                    consulta.cod_supervisor = consultor.id;
                    consulta.resposta_supervisor = _resposta;
                    consulta.dataRespSupervisor = DateTime.Now;
                    consulta.dataUltimoAcesso = DateTime.Now;
                    consulta.codFuncUltimoAcesso = consultor.id.ToString();
                    consultsrv.Merge(consulta);

                    response.message = Message.Success("Resposta salva com sucesso.");
                    response.success = true;
                }
                else
                {
                    response.message = Message.Success("Seu login não permite nem salvar nem responder consultas deste tipo.");
                    response.success = false;
                }
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult SalvarEnviarResposta(int _id, string _resposta)
        {
            ConsultoriaPortalSRV consultsrv = new ConsultoriaPortalSRV();
            ConsultoresPortalSRV consultoressrv = new ConsultoresPortalSRV();
            ConsultaEmailPerfilColecSRV ccep = new ConsultaEmailPerfilColecSRV();

            var colec = ccep.BuscarColecionadorPorPerfil(SessionContext.autenticado.PER_ID);
            var consulta = consultsrv.FindById(_id);

            var consultor = consultoressrv.BuscarConsultorPorLoginEColecionador(SessionContext.autenticado.USU_LOGIN, colec.COLEC_ID.ToString());
            JSONResponse response = new JSONResponse();
            try
            {
                if (consultor != null)
                {
                    consultsrv.SalvarEnviarConsultasPorEmail(consultor, consulta, _resposta);
                    response.message = Message.Success("Resposta salva e enviada com sucesso.");
                    response.success = true;
                }
                else
                {
                    response.message = Message.Success("Seu login não permite nem salvar nem responder consultas deste tipo.");
                    response.success = false;
                }
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true, PorMenu = false)]
        public JsonResult ReenviarResposta(int _id)
        {
            ConsultoriaPortalSRV consultsrv = new ConsultoriaPortalSRV();
            ConsultoresPortalSRV consultoressrv = new ConsultoresPortalSRV();
            ConsultaEmailPerfilColecSRV ccep = new ConsultaEmailPerfilColecSRV();

            var colec = ccep.BuscarColecionadorPorPerfil(SessionContext.autenticado.PER_ID);
            var consulta = consultsrv.FindById(_id);

            var consultor = consultoressrv.BuscarConsultorPorLoginEColecionador(SessionContext.autenticado.USU_LOGIN, colec.COLEC_ID.ToString());
            JSONResponse response = new JSONResponse();
            try
            {
                if (consultor != null)
                {
                    consulta.dataUltimoAcesso = DateTime.Now;
                    consulta.codFuncUltimoAcesso = SessionContext.autenticado.USU_LOGIN;
                    consultsrv.Merge(consulta);
                    consultsrv.EnviarRespostaConsultoriaEmail(consulta);

                    response.message = Message.Success("Resposta reenviada com sucesso.");
                    response.success = true;
                }
                else
                {
                    response.message = Message.Success("Seu login não permite nem salvar nem responder consultas deste tipo.");
                    response.success = false;
                }
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
