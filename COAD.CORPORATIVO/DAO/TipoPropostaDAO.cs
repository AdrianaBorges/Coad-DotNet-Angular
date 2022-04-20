using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons;
namespace COAD.CORPORATIVO.DAO
{
    public class TipoPropostaDAO : DAOAdapter<TIPO_PROPOSTA, TipoPropostaDTO, int>
    {
        private COADCORPEntities db { get; set; }

        public TipoPropostaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

    }
}