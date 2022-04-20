using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.UTIL.Grafico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    public class LogSimuladorSRV : GenericService<LOG_SIMULADOR, LogSimuladorDTO, int>
    {
        private LogSimuladorDAO _dao = new LogSimuladorDAO();

        public LogSimuladorSRV()
        {
            Dao = _dao;
        }
        public LogSimuladorDTO RegistrarLogSimulador(LogSimuladorDTO log)
        {
            return this.Save(log);
        }
        public IList<JsonGrafico> BuscarTotalPorDia(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null,string _tdc_id = null)
        {
            return _dao.BuscarTotalPorDia(_dtini, _dtfim, _tipoacesso, _tdc_id);
        }
        public IList<JsonGrafico> BuscarTotalPorHora(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null, string _tdc_id = null)
        {
            return _dao.BuscarTotalPorHora(_dtini, _dtfim, _tipoacesso, _tdc_id);
        }
        public IList<JsonGrafico> BuscarTotalPorUFCalc(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, string _tipoacesso = null, string _tdc_id = null)
        {
            return _dao.BuscarTotalPorUFCalc(_dtini, _dtfim, _tipoacesso, _tdc_id);
        }
        public IList<JsonGrafico> BuscarTabelasPorPeriodo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0)
        {
            return _dao.BuscarTabelasPorPeriodo(_dtini, _dtfim, _grupo);
        }
        public IList<TabelasGrupoAcesso> BuscarTabelasPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            return _dao.BuscarTabelasPorGrupo(_dtini, _dtfim, _grupo, _tdc_id, _qteregistros);
        }
        public IList<TabelasGrupoAcesso> BuscarSimuladorPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            return _dao.BuscarSimuladorPorGrupo(_dtini, _dtfim, _grupo, _tdc_id, _qteregistros);
        }
        public IList<TabelasGrupoAcesso> BuscarTabelasPorGrupo(int _mes, int _ano, int _grupo = 0, string _tdc_id = null, int _qteregistros = 0)
        {
            return _dao.BuscarTabelasPorGrupo(_mes, _ano, _grupo, _tdc_id, _qteregistros);
        }
        public IList<JsonGrafico> BuscarTabelasPorGrupo(Nullable<DateTime> _dtini = null, Nullable<DateTime> _dtfim = null)
        {
            return _dao.BuscarTabelasPorGrupo(_dtini, _dtfim);
        }

        public IList<JsonGrafico> BuscarTabelasPorGrupo(int _ano)
        {
            return _dao.BuscarTabelasPorGrupo(_ano);
        }
        public IList<QTDE_ACESSOS_POR_CLIENTE_VW> BuscarAcessoClientesPorPeriodo(int _mes, int _ano, string _tdc_id)
        {
            return _dao.BuscarAcessoClientesPorPeriodo(_mes, _ano, _tdc_id);
        }
        public IList<QTDE_ACESSOS_POR_CLIENTE_VW> BuscarAcessoClientesPorPeriodo(int _mes, int _ano, string _tipo_acesso, string _assinatura)
        {
            return _dao.BuscarAcessoClientesPorPeriodo(_mes, _ano, _tipo_acesso, _assinatura);
        }
        public IList<LISTA_ACESSOS_POR_CLIENTE_VW> BuscarListaClientesPorPeriodo(int _mes, int _ano, string _tdc_id)
        {
            return _dao.BuscarListaClientesPorPeriodo(_mes, _ano, _tdc_id);
        }

        public IList<TabelasGrupoAcesso> BuscarTabelasMaisAcessadas(int _qteregistros, int _tdc_tipo, int? _tgr_tipo)
        {
            return _dao.BuscarTabelasMaisAcessadas(_qteregistros, _tdc_tipo, _tgr_tipo);
        }
        public IList<TabelasGrupoAcesso> BuscarTabelasAcessadas(int _qteregistros, int _tdc_tipo, int? _tgr_tipo)
        {
            return _dao.BuscarTabelasAcessadas(_qteregistros, _tdc_tipo, _tgr_tipo);
        }

    }
}
