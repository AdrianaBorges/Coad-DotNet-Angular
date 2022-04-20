using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class InformativoSemanalAnaliticoDAO : DAOAdapter<INFORMATIVO_SEMANAL_ANALITICO, InformativoSemanalAnaliticoDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public InformativoSemanalAnaliticoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public List<InformativoSemanalAnaliticoDTO> Buscar(string _ano, string _remessa, int _envio, int? _produto=null, bool? _protocolada=null, string _uf=null, string _assinatura=null)
        {
            IQueryable<INFORMATIVO_SEMANAL_ANALITICO> q = GetDbSet();

            q = q.Where(x => x.INF_ANO == _ano && x.INF_REMESSA == _remessa && x.INF_ENVIO == _envio);

            if (_produto != null)
                q = q.Where(x => x.INF_PRO_ID == _produto);
            if (_protocolada != null)
                q = q.Where(x => x.INF_PROTOCOLADA == _protocolada);
            if (!String.IsNullOrWhiteSpace(_uf))
                q = q.Where(x => x.END_UF == _uf);
            if (!String.IsNullOrWhiteSpace(_assinatura))
                q = q.Where(x => x.ASN_NUM_ASSINATURA == _assinatura);

            return ToDTO(q).ToList();
        }

        public IQueryable<InformativoSemanalAnaliticoDTO> BuscarAssinatura(string _assinatura)
        {
            var q = "select distinct s.INF_ANO, s.INF_REMESSA, s.INF_ENVIO, INF_DATA, INF_ENTREGA, a.INF_PRO_ID, INF_PROTOCOLADA, END_UF, INF_ARQUIVO, ASN_NUM_ASSINATURA, " +
                    "PRODUTO=(select PRO_SIGLA from PRODUTOS p where a.INF_PRO_ID=PRO_ID) " +
                    "from INFORMATIVO_SEMANAL s " +
                    "inner join INFORMATIVO_SEMANAL_ANALITICO a on s.INF_ANO = a.INF_ANO and s.INF_REMESSA = a.INF_REMESSA and s.INF_ENVIO = a.INF_ENVIO " +
                    "inner join INFORMATIVO_SEMANAL_ENVIO e on s.INF_ANO = e.INF_ANO and s.INF_REMESSA = e.INF_REMESSA and s.INF_ENVIO = e.INF_ENVIO and a.INF_PRO_ID = e.INF_PRO_ID " +
                    "where a.ASN_NUM_ASSINATURA = '" + _assinatura + "'";

            IQueryable<InformativoSemanalAnaliticoDTO> query = db.Database.SqlQuery<InformativoSemanalAnaliticoDTO>(q).AsQueryable();

            return query;
        }

        public IQueryable<InformativoSemanalAnaliticoDTO> Estatisticas(string _ano, string _remessa, int envio, int nivel = 1)
        {
            var q = "";

            nivel = nivel < 1 || nivel > 3 ? 1 : nivel;

            //--1 = por envio--
            if (nivel == 1)
                q = "select INF_ANO, INF_REMESSA, INF_ENVIO, QTD = count(1), PROTOCOLADAS = sum(case when INF_PROTOCOLADA = 1 then 1 else 0 end) " +
                    "from INFORMATIVO_SEMANAL_ANALITICO " +
                    "group by INF_ANO, INF_REMESSA, INF_ENVIO " +
                    "having INF_ENVIO=" + envio + " and INF_ANO='" + _ano + "' and INF_REMESSA='" + _remessa + "'" +
                    "order by INF_ANO, INF_REMESSA, INF_ENVIO";

            //--2 = por envio / produto--
            if (nivel == 2)
                q = "select INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID, "+
                    "PRODUTO=(select PRO_SIGLA from PRODUTOS where INF_PRO_ID=PRO_ID), " +
                    "QTD = count(1), PROTOCOLADAS = sum(case when INF_PROTOCOLADA = 1 then 1 else 0 end) " +
                    "from INFORMATIVO_SEMANAL_ANALITICO " +
                    "group by INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID " +
                    "having INF_ENVIO=" + envio + " and INF_ANO='" + _ano + "' and INF_REMESSA='" + _remessa + "'" +
                    "order by INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID";

            //--3 = por envio / produto / uf
            if (nivel == 3)
                q = "select INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID, END_UF, "+
                    "PRODUTO=(select PRO_SIGLA from PRODUTOS where INF_PRO_ID=PRO_ID), " +
                    "QTD = count(1), PROTOCOLADAS = sum(case when INF_PROTOCOLADA = 1 then 1 else 0 end) " +
                    "from INFORMATIVO_SEMANAL_ANALITICO " +
                    "group by INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID, END_UF " +
                    "having INF_ENVIO=" + envio + " and INF_ANO='" + _ano + "' and INF_REMESSA='" + _remessa + "'" +
                    "order by INF_ANO, INF_REMESSA, INF_ENVIO, INF_PRO_ID, END_UF";

            IQueryable<InformativoSemanalAnaliticoDTO> query = db.Database.SqlQuery<InformativoSemanalAnaliticoDTO>(q).AsQueryable();

            return query;
        }
    }
}
