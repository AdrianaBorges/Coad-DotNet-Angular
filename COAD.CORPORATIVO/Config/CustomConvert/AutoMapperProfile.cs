using AutoMapper;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Config.CustomConvert
{
    public class AutoMapperProfile : GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            var mPrepedidoStatus = store.CreateMap<PRE_PEDIDO_STATUS, PrePedidoStatusDTO>();
            mPrepedidoStatus.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());
            var mPrepedidoStatusInverso = mPrepedidoStatus.ReverseMap();
            mPrepedidoStatusInverso.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            //
            // ---- Municipio
            var mapeamentoMunicipio = store.CreateMap<MUNICIPIO, MunicipioDTO>();
            mapeamentoMunicipio.ForMember(s => s.UF1, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.FORNECEDOR, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.TRANSPORTADOR, opt => opt.Ignore());

            var mMunicipioInverso = mapeamentoMunicipio.ReverseMap();
            mMunicipioInverso.ForMember(s => s.UF1, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.FORNECEDOR, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.TRANSPORTADOR, opt => opt.Ignore());


            // PrePedido---------
            var preConfig = store.CreateMap<PRE_PEDIDO, PrePedidoDTO>();
            preConfig.ForMember(s => s.PEDIDO, opt => opt.Ignore());
            preConfig.ForMember(s => s.PEDIDO_PAGAMENTO, opt => opt.Ignore());


            var preConfigInverse = preConfig.ReverseMap();

            preConfigInverse.ForMember(s => s.REPRESENTANTE, opt => opt.Ignore());
            preConfigInverse.ForMember(s => s.BRINDE, opt => opt.Ignore());
            preConfigInverse.ForMember(s => s.CLIENTES, opt => opt.Ignore());
            preConfigInverse.ForMember(s => s.PEDIDO, opt => opt.Ignore());
            preConfigInverse.ForMember(s => s.PEDIDO_PAGAMENTO, opt => opt.Ignore());
            preConfigInverse.ForMember(s => s.CARTEIRA, opt => opt.Ignore());

            //----------------
            // Pedido---------------



            var pedidoConfig = store.CreateMap<PEDIDO, PedidoDTO>();

            pedidoConfig.ForMember(s => s.PEDIDO_PAGAMENTO, opt => opt.Ignore());

            var pedidoConfigInverse = pedidoConfig.ReverseMap();
            pedidoConfigInverse.ForMember(s => s.ASSINATURA, opt => opt.Ignore());
            pedidoConfigInverse.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());
            pedidoConfigInverse.ForMember(s => s.PRODUTO_COMPOSICAO, opt => opt.Ignore());
            pedidoConfigInverse.ForMember(s => s.TIPO_PEDIDO, opt => opt.Ignore());


            //--------------
            //----- Pedido Pagamento
            var pedidoPagamento = store.CreateMap<PEDIDO_PAGAMENTO, FormaDePagamentoDTO>();
            var pedidoPagamentoReverse = pedidoPagamento.ReverseMap();

            pedidoPagamentoReverse.ForMember(s => s.PEDIDO, opt => opt.Ignore());
            pedidoPagamentoReverse.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());
            pedidoPagamentoReverse.ForMember(s => s.TIPO_PAGAMENTO, opt => opt.Ignore());
            
           
            //--------------------

            //-- m telefone -----

            var mTelefonePrePedido = store.CreateMap<PRE_PEDIDO_TELEFONE, TelefoneClienteDTO>();
            mTelefonePrePedido.ForMember(s => s.PEDIDO, opt => opt.Ignore());

            var mTelefonePrePedidoReverse = mTelefonePrePedido.ReverseMap();
            mTelefonePrePedidoReverse.ForMember(s => s.OPCAO_ATENDIMENTO, opt => opt.Ignore());
            //---------

            //-- m email
            var mEmailPrePedido = store.CreateMap<PRE_PEDIDO_EMAIL, EmailClienteDTO>();
            mEmailPrePedido.ForMember(s => s.PEDIDO, opt => opt.Ignore());
            

            //mEmailPrePedido.ForMember(s => s.email, opt => opt.MapFrom(c => c.PPE_EMAIL));
            //mEmailPrePedido.ForMember(s => s.idtipo, opt => opt.MapFrom(c => c.OPC_ID));
            //mEmailPrePedido.ForMember(s => s.idemail, opt => opt.MapFrom(c => c.PPE_ID));

            var mEmailPrePedidoReverso = mEmailPrePedido.ReverseMap();
            mEmailPrePedidoReverso.ForMember(s => s.OPCAO_ATENDIMENTO, opt => opt.Ignore());
            mEmailPrePedidoReverso.ForMember(s => s.PEDIDO, opt => opt.Ignore());

            //mEmailPrePedidoReverso.ForMember(s => s.PPE_EMAIL, opt => opt.MapFrom(c => c.email));
            //mEmailPrePedidoReverso.ForMember(s => s.OPC_ID, opt => opt.MapFrom(c => c.idtipo));
            //mEmailPrePedidoReverso.ForMember(s => s.PPE_ID, opt => opt.MapFrom(c => c.idemail));

            
            var mCliente = store.CreateMap<CLIENTES, ClienteDto>();
                       
            var mClienteR = mCliente.ReverseMap();

            var mClienteEndereco = store.CreateMap<CLIENTES_ENDERECO, ClienteEnderecoDto>();
            mClienteEndereco.ForMember(s => s.CLIENTES, opt => opt.Ignore());
            var mClienteEnderecoR = mClienteEndereco.ReverseMap();

            mClienteEnderecoR.ForMember(s => s.IBGE_MUNICIPIO, opt => opt.Ignore());
            //----------

            var mBrinde = store.CreateMap<BRINDE, BrindeDTO>();
            mBrinde.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            var mBrindeR = mBrinde.ReverseMap();
            mBrindeR.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            var mProduto = store.CreateMap<PRODUTOS, ProdutosDTO>();
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
        
            var mProdutoReverso = mProduto.ReverseMap();
            mProdutoReverso.ForMember(obj => obj.TIPO_PROD_COMPORTAMENTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.GRUPO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.TIPO_PRODUTO, conf => conf.Ignore());



            var mAreas = store.CreateMap<AREAS, AreasDTO>();
            mAreas.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreas.ReverseMap();

            var mGrupo = store.CreateMap<GRUPO, GrupoDTO>();
            mGrupo.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mGrupo.ReverseMap();

            var mProdutoComposicao = store.CreateMap<PRODUTO_COMPOSICAO, ProdutoComposicaoDTO>();
            mProdutoComposicao.ForMember(obj => obj.PEDIDO, conf => conf.Ignore());
            mProdutoComposicao.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());

            var mProdutoComposicaoReverse = mProdutoComposicao.ReverseMap();
            mProdutoComposicaoReverse.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.TIPO_ENVIO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.TIPO_PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.UNIDADE_NEGOCIO, conf => conf.Ignore());

            var mProdutoComposicaoItem = store.CreateMap<PRODUTO_COMPOSICAO_ITEM, ProdutoComposicaoItemDTO>();

            var mProdutoComposicaoItemR = mProdutoComposicaoItem.ReverseMap();
            mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mProdutoComposicaoItemR.ForMember(obj => obj.TIPO_PERIODO, conf => conf.Ignore());


            var mTipoProduto = store.CreateMap<TIPO_PRODUTO, TipoProdutoDTO>();
            mTipoProduto.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            var mTipoProdutoReserso = mTipoProduto.ReverseMap();

            var mUnidadeMedida = store.CreateMap<UNIDADE_MEDIDA, UnidadeMedidaDTO>();
            mUnidadeMedida.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mUnidadeMedida.ForMember(obj => obj.PRODUTOS1, conf => conf.Ignore());
            mUnidadeMedida.ReverseMap();

            var mTipoProdComportamento = store.CreateMap<TIPO_PROD_COMPORTAMENTO, TipoProdComportamentoDTO>();
            mTipoProdComportamento.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mTipoProdComportamento.ReverseMap();

            var mTipoEnvio = store.CreateMap<TIPO_ENVIO, TipoEnvioDTO>();
            mTipoEnvio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoEnvio.ReverseMap();

            var mTipoProdutoCom = store.CreateMap<TIPO_PRODUTO_COMPOSICAO, TipoProdutoComposicaoDTO>();
            mTipoProdutoCom.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoProdutoCom.ReverseMap();


            var mUnidadeNegocio = store.CreateMap<UNIDADE_NEGOCIO, UnidadeNegocioDTO>();
            mUnidadeNegocio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mUnidadeNegocio.ReverseMap();

            var mTipoPeriodo = store.CreateMap<TIPO_PERIODO, TipoPeriodoDTO>();
            mTipoPeriodo.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mTipoPeriodo.ReverseMap();

            var mTipoNegocio = store.CreateMap<UNIDADE_NEGOCIO, UnidadeNegocioDTO>();
            mTipoNegocio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoNegocio.ReverseMap();

            var mCarteiramento = store.CreateMap<CARTEIRA, CarteiraDTO>();
            mCarteiramento.ForMember(obj => obj.UF, conf => conf.Ignore());
            mCarteiramento.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mCarteiramento.ForMember(obj => obj.CARTEIRA_REPRESENTANTE, conf => conf.Ignore());
                        
            var mCarteiramentoR = mCarteiramento.ReverseMap();
            mCarteiramentoR.ForMember(obj => obj.UF, conf => conf.Ignore());
            mCarteiramentoR.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());

            var mCarteiraRepresentante = store.CreateMap<CARTEIRA_REPRESENTANTE, CarteiraRepresentanteDTO>();
            var mCarteiraRepresentanteReverso = mCarteiraRepresentante.ReverseMap();

            mCarteiraRepresentanteReverso.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mCarteiraRepresentanteReverso.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            var mCarteiramentoAssinatura = store.CreateMap<CARTEIRA_ASSINATURA, CarteiraAssinaturaDTO>();
            //mCarteiramentoAssinatura.ForMember(obj => obj.ASSINATURA.PEDIDO, conf => conf.Ignore());
            var mCarteiramentoAssinaturaR = mCarteiramentoAssinatura.ReverseMap();

            mCarteiramentoAssinaturaR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mCarteiramentoAssinaturaR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mCarteiramentoAssinaturaR.ForMember(obj => obj.CLASSIFICACAO, conf => conf.Ignore());

            var mAssinatura = store.CreateMap<ASSINATURA, AssinaturaDTO>();
            mAssinatura.ForMember(obj => obj.PEDIDO, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mAssinaturaR = mAssinatura.ReverseMap();
            mAssinaturaR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mClassificacao = store.CreateMap<CLASSIFICACAO, ClassificacaoDTO>();
            mClassificacao.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mClassificacao.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            // ---- UF
            var mapeamentoUF = store.CreateMap<UF, UFDTO>();
            mapeamentoUF.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.CFOP_ICMS, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.CFOP_ICMS1, conf => conf.Ignore());
            //mapeamentoUF.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mapeamentoUF.ForMember(obj => obj.URA, conf => conf.Ignore());

            var mapeamentoUFr = mapeamentoUF.ReverseMap();
            mapeamentoUFr.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CFOP_ICMS, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CFOP_ICMS1, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.URA, conf => conf.Ignore());


            // ---- Transportador
            var mapeamentoTransportador = store.CreateMap<TRANSPORTADOR, Transportador>();
            //mapeamentoTransportador.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            var mapeamentoTransportadorr = mapeamentoTransportador.ReverseMap();
            mapeamentoTransportadorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            // ---- Fornecedor
            var mapeamentoFornecedor = store.CreateMap<FORNECEDOR, Fornecedor>();
            mapeamentoFornecedor.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mapeamentoFornecedor.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoFornecedor.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());
            
            var mapeamentoFornecedorr = mapeamentoFornecedor.ReverseMap();
            mapeamentoFornecedorr.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mapeamentoFornecedorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoFornecedorr.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());


            // ---- Nota Fiscal 

            var mapeamentoNotaFiscal = store.CreateMap<NOTA_FISCAL, NotaFiscal>();
            mapeamentoNotaFiscal.ForMember(obj => obj.Transportador, conf => conf.Ignore());

            var mapeamentoNotaFiscalr = mapeamentoNotaFiscal.ReverseMap();
            mapeamentoNotaFiscalr.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_SITUACAO, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TIPO_DOC_FISCAL, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TRANSPORTADOR, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.UF, conf => conf.Ignore());

            // ---- Classificacao

            var mClassificacaoR = mClassificacao.ReverseMap();
            mClassificacaoR = mClassificacaoR.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mClassificacaoR = mClassificacaoR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            // ---- Representante

            var mRepresentante = store.CreateMap<REPRESENTANTE, RepresentanteDTO>();
            mRepresentante.ForMember(obj => obj.PRE_PEDIDO, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.CARTEIRA_REPRESENTANTE, conf => conf.Ignore());


            var mRepresentanteR = mRepresentante.ReverseMap();
            //mRepresentanteR.ForMember(obj => obj., conf => conf.Ignore());
            mRepresentanteR.ForMember(obj => obj.UF, conf => conf.Ignore());

            // ----Config Sped Fiscal

            var mConfigSpedFiscal = store.CreateMap<CONFIG_SPED_FISCAL, ConfigSpedFiscalDTO>();
            mConfigSpedFiscal.ForMember(obj => obj.CRE_DEB_ICMS, conf => conf.Ignore());
            
            var mConfigSpedFiscalR = mConfigSpedFiscal.ReverseMap();
            mConfigSpedFiscalR.ForMember(obj => obj.CRE_DEB_ICMS, conf => conf.Ignore());


            // ----Total Vendas Cartão

            var mTotalVendasCartao = store.CreateMap<TOTAL_VENDAS_CARTAO, TotalVendasCartaoDTO>();
           // mTotalVendasCartao.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());

            var mTotalVendasCartaoR = mTotalVendasCartao.ReverseMap();
            mTotalVendasCartaoR.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());

            // ----Empresa REF
                        
            var mEmpresaREF = store.CreateMap<EMPRESA_REF, EmpresaRefDTO>();
            mEmpresaREF.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            mEmpresaREF.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.CONTRATOS { get; set; }
            mEmpresaREF.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.PARCELAS , conf => conf.Ignore());
            mEmpresaREF.ForMember(obj => obj.SPED_ARQUIVO , conf => conf.Ignore());
            mEmpresaREF.ForMember(obj => obj.TOTAL_VENDAS_CARTAO , conf => conf.Ignore());

            var mEmpresaREFR = mEmpresaREF.ReverseMap();
            mEmpresaREFR.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            mEmpresaREFR.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.CONTRATOS { get; set; }
            mEmpresaREFR.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.PARCELAS , conf => conf.Ignore());
            mEmpresaREFR.ForMember(obj => obj.SPED_ARQUIVO, conf => conf.Ignore());
            mEmpresaREFR.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());

            // ----Config Sped Fiscal

            var mSpedArquivo = store.CreateMap<SPED_ARQUIVO, SpedArquivoDTO>();
            var mSpedArquivoR = mSpedArquivo.ReverseMap();

             // ----Nota Fiscal Item

            var mNotaFiscalItem = store.CreateMap<NOTA_FISCAL_ITEM, NotaFiscalItemDTO>();
            mNotaFiscalItem.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.CST , conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.NOTA_FISCAL , conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.NOTA_FISCAL_ITEM_OBS , conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.PRODUTOS , conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());

            var mNotaFiscalItemR = mNotaFiscalItem.ReverseMap();
            mNotaFiscalItemR.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.CST, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.NOTA_FISCAL_ITEM_OBS, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            
            // ----Nota Fiscal Item Obs

            var mNotaFiscalItemOBS = store.CreateMap<NOTA_FISCAL_ITEM_OBS, NotaFiscalItemOBSDTO>();
            mNotaFiscalItemOBS.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            var mNotaFiscalItemOBSR = mNotaFiscalItemOBS.ReverseMap();
            mNotaFiscalItemOBSR.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            // ----CST

            var mCST = store.CreateMap<CST, CSTDTO>();
            mCST.ForMember(obj => obj.CST_TIPO , conf => conf.Ignore());
            mCST.ForMember(obj => obj.NOTA_FISCAL_ITEM , conf => conf.Ignore());

            var mCSTR = mCST.ReverseMap();
            mCSTR.ForMember(obj => obj.CST_TIPO, conf => conf.Ignore());
            mCSTR.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());
            
            // ----CST_tipo

            var mCSTTipo = store.CreateMap<CST_TIPO, CSTTipoDTO>();
            mCSTTipo.ForMember(obj => obj.CST, conf => conf.Ignore());

            var mCSTTipoR = mCSTTipo.ReverseMap();
            mCSTTipoR.ForMember(obj => obj.CST, conf => conf.Ignore());

            // ----TIPO_FORNECEDOR

            var mTipoFornecedor = store.CreateMap<TIPO_FORNECEDOR, TipoFornecedorDTO>();
            mTipoFornecedor.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());

            var mTipoFornecedorr = mTipoFornecedor.ReverseMap();
            mTipoFornecedorr.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());

            // ----PAIS

            var mPais = store.CreateMap<PAIS, PaisDTO>();

            var mPaisr = mPais.ReverseMap();

            // ----ura

            var mURA = store.CreateMap<URA, UraDTO>();
            mURA.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mURA.ForMember(obj => obj.UF, conf => conf.Ignore());

            var mURAr = mURA.ReverseMap();
            mURAr.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mURAr.ForMember(obj => obj.UF, conf => conf.Ignore());

            // ----
            var mURAConfig = store.CreateMap<URA_CONFIG, UraConfigDTO>();
            mURAConfig.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            
            var mURAConfigr = mURAConfig.ReverseMap();
            mURAConfigr.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            

            // ----
            var mURAProduto = store.CreateMap<URA_PRODUTO, UraProdutoDTO>();
            mURAProduto.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAProduto.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());
            mURAProduto.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());
          

            var mURAProdutor = mURAProduto.ReverseMap();
            mURAProdutor.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAProdutor.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());
            mURAProdutor.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());
            mURAProdutor.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            // ----
            var mProdutoArea = store.CreateMap<URA_PRODUTO_AREA, UraProdutoAreaDTO>();
            mProdutoArea.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());

            var mProdutoArear = mProdutoArea.ReverseMap();
            mProdutoArear.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mProdutoArear.ForMember(obj => obj.AREA_CONSULTORIA, conf => conf.Ignore());

            // ----Areas Consultoria
            var mAreaConsultoria = store.CreateMap<AREA_CONSULTORIA, AreaConsultoriaDTO>();
            mAreaConsultoria.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreaConsultoria.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());

            var mAreaConsultoriar = mAreaConsultoria.ReverseMap();
            mAreaConsultoriar.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreaConsultoriar.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());
            


        }
    }
}
