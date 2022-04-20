

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Enumerados;
using GenericCrud.Util;
using COAD.FISCAL.Model.NFSe;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("NF_ID", "NF_TIPO", "NF_NUMERO", "NF_SERIE", "PRO_ID")]

    public class NotaFiscalItemSRV : GenericService<NOTA_FISCAL_ITEM, NotaFiscalItemDTO, object>
	{

        public NotaFiscalItemDAO _dao { get; set; }

        public NotaFiscalItemSRV()
        {
            _dao = new NotaFiscalItemDAO();
        }


        public NotaFiscalItemSRV(NotaFiscalItemDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

		public ICollection<NotaFiscalItemDTO> SalvarItens(NotaFiscalDTO nota, InfoNfeDTO infoNFe)
        {
            if(nota != null && nota.NF_ID != 0 && infoNFe != null)
            {
                var lstNotasItens = new List<NotaFiscalItemDTO>();

                foreach(var ntItem in infoNFe.Detalhamento)
                {
                    int proID = 0;
                    int.TryParse(ntItem.Produto.CodigoProduto, out proID);

                    var notaItem = new NotaFiscalItemDTO()
                    {
                        NF_ID = nota.NF_ID,
                        NF_TIPO = nota.NF_TIPO,
                        NF_NUMERO = nota.NF_NUMERO,
                        NF_SERIE = nota.NF_SERIE,
                        PRO_ID = proID,
                        NFI_VLR_UNIT = ntItem.Produto.ValorUnitario,
                        NFI_VLR_TOTAL = ntItem.Produto.ValorTotal,
                        NFI_BASE_CALC_ICMS = (ntItem.Imposto.ICMS.ICMS40.CST == 6) ? 0 : 0, // TODO: Implementar os valores do imposto
                        NFI_VLR_IPI = (ntItem.Imposto.IPI.IPINaoTributado.CST == TipoTributacaoIPIEnum.SaidaNaoTributada) ? 0 : 0,
                        NFI_ALIQ_ICMS = 0,
                        NFI_ALIQ_IPI = 0,
                        CFOP = (ntItem.Produto.CFOP != null) ? ntItem.Produto.CFOP.ToString() : null,
                        NFI_UN = StringUtil.Truncate(ntItem.Produto.Unidade, 2),
                        NFI_PRO_NOME = ntItem.Produto.NomeProduto,
                        CST_ID = (ntItem.Imposto.ICMS.ICMS40.CST != null) ? ntItem.Imposto.ICMS.ICMS40.Origem.ToString() + ntItem.Imposto.ICMS.ICMS40.CST.ToString() : null,
                        NCM_ID = ntItem.Produto.NCM,
                        NFI_QTDE = ntItem.Produto.Quantidade
                    };
                    lstNotasItens.Add(notaItem);
                }

                return SaveAll(lstNotasItens).ToList();
            }
            return new List<NotaFiscalItemDTO>();
        }

        public ICollection<NotaFiscalItemDTO> SalvarItensNfse(NotaFiscalDTO nota, InfNfse infNFe, int proID = 0)
        {
            if (nota != null && nota.NF_ID != 0 && infNFe != null)
            {                
                var notaItem = new NotaFiscalItemDTO()
                {
                    NF_ID = nota.NF_ID,
                    NF_TIPO = nota.NF_TIPO,
                    NF_NUMERO = nota.NF_NUMERO,
                    NF_SERIE = nota.NF_SERIE,
                    PRO_ID = proID,
                    NFI_VLR_TOTAL = infNFe.Servico.Valores.ValorServicos,
                    NFI_DISCRIMINACAO_SERVICO = infNFe.Servico.Discriminacao,
                    NFI_QTDE = 1
                };

                var notaSalva = Save(notaItem);
                return new List<NotaFiscalItemDTO>() {notaSalva};
            }
            return new List<NotaFiscalItemDTO>();
        }



        public void ChecarExcluirPropostaItemAusentes(NotaFiscalDTO notaFiscal)
        {
            var propostaSRV = ServiceFactory.RetornarServico<PropostaSRV>();

            if (notaFiscal != null)
            {
                var objetoDoBanco = ServiceFactory.RetornarServico<NotaFiscalSRV>().FindByIdFullLoaded(notaFiscal.NF_ID, true);
                var lstParaExcluir = GetMissinList(objetoDoBanco.NOTA_FISCAL_ITEM, notaFiscal.NOTA_FISCAL_ITEM);

                ExcluidoNotaFiscalItem(lstParaExcluir);
            }
        }

        public IList<NotaFiscalItemDTO> ListarNotaFiscalItensPorNota(int nfId)
        {
            return _dao.ListarNotaFiscalItensPorNota(nfId);
        }

        public void PreencherNotaFiscalItemNaNota(NotaFiscalDTO notaFiscal)
        {
            if(notaFiscal != null && notaFiscal.NF_ID > 0)
            {
                notaFiscal.NOTA_FISCAL_ITEM = ListarNotaFiscalItensPorNota(notaFiscal.NF_ID);
            }
        }

        public void ExcluidoNotaFiscalItem(IEnumerable<NotaFiscalItemDTO> lstNotaItem)
        {
            if (lstNotaItem != null)
            {
                DeleteAll(lstNotaItem);
            }
        }

        public void SalvarNotaFiscalItem(NotaFiscalDTO notaFiscal)
        {
            if (notaFiscal != null)
            {
                var lstNotaFiscalItem = notaFiscal.NOTA_FISCAL_ITEM;

                if(lstNotaFiscalItem != null && lstNotaFiscalItem.Count() > 0)
                {
                    var produtoCmpSRV = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>();
                    foreach(var itm in lstNotaFiscalItem)
                    {
                        if(itm.CMP_ID != null)
                        {
                            var proId = produtoCmpSRV.ObterProIdParaGerarAssinatura(itm.CMP_ID);
                            if(proId != null)
                            {
                                itm.PRO_ID = (int)proId;
                            }
                        }
                    }
                }
                CheckAndAssignKeyFromParentToChildsList(notaFiscal, lstNotaFiscalItem, "NF_ID", "NF_TIPO", "NF_NUMERO", "NF_SERIE");
                ChecarExcluirPropostaItemAusentes(notaFiscal);
                SaveOrUpdateNonIdentityKeyEntity(lstNotaFiscalItem);
            }
        }
    }
}
