using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class AuditoriaSRV : ServiceCorp<AUDITORIA>
    {
        public List<CLIENTES_HISTORICO> BuscaAuditoriaCliente(int _cli_id , DateTime _dtinicial, DateTime _dtfinal)
        {
            return new AuditoriaDAO().BuscaAuditoriaCliente(_cli_id, _dtinicial, _dtfinal); 
        }
        public List<FORNECEDOR_HISTORICO> BuscaAuditoriaFonecendor(int _for_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            return new AuditoriaDAO().BuscaAuditoriaFonecendor(_for_id, _dtinicial, _dtfinal);

        }
        public List<PRODUTO_HISTORICO> BuscaAuditoriaProduto(int _pro_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            return new AuditoriaDAO().BuscaAuditoriaProduto(_pro_id, _dtinicial, _dtfinal);
        }

    }
}
