using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class CepLogradouroDAO : DAOAdapter<CEP_LOGRADOURO, CepLogradouroDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CepLogradouroDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public CepLogradouroDTO BuscarCep(string _cep_numero)
        {
            MUNICIPIO _mun = db.MUNICIPIO.Where(x => x.MUN_CEP == _cep_numero).FirstOrDefault();
            
            CEP_LOGRADOURO _query = new CEP_LOGRADOURO();

            if (_mun == null)
                _query = db.CEP_LOGRADOURO.Where(x => x.CEP_NUMERO == _cep_numero).FirstOrDefault();
            else
            {
                _query.MUN_ID = _mun.MUN_ID;
                _query.MUNICIPIO = _mun;
                //_query.MUNICIPIO.MUN_DESCRICAO = _mun.MUN_DESCRICAO;
                _query.CEP_UF = _mun.UF;
                _query.CEP_LOG = null;
                _query.CEP_NUMERO = _mun.MUN_CEP;
            }

            return ToDTO(_query);
        }
        public Pagina<CepLogradouroDTO> BuscarCep(string _cep_numero, string _cep_log, int pagina = 1, int registroPorPagina = 10)
        {
            var _query = (from c in db.CEP_LOGRADOURO
                          join m in db.MUNICIPIO on c.MUN_ID equals m.MUN_ID 
                          //into leftjoin from x in leftjoin.DefaultIfEmpty()
                         where (c.CEP_NUMERO == _cep_numero) 
                        select c);

            if (!String.IsNullOrWhiteSpace(_cep_log))
            {
                _query = (from c in db.CEP_LOGRADOURO
                          join m in db.MUNICIPIO on c.MUN_ID equals m.MUN_ID
                          //into leftjoin from x in leftjoin.DefaultIfEmpty()
                          where (c.CEP_LOG.Contains(_cep_log))
                          select c);
            }


            return ToDTOPage(_query, pagina, registroPorPagina);
        }

        
        public int BuscarUltimoID()
        {
            var _utilmoID = db.CEP_LOGRADOURO.Max(x => x.CEP_ID);

            return _utilmoID;
        }
    }
}
