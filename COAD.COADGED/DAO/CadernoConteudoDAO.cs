using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class CadernoConteudoDAO : AbstractGenericDao<CADERNO_CONTEUDO, CadernoConteudoDTO, int>
    {
        public CadernoConteudoDAO()
            : base()
        {
            SetProfileName( "GED" );           
        }

    }
}
