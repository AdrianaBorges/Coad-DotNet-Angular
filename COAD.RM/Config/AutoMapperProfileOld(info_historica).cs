using AutoMapper;
using COAD.CORPORATIVO.Config.CustomConvert;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COADCORP.Config
{
    public class AutoMapperProfileOld : Profile
    {
        protected override void Configure()
        {
           
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();

            var mPrepedidoStatus = Mapper.CreateMap<PRE_PEDIDO_STATUS, PrePedidoStatusDTO>();
            mPrepedidoStatus.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());
            var mPrepedidoStatusInverso = mPrepedidoStatus.ReverseMap();
            mPrepedidoStatusInverso.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            var mMunicipio = Mapper.CreateMap<MUNICIPIO, MunicipioDTO>();
            mMunicipio.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            //
            // ---- Municipio
            var mapeamentoMunicipio = Mapper.CreateMap<MUNICIPIO, MunicipioDTO>();
            mapeamentoMunicipio.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.FORNECEDOR, opt => opt.Ignore());
            var mMunicipioInverso = mMunicipio.ReverseMap();
            mMunicipioInverso.ForMember(s => s.UF1, opt => opt.Ignore());


            // PrePedido---------
            var preConfig = Mapper.CreateMap<PRE_PEDIDO, PrePedidoDTO>();

            //preConfig.ForMember(s => s.empresa, opt => opt.MapFrom(c => c.EMP_ID));
            //preConfig.ForMember(s => s.anobase, opt => opt.MapFrom(c => c.PRE_ANO));
            //preConfig.ForMember(s => s.periodo, opt => opt.MapFrom(c => c.PRE_PERIODO));
            //preConfig.ForMember(s => s.semana, opt => opt.MapFrom(c => c.PER_SEMANA));
            //preConfig.ForMember(s => s.brindeid, opt => opt.MapFrom(c => c.BRI_ID));
            //preConfig.ForMember(s => s.pastaprod, opt => opt.MapFrom(c => c.PRE_ENTREGA_PASTA));
            //preConfig.ForMember(s => s.livroprod, opt => opt.MapFrom(c => c.PRE_ENTREGA_LIVRO));
            //preConfig.ForMember(s => s.cdromprod, opt => opt.MapFrom(c => c.PRE_ENTREGA_BRINDE));
            //preConfig.ForMember(s => s.predata, opt => opt.MapFrom(c => c.PRE_DATA));
            //preConfig.ForMember(s => s.cliente, opt => opt.MapFrom(c => c.CLIENTES));


            //var preConfigInverse = preConfig.ReverseMap();

            //preConfigInverse.ForMember(s => s.EMP_ID, opt => opt.MapFrom(c => c.empresa));
            //preConfigInverse.ForMember(s => s.PRE_ANO, opt => opt.MapFrom(c => c.anobase));
            //preConfigInverse.ForMember(s => s.PRE_PERIODO, opt => opt.MapFrom(c => c.periodo));
            //preConfigInverse.ForMember(s => s.PER_SEMANA, opt => opt.MapFrom(c => c.semana));
            //preConfigInverse.ForMember(s => s.BRI_ID, opt => opt.MapFrom(c => c.brindeid));
            //preConfigInverse.ForMember(s => s.PRE_ENTREGA_PASTA, opt => opt.MapFrom(c => c.pastaprod));
            //preConfigInverse.ForMember(s => s.PRE_ENTREGA_LIVRO, opt => opt.MapFrom(c => c.livroprod));
            //preConfigInverse.ForMember(s => s.PRE_ENTREGA_BRINDE, opt => opt.MapFrom(c => c.cdromprod));
            //preConfigInverse.ForMember(s => s.PRE_DATA, opt => opt.MapFrom(c => c.predata));

            //----------------
            // Pedido---------------

           

            var pedidoConfig = Mapper.CreateMap<PEDIDO, PedidoDTO>();

            //pedidoConfig.ForMember(s => s.valor, opt => opt.MapFrom(c => c.PED_VLR_PEDIDO));
            //pedidoConfig.ForMember(s => s.tiponegocioprod, opt => opt.MapFrom(c => c.TIPO_PED_ID));
            //pedidoConfig.ForMember(s => s.desconto, opt => opt.MapFrom(c => c.PED_PERC_DESCONTO));
            //pedidoConfig.ForMember(s => s.total, opt => opt.MapFrom(c => c.PED_VLR_TOTAL_PEDIDO));
            //pedidoConfig.ForMember(s => s.inicioVigenciaprod, opt => opt.MapFrom(c => c.PED_VIGENCIA_INI));
            //pedidoConfig.ForMember(s => s.fimVigenciaprod, opt => opt.MapFrom(c => c.PED_VIGENCIA_FIM));
            //pedidoConfig.ForMember(s => s.produtoProd, opt => opt.MapFrom(c => c.CMP_ID));
            //pedidoConfig.ForMember(s => s.lstDeEmails, opt => opt.MapFrom(c => c.PRE_PEDIDO_EMAIL));
            //pedidoConfig.ForMember(s => s.lstDeTelefones, opt => opt.MapFrom(c => c.PRE_PEDIDO_TELEFONE));

            var pedidoConfigInverse = pedidoConfig.ReverseMap();

            //pedidoConfigInverse.ForMember(s => s.PED_VLR_PEDIDO, opt => opt.MapFrom(c => c.valor));
            //pedidoConfigInverse.ForMember(s => s.TIPO_PED_ID, opt => opt.MapFrom(c => c.tiponegocioprod));
            //pedidoConfigInverse.ForMember(s => s.PED_PERC_DESCONTO, opt => opt.MapFrom(c => c.desconto));
            //pedidoConfigInverse.ForMember(s => s.PED_VLR_TOTAL_PEDIDO, opt => opt.MapFrom(c => c.total));
            //pedidoConfigInverse.ForMember(s => s.PED_VIGENCIA_INI, opt => opt.MapFrom(c => c.inicioVigenciaprod));
            //pedidoConfigInverse.ForMember(s => s.PED_VIGENCIA_FIM, opt => opt.MapFrom(c => c.fimVigenciaprod));
            //pedidoConfigInverse.ForMember(s => s.CMP_ID, opt => opt.MapFrom(c => c.produtoProd));
            //pedidoConfigInverse.ForMember(s => s.PRE_PEDIDO_EMAIL, opt => opt.MapFrom(c => c.lstDeEmails));
            //pedidoConfigInverse.ForMember(s => s.PRE_PEDIDO_TELEFONE, opt => opt.MapFrom(c => c.lstDeTelefones));

            //--------------
            //----- Pedido Pagamento
            var pedidoPagamento = Mapper.CreateMap<PEDIDO_PAGAMENTO, FormaDePagamentoDTO>();

            //pedidoPagamento.ForMember(s => s.formapagtoprod, opt => opt.MapFrom(c => c.TPG_ID));
            //pedidoPagamento.ForMember(s => s.total, opt => opt.MapFrom(c => c.PGT_VLR_TOTAL));
            //pedidoPagamento.ForMember(s => s.qteparcelas, opt => opt.MapFrom(c => c.PGT_QTDE_PARCELAS));
            //pedidoPagamento.ForMember(s => s.entrada, opt => opt.MapFrom(c => c.PGT_VLR_ENTRADA));
            //pedidoPagamento.ForMember(s => s.valorparcelas, opt => opt.MapFrom(c => c.PGT_VLR_PARCELA));
            //pedidoPagamento.ForMember(s => s.vencsegparcela, opt => opt.MapFrom(c => c.PGT_SEGUNDO_VENCTO));
            //pedidoPagamento.ForMember(s => s.tipodocumento, opt => opt.MapFrom(c => c.PGT_TIPO_DOCUMENTO));
            //pedidoPagamento.ForMember(s => s.numdocumento, opt => opt.MapFrom(c => c.PGT_NUMERO_DOCUMENTO));
            //pedidoPagamento.ForMember(s => s.bancoprod, opt => opt.MapFrom(c => c.PGT_BANCO));
            //pedidoPagamento.ForMember(s => s.chequebompara, opt => opt.MapFrom(c => c.PGT_CHEQUE_BOM_PARA));
            //pedidoPagamento.ForMember(s => s.observacaoprod, opt => opt.MapFrom(c => c.PGT_OBS));

            var pedidoPagamentoReverse = pedidoPagamento.ReverseMap();

            //pedidoPagamentoReverse.ForMember(s => s.TPG_ID, opt => opt.MapFrom(c => c.formapagtoprod));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_VLR_TOTAL, opt => opt.MapFrom(c => c.total));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_QTDE_PARCELAS, opt => opt.MapFrom(c => c.qteparcelas));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_VLR_ENTRADA, opt => opt.MapFrom(c => c.entrada));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_VLR_PARCELA, opt => opt.MapFrom(c => c.valorparcelas));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_SEGUNDO_VENCTO, opt => opt.MapFrom(c => c.vencsegparcela));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_TIPO_DOCUMENTO, opt => opt.MapFrom(c => c.tipodocumento));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_NUMERO_DOCUMENTO, opt => opt.MapFrom(c => c.numdocumento));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_BANCO, opt => opt.MapFrom(c => c.bancoprod));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_CHEQUE_BOM_PARA, opt => opt.MapFrom(c => c.chequebompara));
            //pedidoPagamentoReverse.ForMember(s => s.PGT_OBS, opt => opt.MapFrom(c => c.observacaoprod));

            
            //--------------------

            //-- m telefone -----

            var mTelefonePrePedido = Mapper.CreateMap<PRE_PEDIDO_TELEFONE, TelefoneClienteDTO>();
            //mTelefonePrePedido.ForMember(s => s.tipo, opt => opt.MapFrom(c => c.TIPO_TEL_ID));
            //mTelefonePrePedido.ForMember(s => s.telefone, opt => opt.MapFrom(c => c.PPT_TELEFONE));
            //mTelefonePrePedido.ForMember(s => s.idsetor, opt => opt.MapFrom(c => c.OPC_ID));
            //mTelefonePrePedido.ForMember(s => s.idtelefone, opt => opt.MapFrom(c => c.PPT_ID));           


            //var mTelefonePrePedidoReverse = mTelefonePrePedido.ReverseMap();
            //mTelefonePrePedidoReverse.ForMember(s => s.OPC_ID, opt => opt.MapFrom(c => c.idsetor));
            //mTelefonePrePedidoReverse.ForMember(s => s.PPT_TELEFONE, opt => opt.MapFrom(c => c.telefone));
            //mTelefonePrePedidoReverse.ForMember(s => s.PPT_ID, opt => opt.MapFrom(c => c.idtelefone));
            //mTelefonePrePedidoReverse.ForMember(s => s.TIPO_TEL_ID, opt => opt.MapFrom(c => c.tipo));
            //---------

            //-- m email
            var mEmailPrePedido = Mapper.CreateMap<PRE_PEDIDO_EMAIL, EmailClienteDTO>();
            //mEmailPrePedido.ForMember(s => s.email, opt => opt.MapFrom(c => c.PPE_EMAIL));
            //mEmailPrePedido.ForMember(s => s.idtipo, opt => opt.MapFrom(c => c.OPC_ID));
            //mEmailPrePedido.ForMember(s => s.idemail, opt => opt.MapFrom(c => c.PPE_ID));

            //var mEmailPrePedidoReverso = mEmailPrePedido.ReverseMap();
            //mEmailPrePedidoReverso.ForMember(s => s.PPE_EMAIL, opt => opt.MapFrom(c => c.email));
            //mEmailPrePedidoReverso.ForMember(s => s.OPC_ID, opt => opt.MapFrom(c => c.idtipo));
            //mEmailPrePedidoReverso.ForMember(s => s.PPE_ID, opt => opt.MapFrom(c => c.idemail));

            //Mapper.CreateMap<PrePedidoDTO, CLIENTES>().ConvertUsing<PrePedidoToClientConvert>();
            var mCliente = Mapper.CreateMap<CLIENTES, ClienteDto>();
            
            var mClienteEndereco = Mapper.CreateMap<CLIENTES_ENDERECO, ClienteEnderecoDto>();
            mClienteEndereco.ForMember(s => s.CLIENTES, opt => opt.Ignore());
            var  mClienteEnderecoR = mClienteEndereco.ReverseMap();

            mClienteEnderecoR.ForMember(s => s.IBGE_MUNICIPIO, opt => opt.Ignore());
            //----------

            var mBrinde = Mapper.CreateMap<BRINDE, BrindeDTO>();
            mBrinde.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            var mBrindeR = mBrinde.ReverseMap();
            mBrindeR.ForMember(s => s.PRE_PEDIDO, opt => opt.Ignore());

            var mProduto = Mapper.CreateMap<PRODUTOS, ProdutosDTO>();
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            var mProdutoReverso = mProduto.ReverseMap();
            mProdutoReverso.ForMember(obj => obj.TIPO_PROD_COMPORTAMENTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.GRUPO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.TIPO_PRODUTO, conf => conf.Ignore());
           


            var mAreas = Mapper.CreateMap<AREAS, AreasDTO>();
            mAreas.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreas.ReverseMap();

            var mGrupo = Mapper.CreateMap<GRUPO, GrupoDTO>();
            mGrupo.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mGrupo.ReverseMap();

            var mProdutoComposicao = Mapper.CreateMap<PRODUTO_COMPOSICAO, ProdutoComposicaoDTO>();
            mProdutoComposicao.ForMember(obj => obj.PEDIDO, conf => conf.Ignore());
            mProdutoComposicao.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            
            var mProdutoComposicaoReverse = mProdutoComposicao.ReverseMap();
            mProdutoComposicaoReverse.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.TIPO_ENVIO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.TIPO_PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.UNIDADE_NEGOCIO, conf => conf.Ignore());
                        
            var mProdutoComposicaoItem = Mapper.CreateMap<PRODUTO_COMPOSICAO_ITEM, ProdutoComposicaoItemDTO>();

            var mProdutoComposicaoItemR = mProdutoComposicaoItem.ReverseMap();
            mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mProdutoComposicaoItemR.ForMember(obj => obj.TIPO_PERIODO, conf => conf.Ignore());


            var mTipoProduto = Mapper.CreateMap<TIPO_PRODUTO, TipoProdutoDTO>();
            mTipoProduto.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            var mTipoProdutoReserso = mTipoProduto.ReverseMap();

            var mUnidadeMedida = Mapper.CreateMap<UNIDADE_MEDIDA, UnidadeMedidaDTO>();
            mUnidadeMedida.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mUnidadeMedida.ForMember(obj => obj.PRODUTOS1, conf => conf.Ignore());
            mUnidadeMedida.ReverseMap();

            var mTipoProdComportamento = Mapper.CreateMap<TIPO_PROD_COMPORTAMENTO, TipoProdComportamentoDTO>();
            mTipoProdComportamento.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mTipoProdComportamento.ReverseMap();

            var mTipoEnvio = Mapper.CreateMap<TIPO_ENVIO, TipoEnvioDTO>();
            mTipoEnvio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoEnvio.ReverseMap();

            var mTipoProdutoCom = Mapper.CreateMap<TIPO_PRODUTO_COMPOSICAO, TipoProdutoComposicaoDTO>();
            mTipoProdutoCom.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoProdutoCom.ReverseMap();


            var mUnidadeNegocio = Mapper.CreateMap<UNIDADE_NEGOCIO, UnidadeNegocioDTO>();
            mUnidadeNegocio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mUnidadeNegocio.ReverseMap();

            var mTipoPeriodo = Mapper.CreateMap<TIPO_PERIODO, TipoPeriodoDTO>();
            mTipoPeriodo.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mTipoPeriodo.ReverseMap();

            var mTipoNegocio = Mapper.CreateMap<UNIDADE_NEGOCIO, UnidadeNegocioDTO>();
            mTipoNegocio.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mTipoNegocio.ReverseMap();

            var mCarteiramento = Mapper.CreateMap<CARTEIRA, CarteiraDTO>();
            mCarteiramento.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
           // mCarteiramento.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            var mCarteiramentoR = mCarteiramento.ReverseMap();
            mCarteiramentoR.ForMember(obj => obj.UF, conf => conf.Ignore());

            var mCarteiramentoAssinatura = Mapper.CreateMap<CARTEIRA_ASSINATURA, CarteiraAssinaturaDTO>();
            //mCarteiramentoAssinatura.ForMember(obj => obj.ASSINATURA.PEDIDO, conf => conf.Ignore());
            var mCarteiramentoAssinaturaR = mCarteiramentoAssinatura.ReverseMap();

            mCarteiramentoAssinaturaR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mCarteiramentoAssinaturaR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mCarteiramentoAssinaturaR.ForMember(obj => obj.CLASSIFICACAO, conf => conf.Ignore());

            var mAssinatura = Mapper.CreateMap<ASSINATURA, AssinaturaDTO>();
            mAssinatura.ForMember(obj => obj.PEDIDO, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mAssinaturaR = mAssinatura.ReverseMap();
            mAssinaturaR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mClassificacao = Mapper.CreateMap<CLASSIFICACAO, ClassificacaoDTO>();
            mClassificacao.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mClassificacao.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            // ---- UF
            var mapeamentoUF = Mapper.CreateMap<UF, UFDTO>();

            var mapeamentoUFr = mapeamentoUF.ReverseMap();
            mapeamentoUFr.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CFOP_ICMS , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CFOP_ICMS1 , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.CONTRATOS , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.MUNICIPIO , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.NOTA_FISCAL , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.URA_CONFIG , conf => conf.Ignore());
            mapeamentoUFr.ForMember(obj => obj.URA, conf => conf.Ignore());

            // ---- Transportador
            var mapeamentoTransportador = Mapper.CreateMap<TRANSPORTADOR, Transportador>();
            mapeamentoTransportador.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            var mapeamentoTransportadorr = mapeamentoTransportador.ReverseMap();
            mapeamentoTransportadorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            // ---- Fornecedor
            var mapeamentoFornecedor = Mapper.CreateMap<FORNECEDOR, Fornecedor>();
            mapeamentoFornecedor.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            var mapeamentoFornecedorr = mapeamentoFornecedor.ReverseMap();
            mapeamentoFornecedorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());


            // ---- Nota Fiscal 

            var mapeamentoNotaFiscal  = Mapper.CreateMap<NOTA_FISCAL, NotaFiscal>();
            mapeamentoNotaFiscal.ForMember(obj => obj.Transportador, conf => conf.Ignore());

            var mapeamentoNotaFiscalr = mapeamentoNotaFiscal.ReverseMap();
            mapeamentoNotaFiscalr.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_ITEM , conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_SITUACAO , conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TIPO_DOC_FISCAL , conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TRANSPORTADOR , conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.UF, conf => conf.Ignore());

            
            var mClassificacaoR = mClassificacao.ReverseMap();
            mClassificacaoR = mClassificacaoR.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mClassificacaoR = mClassificacaoR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            var mRepresentante = Mapper.CreateMap<REPRESENTANTE, RepresentanteDTO>();
            mRepresentante.ForMember(obj => obj.PRE_PEDIDO, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());

            
            var mRepresentanteR = mRepresentante.ReverseMap();
            mRepresentanteR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mRepresentanteR.ForMember(obj => obj.UF, conf => conf.Ignore());


        }
    }
}