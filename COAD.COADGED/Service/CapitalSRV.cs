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

    public class CapitalSRV : GenericService<CAPITAL, CapitalDTO, int>
    {
        private CapitalDAO _dao = new CapitalDAO();

        public CapitalSRV()
        {
            Dao = _dao;
        }
        
        public Pagina<CapitalDTO> Capital(int? capId, string nome = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Capital(capId, nome, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarCapital(CapitalDTO capital)
        {
            try
            {
                if (capital.CAP_ID == null && _dao.Capital(null, capital.CAP_NOME).lista.Count() > 0)
                {
                    throw new Exception("Já existe uma Capital/Município cadastrada com este nome!");
                }
                else
                {
                    if (capital.CAP_ID != null)
                    {
                        Merge(capital, "CAP_ID");
                    }
                    else
                    {
                        Save(capital);
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
