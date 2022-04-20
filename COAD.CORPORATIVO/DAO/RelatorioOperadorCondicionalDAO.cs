
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
    public class RelatorioOperadorCondicionalDAO : DAOAdapter<RELATORIO_OPERADOR_CONDICIONAL, RelatorioOperadorCondicionalDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RelatorioOperadorCondicionalDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        

    }
}