using Coad.GenericCrud.Service.Base;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("PAR_ID")]
    public class ParametrosSRV : ServiceAdapter<PARAMETROS, ParametrosDTO, int>
    {

        private ParametrosDAO _dao = new ParametrosDAO();

        public ParametrosSRV()
        {
            this._dao = new ParametrosDAO();
            this.Dao = _dao;
        }
        public IList<ParametrosDTO> Listar(int _pgr_id)
        {
            return _dao.Listar(_pgr_id);
        }
        public ParametrosDTO BuscarValor(string _PAR_KEY)
        {
            return _dao.BuscarValor(_PAR_KEY);
        }
        public int BuscarValorInt(string _PAR_KEY)
        {
            return _dao.BuscarValorInt(_PAR_KEY);
        }
        public string BuscarValorString(string _PAR_KEY)
        {
            return _dao.BuscarValorString(_PAR_KEY);
        }

        public string BuscarProximaRemessa()
        {
            return _dao.BuscarProximaRemessa();
        }
        public string BuscarUltimaRemessa()
        {
            return _dao.BuscarUltimaRemessa();
        }
        public int BuscarMesFaturamento()
        {
            return _dao.BuscarMesFaturamento();
        }

        public int? RetornarEmpresaPadraoECommerce()
        {
            // TODO: Colocar na busca da tabela
            return 9;
        }

    }
}