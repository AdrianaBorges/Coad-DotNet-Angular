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
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using GenericCrud.Util;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CNQ_ID")]
    public class CnabArquivosSRV : GenericService<CNAB_ARQUIVOS, CnabArquivosDTO, int>
    {
        public CnabArquivosDAO _dao { get; set; }

        public CnabArquivosSRV()
        {
            _dao = new CnabArquivosDAO();
        }

        public CnabArquivosSRV(CnabArquivosDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
        public IList<CnabArquivosDTO> Listar()
        {
            return _dao.Listar();
        }

        public IList<CnabArquivosDTO> ListarArquivos(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome)
        {
            return _dao.ListarArquivos(_data_ini, _data_fim, _ban_id,_nome);
        }
        public Pagina<CnabArquivosDTO> ListarArquivos(DateTime? _data_ini, DateTime? _data_fim, string _ban_id, string _nome, int pagina = 1, int registroPorPagina = 20)
        {
            return _dao.ListarArquivos(_data_ini, _data_fim, _ban_id, _nome, pagina, registroPorPagina);
        }
        public IList<CnabArquivosDTO> buscarArquivosCNAB(int? _emp_id = null, string _ban_id = null, int? _cta_id = null, int? _qt_linhas = null, DateTime? _data_arquivo = null, string _nome = null)
        {
            var resp = _dao.buscarArquivosCNAB(_emp_id, _ban_id, _cta_id, _qt_linhas, _data_arquivo, _nome);
            return resp;
        }
    }
}
