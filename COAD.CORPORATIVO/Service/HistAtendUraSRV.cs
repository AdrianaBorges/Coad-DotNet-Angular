using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class HistAtendUraSRV : ServiceAdapter<HIST_ATEND_URA, HistAtendUraDTO, int>
    {
        private HistAtendUraDAO _dao = new HistAtendUraDAO();

        public HistAtendUraSRV()
        {
            SetDao(_dao);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, Nullable<DateTime> _dtinicial, Nullable<DateTime> _dtfinal)
        {
            return _dao.BuscarQtdePorAssinatura(_asn_id, _dtinicial, _dtfinal);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, string _contrato)
        {
            return _dao.BuscarQtdePorAssinatura(_asn_id, _contrato);
        }

        public IList<HistAtendUraDTO> BuscarPorAssinatura(string asn_id)
        {
            return _dao.BuscarPorAssinatura(asn_id);
        }

        public IList<HistAtendUraDTO> BuscarPorPeriodo(string _asn_id, string _ura_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarPorPeriodo(_asn_id, _ura_id, _dtini, _dtfim);
        }

        public IList<HistAtendUraDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarPorPeriodo(_asn_id, _dtini, _dtfim);
        }

        public IList<QtdeConsumoDTO> BuscarTotalPorAssinatura(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTotalPorAssinatura(_asn_id, _dtini, _dtfim);
        }

        public IList<JsonGrafico> BuscarTotalPorUF(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTotalPorUF(_dtini, _dtfim);
        }
        public IList<JsonGrafico> BuscarTotalPorProduto(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTotalPorProduto(_dtini, _dtfim);
        }
        public IList<JsonGrafico> BuscarTotalPorRamal(string _ura_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTotalPorRamal(_ura_id, _dtini, _dtfim);
        }

    }
}
