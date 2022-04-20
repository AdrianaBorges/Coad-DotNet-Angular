using Coad.GenericCrud.ActionResultTools;
using COAD.CORPORATIVO.Service.Dp;
using COAD.RM.Service;
using COAD.SEGURANCA.Filter;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace COADCORP.Controllers.Dp
{
    // ALT: 30/09/2015 17h33m
    // Departamento Pessoal...

    [ValidateInput(false)]
    public class DpController : Controller
    {
        private DpSRV _dp = new DpSRV();

        public void CarregaListas()
        {
            // Período...
            List<SelectListItem> dpPeriodo = new List<SelectListItem>();
            dpPeriodo.AddRange(new[]{
                           new SelectListItem() { Text = "Prêmio", Value = "1" },
                           new SelectListItem() { Text = "Comissões", Value = "2" },
                           new SelectListItem() { Text = "Adiantamento", Value = "3" },
                           new SelectListItem() { Text = "Pagto Mensal / Folha Geral", Value = "4" },
                           new SelectListItem() { Text = "13º Décimo Terceiro", Value = "5" },
                           new SelectListItem() { Text = "Diferença de 13º Salário", Value = "6" },
                           new SelectListItem() { Text = "Adicional", Value = "7" },
            });
            ViewBag.dpPeriodo = new SelectList(dpPeriodo, "Value", "Text");

            // ano...
            List<SelectListItem> dpAno = new List<SelectListItem>();
            dpAno.AddRange(new[]{
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 0).ToString(), Value = ((DateTime.Now.Year) - 0).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 1).ToString(), Value = ((DateTime.Now.Year) - 1).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 2).ToString(), Value = ((DateTime.Now.Year) - 2).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 3).ToString(), Value = ((DateTime.Now.Year) - 3).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 4).ToString(), Value = ((DateTime.Now.Year) - 4).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 5).ToString(), Value = ((DateTime.Now.Year) - 5).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 6).ToString(), Value = ((DateTime.Now.Year) - 6).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 7).ToString(), Value = ((DateTime.Now.Year) - 7).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 8).ToString(), Value = ((DateTime.Now.Year) - 8).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) - 9).ToString(), Value = ((DateTime.Now.Year) - 9).ToString() },
                           new SelectListItem() { Text = ((DateTime.Now.Year) -10).ToString(), Value = ((DateTime.Now.Year) -10).ToString() },
            });
            ViewBag.dpAno = new SelectList(dpAno, "Value", "Text");
                        
            // mês...
            List<SelectListItem> dpMes = new List<SelectListItem>();
            dpMes.AddRange(new[]{
                           new SelectListItem() { Text = "Janeiro", Value = "1" },
                           new SelectListItem() { Text = "Fevereiro", Value = "2" },
                           new SelectListItem() { Text = "Março", Value = "3" },
                           new SelectListItem() { Text = "Abril", Value = "4" },
                           new SelectListItem() { Text = "Maio", Value = "5" },
                           new SelectListItem() { Text = "Junho", Value = "6" },
                           new SelectListItem() { Text = "Julho", Value = "7" },
                           new SelectListItem() { Text = "Agosto", Value = "8" },
                           new SelectListItem() { Text = "Setembro", Value = "9" },
                           new SelectListItem() { Text = "Outubro", Value = "10" },
                           new SelectListItem() { Text = "Novembro", Value = "11" },
                           new SelectListItem() { Text = "Dezembro", Value = "12" },
            });
            ViewBag.dpMes = new SelectList(dpMes, "Value", "Text");

            // empresas...
            List<SelectListItem> dpEmpresa = new List<SelectListItem>();
            dpEmpresa.AddRange(new[]{
                           new SelectListItem() { Text = "COAD", Value = "1" },
                           new SelectListItem() { Text = "APC", Value = "2" },
                           new SelectListItem() { Text = "APCJ", Value = "3" }
            });
            ViewBag.dpEmpresa = new SelectList(dpEmpresa, "Value", "Text");
        }

        // index...
        [Autorizar(PorMenu = true)]
        public ActionResult Index() 
        {
            TempData["erro"] = "";

            
            ViewBag.cpf = SessionContext.autenticado.USU_CPF;

            this.CarregaListas();

            TempData["msg"] = "Emissor de contracheques";
            
            return View();
        }

        // gerentes somente...
        //[Autorizar(PorMenu = true)]
        public ActionResult Gerente()
        {
            TempData["erro"] = "";

            // Gerentes podem ler qualquer Cpf...
            ViewBag.cpf = "";

            this.CarregaListas();

            TempData["msg"] = "Emissor de contracheques.";

            return View();
        }
        public ActionResult BuscarContraCheque(string _cpf)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (SessionContext.autenticado.ADMIN != true &&
                    SessionContext.autenticado.USU_CPF != _cpf)
                    throw new Exception("CPF Inválido !!");

                var _cotrachequesrv = ServiceFactory.RetornarServico<PfperffSRV>();
                var _chlst  = _cotrachequesrv.ListarContraCheque(_cpf);
                
                result.Add("chlst", _chlst);
                result.success = true;

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        // contracheque...
        public ActionResult EmitirContracheque(string _dpCpf,  string _dpEmpresa,  string _dpAno, string _dpMes, string _dpPeriodo)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                var _emp_id = Convert.ToInt32(_dpEmpresa);
                var _empresasrv = ServiceFactory.RetornarServico<EmpresaSRV>();
                var _cotrachequesrv = ServiceFactory.RetornarServico<PfperffSRV>();
                var _cotracheque = _cotrachequesrv.BuscarContraCheque(_dpCpf, _dpEmpresa, _dpAno, _dpMes, _dpPeriodo);
                var _empresa = _empresasrv.FindById(_emp_id);

                result.Add("empresa", _empresa);
                result.Add("cotracheque", _cotracheque);
                result.success = true;

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado);
                else
                {
                    Autenticado aut = new Autenticado();
                    aut.USU_LOGIN = SessionContext.usu_login_desktop;

                    SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut);
                }

                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
   }
}