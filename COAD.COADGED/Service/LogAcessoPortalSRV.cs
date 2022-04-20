using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class LogAcessoPortalSRV : GenericService<LOG_ACESSO_PORTAL, LogAcessoPortalDTO, int>
    {
        private LogAcessoPortalDAO _dao = new LogAcessoPortalDAO();

        public LogAcessoPortalSRV()
        {
            Dao = _dao;
        }
        public List<int?> EncontrarMaisLidas()
        {
            return _dao.RetornarNoticiasMaisLidas();
        }
    }
}
