using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace CONFIGSIS.Controllers
{
    public class EmpresaController : Controller
    {
        public ActionResult Index(string _razao_social)
        {
            try
            {
                ViewBag.Title = "Empresa (Consultar)";

                List<EmpresaModel> _empresa = new EmpresaSRV().Listar(_razao_social);

                if (_empresa.Count == 0)
                    throw new Exception("Nenhum resultado encontrado para a consulta.");

                return View(_empresa);
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Empresa (Consultar)";

                ModelState.AddModelError("Informação", ex.Message);

                List<EmpresaModel> _empresa = new EmpresaSRV().Listar(_razao_social);

                return View("Index");
            }

        }

        public ActionResult Novo()
        {
            ViewBag.Title = "Empresa (Novo)";

            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        public ActionResult Novo(EmpresaModel empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new EmpresaSRV().IncluirReg(empresa);
                    
                    TempData.Add("Resultado", "Inclusão realizada com sucesso.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Title = "Empresa (Novo)";

                    ModelState.AddModelError("error", SysException.Show(ex));

                    return View("Novo");
                }
            }

            return View("Novo");
        }
        public ActionResult Excluir(int _emp_id)
        {
            ViewBag.Title = "Confirma a exclusão desta empresa ?";

            EmpresaModel empresa = new EmpresaSRV().Buscar(_emp_id);

            return View(empresa);
        }

        [HttpPost]
        public ActionResult Excluir(EmpresaModel empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new EmpresaSRV().ExcluirReg(empresa);
                    
                    TempData.Add("Resultado", "Exclusão realizada com sucesso.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Title = "Confirma a exclusão desta empresa ?";

                    ModelState.AddModelError("error", SysException.Show(ex));

                    return View("Excluir");
                }
            }

            return View("Excluir");

        }

        public ActionResult Editar(int _emp_id)
        {
            ViewBag.Title = "Empresa (Editar)";

            EmpresaModel empresa = new EmpresaSRV().Buscar(_emp_id);

            return View(empresa);
        }


        [HttpPost]
        public ActionResult Editar(EmpresaModel empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new EmpresaSRV().SalvarReg(empresa);
                    
                    TempData.Add("Resultado", "Alteração realizada com sucesso.");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Title = "Empresa (Editar)";

                    ModelState.AddModelError("error", SysException.Show(ex));

                    return View("Editar");
                }
            }

            return View("Editar");
        }

    }
}
