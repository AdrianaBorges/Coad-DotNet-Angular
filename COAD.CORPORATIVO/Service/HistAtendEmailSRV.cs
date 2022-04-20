using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{

    public class HistAtendEmailSRV : ServiceAdapter<HIST_ATEND_EMAIL, HistAtendEmailDTO, int>
    {
        private HistAtendEmailDAO _dao = new HistAtendEmailDAO();

        public HistAtendEmailSRV()
        {
            SetDao(_dao);
        }
        public IList<HistAtendEmailDTO> BuscarPorPeriodo(string _asn_id, Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarPorPeriodo(_asn_id, _dtini, _dtfim);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, Nullable<DateTime> _dtinicial = null, Nullable<DateTime> _dtfinal = null)
        {
            return _dao.BuscarQtdePorAssinatura(_asn_id, _dtinicial, _dtfinal);
        }
        public IList<QtdeConsumoDTO> BuscarQtdePorAssinatura(string _asn_id, string _contrato)
        {
            return _dao.BuscarQtdePorAssinatura(_asn_id, _contrato);
        }


    }
}
