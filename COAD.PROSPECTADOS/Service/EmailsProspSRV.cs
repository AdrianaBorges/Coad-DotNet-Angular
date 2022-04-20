using Coad.GenericCrud.Service.Base;
using COAD.PROSPECTADOS.Dao;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Service
{
    [ServiceConfig("CODIGO", "E_MAIL")]
    public class EmailsProspSRV : GenericService<EMAILS_PROSP, EmailsProspDTO, string>
    {
        private EmailsProspDAO _dao = new EmailsProspDAO();

        public EmailsProspSRV()
        {
            Dao = _dao;
        }

        public IList<EmailsProspDTO> FindByCodigo(string codigo)
        {
            return _dao.FindByCodigo(codigo);
        }

        public void PreencherEmailProspect(CartCoadDTO cartCoad)
        {

            if (cartCoad != null && !string.IsNullOrWhiteSpace(cartCoad.CODIGO))
            {
                var emails = FindByCodigo(cartCoad.CODIGO);
                cartCoad.EMAILS_PROSP = emails;
            }
        }


        public void ChecarExcluirEmailProsp(CartCoadDTO cartCoad)
        {
            if (cartCoad != null)
            {
                var codigo = cartCoad.CODIGO;
                var objetoDoBanco = new CartCoadSRV().FindByIdFullLoaded(codigo, true, true);
                ExcluirList<CartCoadDTO>(cartCoad, objetoDoBanco, "EMAILS_PROSP");
            }
        }

        public void SalvarEmailsProsp(CartCoadDTO cartCoad)
        {
            if (cartCoad != null && cartCoad.EMAILS_PROSP != null)
            {
                var lstProspEmail = cartCoad.EMAILS_PROSP;
                CheckAndAssignKeyFromParentToChildsList(cartCoad, lstProspEmail, "CODIGO");
                lstProspEmail = lstProspEmail.Distinct(GetComparator(true)).ToList();

                ChecarExcluirEmailProsp(cartCoad);
                SaveOrUpdateNonIdentityKeyEntity(lstProspEmail);
            }
        }

        public void SalvarRelatorioTabelaColunas(ICollection<CartCoadDTO> lstEmailProsp)
        {
            if (lstEmailProsp != null)
            {
                foreach (var relTab in lstEmailProsp)
                {
                    SalvarEmailsProsp(relTab);
                }
            }
        }

        //public void SalvarEmails(IEnumerable<EmailsProspDTO> lstEmails, string CODIGO)
        //{
        //    if (lstEmails != null && lstEmails.Count() > 0)
        //    {
        //        if (!string.IsNullOrWhiteSpace(CODIGO))
        //        {
        //            foreach (var email in lstEmails)
        //            {
        //                if (string.IsNullOrWhiteSpace(email.CODIGO))
        //                {
        //                    email.CODIGO = CODIGO;
        //                }
        //            }

        //            SaveOrUpdateNonIdentityKeyEntity(lstEmails);
        //        }
        //    }
        //}

    }
}
