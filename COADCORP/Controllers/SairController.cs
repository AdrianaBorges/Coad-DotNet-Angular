using System;
using System.Web;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Util;
using COAD.CORPORATIVO.Service;

namespace COADCORP.Controllers
{
    public class SairController : Controller
    {
        //
        // GET: /Sair/
        private RepresentanteSRV _representanteSRV = new RepresentanteSRV();
        private FilaCadastroSRV _filaCadastroSRV = new FilaCadastroSRV();

        public ActionResult Index()
        {
            if (SessionContext.autenticado != null)
            {
                SysException.RegistrarLog("LogOff Usuário (" + SessionContext.autenticado.USU_LOGIN + ")", "", SessionContext.autenticado);

                int? REP_ID = null;

                if (AuthUtil.TryGetRepId(out REP_ID))
                {
                    var representante = _representanteSRV.FindById(REP_ID);

                    if (representante != null && representante.RG_ID != null)
                    {
                        var regiaoId = representante.RG_ID;
                        _filaCadastroSRV.RemoverFilaCadastro(regiaoId, REP_ID);
                    }
                }
            }

            SessionContext.RemoveSession(System.Web.HttpContext.Current);
            
            return RedirectToAction("Login", "Login");
        }

    }
}
