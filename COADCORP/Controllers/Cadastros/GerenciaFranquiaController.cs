using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using GenericCrud.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class GerenciaFranquiaController : GenericController<FRANQUIA, FranquiaDTO, int>
    {
        private FranquiaSRV franquiaSRV { get; set; }

        public GerenciaFranquiaController() : base()
        {

        }

        public GerenciaFranquiaController(FranquiaSRV franquiaSRV) : base(franquiaSRV)
        {
            this.franquiaSRV = franquiaSRV;
        }
        //
        // GET: /GerenciaFranquia/

        public ActionResult Index()
        {
            return View();
        }

    }
}
