using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Model;
using COAD.CORPORATIVO.Model.Dto;
using System.Transactions;

namespace COADCORP.Controllers.Ocorrencia
{
    public class OcorrenciaRemessaController : Controller
    {
        private OcorrenciaRemessaSRV _service = new OcorrenciaRemessaSRV();
        private BancosSRV _serviceBanco = new BancosSRV();

        [Autorizar(PorMenu = false, IsAjax = true)]
        public ActionResult Index()
        {
            ViewBag.bco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString()+" - "+c.BAN_NOME, Value = c.BAN_ID.ToString() });
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Listar(string bco = null, string cod = null, string rem = null, int pagina = 0)
        {
            Pagina<OcorrenciaRemessaDTO> page = _service.LerOcorrenciaRemessa(bco, cod, rem, pagina, 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("ocorrencia", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            ViewBag.cod = "";
            ViewBag.bco = "";
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(string bco, string cod)
        {
            ViewBag.cod = cod;
            ViewBag.bco = bco;
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(OcorrenciaRemessaDTO ocorrencia)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        _service.SalvarOcorrenciaRemessa(ocorrencia);
                        scope.Complete();
                    }
                    return Json(result);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.message = Message.Warning(ex.ToString().Substring(0,ex.ToString().IndexOf("\r\n")));
                
                int i = result.message.message.IndexOf(':') + 2;
                int f = result.message.message.Length - i;
                
                result.message.message = result.message.message.Substring(i, f);

                return Json(result);
            }
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult ReadOcorrencia(string bco, string cod)
        {
            bco = bco.PadLeft(3,'0');
            cod = cod.PadLeft(2,'0');

            var ocorrencia = _service.FindById(bco, cod);
            
            JSONResponse response = new JSONResponse();
            response.Add("ocorrencia", ocorrencia);

            return Json(response);
        }
    }
}