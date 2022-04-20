using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service;
using COADCORP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Areas.franquia.Controllers
{
    public class InfoMarketingController : Controller
    {
        private InfoMarketingSRV _service = new InfoMarketingSRV();
        private OrigemCadastroSRV _origemService = new OrigemCadastroSRV();
        private AreasSRV _areas = new AreasSRV();
        private ProdutoComposicaoSRV _composicaoService = new ProdutoComposicaoSRV();

        //
        // GET: /franquia/InfoMarketing/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarOrigensCadastro(){

            JSONResponse response = new JSONResponse();

            try
            {
                var lstOrigemCadastro = _origemService.FindAll();
                response.Add("lstOrigemCadastro", lstOrigemCadastro); 
            }
            catch(Exception e){

                response.message = Message.Fail(e);
            }

            return Json(response); 

        }

        public ActionResult ListarAreas()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstAreas = _areas.FindAll();
                response.Add("lstAreas", lstAreas);
            }
            catch (Exception e)
            {

                response.message = Message.Fail(e);
            }

            return Json(response);
        }

        public ActionResult ListarProdutosDeInteresse()
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var lstProdutosInteresse = _composicaoService.ProdutosDeInteresse();
                response.Add("lstProdutosInteresse", lstProdutosInteresse);
            }
            catch (Exception e)
            {

                response.message = Message.Fail(e);
            }

            return Json(response);
        }

    }
}
