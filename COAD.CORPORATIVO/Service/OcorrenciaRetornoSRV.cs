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
    [ServiceConfig("BAN_ID", "OCT_CODIGO")]
    public class OcorrenciaRetornoSRV : GenericService<OCORRENCIA_RETORNO, OcorrenciaRetornoDTO, string>
    {
        public OcorrenciaRetornoDAO _dao = new OcorrenciaRetornoDAO();

        public OcorrenciaRetornoSRV()
        {
            this.Dao = _dao;
        }

        public IList<OcorrenciaRetornoDTO> LerOcorrenciaRetorno(string _ban_id)
        {
            return _dao.LerOcorrenciaRetorno(_ban_id);
        }

        public Pagina<OcorrenciaRetornoDTO> LerOcorrenciaRetorno(string bco = null, string cod = null, bool? baixa = null, bool? desaloca = null, bool? registra = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.LerOcorrenciaRetorno(bco, cod, baixa, desaloca, registra, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarOcorrenciaRetorno(OcorrenciaRetornoDTO ocorRet)
        {
            try
            {
                SaveOrUpdateNonIdentityKeyEntity(ocorRet);
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
