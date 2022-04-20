using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;
using COAD.SEGURANCA.Repositorios.Base;
using System.Data.Objects;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabArquivosDAO : AbstractGenericDao<CNAB_ARQUIVOS, CnabArquivosDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CnabArquivosDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<CnabArquivosDTO> ListarArquivos(DateTime ? _data_ini, DateTime? _data_fim, string _ban_id, string _nome)
        {
            if (_ban_id == "")
                _ban_id = null;
            if (_nome == "")
                _nome = null;

            var query = (from p in db.CNAB_ARQUIVOS
                         where (EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) >= EntityFunctions.TruncateTime(_data_ini)  &&
                                EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) <= EntityFunctions.TruncateTime(_data_fim)) &&
                               ( _ban_id == null || (_ban_id !=null && _ban_id == p.BAN_ID))&&
                               ( _nome   == null || (_nome != null && _nome == p.CNQ_NOME))
                         orderby p.EMP_ID, p.CTA_ID
                         select p).OrderByDescending(x => x.CNQ_DATA_ARQUIVO);

            return ToDTO(query);
        }
        public Pagina<CnabArquivosDTO> ListarArquivos(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int registroPorPagina = 20)
        {
            if (_ban_id == "")
                _ban_id = null;
            if (_nome == "")
                _nome = null;

            var query = (from p in db.CNAB_ARQUIVOS
                         where (EntityFunctions.TruncateTime(p.CNQ_DATA_LIDO) >= EntityFunctions.TruncateTime(_data_ini) &&
                                EntityFunctions.TruncateTime(p.CNQ_DATA_LIDO) <= EntityFunctions.TruncateTime(_data_fim)) &&
                               (_ban_id == null || (_ban_id != null && _ban_id == p.BAN_ID)) &&
                               (_nome == null || (_nome != null && _nome == p.CNQ_NOME))
                         orderby p.EMP_ID, p.CTA_ID
                         select p).OrderByDescending(x => x.CNQ_DATA_LIDO);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public IList<CnabArquivosDTO> Listar()
        {
            try
            {
                var _query = (from p in db.CNAB_ARQUIVOS
                              select p).OrderByDescending(x => x.DATA_CADASTRO);

                return ToDTO(_query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IList<CnabArquivosDTO> buscarArquivosCNAB(int? _emp_id = null, string _ban_id = null, int? _cta_id = null, int? _qt_linhas = null, DateTime? _data_arquivo = null, string _nome = null)
        {
            IQueryable<CNAB_ARQUIVOS> query = GetDbSet();

            if (_emp_id != null)
                query = query.Where(x => x.EMP_ID == _emp_id);
            if (!String.IsNullOrWhiteSpace(_ban_id))
                query = query.Where(x => x.BAN_ID == _ban_id);
            if (_cta_id != null)
                query = query.Where(x => x.CTA_ID == _cta_id);
            if (_qt_linhas != null)
                query = query.Where(x => x.CNQ_QTD_LINHAS == _qt_linhas);
            if (_data_arquivo != null)
                query = query.Where(x => EntityFunctions.TruncateTime(x.CNQ_DATA_ARQUIVO) == EntityFunctions.TruncateTime(_data_arquivo));
            if (!String.IsNullOrWhiteSpace(_nome))
                query = query.Where(x => x.CNQ_NOME == _nome);

            return ToDTO(query);

        }
    }
}
