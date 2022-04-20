using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.PROSPECTADOS.Model.Dto;
using COAD.PROSPECTADOS.Repositorios.Contexto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Dao
{
    public class CartCoadDAO : AbstractGenericDao<cart_coad, CartCoadDTO, string>
    {
        public prospectadosEntities db { get { return GetDb<prospectadosEntities>(); } set { } }

        public CartCoadDAO()
        {
            SetProfileName("prospectados");
            db = GetDb<prospectadosEntities>(false);
        }

        public IQueryable<cart_coad> TemplateListByCliId(int? CLI_ID)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID);
            return query;            
        }

        public IList<CartCoadDTO> ListByCliId(int? CLI_ID)
        {
            var query = TemplateListByCliId(CLI_ID);
            return ToDTO(query);
        }

        public CartCoadDTO ObterPorCliente(int? CLI_ID)
        {
            var query = TemplateListByCliId(CLI_ID).FirstOrDefault();
            return ToDTO(query);
        }

        public int QtdCartCoadByCliId(int? CLI_ID)
        {
            var count = TemplateListByCliId(CLI_ID).Count();
            return count;
        }

        public Pagina<CartCoadDTO> BuscarProspects(
            string nome, 
            string cnpjCpf, 
            string email, 
            string ddd, 
            string telefone, 
            IList<string> IdsParaExcluir = null,
            bool pesquisaCpfCnpjPorIqualdade = true,
            int pagina = 1,
            int registrosPorPagina = 15)
        {

            IdsParaExcluir = new List<string>();

            if (string.IsNullOrWhiteSpace(nome))
            {
                nome = null;
            }

            if (string.IsNullOrWhiteSpace(cnpjCpf))
            {
                cnpjCpf = null;
            }


            if (string.IsNullOrWhiteSpace(email))
            {
                email = null;
            }

            if (string.IsNullOrWhiteSpace(ddd))
            {
                ddd = null;
            }

            if (string.IsNullOrWhiteSpace(telefone))
            {
                telefone = null;
            }

            
            var query = (from cart in db.cart_coad
                         join
                             prop in db.prospects on cart.CODIGO equals prop.CODIGO
                             where 
                             (nome == null || cart.NOME.Contains(nome)) &&
                             (
                                (cnpjCpf == null) || 
                                    (
                                        (pesquisaCpfCnpjPorIqualdade && prop.CPF_CNPJ == cnpjCpf) ||
                                        (pesquisaCpfCnpjPorIqualdade == false && prop.CPF_CNPJ.Contains(cnpjCpf))
                             
                                    )
                             )
                             &&
                             (cart.CLI_ID == null) && 
                             (
                                !(IdsParaExcluir.Contains(cart.CODIGO))
                             )
                         select cart);

            if (email != null)
            {
                query = (from cart in query
                         join objEmail in db.EMAILS_PROSP on cart.CODIGO equals objEmail.CODIGO
                         where objEmail.E_MAIL.Contains(email)
                         select cart);
            }

            if (ddd != null ||  telefone != null)
            {
                query = (from cart in query 
                         join objTelefone in db.TELEFONES_PROSP on cart.CODIGO equals objTelefone.CODIGO
                         where 
                             objTelefone.DDD_TEL == ddd && 
                             objTelefone.TELEFONE == telefone
                         select cart);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        }

    }
}
