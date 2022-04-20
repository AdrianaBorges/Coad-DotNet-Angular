using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.DAO
{
    public class EmailsDeClienteDAO : DAOAdapter<ASSINATURA_EMAIL, AssinaturaEmailDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public EmailsDeClienteDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<AssinaturaEmailDTO> BuscarEmails(string _assinatura, int _cli_id)
        {
            var query = (from a in db.ASSINATURA_EMAIL
                         where a.ASN_NUM_ASSINATURA == _assinatura
                         select a);

            var query2 = (from a in db.ASSINATURA_EMAIL
                          where a.CLI_ID == _cli_id
                          select a);

            var query3 = query.Union(query2).Distinct();


            return ToDTO(query);

        }


        public IList<AssinaturaEmailDTO> BuscarEmailsDeCliente(string assinatura) 
        {
            IList<ASSINATURA_EMAIL> listaDeEmails = db.ASSINATURA_EMAIL.Where(x => x.ASN_NUM_ASSINATURA == assinatura).ToList();
            
            return ToDTO(listaDeEmails);

        }

        public IList<AssinaturaEmailDTO> BuscarEmailPorBoleto(string _parcela)
        {
            var query = (from a in db.ASSINATURA_EMAIL
                         join c in db.CONTRATOS on a.ASN_NUM_ASSINATURA equals c.ASN_NUM_ASSINATURA
                         join p in db.PARCELAS on c.CTR_NUM_CONTRATO equals p.CTR_NUM_CONTRATO
                         where p.PAR_NUM_PARCELA == _parcela
                         select a);
            
            var query2 = (from p in db.PARCELAS
                          join i in db.ITEM_PEDIDO on p.IPE_ID equals i.IPE_ID
                          join e in db.PEDIDO_CRM on i.PED_CRM_ID equals e.PED_CRM_ID
                          join a in db.ASSINATURA_EMAIL on e.ASN_NUM_ASSINATURA equals a.ASN_NUM_ASSINATURA
                          where p.PAR_NUM_PARCELA == _parcela
                          select a);

            var query3 = query.Union(query2).Distinct();


            return ToDTO(query);

        }


    }
}
