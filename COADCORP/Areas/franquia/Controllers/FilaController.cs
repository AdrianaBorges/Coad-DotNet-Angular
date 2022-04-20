using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.SessionUtils;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    public class FilaController : Controller
    {

        private FilaCadastroSRV _service = new FilaCadastroSRV();
        //
        // GET: /franquia/Fila/

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        public ActionResult ListarPorRegiao(int? RG_ID, string nomeRepresentante, int pagina = 1, int registroPorPagina = 50)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                if (SessionContext.PossuiRepresentante())
                {
                    Pagina<FilaCadastroDTO> lstFila = null;

                    if (SessionContext.IsGerenteDepartamento("Franquiado"))
                    {
                        if (RG_ID == null)
                        {
                            RG_ID = SessionUtil.GetRegiao();
                        }
                        lstFila = _service.FindByRegiao(RG_ID, DateTime.Now, nomeRepresentante, pagina, registroPorPagina);
                    }
                    else if (SessionContext.IsGerenteDepartamento("Franquiador", true) || SessionContext.IsGerenteDepartamento("TI", true))
                    {
                        var uenId = SessionUtil.GetUenId();
                        lstFila = _service.FindByData(DateTime.Now, nomeRepresentante, pagina, registroPorPagina, uenId);
                    }
                    
                    response.AddPage("lstFila", lstFila);
                }
                else
                {
                    response.success = false;
                    response.message = Message.Fail(@"O usuário logado no sistema precisa ser um login de representante, ou precisar estar relacionado a um representante.");
                }
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true, GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        [HttpPost]
        public ActionResult MoverFila(int? FLC_ID, string tipo)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if(tipo == "cima"){
                    _service.MoverFilaParaCima(FLC_ID);
                }
                else if(tipo == "baixo"){
                    _service.MoverFilaParaBaixo(FLC_ID);
                }
                
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [Autorizar(IsAjax = true, GerenteDepartamento = "Franquiado, Franquiador, TI", PermitirNiveisPrivilegiosSuperiores = true)]
        [HttpPost]
        public ActionResult RemoverFila(int? FLC_ID)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                _service.RemoverDaFila(FLC_ID);
            }
            catch (Exception ex)
            {
                SessionUtil.HandleException(ex);

                result.success = false;
                result.message = Message.Fail(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
