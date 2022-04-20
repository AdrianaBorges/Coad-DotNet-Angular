using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CONTRATO")]
    public class ContratoLegadoSRV : GenericService<CONTRATOS, ContratoLegadoDTO, string>
    {
        private ContratoLegadoDAO _dao = new ContratoLegadoDAO();

        public ContratoLegadoSRV()
        {
            Dao = _dao;
        }


        public void SalvarContrato(ContratoLegadoDTO contratoLegado)
        {
            if (contratoLegado != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(contratoLegado);
            }
        }

    }
}
