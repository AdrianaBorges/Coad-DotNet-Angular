using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{

    public class CnabArquivosItemErroDAO : AbstractGenericDao<CNAB_ARQUIVOS_ITEM_ERRO, CnabArquivosItemErroDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CnabArquivosItemErroDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public IList<CnabArquivosItemErroDTO> Listar(int _cnq_id)
        {

            var query = (from i in db.CNAB_ARQUIVOS_ITEM_ERRO
                         where i.CNQ_ID == _cnq_id
                         select i);

            return ToDTO(query);
        }
        public Pagina<CnabArquivosItemErroDTO> Listar(int _cnq_id, int pagina = 1, int registroPorPagina = 20)
        {

            var query = (from i in db.CNAB_ARQUIVOS_ITEM_ERRO
                         where i.CNQ_ID == _cnq_id
                         select i);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public IList<CnabArquivosItemErroDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome)
        {
            if (_ban_id == "")
                _ban_id = null;
            if (_nome == "")
                _nome = null;

            var query = (from i in db.CNAB_ARQUIVOS_ITEM_ERRO
                         join p in db.CNAB_ARQUIVOS on i.CNQ_ID equals p.CNQ_ID
                         where (EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) >= EntityFunctions.TruncateTime(_data_ini) &&
                                EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) <= EntityFunctions.TruncateTime(_data_fim)) &&
                               (_ban_id == null || (_ban_id != null && _ban_id == p.BAN_ID)) &&
                               (_nome == null || (_nome != null && _nome == p.CNQ_NOME))
                         orderby p.EMP_ID, p.CTA_ID
                         select i).OrderByDescending(x => x.CNE_ID);

            return ToDTO(query);
        }
        public Pagina<CnabArquivosItemErroDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int registroPorPagina = 20)
        {
            if (_ban_id == "")
                _ban_id = null;
            if (_nome == "")
                _nome = null;

            var query = (from i in db.CNAB_ARQUIVOS_ITEM_ERRO
                         join p in db.CNAB_ARQUIVOS on i.CNQ_ID equals p.CNQ_ID
                         where (EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) >= EntityFunctions.TruncateTime(_data_ini) &&
                                EntityFunctions.TruncateTime(p.CNQ_DATA_ARQUIVO) <= EntityFunctions.TruncateTime(_data_fim)) &&
                               (_ban_id == null || (_ban_id != null && _ban_id == p.BAN_ID)) &&
                               (_nome == null || (_nome != null && _nome == p.CNQ_NOME))
                         orderby p.EMP_ID, p.CTA_ID
                         select i).OrderByDescending(x => x.CNE_ID);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

    }
}
