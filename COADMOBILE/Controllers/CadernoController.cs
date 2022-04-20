using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using COAD.COADGED.Service;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using System.Collections.Generic;
using Coad.GenericCrud.ActionResultTools;

namespace COADMOBILE.Controllers
{
    public class CadernoController : Controller
    {
        CadernoSRV _cadernoSRV = new CadernoSRV();

        [Autorizar(PorMenu = false)]
        public ActionResult Index()
        {
            string userId = "0";
            if (Request.Cookies["InfClientes"] != null)
            {
                if (Request.Cookies["InfClientes"]["IdCliente"] != null)
                {
                    userId = Request.Cookies["InfClientes"]["IdCliente"];
                }
            }
            var cadernos = _cadernoSRV.BuscarCadernosCliente(int.Parse(userId));
            return View(cadernos.ToList());
        }

        [Autorizar(PorMenu = false)]
        [HttpGet]
        public ActionResult Caderno(int id)
        {
            CadernoDTO cdto = new CadernoDTO();
            try
            {
                string userId = "0";
                if (Request.Cookies["InfClientes"] != null)
                {
                    if (Request.Cookies["InfClientes"]["IdCliente"] != null)
                    {
                        userId = Request.Cookies["InfClientes"]["IdCliente"];
                    }
                }

                cdto = _cadernoSRV.FindById(id);

                if(cdto.CLI_ID != int.Parse(userId)){
                    cdto.CAD_ID = 0;
                    cdto.CLI_ID = 0;
                    cdto.CAD_NOME = "CADERNO NÂO ENCONTRADO";
                }
            }
            catch
            {
                cdto.CAD_ID = 0;
                cdto.CLI_ID = 0;
                cdto.CAD_NOME = "CADERNO NÂO ENCONTRADO";
            }
            return View(cdto);
        }

        [Autorizar(PorMenu = false)]
        [HttpGet]
        public ActionResult Adicionar(string titulocaderno)
        {
            string userId = "0";
            if (Request.Cookies["InfClientes"] != null)
            {
                if (Request.Cookies["InfClientes"]["IdCliente"] != null)
                {
                    userId = Request.Cookies["InfClientes"]["IdCliente"];
                }
            }

            IList<CadernoDTO> cadernos = new List<CadernoDTO>();

            try
            {
                var buscarCadRep = _cadernoSRV.BuscarCadernoRepetido(int.Parse(userId), titulocaderno);

                if (buscarCadRep == null)
                {
                    CadernoDTO cdto = new CadernoDTO();
                    cdto.CLI_ID = int.Parse(userId);
                    cdto.CAD_NOME = titulocaderno;
                    cdto.CAD_ATIVO = 0;
                    _cadernoSRV.Save(cdto);
                }

                cadernos = _cadernoSRV.BuscarCadernosCliente(int.Parse(userId));
            }
            catch
            {
                
            }

            return View("Index", cadernos.ToList());
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Renomear(string novoTitulo, int id = 0)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<CadernoDTO> cadernos = new List<CadernoDTO>();

                var buscarCad = _cadernoSRV.FindById(id);
                if (buscarCad != null)
                {
                    buscarCad.CAD_NOME = novoTitulo;
                    _cadernoSRV.Merge(buscarCad, "CAD_ID");
                }

                response.Add("ntitulo", novoTitulo);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Excluir(int id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                IList<CadernoDTO> cadernos = new List<CadernoDTO>();

                var buscarCad = _cadernoSRV.FindById(id);
                if (buscarCad != null)
                {
                    _cadernoSRV.Delete(buscarCad, "CAD_ID");
                }

                response.Add("ntitulo", "Excluido");
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Delete(int id)
        {
            return View();
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
