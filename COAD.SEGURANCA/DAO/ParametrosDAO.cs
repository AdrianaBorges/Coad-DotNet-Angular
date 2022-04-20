using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.DAO
{

    public class ParametrosDAO : AbstractGenericDao<PARAMETROS, ParametrosDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public ParametrosDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>();
        }
        public IList<ParametrosDTO> Listar(int _pgr_id)
        {
            var _query = (from p in db.PARAMETROS
                          where p.PGR_ID == _pgr_id
                          select p);

            return ToDTO(_query);
        }
        public ParametrosDTO BuscarValor(string _PAR_KEY)
        {
            var _query = (from p in db.PARAMETROS
                          where p.PAR_KEY == _PAR_KEY
                          select p).FirstOrDefault();

            return ToDTO(_query);
        }
        public int BuscarValorInt(string _PAR_KEY)
        {
            var _query = (from p in db.PARAMETROS
                          where p.PAR_KEY == _PAR_KEY
                          select p).FirstOrDefault();

            var _retorno = 0;

            if (_query != null)
                int.TryParse(_query.PAR_VALOR, out _retorno);

            return _retorno;
        }
        public string BuscarValorString(string _PAR_KEY)
        {
            var _query = (from p in db.PARAMETROS
                          where p.PAR_KEY == _PAR_KEY
                          select p).FirstOrDefault();

            string _retorno = "";

            if (_query != null)
                _retorno = _query.PAR_VALOR;

            return _retorno;
        }
        public int BuscarMesFaturamento()
        {
            var _query = (from p in db.PARAMETROS
                          where p.PAR_KEY == "MESFATURAMENTO"
                          select p).FirstOrDefault();

            var _retorno = 0;

            if (_query != null)
                int.TryParse(_query.PAR_VALOR, out _retorno);

            return _retorno;
        }

        public string BuscarProximaRemessa()
        {

            var _valor = this.BuscarValorInt("ULTIMOINFORMATIVO") + 1;

            string rem = StringUtil.PreencherZeroEsquerda("0", 2 - _valor.ToString().Length) + _valor.ToString();

            return rem;
        }
        public string BuscarUltimaRemessa()
        {

            var rem =  this.BuscarValorString("ULTIMOINFORMATIVO");

            if (rem.Length < 2)
                rem = StringUtil.PreencherZeroEsquerda("0", 2 - rem.Length) + rem;

            return rem;
        }

    }
}