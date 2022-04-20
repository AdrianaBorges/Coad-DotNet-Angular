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
    [ServiceConfig("BAN_ID", "OCE_CODIGO", "OCT_CODIGO")]
    public class OcorrenciaErroSRV : GenericService<OCORRENCIA_ERRO, OcorrenciaErroDTO, string>
    {
        public OcorrenciaErroDAO _dao = new OcorrenciaErroDAO();

        public OcorrenciaErroSRV()
        {
            this.Dao = _dao;
        }

        public Pagina<OcorrenciaErroDTO> LerOcorrenciaErro(string bco = null, string cod = null, string codRet = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.LerOcorrenciaErro(bco, cod, codRet, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarOcorrenciaErro(OcorrenciaErroDTO ocorErr)
        {
            try
            {
                SaveOrUpdateNonIdentityKeyEntity(ocorErr);
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
