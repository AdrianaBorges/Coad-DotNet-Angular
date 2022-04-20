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
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("MKT_CLI_ID")]
    public class InfoMarketingSRV : GenericService<INFO_MARKETING, InfoMarketingDTO, int>
    {
        public InfoMarketingDAO _dao = new InfoMarketingDAO();
        public AreaInfoMarketingSRV _areaInfoMarketingService = new AreaInfoMarketingSRV();
        private ProdutoComposicaoInfoMarketingSRV _cmpInfoMarketingService = new ProdutoComposicaoInfoMarketingSRV();

        public InfoMarketingSRV()
        {
            this.Dao = _dao;
        }

         /// <summary>
        /// Traz a informação de marketing com as listas populadas baseadas no id do cliente
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <returns></returns>
        public InfoMarketingDTO FindByCliIdFullLoaded(int CLI_ID)
        {
            var infMkt = _dao.FindByCliIdFullLoaded(CLI_ID);

            if(infMkt != null)
                infMkt.PodeEditarOrigem = (infMkt.O_CAD_ID == null);
            
            return infMkt;
        }


        public void PreencherInformacoesDeMarketing(IEnumerable<ClienteDto> clientes)
        {
            if (clientes != null)
            {

                foreach(var cli in clientes){

                    if (cli.CLI_ID != null)
                    {
                        cli.INFO_MARKETING = FindByCliIdFullLoaded((int) cli.CLI_ID);
                    }
                   
                }
            }
        }

        public void PreencherInformacoesDeMarketing(ClienteDto cliente)
        {
            var _list = new List<ClienteDto>() { cliente};
            PreencherInformacoesDeMarketing(_list);
        }

        public void SalvarInfoMarketing(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null && cliente.INFO_MARKETING != null)
            {
                var infoMarketing = cliente.INFO_MARKETING;
                if (infoMarketing.MKT_CLI_ID == null)
                {
                    infoMarketing.MKT_CLI_ID = cliente.CLI_ID;                   
                }
                SalvarInfoMarketing(infoMarketing);
            }
        }

        private InfoMarketingDTO _processarSalvamento(InfoMarketingDTO dto)
        {
            if (dto != null)
            {
                if (FindById(dto.MKT_CLI_ID) != null)
                {
                   return Merge(dto, "MKT_CLI_ID");
                }
                else
                {
                   return Save(dto);
                }
            }

            return null;
        }

        public void SalvarInfoMarketing(InfoMarketingDTO marketingDTO)
        {
            if (marketingDTO == null)
            {
                return;
            }

            if (marketingDTO.MKT_CLI_ID != null)
            { 
                _processarSalvamento(marketingDTO);
                _areaInfoMarketingService.ProcessarExclusaoEAtualizacaoAreaInfoMarketing(marketingDTO);
                _cmpInfoMarketingService.ProcessarExclusaoEAtualizacaoProdutoComposicaoInfoMarketing(marketingDTO);               
                
            }
        }

        public void SalvarVariasInfoMarketing(IEnumerable<ClienteDto> lstClientes)
        {
            if (lstClientes != null)
            {
                var lstInfoMarketing = new List<InfoMarketingDTO>();

                foreach (var cliente in lstClientes)
                {
                    if (cliente.CLI_ID != null && cliente.INFO_MARKETING != null)
                    {
                        var infoMarketing = cliente.INFO_MARKETING;
                        if (infoMarketing.MKT_CLI_ID == null)
                        {
                            infoMarketing.MKT_CLI_ID = cliente.CLI_ID;
                        }

                        lstInfoMarketing.Add(infoMarketing);
                    }
                }

                IncluirVariasInfoMarketing(lstInfoMarketing);
            }
        }

        public void IncluirVariasInfoMarketing(IEnumerable<InfoMarketingDTO> lstMarketingDTO)
        {
            if (lstMarketingDTO == null)
            {
                return;
            }
            IList<ProdutoComposicaoInfoMarketingDTO> lstProdutoComposicaoInfoMarketing = new List<ProdutoComposicaoInfoMarketingDTO>();
            IList<AreaInfoMarketingDTO> lstAreaInfoMarketing = new List<AreaInfoMarketingDTO>();
            
            foreach (var mkt in lstMarketingDTO)
            {
                lstAreaInfoMarketing = _areaInfoMarketingService.ProcessarEConcatenarAreaInfoMarketing(mkt, lstAreaInfoMarketing);
                lstProdutoComposicaoInfoMarketing = _cmpInfoMarketingService.ProcessarEConcatenarAreaInfoMarketing(mkt, lstProdutoComposicaoInfoMarketing);
            }

            foreach (var inf in lstMarketingDTO)
            {
                inf.PRODUTO_COMPOSICAO_INFO_MARKETING = null;
                inf.AREA_INFO_MARKETING = null;
            }

            SaveOrUpdateNonIdentityKeyEntity(lstMarketingDTO, null, null, true);

            _areaInfoMarketingService.SaveOrUpdateNonIdentityKeyEntity(lstAreaInfoMarketing, null, null, true);
            _cmpInfoMarketingService.SaveOrUpdateNonIdentityKeyEntity(lstProdutoComposicaoInfoMarketing, null, null, true);

        }
        

    

    }
}
