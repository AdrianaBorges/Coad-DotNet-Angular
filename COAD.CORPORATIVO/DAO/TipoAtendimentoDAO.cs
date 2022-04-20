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
    public class TipoAtendimentoDAO : DAOAdapter<TIPO_ATENDIMENTO, TipoAtendimentoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TipoAtendimentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<TipoAtendimentoDTO> BuscarTipoAtendimento(string _grupo = null, int _classificacao = 0)
        {
            var _tipodocfical = (from t in db.TIPO_ATENDIMENTO
                                 where ((_grupo == null || _grupo == "") || (t.TIP_ATEND_GRUPO == _grupo))  &&
                                       ((_classificacao ==  0) || (t.CLA_ATEND_ID == _classificacao))
                                 select t);

            return ToDTO(_tipodocfical);
        }
        public IList<TipoAtendimentoDTO> BuscarTipoAtendimento(int _classificacao)
        {
            var _tipodocfical = (from t in db.TIPO_ATENDIMENTO
                                 where (t.CLA_ATEND_ID == _classificacao)
                                 select t);

            return ToDTO(_tipodocfical);
        }



    }

}
