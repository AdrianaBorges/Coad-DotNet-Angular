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
    public class TipoAtendimentoSRV : ServiceAdapter<TIPO_ATENDIMENTO, TipoAtendimentoDTO,int>
    {
        private TipoAtendimentoDAO _dao = new TipoAtendimentoDAO();

        public TipoAtendimentoSRV()
        {
            SetDao(_dao);
        }
        public IList<TipoAtendimentoDTO> BuscarTipoAtendimento(string _grupo, int _classificacao)
        {
            return _dao.BuscarTipoAtendimento(_grupo, _classificacao);
        }
        public IList<TipoAtendimentoDTO> BuscarTipoAtendimento(int _classificacao)
        {
            return _dao.BuscarTipoAtendimento(_classificacao);
        }
    }
}
