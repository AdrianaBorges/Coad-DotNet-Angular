using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using COAD.PROSPECTADOS.Service;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CODIGO")]
    public class cart_coadSRV : GenericService<cart_coad, cart_coadDTO, object>
    {
        private UltimoCodigoLegadoSRV _ultimoCodigoSRV = new UltimoCodigoLegadoSRV();
        private cart_coadDAO _dao = new cart_coadDAO();
        private clienteLegSRV _clienteLegSRV = new clienteLegSRV();

        public cart_coadSRV()
        {
            Dao = _dao;
        }

        public cart_coadDTO SalvarCartCoad(cart_coadDTO cartCoad)
        {
            if (cartCoad != null)
            {
                var cliente = cartCoad.CLIENTES;

                decimal? codigo = _ultimoCodigoSRV.GerarCodigo();

                if (codigo != null)
                {
                    var codigoStr = codigo.ToString();
                    cartCoad.CODIGO = codigoStr;

                    //var lstEmails = cartCoad.EMAILS_PROSP;
                    //var lstTelefone = cartCoad.TELEFONES_PROSP;


                    //cartCoad.EMAILS_PROSP = null;
                    //cartCoad.TELEFONES_PROSP = null;

                    Save(cartCoad);

                    if (cliente != null)
                    {
                        cliente.CODIGO = codigoStr;
                        _clienteLegSRV.Save(cliente);
                    }

                    //if (lstEmails != null && lstEmails.Count() > 0)
                    //{
                    //    _emailProspSRV.SalvarEmails(lstEmails, codigoStr);
                    //}

                    //if (lstTelefone != null && lstTelefone.Count() > 0)
                    //{
                    //    _telefoneProspSRV.SalvarTelefones(lstTelefone, codigoStr);
                    //}

                }
            }

            return cartCoad;
        }

        public string PreencherTipoLogradouro(int? TIPO_RUA)
        {
            switch (TIPO_RUA)
            {

                case 1: return "TRAVESS";
                case 2: return "AV";
                case 7: return "BL";
                case 10: return "CAIXA POST";
                case 15: return "COND";
                case 18: return "ENTRE QUAD";
                case 19: return "ESCADA";
                case 21: return "Estrada Vi";
                case 35: return "PRACA";
                case 39: return "R";
                case 38: return "ROD";
                case 42: return "V";
                default: return null;

            }
        }


        public cart_coadDTO BuscarPrimeiroCartCoadPorCnpjCpf(string cpfCnpj)
        {

            return _dao.BuscarPrimeiroCartCoadPorCnpjCpf(cpfCnpj);
        }

        public cart_coadDTO BuscarPrimeiroCartCoadPorCliId(int? CLI_ID)
        {
            return _dao.BuscarPrimeiroCartCoadPorCliId(CLI_ID);
        }

    }
}
