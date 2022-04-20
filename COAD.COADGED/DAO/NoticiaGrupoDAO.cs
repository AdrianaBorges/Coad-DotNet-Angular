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
    public class NoticiaGrupoDAO : AbstractGenericDao<NOTICIA_GRUPO, NoticiaGrupoDTO, int>
    {
        public NoticiaGrupoDAO()
            : base()
        {
            SetProfileName("GED");

        }
    }
}
