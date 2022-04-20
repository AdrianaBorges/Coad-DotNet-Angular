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
    public class RegistroFaturamentoDAO : DAOAdapter<REGISTRO_FATURAMENTO, RegistroFaturamentoDTO, int>
    {
         /// <summary>
         /// 
         /// </summary>
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RegistroFaturamentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<RegistroFaturamentoDTO> RetornarRegistroDeFaturamentoPorItemDePedido(int? ipeId)
        {
            var query = (from reFat in db.REGISTRO_FATURAMENTO
                         where reFat.IPE_ID == ipeId
                         select reFat);

            return ToDTO(query);
        }
                
    }
}
