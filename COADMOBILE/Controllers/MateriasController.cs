using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.PORTAL.Utils;
using COAD.SEGURANCA.Filter;
using System.Collections.Generic;
using COAD.PORTAL.Service.PortalCoad;
using Coad.GenericCrud.ActionResultTools;
using COAD.PORTAL.Service.CalendarioObrigacoes;

namespace COADMOBILE.Controllers
{
    public class MateriasController : Controller
    {
        //
        // GET: /Materias/
        //private VerbetesSRV _serviceVerbetes = new VerbetesSRV();
        private Tab_31SRV _serviceTabTrintaUm = new Tab_31SRV();
        private MateriasSRV _serviceMaterias = new MateriasSRV();
        private MateriaSRV _serviceMateria = new MateriaSRV();

        [Autorizar(PorMenu = false)]
        public ActionResult Busca()
        {
            UtilsPortal uc = new UtilsPortal();
            ViewBag.Orientacoes = uc.Orientacoes("");
            ViewBag.AtosLegais = uc.AtosLegais("");
            ViewBag.Anos = uc.Ano("");
            ViewBag.BuscarPor = uc.TiposDeMateria();
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult MateriasAtoJson(string label, string tipo, string num_ato, string ano, int pagina = 1, int nLinha = 7)
        {
            JSONResponse response = new JSONResponse();
            try
            {
            var materias = _serviceMaterias.Materias(label, tipo, num_ato, ano, pagina, nLinha);
            response.AddPage("materiasAto", materias);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult MateriasOriJson(string label, string tipo, string num_ato, string ano, int pagina = 1, int nLinha = 7)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var materias = _serviceMaterias.Materias(label, tipo, num_ato, ano, pagina, nLinha);
                response.AddPage("materiasOri", materias);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Detalhes(string id)
        {
            var materia = _serviceMateria.Materia(id);
            return View(materia);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult TrazerItensOrientacao(string num_ato)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                //response.Add("itensArea", _serviceVerbetes.Verbetes(num_ato));
                response.Add("itensArea", _serviceTabTrintaUm.RecuperarVerbetesSemRepeticao(num_ato));
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
