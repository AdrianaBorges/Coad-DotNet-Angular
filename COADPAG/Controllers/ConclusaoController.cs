using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADPAG.Controllers
{
    public class ConclusaoController : Controller
    {
        
    
        public ActionResult Index(string _nome, int? _formapgto)
        {
            ViewBag.TelaTopo = "CONCLUSÃO";
            ViewBag.Tela = "CONCLUSÃO";
            @ViewBag.Nome = _nome;
            @ViewBag.Boleto = (_formapgto == 1) ? true : false;
            
            return View();
        }

    }
}
