using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{
    public class ConsultaEmailPerfilColecDAO : AbstractGenericDao<CONSULTA_EMAIL_PERFIL_COLEC, ConsultaEmailPerfilColecDTO, string>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }
        public ConsultaEmailPerfilColecDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }

        public ConsultaEmailPerfilColecDTO BuscarColecionadorPorPerfil(string perfil)
        {
            CONSULTA_EMAIL_PERFIL_COLEC query = query = GetDbSet().Where(x => x.PC_ID.Equals(perfil)).FirstOrDefault();
            return ToDTO(query);
        }
    }
}
