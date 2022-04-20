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
    [ServiceConfig("CODIGO", "DDD_TEL", "TELEFONE", "TIPO")]
    public class TelefoneProspectSRV : GenericService<TELEFONES_PROSP, TelefoneProspectDTO, string>
    {
        private TelefoneProspectDAO _dao = new TelefoneProspectDAO();

        public TelefoneProspectSRV()
        {
            Dao = _dao;
        }

        public IList<TelefoneProspectDTO> FindByCodigo(string codigo)
        {
            return _dao.FindByCodigo(codigo);
        }

        public void PreencherTelefoneProspect(CartCoadDTO cartCoad)
        {
            if (cartCoad != null && !string.IsNullOrWhiteSpace(cartCoad.CODIGO))
            {
                var telefones = FindByCodigo(cartCoad.CODIGO);
                cartCoad.TELEFONES_PROSP = telefones;
            }
        }



        public void ChecarExcluirTelefoneProsp(CartCoadDTO cartCoad)
        {
            if (cartCoad != null)
            {
                var codigo = cartCoad.CODIGO;
                var objetoDoBanco = new CartCoadSRV().FindByIdFullLoaded(codigo, true, true);
                ExcluirList<CartCoadDTO>(cartCoad, objetoDoBanco, "TELEFONES_PROSP");
            }
        }

        public void SalvarTelefonesProsp(CartCoadDTO cartCoad)
        {
            if (cartCoad != null && cartCoad.TELEFONES_PROSP != null)
            {
                var lstProspTelefones = cartCoad.TELEFONES_PROSP;
                CheckAndAssignKeyFromParentToChildsList(cartCoad, lstProspTelefones, "CODIGO");

                ChecarExcluirTelefoneProsp(cartCoad);
                SaveOrUpdateNonIdentityKeyEntity(lstProspTelefones);
            }
        }

        public void SalvarTelefonesProsp(ICollection<CartCoadDTO> lstTelefonesProsp)
        {
            if (lstTelefonesProsp != null)
            {
                foreach (var relTab in lstTelefonesProsp)
                {
                    SalvarTelefonesProsp(relTab);
                }
            }
        }

        //public void SalvarTelefones(IEnumerable<TelefoneProspectDTO> lstTelefones, string CODIGO)
        //{
        //    if (lstTelefones != null && lstTelefones.Count() > 0)
        //    {
        //        if (!string.IsNullOrWhiteSpace(CODIGO))
        //        {
        //            foreach (var email in lstTelefones)
        //            {
        //                if (string.IsNullOrWhiteSpace(email.CODIGO))
        //                {
        //                    email.CODIGO = CODIGO;
        //                }
        //            }
        //            SaveOrUpdateNonIdentityKeyEntity(lstTelefones);
        //        }
        //    }
        //}
    }
}
