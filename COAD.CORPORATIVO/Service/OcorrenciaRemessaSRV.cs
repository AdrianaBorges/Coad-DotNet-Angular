using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("BAN_ID", "OCM_CODIGO")]
    public class OcorrenciaRemessaSRV : GenericService<OCORRENCIA_REMESSA, OcorrenciaRemessaDTO, string>
    {
        public OcorrenciaRemessaDAO _dao = new OcorrenciaRemessaDAO();

        public OcorrenciaRemessaSRV()
        {
            this.Dao = _dao;
        }

        public  IList<OcorrenciaRemessaDTO> LerOcorrenciaRemessa(string bco)
        {
            return _dao.LerOcorrenciaRemessa(bco);
        }

        public Pagina<OcorrenciaRemessaDTO> LerOcorrenciaRemessa(string bco = null, string cod = null, string rem = null, int paginaInicial = 1, int linhasPorPaginas = 7)
        {
            return _dao.LerOcorrenciaRemessa(bco, cod, rem, paginaInicial, linhasPorPaginas);
        }

        public void SalvarOcorrenciaRemessa(OcorrenciaRemessaDTO ocorRem)
        {
            try
            {
                SaveOrUpdateNonIdentityKeyEntity(ocorRem);
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
