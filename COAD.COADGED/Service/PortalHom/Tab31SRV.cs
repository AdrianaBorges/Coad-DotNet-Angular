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

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class Tab31SRV : GenericService<tab_31, Tab31DTO, int>
    {
        private Tab31DAO _dao = new Tab31DAO();

        public Tab31SRV()
        {
            Dao = _dao;
        }
        
        public void SalvarTab31(Tab31DTO tab31dto)
        {
            try
            {
                if (tab31dto.idGED != null)
                {
                    Merge(tab31dto, "idGED");
                }
                else
                {
                    tab31dto.dataCadastro = DateTime.Now;
                    Save(tab31dto);
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
