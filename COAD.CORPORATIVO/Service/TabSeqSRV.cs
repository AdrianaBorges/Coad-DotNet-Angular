using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TabSeqSRV : GenericService<TAB_SEQ, TabSeqDTO, string>
    {
        public TabSeqDAO _dao = new TabSeqDAO();

        public TabSeqSRV()
        {
            SetKeys("TABELA");
            this.Dao = _dao;
        }

        public int? GetSeqAssinatura()
        {
            return _dao.GetSeqAssinatura();
        }

        public void AcrescentaSeqAssinatura()
        {
            var seq = FindById("ASSINATURA");
            seq.SEQ++;
            Merge(seq);
        }

        public void AcrescentaSeqAssinatura(int sequencia)
        {
            var seq = FindById("ASSINATURA");
            seq.SEQ = sequencia;
            Merge(seq);
        }


        public int? GetSeqCarteira()
        {
            return _dao.GetSeqCarteira();
        }

        public void AcrescentaSeqCarteira()
        {
            var seq = FindById("CARTEIRA");
            seq.SEQ++;
            Merge(seq);
        }

        public void AcrescentaSeqCarteira(int sequencia)
        {
            var seq = FindById("CARTEIRA");
            seq.SEQ = sequencia;
            Merge(seq);
        }

        public int? GetSeqContrato()
        {
            return _dao.GetSeqContrato();
        }
        public void AcrescentaSeqContrato()
        {
            var seq = FindById("CONTRATO");
            seq.SEQ++;
            Merge(seq);
        }

        public void AcrescentaSeqContrato(int sequencia)
        {
            var seq = FindById("CONTRATO");
            seq.SEQ = sequencia;
            Merge(seq);
        }

        public int? GetSeqParcela()
        {
            return _dao.GetSeqParcela();
        }

        public void AcrescentaSeqParcela()
        {
            var seq = FindById("PARCELA");
            seq.SEQ++;
            Merge(seq);
        }

        public void AcrescentaSeqParcela(int sequencia)
        {
            var seq = FindById("PARCELA");
            seq.SEQ = sequencia;
            Merge(seq);
        }
    }
}
