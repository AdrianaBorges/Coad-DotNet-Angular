
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("INF_ANO", "INF_REMESSA", "INF_ENVIO", "ASN_NUM_ASSINATURA")]
    public class InformativoSemanalAnaliticoSRV : GenericService<INFORMATIVO_SEMANAL_ANALITICO, InformativoSemanalAnaliticoDTO, object>
    {
        public InformativoSemanalAnaliticoDAO _dao = new InformativoSemanalAnaliticoDAO();

        public InformativoSemanalAnaliticoSRV()
        {
            this.Dao = _dao;
        }

        public List<InformativoSemanalAnaliticoDTO> Buscar(string _ano, string _remessa, int _envio, int? _produto = null, bool? _protocolada = null, string _uf = null, string _assinatura = null)
        {
            return _dao.Buscar(_ano, _remessa, _envio, _produto, _protocolada, _uf, _assinatura);
        }

        public IQueryable<InformativoSemanalAnaliticoDTO> BuscarAssinatura(string _assinatura)
        {
            return _dao.BuscarAssinatura(_assinatura);
        }

        public IQueryable<InformativoSemanalAnaliticoDTO> Estatisticas(string _ano, string _remessa, int envio, int nivel = 1)
        {
            return _dao.Estatisticas(_ano, _remessa, envio, nivel);
        }
    }
}
