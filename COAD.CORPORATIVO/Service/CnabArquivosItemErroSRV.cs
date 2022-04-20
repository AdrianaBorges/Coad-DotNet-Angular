using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{

    [ServiceConfig("CNE_ID")]
    public class CnabArquivosItemErroSRV : GenericService<CNAB_ARQUIVOS_ITEM_ERRO, CnabArquivosItemErroDTO, int>
    {
        public CnabArquivosItemErroDAO _dao { get; set; }

        public CnabArquivosItemErroSRV()
        {
            _dao = new CnabArquivosItemErroDAO();
        }

        public CnabArquivosItemErroSRV(CnabArquivosItemErroDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
        public IList<CnabArquivosItemErroDTO> Listar(int _cnq_id)
        {
            return _dao.Listar(_cnq_id);
        }
        public Pagina<CnabArquivosItemErroDTO> Listar(int _cnq_id, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.Listar(_cnq_id, pagina, registroPorPagina);
        }
        public IList<CnabArquivosItemErroDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome)
        {
            return _dao.Listar(_data_ini, _data_fim, _ban_id, _nome);
        }
        public Pagina<CnabArquivosItemErroDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.Listar(_data_ini, _data_fim, _ban_id, _nome, pagina, registroPorPagina);
        }

    }
}
