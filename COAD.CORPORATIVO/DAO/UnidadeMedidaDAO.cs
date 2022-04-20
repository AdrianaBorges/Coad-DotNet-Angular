using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class UnidadeMedidaDAO : DAOAdapter<UNIDADE_MEDIDA, UnidadeMedidaDTO>
    {     
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UnidadeMedidaDAO()
        {
             db = GetDb<COADCORPEntities>();
        }
        public List<UNIDADE_MEDIDA> Buscar()
        {
            var _unmedida = (from u in db.UNIDADE_MEDIDA
                              select u).ToList();

            if (_unmedida == null)
                _unmedida = new List<UNIDADE_MEDIDA>();

            return (List<UNIDADE_MEDIDA>)_unmedida;
        }
        public List<FATOR_CONVERSAO> BuscarFatorConversao(string _und_id)
        {
            var _unmedida = (from u in db.FATOR_CONVERSAO
                            where u.UND_ID == _und_id
                           select u).ToList();

            if (_unmedida == null)
                _unmedida = new List<FATOR_CONVERSAO>();

            return (List<FATOR_CONVERSAO>)_unmedida;
        }
        public List<UNIDADE_MEDIDA> BuscarPorPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _lista = (from n in db.NOTA_FISCAL_ITEM
                          where (((n.NOTA_FISCAL.NF_TIPO == 0 || n.NOTA_FISCAL.NF_TIPO == 2) && ((n.NOTA_FISCAL.NF_DATA_ENTRADA >= _dtini && n.NOTA_FISCAL.NF_DATA_ENTRADA < _dtfim))) ||
                                 ((n.NOTA_FISCAL.NF_TIPO == 1 || n.NOTA_FISCAL.NF_TIPO == 3) && ((n.NOTA_FISCAL.NF_DATA_EMISSAO >= _dtini && n.NOTA_FISCAL.NF_DATA_EMISSAO < _dtfim)))) &&
                                 (n.NOTA_FISCAL.EMP_ID == _emp_id)
                          orderby n.NF_TIPO ascending
                          select n.UNIDADE_MEDIDA).Distinct();

            return _lista.ToList();

        }
        public List<UNIDADE_MEDIDA> BuscarEntradaPorPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {

            DateTime d = new DateTime();
            d = (DateTime)_dtfim;
            _dtfim = d.AddDays(1);

            var _lista = (from n in db.NOTA_FISCAL_ITEM
                          where (n.NOTA_FISCAL.NF_TIPO == 0 || n.NOTA_FISCAL.NF_TIPO == 2) && 
                                (n.NOTA_FISCAL.NF_DATA_ENTRADA >= _dtini && n.NOTA_FISCAL.NF_DATA_ENTRADA < _dtfim)  &&
                                (n.NOTA_FISCAL.EMP_ID == _emp_id)
                          orderby n.NF_TIPO ascending
                          select n.UNIDADE_MEDIDA).Distinct();

            return _lista.ToList();

        }


    }
}
