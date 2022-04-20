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
    [ServiceConfig("id")]
    public class Telefones2SRV : GenericService<TELEFONES2, Telefones2DTO, object>
    {
        private Telefones2DAO _dao = new Telefones2DAO();

        public Telefones2SRV()
        {
            Dao = _dao;
        }


        public IEnumerable<Telefones2DTO> SalvarTelefones(IEnumerable<Telefones2DTO> lstTelefones, string assinatura)
        {
            IEnumerable<Telefones2DTO> lstTelefoneRetorno = new List<Telefones2DTO>();

            if (lstTelefones != null && lstTelefones.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(assinatura))
                {
                    foreach (var email in lstTelefones)
                    {
                        if (string.IsNullOrWhiteSpace(email.ASSINATURA))
                        {
                            email.ASSINATURA = assinatura;
                        }
                    }
                    lstTelefoneRetorno = SaveOrUpdateAll(lstTelefones);
                }
            }

            return lstTelefoneRetorno;
        }

    }
}
