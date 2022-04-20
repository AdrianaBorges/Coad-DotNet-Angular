using COAD.COADGED.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.PROXY.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Service
{
    public class ColecionadorProxySRV : AreasSRV
    {
        public void PreencherColecionador(AreaConsultoriaRepresentanteProxyDTO areaConsultoriaRepresentante)
        {
            if (areaConsultoriaRepresentante != null && areaConsultoriaRepresentante.ARE_CONS_ID != null)
            {
                var ACR_CONS_ID = areaConsultoriaRepresentante.ARE_CONS_ID;
                var area = FindById(ACR_CONS_ID);

                areaConsultoriaRepresentante.COLECIONADOR = area;
            }
        }
    }
}
