using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class CargosSRV : GenericService<CARGOS, CargosDTO, int>
    {
        private CargosDAO _dao = new CargosDAO();

        public CargosSRV()
        {
            Dao = _dao;
        }
        
        public Pagina<CargosDTO> Cargos(int? cargoId, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Cargos(cargoId, descricao, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarCargo(CargosDTO cargos)
        {
            try
            {
                if (cargos.CRG_ID == null && _dao.Cargos(null, cargos.CRG_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe um cargo cadastrado com este nome!");
                }
                else
                {
                    if (cargos.CRG_ID != null)
                    {
                        cargos.DATA_ALTERA = DateTime.Now;
                        Merge(cargos, "CRG_ID");
                    }
                    else
                    {
                        cargos.DATA_CADASTRO = DateTime.Now;
                        Save(cargos);
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

    }
}
