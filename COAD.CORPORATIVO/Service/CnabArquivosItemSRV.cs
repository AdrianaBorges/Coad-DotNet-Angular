using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CNI_ID")]
    public class CnabArquivosItemSRV : GenericService<CNAB_ARQUIVOS_ITEM, CnabArquivosItemDTO, int>
    {
        public CnabArquivosItemDAO _dao { get; set; }

        public CnabArquivosItemSRV()
        {
            _dao = new CnabArquivosItemDAO();
        }

        public CnabArquivosItemSRV(CnabArquivosItemDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
        public IList<CnabArquivosItemDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome)
        {
            return _dao.Listar(_data_ini, _data_fim, _ban_id, _nome);
        }
        public Pagina<CnabArquivosItemDTO> Listar(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.Listar(_data_ini, _data_fim, _ban_id, _nome, pagina, registroPorPagina);
        }
        
    }
}