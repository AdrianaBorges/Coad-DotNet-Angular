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

    public class PeriodicidadeSRV : GenericService<PERIODICIDADE, PeriodicidadeDTO, int>
    {
        private PeriodicidadeDAO _dao = new PeriodicidadeDAO();

        public PeriodicidadeSRV()
        {
            Dao = _dao;
        }
        
        public Pagina<PeriodicidadeDTO> Periodicidades(int? periodoId, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Periodicidades(periodoId, descricao, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarPeriodicidade(PeriodicidadeDTO periodicidade)
        {
            try
            {
                if (periodicidade.PRD_ID == null && _dao.Periodicidades(null, periodicidade.PRD_DESCRICAO).lista.Count() > 0)
                {
                    throw new Exception("Já existe uma periodicidade cadastrada com este nome!");
                }
                else
                {
                    if (periodicidade.PRD_ID != null)
                    {
                        Merge(periodicidade, "PRD_ID");
                    }
                    else
                    {
                        Save(periodicidade);
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
