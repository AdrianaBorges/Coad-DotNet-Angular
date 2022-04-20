using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class AuditoriaDAO : RepositorioCorp<AUDITORIA>
    {
        public List<CLIENTES_HISTORICO> BuscaAuditoriaCliente(int _cli_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfinal;
            _dtfinal = d.AddDays(1);
            
            var _auditoria = (from ch in db.CLIENTES_HISTORICO
                              where (ch.DATA_ALTERA >= _dtinicial && ch.DATA_ALTERA <= _dtfinal) &&
                                    (ch.CLI_ID == _cli_id)
                             select ch).ToList();

            if (_auditoria == null)
                _auditoria = new List<CLIENTES_HISTORICO>();

            return (List<CLIENTES_HISTORICO>)_auditoria;
        }
        public List<FORNECEDOR_HISTORICO> BuscaAuditoriaFonecendor(int _for_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfinal;
            _dtfinal = d.AddDays(1);

            var _auditoria = (from fh in db.FORNECEDOR_HISTORICO
                             where (fh.DATA_ALTERA >= _dtinicial && fh.DATA_ALTERA <= _dtfinal) && 
                                   (fh.FOR_ID      == _for_id)
                             select fh).ToList();

            if (_auditoria == null)
                _auditoria = new List<FORNECEDOR_HISTORICO>();

            return (List<FORNECEDOR_HISTORICO>)_auditoria;

        }
        public List<PRODUTO_HISTORICO> BuscaAuditoriaProduto(int _pro_id, DateTime _dtinicial, DateTime _dtfinal)
        {
            DateTime d = new DateTime();
            d = (DateTime)_dtfinal;
            _dtfinal = d.AddDays(1);

            var _prodHist = (from ph in db.PRODUTO_HISTORICO
                             where (ph.DATA_ALTERA >= _dtinicial && ph.DATA_ALTERA <= _dtfinal) && 
                                  (ph.PRO_ID == _pro_id)
                             select ph).ToList();

            if (_prodHist == null)
                _prodHist = new List<PRODUTO_HISTORICO>();

            return (List<PRODUTO_HISTORICO>)_prodHist;


        }
        
    }
}
