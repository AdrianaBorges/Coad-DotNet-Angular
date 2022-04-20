using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Service.CalendarioObrigacoes;
using COAD.PORTAL.Service.PortalCoad;
using COAD.PORTAL.Service.SevicesPortalCoad;
using COAD.SEGURANCA.Filter;

namespace COADMOBILE.Controllers
{
    public class TributarioController : Controller
    {
        //
        // GET: /Tributario/
        private CoCalendarioSRV _serviceCalendario = new CoCalendarioSRV();
        private IndiceSRV _serviceIndices = new IndiceSRV();
        private VW_NoticiasSRV _serviceVWNoticias = new VW_NoticiasSRV();

        [Autorizar(PorMenu=false)]
        public ActionResult Index()
        {
            IList<CoCalendarioDTO> lstCalendario = null;
            try
            {
                lstCalendario = _serviceCalendario.CalendarioTop();
                ViewBag.IndicesEconomico = _serviceIndices.FindAll();
                ViewBag.Noticias = _serviceVWNoticias.NoticiasEmOrdemDescendente();
            }
            catch (Exception e)
            {

            }
            return View(lstCalendario);
        }
        

    }
}
