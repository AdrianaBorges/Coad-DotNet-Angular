using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;


namespace COAD.CORPORATIVO.DAO
{
    public class TipoNotificacaoSistemaDAO : AbstractGenericDao<TIPO_NOTIFICACAO_SISTEMA, TipoNotificacaoSistemaDTO,int>
    {
        public TipoNotificacaoSistemaDAO()
        {
            SetProfileName("coadsys");
        }
    }
}
