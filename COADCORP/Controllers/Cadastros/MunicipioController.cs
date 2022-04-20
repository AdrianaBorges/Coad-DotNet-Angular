using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADCORP.Controllers.Cadastros
{
    public class MunicipioController : Controller
    {
        public ActionResult BuscarMunicipio(string _nomemunicipio)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                List<MunicipioDTO> _cidade = new MunicipioSRV().Buscar(_nomemunicipio);

                response.Add("dbMunicipio", _cidade);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }
         
        }

        public ActionResult BuscarMunicipioEUF(string _nomemunicipio, string uf)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<MunicipioDTO> _cidade = new MunicipioSRV().BuscarPorDescricaoEUF(_nomemunicipio, uf);

                response.Add("dbMunicipio", _cidade);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult BuscarBairro(string _bairrodescricao)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<CepBairroDTO> _listaBairro = new CepBairroSRV().Buscar(_bairrodescricao);

                response.Add("listaBairro", _listaBairro);
                response.success = true;

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Salvar(MunicipioDTO _municipio)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                new MunicipioSRV().Merge(_municipio);
                response.success = true;
                response.message = Message.Info("Município atualizado com sucesso !!");

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(SysException.Show(e));

                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
