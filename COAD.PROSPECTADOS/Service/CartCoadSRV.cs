using Coad.GenericCrud.Dao.Base.Pagination;
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
    [ServiceConfig("CODIGO")]
    public class CartCoadSRV : GenericService<cart_coad, CartCoadDTO, string>
    {
        private CartCoadDAO _dao;
        public ProspectsSRV _prospectadosSRV { get; set; }
        public UltimoCodigoSRV _ultimoCodigoSRV { get; set; }
        public EmailsProspSRV _emailProspSRV { get; set; }
        public TelefoneProspectSRV _telefoneProspSRV { get; set; }

        public CartCoadSRV(CartCoadDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public CartCoadSRV()
        {

            this._dao = new CartCoadDAO();
            this._prospectadosSRV = new ProspectsSRV();
            this._ultimoCodigoSRV = new UltimoCodigoSRV();
            this._emailProspSRV = new EmailsProspSRV();
            this._telefoneProspSRV = new TelefoneProspectSRV();

            Dao = _dao;
        }

        public CartCoadDTO SalvarCartCoad(CartCoadDTO cartCoad)
        {
            if (cartCoad != null)
            {
                var prospects = cartCoad.prospects;
                string codigoStr = null;

                if (!string.IsNullOrWhiteSpace(cartCoad.CODIGO))
                {
                    codigoStr = cartCoad.CODIGO;
                }
                else
                {
                    int? codigo = _ultimoCodigoSRV.GerarCodigo();

                    if (codigo != null)
                    {
                        codigoStr = codigo.ToString();
                        cartCoad.CODIGO = codigoStr;
                    }
                }

                var lstEmails = cartCoad.EMAILS_PROSP;
                var lstTelefone = cartCoad.TELEFONES_PROSP;


                cartCoad.EMAILS_PROSP = null;
                cartCoad.TELEFONES_PROSP = null;

                SaveOrUpdateNonIdentityKeyEntity(cartCoad);

                cartCoad.EMAILS_PROSP = lstEmails;
                cartCoad.TELEFONES_PROSP = lstTelefone;

                
                if (prospects != null)
                {
                    prospects.CODIGO = codigoStr;
                    _prospectadosSRV.SaveOrUpdateNonIdentityKeyEntity(prospects);
                }

                if (lstEmails != null && lstEmails.Count() > 0)
                {
                    _emailProspSRV.SalvarEmailsProsp(cartCoad);
                }

                if (lstTelefone != null && lstTelefone.Count() > 0)
                {
                    _telefoneProspSRV.SalvarTelefonesProsp(cartCoad);
                }                
            }

           return cartCoad;
        }

        public string PreencherTipoLogradouro(int? TIPO_RUA)
        {	
            switch(TIPO_RUA){
                                    
                    case 1 : return "TRAVESS";
                    case 2 : return "AV"; 
			        case 7 : return "BL";
			        case 10 : return "CAIXA POST";
			        case 15 : return "COND";
			        case 18 : return "ENTRE QUAD";
			        case 19 : return "ESCADA";
			        case 21 : return "Estrada Vi";
			        case 35 : return "PRACA";
			        case 39 : return "R";
			        case 38 : return "ROD";
                    case 42 : return "V";
                    default : return null;

            }			   
        }

        public IList<CartCoadDTO> ListByCliId(int? CLI_ID)
        {
            return _dao.ListByCliId(CLI_ID);
        }

        public CartCoadDTO ObterPorCliente(int? CLI_ID)
        {
            return _dao.ObterPorCliente(CLI_ID);
        }

        public int QtdCartCoadByCliId(int? CLI_ID)
        {
            return _dao.QtdCartCoadByCliId(CLI_ID);
        }

        public bool HasCartCoadByCliId(int? CLI_ID)
        {
            var count = QtdCartCoadByCliId(CLI_ID);
            return (count > 0);
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
            return _dao.BuscarProspects(nome, cnpjCpf, email, ddd, telefone, IdsParaExcluir, pesquisaCpfCnpjPorIqualdade, pagina, registrosPorPagina);
        }

        public CartCoadDTO FindByIdFullLoaded(string codigo, bool preencherEmail = false, bool preencherTelefone = false)
        {
            var cartProspect = FindById(codigo);

            if (cartProspect != null)
            {
                if(preencherTelefone)
                    _telefoneProspSRV.PreencherTelefoneProspect(cartProspect);
                
                if(preencherEmail)
                    _emailProspSRV.PreencherEmailProspect(cartProspect);
            }

            return cartProspect;
        }

    }
}
