using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
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
    public class DocLiquidacaoSRV : GenericService<DOC_LIQUIDACAO, DocLiquidacaoDTO, string>
    {
        public DocLiquidacaoDAO _dao = new DocLiquidacaoDAO();

        public DocLiquidacaoSRV()
        {
            this.Dao = _dao;
        }

        public void SalvarDocLiquidacao(DocLiquidacaoDTO dliq)
        {
            try
            {
                SaveOrUpdateNonIdentityKeyEntity(dliq);
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

        public void DeletarDocLiquidacao(string sigla)
        {
            var dliq = this.FindById(sigla);
            dliq.DLI_DATA_EXCLUSAO = DateTime.Now;
            SaveOrUpdateNonIdentityKeyEntity(dliq);
        }
    }
}
