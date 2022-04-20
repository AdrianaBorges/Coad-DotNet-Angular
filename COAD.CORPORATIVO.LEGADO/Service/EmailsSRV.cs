using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("AUTOID")]
    public class EmailsSRV : GenericService<EMAILS, EmailsDTO, object>
    {
        private EmailsDAO _dao = new EmailsDAO();

        public EmailsSRV()
        {
            Dao = _dao;
        }

        public IEnumerable<EmailsDTO> SalvarEmails(IEnumerable<EmailsDTO> lstEmails, string ASSINATURA)
        {
            IEnumerable<EmailsDTO> lstEmailRetorno = new List<EmailsDTO>();

            if (lstEmails != null && lstEmails.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(ASSINATURA))
                {
                    foreach (var email in lstEmails)
                    {
                        if (string.IsNullOrWhiteSpace(email.ASSINATURA))
                        {
                            email.ASSINATURA = ASSINATURA;
                        }
                    }

                    lstEmailRetorno = SaveOrUpdateAll(lstEmails);

                    
                }
            }

            return lstEmailRetorno;
        }

    }
}
