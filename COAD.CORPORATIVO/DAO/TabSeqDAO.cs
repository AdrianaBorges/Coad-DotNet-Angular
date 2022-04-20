using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class TabSeqDAO : AbstractGenericDao<TAB_SEQ, TabSeqDTO, string>
    {
        public int? Template(string chave)
        {
            var query = GetDbSet().Where(x => x.TABELA == chave).Select(s => s.SEQ).FirstOrDefault();
            return query;
        }

        public int? GetSeqAssinatura()
        {
            return Template("ASSINATURA");
        }

        public int? GetSeqCarteira()
        {
            return Template("CARTEIRA");
        }

        public int? GetSeqContrato()
        {
            return Template("CONTRATO");
        }

        public int? GetSeqParcela()
        {
            return Template("PARCELA");
        }
    }
}
