
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REP_ID", "ARE_CONS_ID", "TIT_ID")]
    public class AreaConsultoriaRepresentanteSRV : GenericService<AREA_CONSULTORIA_REPRESENTANTE, AreaConsultoriaRepresentanteDTO, int>
    {
        
        public AreaConsultoriaRepresentanteDAO _dao = new AreaConsultoriaRepresentanteDAO();

        public AreaConsultoriaRepresentanteSRV()
        {
            this.Dao = _dao;
        }

        public IList<AreaConsultoriaRepresentanteDTO> FindByRepId(int? repId)
        {
            return _dao.FindByRepId(repId);
        }

        public void PreencherAreaConsultoriaRepresentante(RepresentanteDTO rep)
        {
            if (rep != null && rep.REP_ID != null)
            {
                var repId = rep.REP_ID;

                var lstAreaConsultoriaRepresentante = FindByRepId(repId);
                rep.AREA_CONSULTORIA_REPRESENTANTE = lstAreaConsultoriaRepresentante;
            }
        }
        
    }
}
