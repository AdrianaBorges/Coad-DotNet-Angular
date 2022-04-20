using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.DAO;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Transactions;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.CORPORATIVO.Service.Base;
using COAD.CORPORATIVO.Service.Mundipagg;
using MundiAPI.PCL.Models;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CMP_ID")]
    public class ProdutoComposicaoSRV : GenericService<PRODUTO_COMPOSICAO, ProdutoComposicaoDTO, int>
    {
        private ProdutoComposicaoDAO _dao;

        [ServiceProperty("CMP_ID", Name = "areaConsultoriaCurso", PropertyName = "AREA_CONSULTORIA_CURSO")]
        public AreaConsultoriaCursoSRV areaConsultoriaSRV { get; set; }

        [ServiceProperty("CMP_ID", Name = "produtoComposicaoTipoPeriodo", PropertyName = "PRODUTO_COMPOSICAO_TIPO_PERIODO")]
        public ProdutoComposicaoTipoPeriodoSRV _produtoComposicaoTipoPeriodoSRV { get; set; }
        public EmpresaSRV EmpresaSRV { get; set; }
        public ImpostoSRV _impostoSRV { get; set; }
        public PlanoMundipaggSRV _planoMundipaggSRV { get;set;}
        public PlanoItemMundipaggSRV _planoItemMundipaggSRV { get; set; }
        public ProdutosSRV _produtosSRV { get; set; }



        public ProdutoIntegracaoDTO BuscarProdutoComposicaoAtivoPorProdutoId(int produtoId)
        {
            var produtoIntegracao = new ProdutoIntegracaoDTO();

            var produtoCompList = _dao.BuscarProdutoComposicaoAtivoPorProdutoId(produtoId);
            if (produtoCompList == null)
            {
                throw new Exception("Não existe produto composição para este produto informado.");
            }

            var prodCompIntegrList = new List<ProdutoComposicaoIntegracaoDTO>();

            for (int i=0; i< produtoCompList.Count; i++)
            {
                var produtoComposicao = produtoCompList[i];

                var produtoComposicaoIntegracao = new ProdutoComposicaoIntegracaoDTO();
                produtoComposicaoIntegracao.prodCompId = produtoComposicao.CMP_ID;
                produtoComposicaoIntegracao.prodCompDesc = produtoComposicao.CMP_DESCRICAO;
                produtoComposicaoIntegracao.prodCompValVend = produtoComposicao.CMP_VLR_VENDA;

                prodCompIntegrList.Add(produtoComposicaoIntegracao);
            }

            produtoIntegracao.prodId = produtoId;
            produtoIntegracao.prodCompIntegrList = prodCompIntegrList;
            return produtoIntegracao;
        }

        public ProdutoComposicaoSRV()
        {
            this._dao = new ProdutoComposicaoDAO();
            this.areaConsultoriaSRV = new AreaConsultoriaCursoSRV();
            this._produtoComposicaoTipoPeriodoSRV = new ProdutoComposicaoTipoPeriodoSRV();

            Dao = _dao;
        }

        public ProdutoComposicaoSRV(ProdutoComposicaoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public IList<PRODUTO_COMPOSICAO> BuscarTodos()
        {
            return _dao.FindAll();
        }

        public ProdutoComposicaoDTO ObterProdutoRenovacao (int? pro_id)
        {
            return _dao.ObterProdutoRenovacao(pro_id);
        }

        public Pagina<ProdutoComposicaoDTO> ProdutosComposicoes(string nome = null,
            string nomeEstrangeiro = null, string nomeProduto = null, int pagina = 1, int itensPorPaginas = 7, bool? produtoInteresse = null, int? proId = null)
        {
            return _dao.ProdutosComposicoes(nome, nomeEstrangeiro, nomeProduto, pagina, itensPorPaginas, produtoInteresse, proId);
        }

        /// <summary>
        /// Inclui um novo produto se ele não contiver um ID,
        /// Se ele possuir um ID ele será alterado
        /// </summary>
        //public void SalvarProdutoComposicao(ProdutoComposicaoDTO produtoComposicao)
        //{
        //    if (produtoComposicao.CMP_ID != null)
        //    {
        //        AtualizarProdutoComposicao(produtoComposicao);
        //    }
        //    else
        //    {
        //        Save(produtoComposicao);
        //    }
        //}


        public ProdutoComposicaoDTO SalvarProdutoComposicao(ProdutoComposicaoDTO produtoComposicao)
        {
            ProdutoComposicaoDTO composicaoSalvaRetorno = null;

            if (produtoComposicao != null)
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    var lstProdutoComposicaoTipoPeriodo = produtoComposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO;
                    produtoComposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO = null;

                    if (produtoComposicao.PRODUTO_COMPOSICAO2 != null && produtoComposicao.CMP_ID_ORIGEM == null)
                    {
                        produtoComposicao.CMP_ID_ORIGEM = produtoComposicao.PRODUTO_COMPOSICAO2.CMP_ID;
                    }

                    if (produtoComposicao.PRODUTO_COMPOSICAO2 == null)
                    {
                        produtoComposicao.CMP_ID_ORIGEM = null;
                    }
                    else
                    {
                        produtoComposicao.PRODUTO_COMPOSICAO2 = null;
                    }
                    var composicaoSalva = SaveOrUpdate(produtoComposicao);

                    composicaoSalva.PRODUTO_COMPOSICAO_ITEM = produtoComposicao.PRODUTO_COMPOSICAO_ITEM;
                    composicaoSalva.PRODUTO_COMPOSICAO_TIPO_PERIODO = lstProdutoComposicaoTipoPeriodo;

                    var service = new ProdutoComposicaoItemSRV();

                    service.DeletarComposicaoItens(composicaoSalva);

                    service.SalvarItensComposicao(composicaoSalva, composicaoSalva.PRODUTO_COMPOSICAO_ITEM);
                    _produtoComposicaoTipoPeriodoSRV.SalvarEExcluirProdutoComposicaoTipoPeriodo(composicaoSalva);


                    if (!string.IsNullOrEmpty(composicaoSalva.CMP_MUNDIPAGG_PLANO_ID))
                    {
                        using (TransactionScope scopeTwo = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            _planoMundipaggSRV.RemoverPlano(composicaoSalva.CMP_MUNDIPAGG_PLANO_ID);
                        }
                    }

                    if (composicaoSalva.TPV_ID == 1)
                    {
                        if (composicaoSalva.PRODUTO_COMPOSICAO_ITEM == null)
                        {
                            throw new Exception("Para produto composição marcado como venda online, é necessário cadastrar ao menos um produto composição item.");
                        }
                        if (composicaoSalva.PRODUTO_COMPOSICAO_ITEM.Count == 0)
                        {
                            throw new Exception("Para produto composição marcado como venda online, é necessário cadastrar ao menos um produto composição item.");
                        }

                        var produtoComposicaoItens = composicaoSalva.PRODUTO_COMPOSICAO_ITEM.ToList();
                        var planItemRequestList = new List<CreatePlanItemRequest>();
                        for (int i = 0; i < produtoComposicaoItens.Count; i++)
                        {
                            var produtoComposicaoItemDTO = produtoComposicaoItens[i];
                            var produtoDTO = _produtosSRV.FindById(produtoComposicaoItemDTO.PRO_ID);
                            decimal cmiPrecoUnit = produtoComposicaoItemDTO.CMI_PRECO_UNIT.GetValueOrDefault(0);

                            var planItem = new CreatePlanItemRequest
                            {
                                Name = produtoDTO.PRO_NOME,
                                Cycles = produtoComposicaoItemDTO.CMI_QTDE_PERIODO,
                                Quantity = produtoComposicaoItemDTO.CMI_QTDE.GetValueOrDefault(0),
                                PricingScheme = new CreatePricingSchemeRequest
                                {
                                    Price = Decimal.ToInt32(cmiPrecoUnit)
                                }
                            };

                            planItemRequestList.Add(planItem);
                        }

                        List<GetPlanItemResponse> getPlanItemResponses = new List<GetPlanItemResponse>();
                        
                        var metadata = new Dictionary<string, string>();
                        metadata.Add("CMP_ID", composicaoSalva.CMP_ID.ToString());
                        var planoMundipagg = new CreatePlanRequest
                        {
                            Name = composicaoSalva.CMP_DESCRICAO,
                            Description = composicaoSalva.CMP_DESCRICAO,
                            PaymentMethods = new List<string> { "credit_card" },
                            Installments = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 },
                            MinimumPrice = Decimal.ToInt32(composicaoSalva.CMP_VLR_VENDA.GetValueOrDefault(0)),
                            Currency = "BRL",
                            Interval = "month",
                            IntervalCount = 1,
                            BillingType = "exact_day",
                            BillingDays = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 },
                            Items = planItemRequestList,
                            Metadata = metadata,
                        };

                        using (TransactionScope scopeTwo = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            var getPlanResponse = _planoMundipaggSRV.CadastrarPlano(planoMundipagg);
                            composicaoSalva.CMP_MUNDIPAGG_PLANO_ID = getPlanResponse.Id;

                            scopeTwo.Complete();
                        }
                        SaveOrUpdate(composicaoSalva);

                    }

                    if (produtoComposicao.CMP_ID == null)
                    {
                        composicaoSalvaRetorno = composicaoSalva;
                    }

                    scope.Complete();
                }
            }

            return composicaoSalvaRetorno;
        }

        //public void AtualizarProdutoComposicao(ProdutoComposicaoDTO produtoComposicao)
        //{

        //    if (produtoComposicao != null && produtoComposicao.CMP_ID != null)
        //    {
        //        //using (TransactionScope tx = new TransactionScope())
        //        //{
        //            Merge(produtoComposicao, "CMP_ID");
        //            var service = new ProdutoComposicaoItemSRV();
        //            var produtoComposicaoDoBanco = FindById((int) produtoComposicao.CMP_ID);
        //            service.DeletarComposicaoItens(produtoComposicaoDoBanco);
        //            service.CriarItensComposicao((int) produtoComposicao.CMP_ID, produtoComposicao.PRODUTO_COMPOSICAO_ITEM);
        //        //}
        //    }
        //}

        public void DeletarProdutoComposicao(int? cmpId)
        {
            var obj = FindById(cmpId);
            obj.DATA_EXCLUSAO = DateTime.Now;
            SaveOrUpdate(obj);
        }

        public IList<ProdutoComposicaoDTO> FindAllValid()
        {
            return _dao.FindAllValid();
        }

        /// <summary>
        /// Pega o produto composição com listas populados.
        /// </summary>
        /// <param name="CMP_ID"></param>
        /// <returns></returns>
        public ProdutoComposicaoDTO FindByIdFullLoad(int CMP_ID, bool obterAreaConsultoriaCurso = false, bool obterProdutoComposicaoTipoPeriodo = false)
        {
            var tabelaPreco = new TabelaPrecoSRV();

            var produtoComposicao = FindById(CMP_ID);
            if (produtoComposicao != null && produtoComposicao.PRODUTO_COMPOSICAO_ITEM != null)
            {
                foreach (var proComItem in produtoComposicao.PRODUTO_COMPOSICAO_ITEM)
                {
                    proComItem.PRODUTO_COMPOSICAO = null;
                }
            }

            produtoComposicao.TABELA_PRECO = tabelaPreco.GetTabelaPrecoByComposicao(CMP_ID);

            if (obterAreaConsultoriaCurso)
            {
                GetAssociations(produtoComposicao, "areaConsultoriaCurso");
            }

            if (obterProdutoComposicaoTipoPeriodo)
            {
                GetAssociations(produtoComposicao, "produtoComposicaoTipoPeriodo");
            }

            PreencherProdutoCompostoOrigem(produtoComposicao);

            return produtoComposicao;

        }

        /// <summary>
        /// Pega todos os produtos composição de interesse
        /// </summary>
        /// <returns></returns>
        public IList<ProdutoComposicaoDTO> ProdutosDeInteresse(string descricao = null)
        {
            return _dao.ProdutosDeInteresse(descricao);
        }

        public Pagina<ProdutoComposicaoDTO> ListarCursos(string descricao, bool? produtoInteresse = null, int pagina = 1, int registrosPorPagina = 7, int? empId = null)
        {
            var paginaCurso = _dao.ListarCursos(descricao, produtoInteresse, pagina, registrosPorPagina, empId);

            _marcarProdutoComposicaoComoCurso(paginaCurso.lista);
            PreencherEmpresa(paginaCurso.lista);

            return paginaCurso;
        }

        /// <summary>
        /// Marca a flag de curso em todos os produtos da lista
        /// </summary>
        /// <param name="lstProdutos"></param>
        private void _marcarProdutoComposicaoComoCurso(IEnumerable<ProdutoComposicaoDTO> lstProdutos)
        {
            if (lstProdutos != null)
            {
                foreach (var prod in lstProdutos)
                {
                    prod.EhCurso = true;
                }
            }
        }

        public IList<ProdutoTabelaPrecoDTO> FindByProdOrigem(int _cmp_origem, int _ttp_id = 1, int _tpg_id = 9)
        {
            // _ttp_id => 1 Mensal Recorrente
            // _ttp_id => 6 Anual

            return _dao.FindByProdOrigem(_cmp_origem, _ttp_id, _tpg_id);
        }

        public Pagina<ProdutoComposicaoDTO> ListarProdutosExcetoCursos(string descricao, bool? produtoInteresse = null, int pagina = 1, int registrosPorPagina = 7, int? empId = null)
        {
            var lstProdutos = _dao.ListarProdutosExcetoCursos(descricao, produtoInteresse, pagina, registrosPorPagina, empId);
            PreencherProdutoCompostoOrigem(lstProdutos.lista);
            PreencherEmpresa(lstProdutos.lista);

            return lstProdutos;
        }

        public bool ChecaProdutoComposicaoEhCurso(int cmdId)
        {
            return _dao.ChecaProdutoComposicaoEhCurso(cmdId);
        }

        public void ChecaEMarcaProdutoCurso(ProdutoComposicaoDTO produtoComposicao)
        {
            if (produtoComposicao != null)
            {
                var cmpId = produtoComposicao.CMP_ID;

                if (cmpId != null)
                {
                    produtoComposicao.EhCurso = ChecaProdutoComposicaoEhCurso((int)cmpId);
                }
            }
        }


        public void PreencherProdutoCompostoOrigem(IEnumerable<ProdutoComposicaoDTO> lstProdutosComposicaoFilho)
        {
            if (lstProdutosComposicaoFilho != null)
            {
                foreach (var pro in lstProdutosComposicaoFilho)
                {
                    PreencherProdutoCompostoOrigem(pro);
                }
            }
        }



        public void PreencherProdutoCompostoOrigem(ProdutoComposicaoDTO produtoComposicaoFilho)
        {
            if (produtoComposicaoFilho != null && produtoComposicaoFilho.CMP_ID_ORIGEM != null)
            {
                var cmpId = produtoComposicaoFilho.CMP_ID_ORIGEM;
                var produtoCompostoDeOrigem = FindById(cmpId);
                produtoComposicaoFilho.PRODUTO_COMPOSICAO2 = produtoCompostoDeOrigem;
            }
        }

        public int? RetornaQuantidadeDeConsultasDoProdutoComposto(int? cmpId)
        {
            var quantidade = _dao.RetornaQuantidadeDeConsultasDoProdutoComposto(cmpId);
            if (quantidade == null)
            {
                var prod = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>()
                    .ObterProdutoComposicaoItemQueGeraAssinatura(cmpId);
                
                if (prod != null)
                {
                    var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(prod.PRO_ID);

                    quantidade = produto.PRO_QTD_CONSULTA_PADRAO;
                }
            }

            return quantidade;
        }

        public ProdutoComposicaoDTO ObterProdutoDeInteressePorNome(string descricao)
        {
            return _dao.ObterProdutoDeInteressePorNome(descricao);
        }

        public ProdutoComposicaoDTO ObterProdutoPorNome(string descricao)
        {
            return _dao.ObterProdutoPorNome(descricao);
        }

        /// <summary>
        /// Tentar encontrar um produto composto que conceda ao cliente o mesmo acesso que a assinatura informa como produto e quantidade de consulta.
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="codAssinatura"></param>
        /// <returns></returns>
        public RequisicaoProdutoRenovacaoDTO RetornarProdutoCompostoSimilar(int? proId, string codAssinatura, int? empId)
        {
            return _dao.RetornarProdutoCompostoSimilar(proId, codAssinatura, empId);
        }

        /// <summary>
        /// Tenta recuperar, baseado na Assinatura, qual o produto composto será usado para renovar.
        /// </summary>
        /// <param name="codAssinatura"></param>
        /// <returns></returns>
        public RequisicaoProdutoRenovacaoDTO RetornarDadosDeRenovacaoDoProdutoComposicao(string codAssinatura, int? empId)
        {
            if (!string.IsNullOrWhiteSpace(codAssinatura))
            {
                var assiSRV = ServiceFactory.RetornarServico<AssinaturaSRV>();
                var assi = assiSRV.FindById(codAssinatura);

                if(assi == null)
                {
                    throw new Exception(string.Format("Não é possível encontrar a assinatura '{0}'", codAssinatura));
                }
                var contrato = ServiceFactory.RetornarServico<ContratoSRV>().BuscarUltimoObjetoContrato(codAssinatura);
                int? codProduto = (assi.PRO_ID != null) ? assi.PRO_ID : int.Parse(codAssinatura.Substring(0, 2));
                var qtdConsulta = assi.ASN_QTDE_CONS_CONTRATO;

                if (contrato == null)
                    throw new Exception("Não existe nenhum contrato nessa assinatura.");

                if(qtdConsulta < 1)
                {
                    var consultaIndividual = ServiceFactory.RetornarServico<BloqueiaConsultaIndividualSRV>().ConsultarPrimeiroPorAssinatura(codAssinatura);
                    if(consultaIndividual != null &&
                        consultaIndividual.qtd_consulta_sem != null &&
                        consultaIndividual.qtd_consulta_sem > 0)
                    {
                        qtdConsulta = consultaIndividual.qtd_consulta_sem.Value;
                    }
                    else
                    {
                        var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(codProduto);
                        if(produto != null && 
                            produto.PRO_QTD_CONSULTA_PADRAO != null &&
                            produto.PRO_QTD_CONSULTA_PADRAO > 0)
                        {
                            qtdConsulta = produto.PRO_QTD_CONSULTA_PADRAO.Value;
                        }
                    }

                    if(qtdConsulta > 0 && assi.ASN_QTDE_CONS_CONTRATO != qtdConsulta)
                    {
                        assi.ASN_QTDE_CONS_CONTRATO = qtdConsulta;
                        assiSRV.Merge(assi);
                    }

                }

                if (contrato != null 
                    && contrato.PRODUTO_COMPOSICAO != null 
                    && codProduto != null 
                    && contrato.PRODUTO_COMPOSICAO.EMP_ID == empId)
                {
                    RequisicaoProdutoRenovacaoDTO requisicao = new RequisicaoProdutoRenovacaoDTO()
                    {
                        CmpId = contrato.CMP_ID,
                        ProdutoComposicao = contrato.PRODUTO_COMPOSICAO,
                        NumeroAssinatura = codAssinatura,
                        ProId = codProduto,
                        QtdConsultas = assi.ASN_QTDE_CONS_CONTRATO,
                        ValorDaVenda = contrato.CTR_VLR_CONTRATO
                    };

                    return requisicao;

                }
                else
                {
                    var requisicaoRenovacao = RetornarProdutoCompostoSimilar(codProduto, codAssinatura, empId);
                    if (requisicaoRenovacao != null)
                    {
                        requisicaoRenovacao.ProdutoComposicao = FindById(requisicaoRenovacao.CmpId);
                        if(requisicaoRenovacao.QtdConsultas == null || requisicaoRenovacao.QtdConsultas == 0)
                        {
                            requisicaoRenovacao.QtdConsultas = qtdConsulta;
                        }
                        if(contrato.CTR_VLR_CONTRATO != null)
                        {
                            requisicaoRenovacao.ValorDaVenda = contrato.CTR_VLR_CONTRATO;
                        }
                        return requisicaoRenovacao;
                    }
                }

                return null;
            }
            else
            {
                throw new ArgumentNullException("Não é possível retornar o produto para renovação. Informe uma assinatura.");
            }
        }

        public Pagina<ProdutoComposicaoDTO> ListarProdutosPorUen(int? uenId, string descricao, bool? produtoInteresse = null, int pagina = 1, int registrosPorPagina = 7, int? empId = null)
        {
            var lstProdutos = new Pagina<ProdutoComposicaoDTO>();
            if (uenId != null)
            {
                if(uenId == 1)
                {
                    lstProdutos = ListarCursos(descricao, produtoInteresse, pagina, registrosPorPagina, empId);
                }
                else if(uenId == 2)
                {
                    lstProdutos = ListarProdutosExcetoCursos(descricao, produtoInteresse, pagina, registrosPorPagina, empId);
                }
            }             
            PreencherProdutoCompostoOrigem(lstProdutos.lista);
            PreencherEmpresa(lstProdutos.lista);

            return lstProdutos;
        }

        public void PreencherEmpresa(IEnumerable<ProdutoComposicaoDTO> lstProdutoComposicao)
        {
            if (lstProdutoComposicao != null)
            {
                foreach (var pro in lstProdutoComposicao)
                {
                    if (pro.EMP_ID != null)
                    {
                        pro.EMPRESAS = EmpresaSRV.FindById(pro.EMP_ID);
                    }
                }
            }
        }

        public void PreencherEmpresa(ProdutoComposicaoDTO cmp)
        {
            if(cmp != null && cmp.EMP_ID != null)
            {
                cmp.EMPRESAS = EmpresaSRV.FindById(cmp.EMP_ID);
            }
        }

        public Pagina<ProdutoEcommerceDTO> ListarProdutosVitrine(string nomeProduto = null, int pagina = 1, int registrosPorPagina = 12, int? cliId = null)
        {
            var resposta = _dao.ListarProdutosVitrine(nomeProduto, pagina, registrosPorPagina);

            if(resposta != null && resposta.numeroRegistros > 0)
            {
                var lstProdutoEcommerce = ConverterParaProdutoEcommerce(resposta.lista, cliId);
                var paginaResp = resposta.Derivar(lstProdutoEcommerce);

                return paginaResp;
            }
            return null;
        }

        private IList<ProdutoEcommerceDTO> ConverterParaProdutoEcommerce(IEnumerable<ProdutoComposicaoDTO> lstProdutoComposicao, int? cliId = null)
        {
            IList<ProdutoEcommerceDTO> lstResult = new List<ProdutoEcommerceDTO>();

            if(lstProdutoComposicao != null)
            {
                foreach(var cmp in lstProdutoComposicao)
                {
                    var infoFatura = _impostoSRV.CalcularDescontoProduto(cmp, null, cliId);
                    lstResult.Add(new ProdutoEcommerceDTO()
                    {
                        infoFatura = infoFatura,
                        ProdutoOriginal = cmp,
                        ValorVenda = (infoFatura != null) ? infoFatura.IFF_TOTAL_LIQUIDO : cmp.CMP_VLR_VENDA
                    });
                }
            }

            return lstResult;
        }

        public ProdutoEcommerceDTO BuscarProduto( int id = 0, int idCli = 0 )
        {
            var resposta = _dao.BuscarProduto(id);

            if (resposta != null)
            {

                var produto = ConverterParaProdutoEcommerce (resposta, idCli);

                return produto;

            }

            return null;
        }

        private ProdutoEcommerceDTO ConverterParaProdutoEcommerce(ProdutoComposicaoDTO produto, int idCli = 0)
        {
            ProdutoEcommerceDTO result = new ProdutoEcommerceDTO();

            if (produto != null)
            {

                InfoFaturaDTO infoFatura;
                
                if (idCli > 0)
                    infoFatura = _impostoSRV.CalcularDescontoProduto(produto, null, idCli);
                else
                    infoFatura = _impostoSRV.CalcularDescontoProduto(produto, null, null);

                result.infoFatura = infoFatura;
                result.ProdutoOriginal = produto;
                result.ValorVenda = (infoFatura != null) ? infoFatura.IFF_TOTAL_LIQUIDO : result.ProdutoOriginal.CMP_VLR_VENDA;

            }

            return result;
        }

    }
}
