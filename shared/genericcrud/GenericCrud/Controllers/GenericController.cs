using Coad.GenericCrud.ActionResultTools;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Dao;
using GenericCrud.Models;
using GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GenericCrud.Controllers
{
    public abstract class GenericController<T, D, Id> : Controller where T : class
    {
        private GenericService<T, D, Id> Service;

        public GenericController() : base(){

        }



        public GenericController(GenericService<T, D, Id> Service) : base()
        {
            this.Service = Service;
        }

        protected virtual JsonResult Pesquisar(AbstractSearchParams<T, D> pesquisaParams)
        {
            JSONResponse response = new JSONResponse();
            try
            {
                if (pesquisaParams != null)
                {
                    var items = Service.Search(pesquisaParams);
                    response.Add("items", items);
                }
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult TestFindAll()
        {
            JSONResponse response = new JSONResponse();
            try
            {
                var items = Service.FindAll();
                response.Add("items", items);
            }
            catch (Exception e)
            {
                response.message = Message.Fail(e);
                response.success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult RetonarDados(Id Id)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var entity = Service.FindByIdFullLoaded(Id);
                response.Add("entity", entity);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Salvar(D entity)
        {
            JSONResponse result = new JSONResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    result.success = true;
                    result.message = Message.Info("Dados atualizados com sucesso!!");
                    Service.Salvar(entity);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result.success = false;
                    result.SetMessageFromModelState(ModelState);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidacaoException ex)
            {
                //SessionUtil.HandleException(ex);
                result.success = false;
                result.SetMessageFromValidacaoException(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //SessionUtil.HandleException(ex);
                result.success = false;
                result.message = Message.Fail(ex);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public GenericService<T, D, Id> GetService()
        {
            return Service;
        }

        public virtual ActionResult RetonarDadosDeFiltro(string nome = null)
        {
            JSONResponse response = new JSONResponse();

            try
            {
                var filters = Service.GetFilters(nome);
                response.Add("filters", filters);
            }
            catch (Exception e)
            {
                response.success = false;
                response.message = Message.Fail(e);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
