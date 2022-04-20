using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
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
    [ServiceConfig("ID")]
    public class ParamSRV : GenericService<PARAM, ParamDTO, int>
    {
        private ParamDAO _dao = new ParamDAO();

        public ParamSRV()
        {
            Dao = _dao;
        }

        public ParamDTO GetParam()
        {
            return _dao.GetParam();
        }

        public int? GetCodigoContrato()
        {
            var param = GetParam();

            if (param != null)
            {
                var numContratoStr = param.NUM_CONTRATO;
                int numContrato = 0;

                if (int.TryParse(numContratoStr, out numContrato))
                {
                    return numContrato;
                }
            }

            return null;
        }

        public void AtualizarCodigoContrato(int? codigoContrato)
        {
            if (codigoContrato != null)
            {
                var param = GetParam();
                param.NUM_CONTRATO = codigoContrato.ToString();

                Merge(param);
            }
            
        }

    }
}
