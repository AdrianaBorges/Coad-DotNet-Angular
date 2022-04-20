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
    public class OcorrenciaErroController : Controller
    {
        private OcorrenciaErroSRV _service = new OcorrenciaErroSRV();
        private OcorrenciaRetornoSRV _serviceRetorno = new OcorrenciaRetornoSRV();
        private BancosSRV _serviceBanco = new BancosSRV();

        [Autorizar(PorMenu=false)]
        public ActionResult Index()
        {
            ViewBag.bco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString()+" - "+c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.cbRet = _serviceRetorno.LerOcorrenciaRetorno().lista.OrderBy(x => x.BAN_ID).ThenBy(x => x.OCT_CODIGO).
                Select(c => new SelectListItem() { Text = "(" + c.BAN_ID.ToString() + ") " + c.OCT_CODIGO.ToString() + " - " + c.OCT_DESCRICAO, Value = c.OCT_CODIGO.ToString() });
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Listar(string bco = null, string cod = null, string codRet = null, int pagina = 0)
        {
            Pagina<OcorrenciaErroDTO> page = _service.LerOcorrenciaErro(bco, cod, codRet, pagina, 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("ocorrencia", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            ViewBag.cod = "";
            ViewBag.bco = "";
            ViewBag.codRet = "";
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.cbRet = _serviceRetorno.LerOcorrenciaRetorno().lista.OrderBy(x => x.BAN_ID).ThenBy(x => x.OCT_CODIGO).
                Select(c => new SelectListItem() { Text = "(" + c.BAN_ID.ToString() + ") " + c.OCT_CODIGO.ToString() + " - " + c.OCT_DESCRICAO, Value = c.OCT_CODIGO.ToString() });
            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(string bco, string cod, string codRet)
        {
            ViewBag.cod = cod;
            ViewBag.bco = bco;
            ViewBag.codRet = codRet;
            ViewBag.banco = _serviceBanco.BuscarTodos().OrderBy(x => x.BAN_ID).Select(c => new SelectListItem() { Text = c.BAN_ID.ToString() + " - " + c.BAN_NOME, Value = c.BAN_ID.ToString() });
            ViewBag.cbRet = _serviceRetorno.LerOcorrenciaRetorno().lista.OrderBy(x => x.BAN_ID).ThenBy(x => x.OCT_CODIGO).
                Select(c => new SelectListItem() { Text = "(" + c.BAN_ID.ToString() + ") " + c.OCT_CODIGO.ToString() + " - " + c.OCT_DESCRICAO, Value = c.OCT_CODIGO.ToString() });
            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(OcorrenciaErroDTO ocorrencia)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        _service.SalvarOcorrenciaErro(ocorrencia);
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
        public ActionResult ReadOcorrencia(string bco, string cod, string codRet)
        {
            if (bco.Where(c => char.IsLetter(c)).Count() == 0) // se não contém letra
                bco = bco.PadLeft(3,'0');

            if (cod.Where(c => char.IsLetter(c)).Count() == 0)  // se não contém letra 
                cod = cod.PadLeft(2, '0');

            if (codRet.Where(c => char.IsLetter(c)).Count() == 0)  // se não contém letra
                codRet = codRet.PadLeft(2, '0');

            var ocorrencia = _service.FindById(bco, cod, codRet);
            
            JSONResponse response = new JSONResponse();
            response.Add("ocorrencia", ocorrencia);

            return Json(response);
        }
    }
}