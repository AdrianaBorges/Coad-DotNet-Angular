using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class AssinaturaEmailDAO : DAOAdapter<ASSINATURA_EMAIL, AssinaturaEmailDTO , int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AssinaturaEmailDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<AssinaturaEmailDTO> BuscarEmailsRemovidos(string _assinatura, List<int> emailsok)
        {
            var query = db.ASSINATURA_EMAIL.Where(x => x.ASN_NUM_ASSINATURA == _assinatura && !emailsok.Contains(x.AEM_ID)).ToList();

            return ToDTO(query);
        }
        
        public IList<AssinaturaEmailDTO> FindByNumAssinatura(string codigoAssinatura)
        {
            if (!string.IsNullOrWhiteSpace(codigoAssinatura))
            {
                var query = GetDbSet().Where(x => x.ASN_NUM_ASSINATURA == codigoAssinatura);
                return ToDTO(query);
            }

            return new List<AssinaturaEmailDTO>();
        }

        public IList<AssinaturaEmailDTO> FindByCliente(int? CLI_ID)
        {
            if (CLI_ID != null)
            {
                var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID);
                return ToDTO(query);
            }

            return new List<AssinaturaEmailDTO>();
        }

        public IList<AssinaturaEmailDTO> FindEmailsDoClienteEAssinatura(int? CLI_ID)
        {
            if (CLI_ID != null)
            {
                var query = GetDbSet().Where(x => 
                    x.CLI_ID == CLI_ID  || 
                    x.ASSINATURA.CLI_ID == CLI_ID);
                return ToDTO(query);
            }

            return new List<AssinaturaEmailDTO>();
        }

        public AssinaturaEmailDTO FindPrimeiroEmailDoClienteOuAssinatura(int? CLI_ID)
        {
            if (CLI_ID != null)
            {
                var query = GetDbSet().Where(x => 
                    x.CLI_ID == CLI_ID  || 
                    x.ASSINATURA.CLI_ID == CLI_ID);
                return ToDTO(query).FirstOrDefault();
            }

            return null;
        }

        public IList<AssinaturaEmailDTO> BuscarEmails(string _email)
        {
            var query = db.ASSINATURA_EMAIL.Where(x => x.AEM_EMAIL.Contains(_email)).ToList();

            return ToDTO(query);
        }

        public bool ChecarClientePossuiEmail(string email, int? cliId)
        {
            var count = (from em in db.ASSINATURA_EMAIL
                         where 
                            em.AEM_EMAIL == email &&
                            em.CLI_ID == cliId
                         select em)
                         .Count();

            return (count > 0);
        }

        public bool ChecarClientePossuiEmailAssinatura(string email, int? cliId)
        {
            var count = (from em in db.ASSINATURA_EMAIL
                         where
                            em.AEM_EMAIL == email &&
                            (em.CLI_ID == cliId || em.ASSINATURA.CLI_ID == cliId) 
                         select em)
                         .Count();

            return (count > 0);
        }

        public IList<AssinaturaEmailDTO> ListarDeTodasAsAssinaturas(int? cliId)
        {
            var query = (from assEmail in 
                             db.ASSINATURA_EMAIL join
                            ass in db.ASSINATURA on assEmail.ASN_NUM_ASSINATURA equals ass.ASN_NUM_ASSINATURA
                         where ass.CLI_ID == cliId
                         select assEmail);
            return ToDTO(query);
        }

        public IList<AssinaturaEmailDTO> FindEmailsDoClienteEAssinaturaPorTipo(int? CLI_ID, int? opcID = null)
        {
            if (CLI_ID != null)
            {
                var query = (from assEm in db.ASSINATURA_EMAIL
                             where (assEm.CLI_ID == CLI_ID ||
                                    assEm.ASSINATURA.CLI_ID == CLI_ID) &&
                                    assEm.OPC_ID == opcID
                             select assEm);
                
                return ToDTO(query);
            }

            return new List<AssinaturaEmailDTO>();
        }

    }

}
