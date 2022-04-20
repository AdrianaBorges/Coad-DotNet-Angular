using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coad.GenericCrud.ActionResultUtils;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Service.CalendarioObrigacoes;
using COAD.PORTAL.Utils;
using COAD.SEGURANCA.Filter;

namespace COADMOBILE.Controllers
{
    public class CalendarioController : Controller
    {
        //
        // GET: /Calendario/

        private CoCalendarioSRV _service = new CoCalendarioSRV();
        private CoAreasSRV _serviceAreas = new CoAreasSRV();
        private CoEstadosSRV _serviceEstados = new CoEstadosSRV();
        private CoMunicipiosSRV _serviceMunicipios = new CoMunicipiosSRV();

        [Autorizar(PorMenu = false)]
        public ActionResult Buscar(string data)
        {
            try
            {
                UtilsPortal uc = new UtilsPortal();
                ViewBag.Abrangencia = uc.AbrangenciaObrigacoes("");
            }
            catch (Exception e)
            {

            }
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult ObrigacoesJson(string data, string abrangencia, string area, string estado, string municipio,  int pagina = 1, int nLinha = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                //var lstObrigacoes = _service.CalendarioAtual(pagina,nLinha);
                var lstObrigacoes = _service.CalendarioFiltro(data, abrangencia, area, estado, municipio, pagina, nLinha);
                response.AddPage("obrigacoes", lstObrigacoes);

                var calendarioPorData = _service.CalendarioPorData(data);

                var lstAreas = _serviceAreas.AreasCalendario(calendarioPorData);
                response.Add("areas", lstAreas);

                var lstEstados = _serviceEstados.EstadosCalendario(calendarioPorData);
                response.Add("estados", lstEstados);

                var lstMunicipios = _serviceMunicipios.MunicipiosCalendario(calendarioPorData);
                response.Add("municipios", lstMunicipios);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Detalhes(int id)
        {
            CoCalendarioDTO obrigacao = null;
            try
            {
                obrigacao = _service.CalendarioPorIDObrigacao(id);
            }
            catch (Exception e)
            {

            }
            return View(obrigacao);
        }

    }
}
