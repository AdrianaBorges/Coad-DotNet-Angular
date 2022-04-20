using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Filter;
using System.Collections.Generic;
using COAD.PORTAL.Service.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using Coad.GenericCrud.ActionResultTools;

namespace COADMOBILE.Controllers
{
    public class NoticiasController : Controller
    {
        private VW_NoticiasSRV _serviceNoticias = new VW_NoticiasSRV();

        [Autorizar(PorMenu=false)]
        public ActionResult Busca()
        {
            return View();
        }

        [Autorizar(PorMenu = false)]
        public ActionResult BuscaJson(string titulo = "", string texto = "", string descricao = "", int pagina = 1, int nLinha = 7)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstNoticias = _serviceNoticias.NoticiasFiltro(titulo, texto, descricao, pagina, nLinha);
                response.AddPage("noticias", lstNoticias);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu=false)]
        public ActionResult Detalhes(int id)
        {
            VW_NoticiasDTO noticias = null;
            try
            {
                noticias = _serviceNoticias.BuscarNoticiaPorId(id);
            }
            catch (Exception e)
            {

            }
            return View(noticias);
        }
    }
}
