using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Service
{
    public class LogOcorrenciaSRV : ServicePadrao<LOG_OCORRENCIA>
    {
        public List<LOG_OCORRENCIA> Listar(DateTime? dtini, DateTime? dtfim, string login)
        {
            return new LogOcorrenciasDAO().Listar(dtini, dtfim, login);
        }
        public LOG_OCORRENCIA Buscar(int id)
        {
            return new LogOcorrenciasDAO().Buscar(id); 
        }
    }
}
