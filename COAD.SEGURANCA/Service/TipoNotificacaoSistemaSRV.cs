using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TNS_ID")]
    public class TipoNotificacaoSistemaSRV : GenericService<TIPO_NOTIFICACAO_SISTEMA, TipoNotificacaoSistemaDTO, int>
    {
        public TipoNotificacaoSistemaDAO _dao { get; set; }

        public TipoNotificacaoSistemaSRV()
        {
            this._dao = new TipoNotificacaoSistemaDAO();
            this.Dao = _dao;
        }

        public TipoNotificacaoSistemaSRV(TipoNotificacaoSistemaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

    }
}
