using System;
using System.Web;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using Coad.GenericCrud.Repositorios.Base;
using COAD.UTIL.Grafico;
using Coad.GenericCrud.ActionResultTools;
//using System.Web.UI.DataVisualization.Charting;
using COAD.COADGED.Service;
using COAD.COADGED.Model.DTO;
using COAD.CORPORATIVO.SessionUtils;
using GenericCrud.Util;
using GenericCrud.Service;
using COAD.SEGURANCA.Service.Interfaces;
using COAD.SEGURANCA.Config.Email;
using COAD.CORPORATIVO.DAO;

namespace SCHEDULER.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
