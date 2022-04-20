using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace CONFIGSIS.Controllers
{
    public class LogOCorrenciaController : Controller
    {
        //
        // GET: /LogOCorrencia/

        public ActionResult Index(DateTime? _dtini, DateTime? _dtfim, string _usu_login)
        {
            try
            {
                if (_usu_login == null)
                    _usu_login = "";

                ViewBag.Title = "Log Ocorrencias (Consultar)";

                List<LOG_OCORRENCIA> a = new LogOcorrenciaSRV().Listar(_dtini, _dtfim, _usu_login);

                if (a.Count == 0)
                    throw new Exception("Nenhum resultado encontrado para a consulta.");

                CarregaIndex();

                return View(a);
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Log Ocorrencias (Consultar)";

                ModelState.AddModelError("error", SysException.Show(ex));

                CarregaIndex();

                return View("Index");
            }
            
        }
        public ActionResult Detalhe(int _log_seq)
        {
            try
            {
                ViewBag.Title = "Log Ocorrencias (Detalhe)";

                LOG_OCORRENCIA _log = new LogOcorrenciaSRV().BuscarPorId(_log_seq);

                return View(_log);
            }
            catch (Exception ex)
            {
                ViewBag.Title = "Log Ocorrencias (Detalhe)";

                ModelState.AddModelError("Informação", SysException.Show(ex));

                CarregaIndex();

                return View("Index");
            }
        }

        public void CarregaIndex()
        {
            List<USUARIO> listaUsuario = new UsuarioSRV().BuscarTodos().ToList();
            List<PERFIL> listaPerfil = new PerfilSRV().BuscarTodos().ToList();
            var listaemp = new EmpresaSRV().FindAll().ToList();

            ViewBag.listaUsuario = new SelectList(listaUsuario, "USU_LOGIN", "USU_LOGIN");
            ViewBag.listaPerfil = new SelectList(listaPerfil, "perId", "perId");
            ViewBag.listaemp = new SelectList(listaemp, "EMP_ID", "EMP_RAZAO_SOCIAL");
        }


    }
}
