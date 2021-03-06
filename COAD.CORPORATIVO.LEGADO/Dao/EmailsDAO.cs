using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class EmailsDAO : AbstractGenericDao<EMAILS, EmailsDTO, object>
    {
        public EmailsDAO()
        {
            SetProfileName("corp_old");
        }
    }
}
