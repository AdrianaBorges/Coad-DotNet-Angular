using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class BloqueiaConsultaIndividualDAO : AbstractGenericDao<bloqueia_consulta_individual, BloqueiaConsultaIndividualDTO, int>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public BloqueiaConsultaIndividualDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public IList<BloqueiaConsultaIndividualDTO> ConsultaPorAssinatura(string codAssinatura)
        {
            var query = (from blo in db.bloqueia_consulta_individual 
                         where blo.assinatura == codAssinatura &&
                            blo.ativo_sn == "S"
                             select blo);

            return ToDTO(query);
        }

        public BloqueiaConsultaIndividualDTO ConsultarPrimeiroPorAssinatura(string codAssinatura)
        {
            var query = (from blo in db.bloqueia_consulta_individual
                         where blo.assinatura == codAssinatura &&
                            blo.ativo_sn == "S"
                         select blo);

            return ToDTO(query.FirstOrDefault());
        }
    }
}
