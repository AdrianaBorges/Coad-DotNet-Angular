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
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CMP_ID", "MKT_CLI_ID")]
    public class ProdutoComposicaoInfoMarketingSRV : GenericService<PRODUTO_COMPOSICAO_INFO_MARKETING, ProdutoComposicaoInfoMarketingDTO, int>
    {
        public ProdutoComposicaoInfoMarketingDAO _dao = new ProdutoComposicaoInfoMarketingDAO();

        public ProdutoComposicaoInfoMarketingSRV()
        {
            this.Dao = _dao;
        }

        public ProdutoComposicaoInfoMarketingDTO FindProdutoComposicaoInfoMarketing(int MKT_CLI_ID, int CMP_ID)
        {
            return _dao.FindProdutoComposicaoInfoMarketing(MKT_CLI_ID, CMP_ID);
        }

        public bool HasProdutoComposicaoInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
            return _dao.HasProdutoComposicaoInfoMarketing(MKT_CLI_ID, AREA_ID);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="regiaoTabPreco"></param>
        /// <param name="excecoes"></param>
        public void ExcluirProdutoComposicaoInfoMarketing(InfoMarketingDTO infoMarketing)
        {

            var MKT_CLI_ID = (int)infoMarketing.MKT_CLI_ID;

            if (infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING != null)
            {

                var excecoes = infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING;
                var infoMarketingOriginal = new InfoMarketingSRV().FindByCliIdFullLoaded(MKT_CLI_ID);

                if (infoMarketingOriginal != null)
                {
                    var produtoComposicaoInfoMarketing = infoMarketingOriginal.PRODUTO_COMPOSICAO_INFO_MARKETING;

                    if (produtoComposicaoInfoMarketing != null && excecoes != null)
                    {
                        var produtoComposicaoInfoMarketingPraExcluir = produtoComposicaoInfoMarketing.Except(excecoes, new ProdutoComposicaoInfoMarketingDTOComparator());

                        if (produtoComposicaoInfoMarketingPraExcluir != null && produtoComposicaoInfoMarketingPraExcluir.Count() > 0)
                        {
                            DeleteAll(produtoComposicaoInfoMarketingPraExcluir, "CMP_ID", "MKT_CLI_ID");
                        }

                    }
                }
                
            }
        }

        public void SalvarProdutoComposicaoInfoMarketing(IEnumerable<ProdutoComposicaoInfoMarketingDTO> lstProdutoComposicaoInfoMarketing, int MKT_CLI_ID)
        {
            if (lstProdutoComposicaoInfoMarketing != null)
            {
                IList<ProdutoComposicaoInfoMarketingDTO> lstAtualizar = new List<ProdutoComposicaoInfoMarketingDTO>();
                IList<ProdutoComposicaoInfoMarketingDTO> lstSalvar = new List<ProdutoComposicaoInfoMarketingDTO>();

                foreach (var produtoComposicaoInfoMarketing in lstProdutoComposicaoInfoMarketing)
                {
                    // var MKT_CLI_ID = infoAreaMarketing.MKT_CLI_ID;
                    produtoComposicaoInfoMarketing.MKT_CLI_ID = MKT_CLI_ID;

                    var CMP_ID = produtoComposicaoInfoMarketing.CMP_ID;

                    if (HasProdutoComposicaoInfoMarketing((int)MKT_CLI_ID, (int)CMP_ID))
                    {
                        lstAtualizar.Add(produtoComposicaoInfoMarketing);
                    }
                    else
                    {
                        lstSalvar.Add(produtoComposicaoInfoMarketing);
                    }
                }

                MergeAll(lstAtualizar, true, "CMP_ID", "MKT_CLI_ID");
                SaveAll(lstSalvar);
            }
        }

        private void _processarChaves(int? MTK_CLI_ID, IEnumerable<ProdutoComposicaoInfoMarketingDTO> lstProdutoComposicaoInfoMarketing)
        {
            if (lstProdutoComposicaoInfoMarketing != null)
            {
                foreach (var produtoComposicaoInfoMarketing in lstProdutoComposicaoInfoMarketing)
                {
                    if (produtoComposicaoInfoMarketing.MKT_CLI_ID == null)
                    {
                        produtoComposicaoInfoMarketing.MKT_CLI_ID = MTK_CLI_ID;
                    }

                    if (produtoComposicaoInfoMarketing.CMP_ID == null)
                    {
                        produtoComposicaoInfoMarketing.CMP_ID = produtoComposicaoInfoMarketing.PRODUTO_COMPOSICAO.CMP_ID;
                    }
                }
            }
        }

        public IList<ProdutoComposicaoInfoMarketingDTO> ProcessarEConcatenarAreaInfoMarketing(InfoMarketingDTO marketingDTO, IList<ProdutoComposicaoInfoMarketingDTO> lstAcumulada)
        {
            if (marketingDTO != null && marketingDTO.PRODUTO_COMPOSICAO_INFO_MARKETING != null && lstAcumulada != null)
            {
                var lstProInfoMkt = marketingDTO.PRODUTO_COMPOSICAO_INFO_MARKETING.ToList();
                _processarChaves(marketingDTO.MKT_CLI_ID, lstProInfoMkt);

                lstAcumulada = lstAcumulada.Concat(lstProInfoMkt).ToList();
            }

            return lstAcumulada;

        }
        
        public void ProcessarExclusaoEAtualizacaoProdutoComposicaoInfoMarketing(InfoMarketingDTO marketingDTO)
        {
            if (marketingDTO.MKT_CLI_ID != null)
            {
                _processarChaves((int)marketingDTO.MKT_CLI_ID, marketingDTO.PRODUTO_COMPOSICAO_INFO_MARKETING);
                ExcluirProdutoComposicaoInfoMarketing(marketingDTO);
                SalvarProdutoComposicaoInfoMarketing(marketingDTO.PRODUTO_COMPOSICAO_INFO_MARKETING, (int)marketingDTO.MKT_CLI_ID);
            }
        }

    }
}
