using Coad.GenericCrud.Exceptions;
using COAD.COADGED.Service;
using COAD.PROXY.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Service
{
    public class GrandeGrupoProxySRV : TitulacaoSRV
    {
        public void PreencherGrandeGrupo(AreaConsultoriaRepresentanteProxyDTO areaConsultoriaRepresentante)
        {
            if (areaConsultoriaRepresentante != null && areaConsultoriaRepresentante.TIT_ID != null)
            {
                var TIT_ID = areaConsultoriaRepresentante.TIT_ID;
                var grandeGrupo = FindById(TIT_ID);

                if (string.IsNullOrWhiteSpace(grandeGrupo.TIT_TIPO) && grandeGrupo.TIT_TIPO != "G")
                {
                    throw new NegocioException("Não é possível recuperar o grande grupo. A titulação informada não é um grande grupo.");
                }

                areaConsultoriaRepresentante.GRANDE_GRUPO = grandeGrupo;
            }
        }
    }
}
