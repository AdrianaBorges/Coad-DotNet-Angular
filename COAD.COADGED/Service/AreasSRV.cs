using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    [ServiceConfig("ARE_CONS_ID")]
    public class AreasSRV : GenericService<AREAS_CONSULTORIA, AreasDTO, int>
    {
        private AreasDAO _dao = new AreasDAO();

        public AreasSRV()
        {
            Dao = _dao;
        }
        
        public Pagina<AreasDTO> Areas(int? areaId, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Areas(areaId, descricao, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarArea(AreasDTO areas)
        {
            try
            {
                if (areas.ARE_CONS_ID == null && _dao.Areas(null, areas.ARE_CONS_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um colecionador cadastrado com este nome!");
                }
                else
                {
                    if (areas.ARE_CONS_ID != null)
                    {
                        areas.DATA_ALTERACAO = DateTime.Now;
                        Merge(areas, "ARE_CONS_ID");
                    }
                    else
                    {
                        areas.DATA_CADASTRO = DateTime.Now;
                        Save(areas);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

        public IList<AreasDTO> ListarAreas()
        {
            return FindAll();
        }       

    }
}
