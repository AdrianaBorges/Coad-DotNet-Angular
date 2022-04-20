using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class ProdutoComposicaoItemController : Controller
    {

        public ProdutoComposicaoItemSRV _service = new ProdutoComposicaoItemSRV();

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Validar(ProdutoComposicaoItemDTO composicaoItem)
        {
            JSONResponse response = new JSONResponse();

            if (!ModelState.IsValid)
            {
                response.SetMessageFromModelState(ModelState);
                response.success = false;               
            }

            try
            {
                _service.ChecaDuplicidade(composicaoItem);
            }
            catch (ValidationException e)
            {
                var msg = new List<string>() { e.Message };
            }
            
            return Json(response);
        }

    }
}
