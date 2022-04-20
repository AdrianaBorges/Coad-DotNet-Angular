using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Service;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COADPORTAL.Controllers
{
    public class SimuladorController : Controller
    {
        private TabDinamicaSRV _tabDinSRV = new TabDinamicaSRV();
        private TabDinamicaItemSRV _tabDinItemSRV = new TabDinamicaItemSRV();
        private TabDinamicaConfigSRV _tabDinConfigSRV = new TabDinamicaConfigSRV();
        private TabDinamicaConfigItemSRV _tabDinConfigItemSRV = new TabDinamicaConfigItemSRV();
        private TabDinamicaPublicacaoSRV _tabDinPublicacaoSRV = new TabDinamicaPublicacaoSRV();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Geral(int? id)
        {
            if (id == null)
                id = 0;

            ViewBag.id = id;

            return View();
        }
        //   [Autorizar(IsAjax = true)]
        public ActionResult CarregarTela(string _id)
        {

            JSONResponse response = new JSONResponse();

            try
            {

                var tabconfig = new TabDinamicaConfigDTO();
                var tabela = new TabDinamicaDTO();
                var tbitem = new Pagina<TabDinamicaItemDTO>();

                if (_id != null && _id != "")
                {
                    tabconfig = _tabDinConfigSRV.FindById(_id);
                    tabela = _tabDinSRV.FindById(_id);
                    tbitem = _tabDinItemSRV.ListarTabDinamicaPag(_id);
                }

                if (tabconfig.TDC_DATA_PUBLICACAO == null)
                    throw new Exception("Este simulador não está mais disponível. Em breve novidades. Aguarde!!");

                if (tabconfig.TAB_DINAMICA_CONFIG_ITEM != null)
                    tabconfig.TAB_DINAMICA_CONFIG_ITEM = tabconfig.TAB_DINAMICA_CONFIG_ITEM.OrderBy(x => x.TCI_ORDEM_APRESENTACAO).ToList();

                if (tabconfig.TDC_TIPO == 1)
                    tabela.TAB_DINAMICA_ITEM.Clear();

                response.Add("tabconfig", tabconfig);
                response.Add("tabela", tabela);
                response.AddPage("tbitem", tbitem);
                response.success = true;
                response.message = Message.Info("Ok");

                return Json(response, JsonRequestBehavior.AllowGet);

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

                response.success = false;
                response.message = Message.Fail(ex);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
