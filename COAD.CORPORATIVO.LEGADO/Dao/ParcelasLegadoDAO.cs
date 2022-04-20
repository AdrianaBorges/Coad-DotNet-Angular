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
    public class ParcelasLegadoDAO : AbstractGenericDao<Parcelas, ParcelasLegadoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public ParcelasLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public IList<ParcelasLegadoDTO> LerParcelasLegado(string contrato = null, string letra = null, string cd = null, int pagina = 1, int itensPorPagina = 999999)
        {
            IQueryable<Parcelas> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(contrato))
            {
                query = query.Where(x => x.CONTRATO == contrato);
            }
            if (!String.IsNullOrWhiteSpace(letra))
            {
                query = query.Where(x => x.LETRA == letra);
            }
            if (!String.IsNullOrWhiteSpace(cd))
            {
                query = query.Where(x => x.CD == cd);
            }

            return ToDTO(query);
        }

        
    }
}
