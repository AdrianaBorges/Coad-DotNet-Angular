using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADAGENDA.Controllers
{
    public class EnderecoController : Controller
    {
        MunicipioSRV _service = new MunicipioSRV();
        ClienteEnderecoSRV _enderecoSRV = new ClienteEnderecoSRV();
        //
        // GET: /Endereco/
        [Autorizar(PorMenu = true)]
        public ActionResult Index()
        {
            return View();
        }

        [Autorizar(IsAjax = true)]
        public ActionResult MunicipiosPorUf(string uf)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var municipios = _service.BuscarPorUF(uf);
                response.Add("municipios", municipios);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult ValidarEndereco(ClienteEnderecoDto end)
        {

            JSONResponse response = new JSONResponse();

            try
            {
                //if(end != null && end.CLI_ID != null && end.END_TIPO != null)
                //{
                //    if (!_enderecoSRV.HasEndereco((int) end.CLI_ID, (int) end.END_TIPO))
                //    {
                //        response.success = false;
                //        response.message = Message.Fail("Já existe esse tipo de endereço.");
                //    }
                //}
                if (!ModelState.IsValid)
                {
                    response.success = false;
                    response.SetMessageFromModelState(ModelState);
                }
                
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        [Autorizar(IsAjax = true)]
        public ActionResult CombosEndereco()
        {

            JSONResponse response = new JSONResponse();

            try
            {
                var tipoLogradouro = new TipoLogradouroSRV().FindAll();
                var tipoEnderecos = new TipoEnderecoSRV().FindAll();

                response.Add("tipoLogradouro", tipoLogradouro);
                response.Add("tipoEnderecos", tipoEnderecos);

            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }

            return Json(response);
        }



    }
}
