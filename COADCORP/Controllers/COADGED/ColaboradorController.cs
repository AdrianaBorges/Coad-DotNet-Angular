using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.COADGED.Service;
using Coad.GenericCrud.ActionResultTools;
using COAD.SEGURANCA.Filter;
using COAD.COADGED.Model.DTO;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;

namespace COADCORP.Controllers.COADGED
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class ColaboradorController : Controller
    {
        private ColaboradorSRV _service = new ColaboradorSRV();
        private CargosSRV _serviceCargos = new CargosSRV();
        private AreasSRV _serviceAreas = new AreasSRV();
        private UsuarioSRV _serviceUsuario = new UsuarioSRV();

        [Autorizar]
        public ActionResult Index()
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");
            
            
            // cargos = "Redator", "..."...................................................
            var cargos = _serviceCargos.FindAll();
            ViewBag.cargos = cargos.Select(c => new SelectListItem() { Text = c.CRG_DESCRICAO, Value = c.CRG_ID.ToString() });

            // colecionador...................................................
            var colecionador = _serviceAreas.FindAll();
            ViewBag.colecionador = colecionador.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            return View();
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Colaboradores(int? colaboradorId, string nome, int ativoId = 1, int? cargoId = null, int? colecionadorId = null, int pagina = 0)
        {
            Pagina<ColaboradorDTO> page = _service.Colaboradores(colaboradorId, nome: nome, ativo: ativoId, cargoId: cargoId, colecionadorId: colecionadorId, pagina: pagina, itensPorPagina: 7);

            JSONResponse response = new JSONResponse();
            response.AddPage("colaboradores", page);

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        public ActionResult Novo()
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // cargos = "Redator", "..."...................................................
            var cargos = _serviceCargos.FindAll();
            ViewBag.cargos = cargos.Select(c => new SelectListItem() { Text = c.CRG_DESCRICAO, Value = c.CRG_ID.ToString() });

            // colecionador...................................................
            var colecionador = _serviceAreas.FindAll();
            ViewBag.colecionador = colecionador.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            //ViewBag.Usuario = u;
            //ViewBag.Perfil = p;
            //ViewBag.UsuariosPorPerfil = pu.Select(c => new SelectListItem() { Text = c.USU_LOGIN, Value = c.perId });

            return View("Editar");
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Editar(int colaboradorId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // cargos = "Redator", "..."...................................................
            var cargos = _serviceCargos.FindAll();
            ViewBag.cargos = cargos.Select(c => new SelectListItem() { Text = c.CRG_DESCRICAO, Value = c.CRG_ID.ToString() });

            // colecionador...................................................
            var colecionador = _serviceAreas.FindAll();
            ViewBag.colecionador = colecionador.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            ViewBag.colaboradorId = colaboradorId;

            //ViewBag.usuario = u;
            //ViewBag.nivelRepresentante = p;

            return View("Editar");
        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Salvar(ColaboradorDTO colaborador)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = _serviceUsuario.Buscar(colaborador.USU_LOGIN);
                    if (usuario == null)
                    {
                        result.success = false;
                        result.message = Message.Warning("ATENÇÃO! Este Login de Usuário não consta no cadastro do COADCORP! Por favor, corrija antes de salvar.");
                    }
                    else
                    {
                        _service.SalvarColaborador(colaborador);
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
                result.message = Message.Warning(SysException.Show(ex) + " ATENÇÃO! Este Login de Usuário não consta no cadastro do COADCORP! Por favor, corrija antes de salvar.");
                return Json(result);
            }
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Remover(int cargoId)
        {

            TempData["message"] = "A remoção deste registro não é permitida!";
            return RedirectToAction("Index");

        }

        [Autorizar(IsAjax = true)]
        [HttpPost]
        public ActionResult Readcolaborador(int colaboradorId)
        {
            var colaborador = _service.FindById(colaboradorId);
            JSONResponse response = new JSONResponse();
            response.Add("colaborador", colaborador);

            return Json(response);
        }

        public ActionResult checaUsuario(string login)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var usuario = _serviceUsuario.Buscar(login);
                response.success = true;
                response.message = null;

                if (usuario == null)
                {
                    response.message = Message.Warning("ATENÇÃO! Este Login de Usuário não consta no cadastro do COADCORP!");
                    response.success = false;
                }

                response.Add("usuario", usuario);
            }
            catch (Exception ex)
            {
                
                response.message = Message.Warning(SysException.Show(ex) + "ATENÇÃO! Este Login de Usuário não consta no cadastro do COADCORP!");
                response.success = false;
            }

            return Json(response);
        }

        [Autorizar(PorMenu = false)]
        [HttpPost]
        public ActionResult Detalhes(int colaboradorId)
        {
            // ativo = "Sim" ou "Não".....................................................
            List<SelectListItem> ativo = new List<SelectListItem>();
            ativo.AddRange(new[]{
                           new SelectListItem() { Text = "Sim", Value = "1" },
                           new SelectListItem() { Text = "Não", Value = "0" }
            });
            ViewBag.ativo = new SelectList(ativo, "Value", "Text");

            // cargos = "Redator", "..."...................................................
            var cargos = _serviceCargos.FindAll();
            ViewBag.cargos = cargos.Select(c => new SelectListItem() { Text = c.CRG_DESCRICAO, Value = c.CRG_ID.ToString() });

            // colecionador...................................................
            var colecionador = _serviceAreas.FindAll();
            ViewBag.colecionador = colecionador.Select(c => new SelectListItem() { Text = c.ARE_CONS_DESCRICAO, Value = c.ARE_CONS_ID.ToString() });

            ViewBag.colaboradorId = colaboradorId;

            return View("Detalhes");
        }
    }
}