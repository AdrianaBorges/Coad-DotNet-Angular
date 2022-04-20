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
    public class FeriadoDAO : DAOAdapter<FERIADO, FeriadoDTO, int>
    {

        string[] meses = {"Meses","Janeiro","Fevereiro","março","Abril","Maio","Junho","Julho","Agosto","Setembro","Outubro","Novembro","Dezembro"};
       

        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public FeriadoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<FeriadoMesDTO> ListarAgrupado(int ano)
        {
            var query = (from f in db.FERIADO
                         where f.FER_DATA.Value.Year == ano
                         select f).OrderBy(x => x.FER_DATA);

            var _feriados = new List<FeriadoMesDTO>();
            var _mes = 0;
            FeriadoMesDTO _feriado = new FeriadoMesDTO();

            foreach (var i in query)
            {
                if (i.FER_DATA.Value.Month != _mes)
                {
                    if (_mes > 0)
                        _feriados.Add(_feriado);

                    _mes = i.FER_DATA.Value.Month;

                    _feriado = new FeriadoMesDTO();
                    _feriado.MES_ID = _mes;
                    _feriado.MES_DESCRICAO = meses[_mes];
                    
                }

                
                var _item = new FeriadoDTO();

                _item.FER_ID = i.FER_ID;
                _item.FER_DESCRICAO = i.FER_DESCRICAO;
                _item.FER_DATA = i.FER_DATA;
                _item.FER_FIXO = i.FER_FIXO;
                _item.FER_TIPO = i.FER_TIPO;

                _feriado.MES_FERIADOS.Add(_item);

            }


            return _feriados; //query.ToList();
        }

        public int BuscarDiasUteisFeriado(DateTime _dataini, DateTime _datafim)
        {
            var _diasnaouteis = this.BuscarDiasNaoUteis(_dataini, _datafim);
            var _diasmes = 0;

            DateTime _dtinicial;
            DateTime _dtfinal;

            _dtinicial = new DateTime(_dataini.Year, _dataini.Month, _dataini.Day);
            _dtfinal = new DateTime(_datafim.Year, _datafim.Month, _datafim.Day);

            _dtfinal = _dtfinal.AddDays(1);

            while (_dtinicial < _dtfinal)
            {
                _diasmes += 1;

                _dtinicial = _dtinicial.AddDays(1);

            }

            var _retorno = _diasmes - _diasnaouteis;

            if (_retorno <= 0)
                _retorno = 0;

            return _retorno;
        }
        public int BuscarDiasNaoUteis(DateTime _dataini, DateTime _datafim)
        {
            DateTime _dtinicial;
            DateTime _dtfinal;

            _dtinicial = new DateTime(_dataini.Year, _dataini.Month, _dataini.Day);
            _dtfinal = new DateTime(_datafim.Year, _datafim.Month, _datafim.Day);
          
            _dtfinal = _dtfinal.AddDays(1);

            var _query = (from f in db.FERIADO
                         where (f.FER_DATA >= _dtinicial && f.FER_DATA <= _dtfinal)
                         select f).Count();
            
            var _diasnaouteis = 0;

            while (_dtinicial < _dtfinal)
            {
                if (_dtinicial.DayOfWeek == DayOfWeek.Monday || _dtinicial.DayOfWeek == DayOfWeek.Saturday)
                   _diasnaouteis += 1;

                _dtinicial = _dtinicial.AddDays(1);

            }

            _diasnaouteis += _query;

            return _diasnaouteis; 

        }
        


    }
}
