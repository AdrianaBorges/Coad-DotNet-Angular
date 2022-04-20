using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.IOCContainer.Proxies;
using System.Data.Objects;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects.SqlClient;
using COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate;

namespace COAD.CORPORATIVO.DAO
{
    public class PropostaItemDAO : DAOAdapter<PROPOSTA_ITEM, PropostaItemDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }        

        public PropostaItemDAO()
        {
            //db = GetDb<COADCORPEntities>();
        }

        public IList<PropostaItemDTO> ListarPropostaItemPorProposta(int? prtId)
        {
            var query = (from prItm in db.PROPOSTA_ITEM 
                         where prItm.PRT_ID == prtId && 
                            prItm.DATA_EXCLUSAO == null
                             select prItm);

            return ToDTO(query);
        }

        public ResumoPropostaDTO ObterResumoDaProposta(int? ppiId)
        {
            var resumo = (from 
                            prItm in db.PROPOSTA_ITEM join
                            pro in db.PROPOSTA on prItm.PRT_ID equals pro.PRT_ID
                         where prItm.PPI_ID == ppiId
                         select new ResumoPropostaDTO() { 
                            EmpId = pro.EMP_ID,
                            NomeRepresentante = pro.REPRESENTANTE.REP_NOME,
                            NomeCliente = pro.CLIENTES.CLI_NOME,
                            NomeDoProduto = prItm.PRODUTO_COMPOSICAO.CMP_DESCRICAO,
                            DataDeVencimentoEntrada = prItm.PPI_DATA_VENCIMENTO,
                            DataDeVencimentoParcela = prItm.PPI_DATA_VENCIMENTO_SEG_PARCELA,
                            TipoPagamentoDesc = prItm.TIPO_PAGAMENTO.TPG_DESCRICAO,
                            TpgId = prItm.TIPO_PAGAMENTO.TPG_ID,
                            Quantidade = prItm.PPI_QTD,
                            ValorEntrada = prItm.PPI_VALOR_ENTRADA,
                            ValorParcela = prItm.PPI_VALOR_PARCELA,
                            QuantidadeParcela = prItm.PPI_QTD_PARCELAS,
                            CodigoProposta = pro.PRT_ID,
                            CodigoItemProposta = prItm.PPI_ID,
                            RepId = pro.REP_ID,
                            EmailRepresentante = pro.REPRESENTANTE.REP_EMAIL,
                            Total = prItm.PPI_TOTAL
                         }).FirstOrDefault();

            return resumo;
        }

        public int? RetornarEmpIdDaPropostaItem(int? ppiId)
        {
            var query = (from proItm in 
                             db.PROPOSTA_ITEM 
                         where proItm.PPI_ID == ppiId
                             select proItm.PROPOSTA.EMP_ID)
                             .FirstOrDefault();
            return query;
        }

        public int? ChecaStatusItensIguais(int? PRT_ID)
        {
            var status = (from subItem in db.PROPOSTA_ITEM
                          where 
                          subItem.PRT_ID == PRT_ID &&
                          subItem.DATA_EXCLUSAO == null
                          group subItem.PST_ID by subItem.PST_ID
                              into grp
                              select new CountDTO()
                              {
                                  Label = SqlFunctions.StringConvert((double)grp.Key.Value).Trim(),
                                  Value = grp.Count()
                              }
                             );

            var count = status.Count();

            if (count == 1)
            {
                return int.Parse(status.Select(x => x.Label).FirstOrDefault());
            }

            return null;

        }

        public bool ChecaSeExisteStatus(int? prtId, int? statusId)
        {
            var count = (from prItm in db.PROPOSTA_ITEM
                         where prItm.PRT_ID == prtId &&
                               prItm.PST_ID == statusId
                         select prItm).Count();

            return (count > 0);
        }

        public PropostaItemDTO ListarPropostaItemDaAssinatura(string assinatura)
        {
            var query = (from pro
                            in db.PROPOSTA_ITEM
                         where pro.ASN_NUM_ASSINATURA == assinatura
                         select pro).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<PropostaItemDTO> ListarPropostaItemDeBoleto(int? prtId)
        {
            var query = (from prItm in db.PROPOSTA_ITEM
                         where prItm.PRT_ID == prtId &&
                            prItm.DATA_EXCLUSAO == null &&
                            (
                                prItm.TPG_ID == 7 || 
                                (from tpComp in db.TIPO_PAGAMENTO_COMPOSICAO
                                 where tpComp.TPG_ID_FILHO == 7 && tpComp.TPC_ORDEM == 0
                                 select (int?) tpComp.TPG_ID_PAI).Contains(prItm.TPG_ID)
                            )
                         select prItm);

            return ToDTO(query);
        }

        public PropostaItemDTO FindByRegistroLiberacao(int? rliId)
        {
            var query = (from proItm in db.PROPOSTA_ITEM
                         where proItm.RLI_ID == rliId
                         select proItm).FirstOrDefault();
            return ToDTO(query);
        }

        public IList<ItemRelatorioPropostaEmAtrasoDTO> ListarQTDPropostasItemEmAtrasoFixo(DateTime? dataVenc)
        {
            if (dataVenc != null)
                dataVenc = dataVenc.Value.Date;
            var query = (from
                            proItm in db.PROPOSTA_ITEM join
                            pro in db.PROPOSTA on proItm.PRT_ID equals pro.PRT_ID join
                            tiPag in db.TIPO_PAGAMENTO on proItm.TPG_ID equals tiPag.TPG_ID join 
                            cli in db.CLIENTES on pro.CLI_ID equals cli.CLI_ID join
                            sta in db.PEDIDO_STATUS on proItm.PST_ID equals sta.PST_ID join
                            tpPed in db.TIPO_PROPOSTA on pro.TPP_ID equals tpPed.TPP_ID join
                            cmp in db.PRODUTO_COMPOSICAO on proItm.CMP_ID equals cmp.CMP_ID join
                            rep in db.REPRESENTANTE on pro.REP_ID equals rep.REP_ID
                         where
                            proItm.PST_ID == 1 && 
                            EntityFunctions.TruncateTime(proItm.PPI_DATA_VENCIMENTO) == dataVenc                            
                         select new ItemRelatorioPropostaEmAtrasoDTO()
                         {
                             Assinatura = proItm.ASN_NUM_ASSINATURA,
                             CodCliente = cli.CLI_ID,
                             CodItemProposta = proItm.PPI_ID,
                             CodProposta = pro.PRT_ID,
                             Produto = cmp.CMP_DESCRICAO,
                             Representante = rep.REP_NOME,
                             DataVencimento = proItm.PPI_DATA_VENCIMENTO,
                             CpfCnpj = cli.CLI_CPF_CNPJ,
                             Entrada = proItm.PPI_VALOR_ENTRADA,
                             NomeCliente = cli.CLI_NOME,
                             QtdParcela = proItm.PPI_QTD_PARCELAS,
                             Status = sta.PST_STATUS,
                             TipoDeProposta = tpPed.TPP_DESCRICAO,
                             TipoPagamento = tiPag.TPG_DESCRICAO,
                             ValorParcela = proItm.PPI_VALOR_PARCELA,
                             CodRepresentante = rep.REP_ID
                         });
            return query.ToList();
        }
    }
}