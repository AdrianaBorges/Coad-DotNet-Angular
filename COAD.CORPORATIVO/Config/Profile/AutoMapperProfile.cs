using AutoMapper;
using COAD.CORPORATIVO.LEGADO.Model.Dto.CorporativoAntigo;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
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
            var mFeriado = store.CreateMap<FERIADO, FeriadoDTO>();
            var mFeriador = mFeriado.ReverseMap();

            // ---- Municipio
            var mapeamentoMunicipio = store.CreateMap<MUNICIPIO, MunicipioDTO>();
            mapeamentoMunicipio.ForMember(s => s.UF1, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.FORNECEDOR, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.TRANSPORTADOR, opt => opt.Ignore());
            mapeamentoMunicipio.ForMember(s => s.CEP_LOGRADOURO, opt => opt.Ignore());

            var mMunicipioInverso = mapeamentoMunicipio.ReverseMap();
            mMunicipioInverso.ForMember(s => s.UF1, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.FORNECEDOR, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.TRANSPORTADOR, opt => opt.Ignore());
            mMunicipioInverso.ForMember(s => s.CEP_LOGRADOURO, opt => opt.Ignore());

            // ---- PrePedido 


            //----------------------
            //----- Pedido Pagamento
            //----------------------
            //var pedidoPagamento = store.CreateMap<PEDIDO_PAGAMENTO, PedidoPagamentoDTO>();
            //var pedidoPagamentoReverse = pedidoPagamento.ReverseMap();

            //-------------------
            //-- m telefone -----
            //-------------------

            //-------------------

            var mOpcaoAtendimento = store.CreateMap<OPCAO_ATENDIMENTO, OpcoesAtendimentoDTO>();
            mOpcaoAtendimento.ForMember(s => s.ASSINATURA_EMAIL, opt => opt.Ignore());
            mOpcaoAtendimento.ForMember(s => s.ASSINATURA_TELEFONE, opt => opt.Ignore());

            var mOpcaoAtendimentoR = mOpcaoAtendimento.ReverseMap();
            mOpcaoAtendimentoR.ForMember(s => s.ASSINATURA_EMAIL, opt => opt.Ignore());
            mOpcaoAtendimentoR.ForMember(s => s.ASSINATURA_TELEFONE, opt => opt.Ignore());


            //--------------------
            //-- m email ---------
            //--------------------


            //mEmailPrePedido.ForMember(s => s.email, opt => opt.MapFrom(c => c.PPE_EMAIL));
            //mEmailPrePedido.ForMember(s => s.idtipo, opt => opt.MapFrom(c => c.OPC_ID));
            //mEmailPrePedido.ForMember(s => s.idemail, opt => opt.MapFrom(c => c.PPE_ID));



            //mEmailPrePedidoReverso.ForMember(s => s.PPE_EMAIL, opt => opt.MapFrom(c => c.email));
            //mEmailPrePedidoReverso.ForMember(s => s.OPC_ID, opt => opt.MapFrom(c => c.idtipo));
            //mEmailPrePedidoReverso.ForMember(s => s.PPE_ID, opt => opt.MapFrom(c => c.idemail));

            var mCliente = store.CreateMap<COAD.CORPORATIVO.Repositorios.Contexto.CLIENTES, ClienteDto>();
            mCliente.ForMember(obj => obj.CLASSIFICACAO, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CARTEIRA_CLIENTE, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.ASSINATURA, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.ASSINATURA_TRANSFERENCIA, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CLIENTES_ENDERECO, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CONTATOS, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CLIENTES_HISTORICO, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.NOTA_FISCAL, opt => opt.Ignore());
            //mCliente.ForMember(obj => obj.TIPO_CLIENTE, opt => opt.Ignore());
            //mCliente.ForMember(obj => obj.CLASSIFICACAO_CLIENTE, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.AUDITORIA, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.ASSINATURA_EMAIL, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.ASSINATURA_TELEFONE, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CONTATOS, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.NOTA_FISCAL, opt => opt.Ignore());
            //mCliente.ForMember(obj => obj.PRE_PEDIDO, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.CLIENTES_TELEFONE, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.INFO_MARKETING, opt => opt.Ignore());
            mCliente.ForMember(obj => obj.AGENDAMENTO, opt => opt.Ignore());
            

            var mClienteR = mCliente.ReverseMap();
            mClienteR.ForMember(obj => obj.CLASSIFICACAO, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CLASSIFICACAO_CLIENTE, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.TIPO_CLIENTE, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.ASSINATURA, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.ASSINATURA_TRANSFERENCIA, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CLIENTES_ENDERECO, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CONTATOS, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CLIENTES_HISTORICO, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.NOTA_FISCAL, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.TIPO_CLIENTE, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CLASSIFICACAO_CLIENTE, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.AUDITORIA, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.INFO_MARKETING, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.CONTATOS, opt => opt.Ignore());
            mClienteR.ForMember(obj => obj.NOTA_FISCAL, opt => opt.Ignore());


            //------------Contatos
            var mContato = store.CreateMap<CONTATOS, ContatoDTO>();
            mContato.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            //------------

            //----------- Cliente_Historico
            var mClientesHistorico = store.CreateMap<CLIENTES_HISTORICO, ClienteHistoricoDTO>();
            mClientesHistorico.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            //---------Tipo Cliente

            var mTipoCliente = store.CreateMap<TIPO_CLIENTE, TipoClienteDTO>();
            mTipoCliente.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            //---------Classificacao Cliente

            var mClassificacaoCliente = store.CreateMap<CLASSIFICACAO_CLIENTE, ClassificacaoClienteDTO>();
            mClassificacaoCliente.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            var mClassificacaoClienteR = mClassificacaoCliente.ReverseMap();
            mClassificacaoClienteR.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            //---Auditoria

            var mAuditoria = store.CreateMap<AUDITORIA, AuditoriaDTO>();
            mAuditoria.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());

            var mAuditoriaR = mAuditoria.ReverseMap();
            mAuditoriaR.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());


            //------------Carteira Cliente

            var mClienteCarteira = store.CreateMap<CARTEIRA_CLIENTE, CarteiraClienteDTO>();
            mClienteCarteira.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());
            mClienteCarteira.ForMember(obj => obj.CARTEIRA, opt => opt.Ignore());
            mClienteCarteira.ForMember(obj => obj.ASSINATURA, opt => opt.Ignore());


            var mClienteCarteiraR = mClienteCarteira.ReverseMap();
            mClienteCarteiraR.ForMember(obj => obj.CLIENTES, opt => opt.Ignore());
            mClienteCarteiraR.ForMember(obj => obj.CARTEIRA, opt => opt.Ignore());
            mClienteCarteiraR.ForMember(obj => obj.ASSINATURA, opt => opt.Ignore());

            //------------CLIENTE ENDERECO

            var mClienteEndereco = store.CreateMap<CLIENTES_ENDERECO, ClienteEnderecoDto>();
            mClienteEndereco.ForMember(s => s.CLIENTES, opt => opt.Ignore());
            mClienteEndereco.ForMember(s => s.ASSINATURA, opt => opt.Ignore());
            //  mClienteEndereco.ForMember(s => s.MUNICIPIO, opt => opt.Ignore());

            var mClienteEnderecoR = mClienteEndereco.ReverseMap();
            mClienteEnderecoR.ForMember(s => s.CLIENTES, opt => opt.Ignore());
            mClienteEnderecoR.ForMember(s => s.ASSINATURA, opt => opt.Ignore());
            mClienteEnderecoR.ForMember(s => s.MUNICIPIO, opt => opt.Ignore());
            mClienteEnderecoR.ForMember(s => s.IBGE_MUNICIPIO, opt => opt.Ignore());
            mClienteEnderecoR.ForMember(s => s.TIPO_ENDERECO, opt => opt.Ignore());

            //mClienteEnderecoR.ForMember(s => s.TIPO_COMPLEMENTO, opt => opt.Ignore());         

            //----------

            var mIbgeMunicipio = store.CreateMap<IBGE_MUNICIPIO, IbgeMunicipioDTO>();
            mIbgeMunicipio.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());

            var mIbgeMunicipioR = mIbgeMunicipio.ReverseMap();

            //----------

            var mTipoLogradouro = store.CreateMap<TIPO_LOGRADOURO, TipoLogradouroDTO>();
            mTipoLogradouro.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());

            var mTipoLogradouroR = mTipoLogradouro.ReverseMap();

            //----------

            var mTipoEndereco = store.CreateMap<TIPO_ENDERECO, TipoEnderecoDTO>();
            mTipoEndereco.ForMember(s => s.CLIENTES_ENDERECO, opt => opt.Ignore());

            var mTipoEnderecoR = mTipoEndereco.ReverseMap();

            //----------

            var mProduto = store.CreateMap<PRODUTOS, ProdutosDTO>();
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.GRUPO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.ORIGEM_ACESSO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_EAN, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_FAMILIA, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_FORNECEDOR, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_HISTORICO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.PRODUTO_PERFIL, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.TIPO_PROD_COMPORTAMENTO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.TIPO_PRODUTO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mProduto.ForMember(obj => obj.AREA_CONSULTORIA, conf => conf.Ignore());
            

            //mProduto.ForMember(obj => obj.PRODUTO_FAMILIA, conf => conf.Ignore()); descomentar

            var mProdutoReverso = mProduto.ReverseMap();
            mProdutoReverso.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.GRUPO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.ORIGEM_ACESSO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_EAN, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_FAMILIA, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_FORNECEDOR, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_HISTORICO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.PRODUTO_PERFIL, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.TIPO_PROD_COMPORTAMENTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.TIPO_PRODUTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mProdutoReverso.ForMember(obj => obj.AREA_CONSULTORIA, conf => conf.Ignore());

            //----------

            var mAreas = store.CreateMap<AREAS, AreasCorpDTO>();
            mAreas.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreas.ForMember(obj => obj.AREA_INFO_MARKETING, conf => conf.Ignore());

            mAreas.ReverseMap();

            //----------

            var mGrupo = store.CreateMap<GRUPO, GrupoDTO>();
            mGrupo.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mGrupo.ReverseMap();

            var mProdutoComposicao = store.CreateMap<PRODUTO_COMPOSICAO, ProdutoComposicaoDTO>();
            mProdutoComposicao.ForMember(obj => obj.PRODUTO_COMPOSICAO_ITEM, conf => conf.Ignore());
            mProdutoComposicao.ForMember(obj => obj.TABELA_PRECO, conf => conf.Ignore());
            mProdutoComposicao.ForMember(obj => obj.PRODUTO_COMPOSICAO_INFO_MARKETING, conf => conf.Ignore());
            mProdutoComposicao.ForMember(obj => obj.PRODUTO_COMPOSICAO1, conf => conf.Ignore());

            var mProdutoComposicaoReverse = mProdutoComposicao.ReverseMap();
            mProdutoComposicaoReverse.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.TIPO_PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mProdutoComposicaoReverse.ForMember(obj => obj.PRODUTO_COMPOSICAO1, conf => conf.Ignore());

            //----------
            var mOrigemAcesso = store.CreateMap<ORIGEM_ACESSO, OrigemAcessoDTO>();
            mOrigemAcesso.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mOrigemAcesso.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());


            var mOrigemAcessor = mOrigemAcesso.ReverseMap();
            mOrigemAcessor.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mOrigemAcessor.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());
            //----------

            var mProdutoComposicaoItem = store.CreateMap<PRODUTO_COMPOSICAO_ITEM, ProdutoComposicaoItemDTO>();
           // mProdutoComposicaoItem.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            //mProdutoComposicaoItem.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
           // mProdutoComposicaoItem.ForMember(obj => obj.TIPO_PERIODO, conf => conf.Ignore());

            var mProdutoComposicaoItemR = mProdutoComposicaoItem.ReverseMap();
            mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            //mProdutoComposicaoItemR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mProdutoComposicaoItemR.ForMember(obj => obj.TIPO_PERIODO, conf => conf.Ignore());

            //----------

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
            mCarteiramento.ForMember(obj => obj.CARTEIRA_CLIENTE, conf => conf.Ignore());
            mCarteiramento.ForMember(obj => obj.AGENDAMENTO, conf => conf.Ignore());
            //mCarteiramento.ForMember(obj => obj.REGIAO, conf => conf.Ignore());

            var mCarteiramentoR = mCarteiramento.ReverseMap();
            mCarteiramentoR.ForMember(obj => obj.UF, conf => conf.Ignore());
            mCarteiramentoR.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mCarteiramentoR.ForMember(obj => obj.UEN, conf => conf.Ignore());
            mCarteiramentoR.ForMember(obj => obj.REGIAO, conf => conf.Ignore());

            var mCarteiraRepresentante = store.CreateMap<CARTEIRA_REPRESENTANTE, CarteiraRepresentanteDTO>();
            //mCarteiraRepresentante.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
           // mCarteiraRepresentante.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mCarteiraRepresentante.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());

            var mCarteiraRepresentanteReverso = mCarteiraRepresentante.ReverseMap();
            //mCarteiraRepresentanteReverso.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mCarteiraRepresentanteReverso.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            //mCarteiraRepresentanteReverso.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            var mCarteiramentoAssinatura = store.CreateMap<CARTEIRA_ASSINATURA, CarteiraAssinaturaDTO>();
            mCarteiramentoAssinatura.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mCarteiramentoAssinatura.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());

            var mCarteiramentoAssinaturaR = mCarteiramentoAssinatura.ReverseMap();
            mCarteiramentoAssinaturaR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mCarteiramentoAssinaturaR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());

            //---- Assinatura
            var mAssinatura = store.CreateMap<COAD.CORPORATIVO.Repositorios.Contexto.ASSINATURA, AssinaturaDTO>();
            //mAssinatura.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.ASSINATURA_TELEFONE, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.ASSINATURA_EMAIL, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CLIENTES_ENDERECO, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.HIST_ATEND_URA, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CHEQUE_DEVOLVIDO, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.ASSINATURA_TRANSFERENCIA, conf => conf.Ignore());
            mAssinatura.ForMember(obj => obj.CARTEIRA_CLIENTE, conf => conf.Ignore());
            //mAssinatura.ForMember(obj => obj.MOTIVO_CANCELAMENTO, conf => conf.Ignore());

            var mAssinaturaR = mAssinatura.ReverseMap();
            mAssinaturaR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.TIPO_PERIODO_ASSINATURA, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.ASSINATURA_TELEFONE, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.ASSINATURA_EMAIL, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.UEN, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.CARTEIRA_ASSINATURA, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.CLIENTES_ENDERECO, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.HIST_ATEND_URA, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.CHEQUE_DEVOLVIDO, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.ASSINATURA_TRANSFERENCIA, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.CARTEIRA_CLIENTE, conf => conf.Ignore());
            mAssinaturaR.ForMember(obj => obj.MOTIVO_CANCELAMENTO, conf => conf.Ignore());

            //---- AssinaturaTransferencia
            var mAssinaturaTransferencia = store.CreateMap<COAD.CORPORATIVO.Repositorios.Contexto.ASSINATURA_TRANSFERENCIA, AssinaturaTransferenciaDTO>();
             mAssinaturaTransferencia.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            var mAssinaturaTransferenciaR = mAssinaturaTransferencia.ReverseMap();
            mAssinaturaTransferenciaR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAssinaturaTransferenciaR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            //-------------CEP_BAIRRO
            var mCepBairro = store.CreateMap<CEP_BAIRRO, CepBairroDTO>();
            mCepBairro.ForMember(obj => obj.CEP_LOGRADOURO, conf => conf.Ignore());

            var mCepBairroR = mCepBairro.ReverseMap();
            mCepBairroR.ForMember(obj => obj.CEP_LOGRADOURO, conf => conf.Ignore());

            //-------------CEP_LOGRADOURO
            var mCepLogradouro = store.CreateMap<CEP_LOGRADOURO, CepLogradouroDTO>();
      
            var mCepLogradouroR = mCepLogradouro.ReverseMap();
            mCepLogradouroR.ForMember(obj => obj.CEP_BAIRRO, conf => conf.Ignore());
            mCepLogradouroR.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());

            //-------------
            var mAssinaturaEmail = store.CreateMap<ASSINATURA_EMAIL, AssinaturaEmailDTO>();
            mAssinaturaEmail.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
      
            var mAssinaturaEmailr = mAssinaturaEmail.ReverseMap();
            mAssinaturaEmailr.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mAssinaturaEmailr.ForMember(obj => obj.OPCAO_ATENDIMENTO, conf => conf.Ignore());
            mAssinaturaEmailr.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            //-------------

            //-------------
            var mContrato = store.CreateMap<COAD.CORPORATIVO.Repositorios.Contexto.CONTRATOS, ContratoDTO>();
            mContrato.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mContrato.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mContrato.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mContrato.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mContrato.ForMember(obj => obj.UF, conf => conf.Ignore());
            
            var mContrator = mContrato.ReverseMap();
            mContrator.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mContrator.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mContrator.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mContrator.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mContrator.ForMember(obj => obj.UF, conf => conf.Ignore());
            mContrator.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());
            //-------------

            //-------------
            var mParcelas = store.CreateMap<PARCELAS, ParcelasDTO>();
            mParcelas.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mParcelas.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            
            //mParcelas.ForMember(obj => obj.BANCOS, conf => conf.Ignore());

            var mParcelasr = mParcelas.ReverseMap();
            mParcelasr.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mParcelasr.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            
            //mParcelasr.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            //-------------

            var mAssinaturaTelefone = store.CreateMap<ASSINATURA_TELEFONE, AssinaturaTelefoneDTO>();
            mAssinaturaTelefone.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            //   mAssinaturaTelefone.ForMember(obj => obj.OPCAO_ATENDIMENTO, conf => conf.Ignore());
            //mAssinaturaTelefone.ForMember(obj => obj.TIPO_TELEFONE, conf => conf.Ignore());;

            var mAssinaturaTelefoner = mAssinaturaTelefone.ReverseMap();
            mAssinaturaTelefoner.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mAssinaturaTelefoner.ForMember(obj => obj.OPCAO_ATENDIMENTO, conf => conf.Ignore());
            mAssinaturaTelefoner.ForMember(obj => obj.TIPO_TELEFONE, conf => conf.Ignore());
            mAssinaturaTelefone.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            //-------------

            var mTipoTelefone = store.CreateMap<TIPO_TELEFONE, TipoTelefoneDTO>();
            mTipoTelefone.ForMember(obj => obj.ASSINATURA_TELEFONE, opt => opt.Ignore());
            mTipoTelefone.ForMember(obj => obj.CLIENTES_TELEFONE, opt => opt.Ignore());

            var mTipoTelefoner = mTipoTelefone.ReverseMap();
            mTipoTelefone.ForMember(obj => obj.ASSINATURA_TELEFONE, opt => opt.Ignore());

            //-------------Tipo Periodo Assinatura
            #region clientes
            var mTipoPeriodoAssinatura = store.CreateMap<TIPO_PERIODO_ASSINATURA, TipoPeriodoAssinaturaDTO>();
            mTipoPeriodoAssinatura.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            var mTipoPeriodoAssinaturaR = mTipoPeriodoAssinatura.ReverseMap();
            mTipoPeriodoAssinatura.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            #endregion
            //-------------Classificação
            var mClassificacao = store.CreateMap<CLASSIFICACAO, ClassificacaoDTO>();
            mClassificacao.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());


            var mClassificacaoR = mClassificacao.ReverseMap();
            mClassificacaoR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

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
            var mapeamentoTransportador = store.CreateMap<TRANSPORTADOR, TransportadorDTO>();
            //mapeamentoTransportador.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            var mapeamentoTransportadorr = mapeamentoTransportador.ReverseMap();
            mapeamentoTransportadorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoTransportador.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            // ---- Fornecedor
            var mapeamentoFornecedor = store.CreateMap<FORNECEDOR, FornecedorDTO>();
            mapeamentoFornecedor.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mapeamentoFornecedor.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoFornecedor.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());

            var mapeamentoFornecedorr = mapeamentoFornecedor.ReverseMap();
            mapeamentoFornecedorr.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mapeamentoFornecedorr.ForMember(obj => obj.MUNICIPIO, conf => conf.Ignore());
            mapeamentoFornecedorr.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());

            // ---- Nota Fiscal 

            var mapeamentoNotaFiscal = store.CreateMap<NOTA_FISCAL, NotaFiscalDTO>();
            //   mapeamentoNotaFiscal.ForMember(obj => obj.NOTA_FISCAL_ITEM , conf => conf.Ignore());

            var mapeamentoNotaFiscalr = mapeamentoNotaFiscal.ReverseMap();
            mapeamentoNotaFiscalr.ForMember(obj => obj.FORNECEDOR, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            //   mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.NOTA_FISCAL_SITUACAO, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TIPO_DOC_FISCAL, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.TRANSPORTADOR, conf => conf.Ignore());
            mapeamentoNotaFiscalr.ForMember(obj => obj.UF, conf => conf.Ignore());

            // ---- Nota Fiscal 

            var mTipoDocFiscalDTO = store.CreateMap<TIPO_DOC_FISCAL, TipoDocFiscalDTO>();
            mTipoDocFiscalDTO.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            var mTipoDocFiscalDTOr = mTipoDocFiscalDTO.ReverseMap();
            mTipoDocFiscalDTOr.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());

            // ---- Representante

            var mRepresentante = store.CreateMap<REPRESENTANTE, RepresentanteDTO>();
            mRepresentante.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            //mRepresentante.ForMember(obj => obj.CARTEIRA_REPRESENTANTE, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.AGENDAMENTO, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            mRepresentante.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            var mRepresentanteR = mRepresentante.ReverseMap();
            //mRepresentanteR.ForMember(obj => obj., conf => conf.Ignore());
            mRepresentanteR.ForMember(obj => obj.UF, conf => conf.Ignore());
            mRepresentanteR.ForMember(obj => obj.REGIAO, conf => conf.Ignore());
            mRepresentanteR.ForMember(obj => obj.CARTEIRA_REPRESENTANTE, conf => conf.Ignore());

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

            //var mEmpresaREF = store.CreateMap<EMPRESA_REF, EmpresaRefDTO>();
            //mEmpresaREF.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.PARCELAS , conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.SPED_ARQUIVO , conf => conf.Ignore());
            //mEmpresaREF.ForMember(obj => obj.TOTAL_VENDAS_CARTAO , conf => conf.Ignore());

            //var mEmpresaREFR = mEmpresaREF.ReverseMap();
            //mEmpresaREFR.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.CONFIG_SPED_FISCAL, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.PARCELAS , conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.SPED_ARQUIVO, conf => conf.Ignore());
            //mEmpresaREFR.ForMember(obj => obj.TOTAL_VENDAS_CARTAO, conf => conf.Ignore());

            // ----Config Sped Fiscal

            var mSpedArquivo = store.CreateMap<SPED_ARQUIVO, SpedArquivoDTO>();
            mSpedArquivo.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());

            var mSpedArquivoR = mSpedArquivo.ReverseMap();
            mSpedArquivoR.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());

            // ----Nota Fiscal Item

            var mNotaFiscalItem = store.CreateMap<NOTA_FISCAL_ITEM, NotaFiscalItemDTO>();
            mNotaFiscalItem.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.CST, conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.NOTA_FISCAL_ITEM_OBS, conf => conf.Ignore());
            //mNotaFiscalItem.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mNotaFiscalItem.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());
            
            var mNotaFiscalItemR = mNotaFiscalItem.ReverseMap();
            mNotaFiscalItemR.ForMember(obj => obj.CFOP_TABLE, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.CST, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            //mNotaFiscalItemR.ForMember(obj => obj.NOTA_FISCAL_ITEM_OBS, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mNotaFiscalItemR.ForMember(obj => obj.UNIDADE_MEDIDA, conf => conf.Ignore());

            // ----Nota Fiscal Item Obs

            var mNotaFiscalItemOBS = store.CreateMap<NOTA_FISCAL_ITEM_OBS, NotaFiscalItemOBSDTO>();
            mNotaFiscalItemOBS.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            var mNotaFiscalItemOBSR = mNotaFiscalItemOBS.ReverseMap();
            //mNotaFiscalItemOBSR.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            // ----CST

            var mCST = store.CreateMap<CST, CSTDTO>();
            mCST.ForMember(obj => obj.CST_TIPO, conf => conf.Ignore());
            mCST.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

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
            //mURA.ForMember(obj => obj.HIST_ATEND_URA , conf => conf.Ignore());
            //mURA.ForMember(obj => obj.URA_COAD , conf => conf.Ignore());
            mURA.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());


            var mURAr = mURA.ReverseMap();
            mURAr.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mURAr.ForMember(obj => obj.UF, conf => conf.Ignore());
            mURAr.ForMember(obj => obj.HIST_ATEND_URA, conf => conf.Ignore());
            mURAr.ForMember(obj => obj.URA_COAD, conf => conf.Ignore());
            mURAr.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());

            // ----
            var mURAConfig = store.CreateMap<URA_CONFIG, UraConfigDTO>();
            mURAConfig.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAConfig.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mURAConfig.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());

            var mURAConfigr = mURAConfig.ReverseMap();
            mURAConfigr.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAConfigr.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());
            mURAConfigr.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());

            // ----
            var mURAProduto = store.CreateMap<URA_PRODUTO, UraProdutoDTO>();
            mURAProduto.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAProduto.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());
            mURAProduto.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());


            var mURAProdutor = mURAProduto.ReverseMap();
            mURAProdutor.ForMember(obj => obj.URA, conf => conf.Ignore());
            mURAProdutor.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());
            mURAProdutor.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            // ----
            var mProdutoArea = store.CreateMap<URA_PRODUTO_AREA, UraProdutoAreaDTO>();
            mProdutoArea.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());  
            mProdutoArea.ForMember(obj => obj.AREA_CONSULTORIA, conf => conf.Ignore());

            var mProdutoArear = mProdutoArea.ReverseMap();
            mProdutoArear.ForMember(obj => obj.URA_CONFIG, conf => conf.Ignore());  
            mProdutoArear.ForMember(obj => obj.AREA_CONSULTORIA, conf => conf.Ignore());

            // ----Areas Consultoria
            var mAreaConsultoria = store.CreateMap<AREA_CONSULTORIA, AreaConsultoriaDTO>();
            mAreaConsultoria.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreaConsultoria.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());

            var mAreaConsultoriar = mAreaConsultoria.ReverseMap();
            mAreaConsultoriar.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());
            mAreaConsultoriar.ForMember(obj => obj.URA_PRODUTO_AREA, conf => conf.Ignore());

            //--------

            var mSeqProd = store.CreateMap<SEQ_PROD, SeqProdDTO>();
            var mSeqProdR = mSeqProd.ReverseMap();

            //---- Ura Log
            var mUraLog = store.CreateMap<URA_LOG, UraLogDTO>();
            //mUraLog.ForMember(obj => obj.URA_TP_ATU, conf => conf.Ignore());
            mUraLog.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            var mUraLogr = mUraLog.ReverseMap();
            mUraLogr.ForMember(obj => obj.URA_TP_ATU, conf => conf.Ignore());
            mUraLogr.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            //------------
            //---- Ura Log Tipo Atualização
            var mUraTPAtu = store.CreateMap<URA_TP_ATU, UraTPAtuDTO>();
            mUraTPAtu.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());

            var mUraTPAtur = mUraTPAtu.ReverseMap();
            mUraTPAtur.ForMember(obj => obj.URA_LOG, conf => conf.Ignore());
            //------------


            var mCondicaoPagamento = store.CreateMap<CONDICAO_PAGAMENTO, CondicaoPagamentoDTO>();
            //mCondicaoPagamento.ForMember(obj => obj.TABELA_PRECO, conf => conf.Ignore());

            var mCondicaoPagamentoR = mCondicaoPagamento.ReverseMap();

            var mTipoPagamento = store.CreateMap<TIPO_PAGAMENTO, TipoPagamentoDTO>();
            mTipoPagamento.ForMember(obj => obj.PEDIDO_PAGAMENTO, conf => conf.Ignore());


            var mTipoPagamentoR = mTipoPagamento.ReverseMap();

            //#region Regiao
            //var mRegiao = store.CreateMap<REGIAO, RegiaoDTO>();
            //mRegiao.ForMember(obj => obj.REGIAO_TABELA_PRECO, conf => conf.Ignore());
            //mRegiao.ForMember(obj => obj.FRANQUIA, conf => conf.Ignore());
            //mRegiao.ForMember(obj => obj.FILA_CADASTRO, conf => conf.Ignore());
            //mRegiao.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            //mRegiao.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());


            //var mRegiaoR = mRegiao.ReverseMap();

            //#endregion
            //var mRegiaoTabelaPreco = store.CreateMap<REGIAO_TABELA_PRECO, RegiaoTabelaPrecoDTO>();

            //var mRegiaoTabelaPrecoR = mRegiaoTabelaPreco.ReverseMap();
            //mRegiaoTabelaPrecoR.ForMember(obj => obj.REGIAO, conf => conf.Ignore());
            //mRegiaoTabelaPrecoR.ForMember(obj => obj.TABELA_PRECO, conf => conf.Ignore());



            //---------
            // ---Tabela Preço
            //---------

            //var mTabelaPreco = store.CreateMap<TABELA_PRECO, TabelaPrecoDTO>();
            //mTabelaPreco.ForMember(obj => obj.REGIAO_TABELA_PRECO, conf => conf.Ignore());

            //var mTabelaPrecoR = mTabelaPreco.ReverseMap();
            //mTabelaPrecoR.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());


            //---------
            // ---Tipo Atendimento
            //---------

            var mTipoAtendimento = store.CreateMap<TIPO_ATENDIMENTO, TipoAtendimentoDTO>();
            mTipoAtendimento.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            //     mTipoAtendimento.ForMember(obj => obj.CLASSIFICACAO_ATENDIMENTO, conf => conf.Ignore());

            var mTipoAtendimentor = mTipoAtendimento.ReverseMap();
            mTipoAtendimentor.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            mTipoAtendimentor.ForMember(obj => obj.CLASSIFICACAO_ATENDIMENTO, conf => conf.Ignore());

            //---------
            // ---Classificacao Atendimento
            //---------

            var mClassAtendimento = store.CreateMap<CLASSIFICACAO_ATENDIMENTO, ClassificacaoAtendimentoDTO>();
            mClassAtendimento.ForMember(obj => obj.TIPO_ATENDIMENTO, conf => conf.Ignore());

            var mClassAtendimentoR = mClassAtendimento.ReverseMap();
            mClassAtendimentoR.ForMember(obj => obj.TIPO_ATENDIMENTO, conf => conf.Ignore());


            //---------
            // ---Ação Atendimento
            //---------

            var mAcaoAtendimento = store.CreateMap<ACAO_ATENDIMENTO, AcaoAtendimentoDTO>();
            mAcaoAtendimento.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());

            var mAcaoAtendimentor = mAcaoAtendimento.ReverseMap();
            mAcaoAtendimentor.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());

            //---------
            // ---Historico Atendimento
            //---------

            var mHistoricoAtendimento = store.CreateMap<HISTORICO_ATENDIMENTO, HistoricoAtendimentoDTO>();
            //mHistoricoAtendimento.ForMember(obj => obj.ACAO_ATENDIMENTO, conf => conf.Ignore());
            mHistoricoAtendimento.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            //mHistoricoAtendimento.ForMember(obj => obj.TIPO_ATENDIMENTO, conf => conf.Ignore());
            mHistoricoAtendimento.ForMember(obj => obj.UEN, conf => conf.Ignore());
            mHistoricoAtendimento.ForMember(obj => obj.AGENDAMENTO, conf => conf.Ignore());

            var mHistoricoAtendimentor = mHistoricoAtendimento.ReverseMap();
            mHistoricoAtendimentor.ForMember(obj => obj.ACAO_ATENDIMENTO, conf => conf.Ignore());
            mHistoricoAtendimentor.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mHistoricoAtendimentor.ForMember(obj => obj.TIPO_ATENDIMENTO, conf => conf.Ignore());
            mHistoricoAtendimentor.ForMember(obj => obj.UEN, conf => conf.Ignore());
            mHistoricoAtendimentor.ForMember(obj => obj.AGENDAMENTO, conf => conf.Ignore());
            mHistoricoAtendimentor.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            //---------
            // ---Linha Produto
            //---------

            var mLinhaProduto = store.CreateMap<LINHA_PRODUTO, LinhaProdutoDTO>();
            mLinhaProduto.ForMember(obj => obj.LINHA_PRODUTO_INFORMATIVO, conf => conf.Ignore());
            mLinhaProduto.ForMember(obj => obj.ORIGEM_ACESSO, conf => conf.Ignore());
            mLinhaProduto.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mLinhaProduto.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mLinhaProdutor = mLinhaProduto.ReverseMap();
            mLinhaProdutor.ForMember(obj => obj.LINHA_PRODUTO_INFORMATIVO, conf => conf.Ignore());
            mLinhaProdutor.ForMember(obj => obj.ORIGEM_ACESSO, conf => conf.Ignore());
            mLinhaProdutor.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());
            mLinhaProdutor.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            //---------
            // ---Linha Produto Informativo
            //---------

            var mLinhaProdutoInformativo = store.CreateMap<LINHA_PRODUTO_INFORMATIVO, LinhaProdutoInformativoDTO>();
            //        mLinhaProdutoInformativo.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());

            var mLinhaProdutoInformativor = mLinhaProdutoInformativo.ReverseMap();
            mLinhaProdutoInformativor.ForMember(obj => obj.LINHA_PRODUTO, conf => conf.Ignore());

            //---------
            // ---Historico de Atendimento (Email)
            //---------

            var mHistAtendEmail = store.CreateMap<HIST_ATEND_EMAIL, HistAtendEmailDTO>();
            mHistAtendEmail.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            var mHistAtendEmailr = mHistAtendEmail.ReverseMap();
            mHistAtendEmailr.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());


            //---------
            // ---Historico de Atendimento (URA)
            //---------

            var mHistAtendUra = store.CreateMap<HIST_ATEND_URA, HistAtendUraDTO>();
            mHistAtendUra.ForMember(obj => obj.URA, conf => conf.Ignore());
            mHistAtendUra.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());


            var mHistAtendUrar = mHistAtendUra.ReverseMap();
            mHistAtendUrar.ForMember(obj => obj.URA, conf => conf.Ignore());
            mHistAtendUrar.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            //---------
            // --- Clientes com acesso a URA (URA_COAD)
            //---------

            var mUraCoad = store.CreateMap<URA_COAD, UraCoadDTO>();
            mUraCoad.ForMember(obj => obj.URA, conf => conf.Ignore());

            var mUraCoadr = mUraCoad.ReverseMap();
            mUraCoadr.ForMember(obj => obj.URA, conf => conf.Ignore());

            //---------
            //--- URA
            //---------

            var mUra = store.CreateMap<URA, UraDTO>();
            mUra.ForMember(obj => obj.UF, conf => conf.Ignore());
            //mUra.ForMember(obj => obj.HIST_ATEND_URA, conf => conf.Ignore());
            //mUra.ForMember(obj => obj.URA_COAD, conf => conf.Ignore());
            mUra.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());


            var mUraR = mUra.ReverseMap();
            mUraR.ForMember(obj => obj.UF, conf => conf.Ignore());
            mUraR.ForMember(obj => obj.HIST_ATEND_URA, conf => conf.Ignore());
            mUraR.ForMember(obj => obj.URA_COAD, conf => conf.Ignore());
            mUraR.ForMember(obj => obj.URA_PRODUTO, conf => conf.Ignore());

            //---------
            //--- UEN
            //---------

            var mUen = store.CreateMap<UEN, UENDTO>();
            mUen.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mUen.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mUen.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            var mUenR = mUen.ReverseMap();
            mUenR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mUenR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mUenR.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());

            //---------
            // ---Franquia
            //---------

            //var mFranquia = store.CreateMap<FRANQUIA, FranquiaDTO>();
            ////mFranquia.ForMember(obj => obj., conf => conf.Ignore());

            //var mFranquiaR = mFranquia.ReverseMap();
            //mFranquiaR.ForMember(obj => obj.REGIAO, conf => conf.Ignore());


            var mUEN = store.CreateMap<UEN, UENDTO>();
            mUEN.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mUEN.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mUEN.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mUEN.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());


            var mUENR = mUEN.ReverseMap();

            // Informações de Market

            var mInfoMarketing = store.CreateMap<INFO_MARKETING, InfoMarketingDTO>();
            mInfoMarketing.ForMember(obj => obj.PRODUTO_COMPOSICAO_INFO_MARKETING, conf => conf.Ignore());
            mInfoMarketing.ForMember(obj => obj.AREA_INFO_MARKETING, conf => conf.Ignore());
            mInfoMarketing.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            var mInfoMarketingR = mInfoMarketing.ReverseMap();
            mInfoMarketingR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mInfoMarketingR.ForMember(obj => obj.ORIGEM_CADASTRO, conf => conf.Ignore());

            // AreaInfoMarketing ------------------------------------------------------------------


            var mAreaInfoMarketing = store.CreateMap<AREA_INFO_MARKETING, AreaInfoMarketingDTO>();
            mAreaInfoMarketing.ForMember(obj => obj.INFO_MARKETING, conf => conf.Ignore());

            var mAreaInfoMarketingR = mAreaInfoMarketing.ReverseMap();
            mAreaInfoMarketingR.ForMember(obj => obj.AREAS, conf => conf.Ignore());
            mAreaInfoMarketingR.ForMember(obj => obj.INFO_MARKETING, conf => conf.Ignore());

            // --------- ProdutoComposicaoInfoMarketing --------------------------------------------

            var mProdutoComposicaoInfoMarketing = store.CreateMap<PRODUTO_COMPOSICAO_INFO_MARKETING, ProdutoComposicaoInfoMarketingDTO>();
            mProdutoComposicaoInfoMarketing.ForMember(obj => obj.INFO_MARKETING, conf => conf.Ignore());

            var mProdutoComposicaoInfoMarketingR = mProdutoComposicaoInfoMarketing.ReverseMap();
            mProdutoComposicaoInfoMarketingR.ForMember(obj => obj.INFO_MARKETING, conf => conf.Ignore());
            mProdutoComposicaoInfoMarketingR.ForMember(obj => obj.PRODUTO_COMPOSICAO, conf => conf.Ignore());

            //------- Origem Cadastro -------------------------------------------------------------
            var mOrigemCadastro = store.CreateMap<ORIGEM_CADASTRO, OrigemCadastroDTO>();
            mOrigemCadastro.ForMember(obj => obj.INFO_MARKETING, conf => conf.Ignore());

            var mOrigemCadastroR = mOrigemCadastro.ReverseMap();

            //// --------- ProdutoComposicaoInfoMarketing --------------------------------------------

            var mProdutoFamilia = store.CreateMap<PRODUTO_FAMILIA, ProdutoFamiliaDTO>();
            mProdutoFamilia.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());

            var mProdutoFamiliaR = mProdutoFamilia.ReverseMap();
            mProdutoFamiliaR.ForMember(obj => obj.PRODUTOS, conf => conf.Ignore());


            ////------- Cliente Telefone -------------------------------------------------------------
            var mClienteTelefone = store.CreateMap<CLIENTES_TELEFONE, ClienteTelefoneDTO>();

            var mClienteTelefoneR = mClienteTelefone.ReverseMap();
            mClienteTelefoneR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            //var mClienteTelefoneR = mClienteTelefone.ReverseMap();
            //mClienteTelefoneR.ForMember(obj => obj.TIPO_TELEFONE, conf => conf.Ignore());
            //mClienteTelefoneR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());

            // ----------------agendamentos ------------------------------------------------------
            var mAgendamento = store.CreateMap<AGENDAMENTO, AgendamentoDTO>();
            mAgendamento.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());
            mAgendamento.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            var mAgendamentoR = mAgendamento.ReverseMap();
            mAgendamentoR.ForMember(obj => obj.CARTEIRA, conf => conf.Ignore());
            mAgendamentoR.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            mAgendamentoR.ForMember(obj => obj.HISTORICO_ATENDIMENTO, conf => conf.Ignore());

            //-------------------- Tipo de Notificação ------------------------------------------
            var mTipoNotificacao = store.CreateMap<TIPO_NOTIFICACAO, TipoNotificacaoDTO>();
            mTipoNotificacao.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            var mTipoNotificacaoR = mTipoNotificacao.ReverseMap();
            mTipoNotificacaoR.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            //--------------------Notificações ----------------------------------------------------

            var mNotificacoes = store.CreateMap<NOTIFICACOES, NotificacoesDTO>();

            var mNotificacoesR = mNotificacoes.ReverseMap();
            mNotificacoesR.ForMember(obj => obj.AGENDAMENTO, conf => conf.Ignore());
            mNotificacoesR.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mNotificacoesR.ForMember(obj => obj.TIPO_NOTIFICACAO, conf => conf.Ignore());
            mNotificacoesR.ForMember(obj => obj.URGENCIA_NOTIFICACAO, conf => conf.Ignore());

            // ------------------------ Urgência Notificações -------------------------------------

            var mUrgenciaNotificacao = store.CreateMap<URGENCIA_NOTIFICACAO, UrgenciaNotificacaoDTO>();
            mUrgenciaNotificacao.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            var mUrgenciaNotificacaoR = mUrgenciaNotificacao.ReverseMap();
            mUrgenciaNotificacaoR.ForMember(obj => obj.NOTIFICACOES, conf => conf.Ignore());

            // ------------------------ Fila de Cadastro -------------------------------------CfoTableDTO

            var mFilaCadastro = store.CreateMap<FILA_CADASTRO, FilaCadastroDTO>();

            var mFilaCadastroR = mFilaCadastro.ReverseMap();
            mFilaCadastroR.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mFilaCadastroR.ForMember(obj => obj.REGIAO, conf => conf.Ignore());

            var mTabSeq = store.CreateMap<TAB_SEQ, TabSeqDTO>();
            var mTabSeqR = mTabSeq.ReverseMap();

            var mTabSeq1 = store.CreateMap<TAB_SEQ, TabSeqDTO>();

            var mTabSeqR1 = mTabSeq.ReverseMap();

            // ------------------------ Fila de Cadastro -------------------------------------

            var mCfopTable = store.CreateMap<CFOP_TABLE, CFOTableDTO>();
            mCfopTable.ForMember(obj => obj.CFOP_ICMS, conf => conf.Ignore());
            mCfopTable.ForMember(obj => obj.CFOP_REFERENCIA, conf => conf.Ignore());
            mCfopTable.ForMember(obj => obj.CFOP_REFERENCIA1, conf => conf.Ignore());
            mCfopTable.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mCfopTable.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            var mCfopTableR = mCfopTable.ReverseMap();
            mCfopTableR.ForMember(obj => obj.CFOP_ICMS, conf => conf.Ignore());
            mCfopTableR.ForMember(obj => obj.CFOP_REFERENCIA, conf => conf.Ignore());
            mCfopTableR.ForMember(obj => obj.CFOP_REFERENCIA1, conf => conf.Ignore());
            mCfopTableR.ForMember(obj => obj.NOTA_FISCAL, conf => conf.Ignore());
            mCfopTableR.ForMember(obj => obj.NOTA_FISCAL_ITEM, conf => conf.Ignore());

            // ------------------------ CFOP_ICMS -------------------------------------------//

            var mCfoIcms = store.CreateMap<CFOP_ICMS, CFOPIcmsDTO>();
            var mCfoIcmsR = mTabSeq.ReverseMap();

            // ------------------------ CFOP_REFERENCIA ------------------------------------//

            var mCfoReferencia = store.CreateMap<CFOP_REFERENCIA, CFOPReferenciaDTO>();
            var mCfoReferenciaR = mTabSeq.ReverseMap();

            // ------------------------ BANCOS -------------------------------------------//

            var mBancos = store.CreateMap<BANCOS, BancosDTO>();
            mBancos.ForMember(obj => obj.CHEQUE_DEVOLVIDO, conf => conf.Ignore());
            mBancos.ForMember(obj => obj.PARCELA_LIQUIDACAO, conf => conf.Ignore());
            mBancos.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());

            var mBancosR = mBancos.ReverseMap();
            mBancosR.ForMember(obj => obj.CHEQUE_DEVOLVIDO, conf => conf.Ignore());
            mBancosR.ForMember(obj => obj.PARCELA_LIQUIDACAO, conf => conf.Ignore());
            mBancosR.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());

            // ------------------------ PARCELA_LIQUIDACAO ------------------------------------//

            var mParcelaLiquidacao = store.CreateMap<PARCELA_LIQUIDACAO, ParcelaLiquidacaoDTO>();
            //mParcelaLiquidacao.ForMember(obj => obj.BANCOS , conf => conf.Ignore());
            //mParcelaLiquidacao.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());
            var mParcelaLiquidacaoR = mParcelaLiquidacao.ReverseMap();
            //mParcelaLiquidacaoR.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            //mParcelaLiquidacaoR.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());

            // ------------------------ CHEQUE_DEVOLVIDO ------------------------------------//

            var mChequeDevolvido = store.CreateMap<CHEQUE_DEVOLVIDO, ChequeDevolvidoDTO>();
            //mChequeDevolvido.ForMember(obj => obj.BANCOS , conf => conf.Ignore());
            //mChequeDevolvido.ForMember(obj => obj.ASSINATURA,  conf => conf.Ignore());
            var mChequeDevolvidoR = mChequeDevolvido.ReverseMap();
            //mChequeDevolvidoR.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            //mChequeDevolvidoR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());


            // ------------------------ ASSINATURA_SENHA ------------------------------------//
            var mAssinaturaSenha = store.CreateMap<ASSINATURA_SENHA, AssinaturaSenhaDTO>();
            //mAssinaturaSenha.ForMember(obj => obj.ASSINATURA,  conf => conf.Ignore());
            var mAssinaturaSenhaR = mAssinaturaSenha.ReverseMap();
            //mAssinaturaSenhaR.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());

            //---------
            // --- Consultor 
            //---------

            var mConsultor = store.CreateMap<CONSULTOR, ConsultorDTO>();
            mConsultor.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());
            mConsultor.ForMember(obj => obj.HIST_ATEND_EMAIL1, conf => conf.Ignore());

            var mConsultorR = mConsultor.ReverseMap();
            mConsultorR.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());
            mConsultorR.ForMember(obj => obj.HIST_ATEND_EMAIL1, conf => conf.Ignore());


            ////---------
            //// --- Email Atendimento (Email)
            ////---------

            var mEmailAtend = store.CreateMap<EMAIL_ATEND, EmailAtendDTO>();
            mEmailAtend.ForMember(obj => obj.EMAIL_ATEND_ANEXO, conf => conf.Ignore());
            mEmailAtend.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());

            var mEmailAtendR = mEmailAtend.ReverseMap();
            mEmailAtendR.ForMember(obj => obj.EMAIL_ATEND_ANEXO, conf => conf.Ignore());
            mEmailAtendR.ForMember(obj => obj.HIST_ATEND_EMAIL, conf => conf.Ignore());


            ////---------
            //// --- Email Atendimento Anexo (Email)
            ////---------

            var mEmailAtendAnexo = store.CreateMap<EMAIL_ATEND_ANEXO, EmailAtendAnexoDTO>();
            mEmailAtendAnexo.ForMember(obj => obj.EMAIL_ATEND, conf => conf.Ignore());

            var mEmailAtendAnexor = mEmailAtendAnexo.ReverseMap();
            mEmailAtendAnexor.ForMember(obj => obj.EMAIL_ATEND, conf => conf.Ignore());


            //---------
            // --- Motivo Cancelamento
            //---------

            var mMotivoCancelamento = store.CreateMap<MOTIVO_CANCELAMENTO, MotivoCancelamentoDTO>();
            mMotivoCancelamento.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mMotivoCancelamento.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            // mMotivoCancelamento.ForMember(obj => obj.TIPO_CANCELAMENTO, conf => conf.Ignore());

            var mMotivoCancelamentor = mMotivoCancelamento.ReverseMap();
            mMotivoCancelamentor.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mMotivoCancelamentor.ForMember(obj => obj.CONTRATOS, conf => conf.Ignore());
            mMotivoCancelamentor.ForMember(obj => obj.TIPO_CANCELAMENTO, conf => conf.Ignore());

            /*
            //---------
            // --- Mundipagg Assinatura
            //---------
            var mMundipaggAssinatura = store.CreateMap<MUNDIPAGG_ASSINATURA, MundipaggAssinaturaDTO>();
            mMundipaggAssinatura.ForMember(obj => obj.MUNDIPAGG_CLIENTE, conf => conf.Ignore());


            //---------
            // --- Mundipagg Cliente
            //---------
            var mMundipaggCliente = store.CreateMap<MUNDIPAGG_CLIENTE, MundipaggClienteDTO>();
            mMundipaggCliente.ForMember(obj => obj.CLIENTES, conf => conf.Ignore());
            */
            //---------
            // --- Tipo Cancelamento
            //---------

            var mTipoCancelamento = store.CreateMap<TIPO_CANCELAMENTO, TipoCancelamentoDTO>();
            mTipoCancelamento.ForMember(obj => obj.MOTIVO_CANCELAMENTO, conf => conf.Ignore());
            var mTipoCancelamentor = mTipoCancelamento.ReverseMap();
            mTipoCancelamentor.ForMember(obj => obj.MOTIVO_CANCELAMENTO, conf => conf.Ignore());


            var mRetornoTransacao = store.CreateMap<RETORNO_TRANSACAO, RetornoTransacaoDTO>();
            mRetornoTransacao.ForMember(obj => obj.ITEM_PEDIDO , conf => conf.Ignore());
            mRetornoTransacao.ForMember(obj => obj.TIPO_PAGAMENTO, conf => conf.Ignore());

            var mRetornoTransacaor = mRetornoTransacao.ReverseMap();
            mRetornoTransacaor.ForMember(obj => obj.ITEM_PEDIDO, conf => conf.Ignore());
            mRetornoTransacaor.ForMember(obj => obj.TIPO_PAGAMENTO, conf => conf.Ignore());

            var mAgendaCobranca = store.CreateMap<AGENDA_COBRANCA, AgendaCobrancaDTO>();
            mAgendaCobranca.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mAgendaCobranca.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());
            var mAgendaCobrancar = mAgendaCobranca.ReverseMap();
            mAgendaCobrancar.ForMember(obj => obj.ASSINATURA, conf => conf.Ignore());
            mAgendaCobrancar.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());

            var mCnabArquivos = store.CreateMap<CNAB_ARQUIVOS, CnabArquivosDTO>();
          //  mCnabArquivos.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            mCnabArquivos.ForMember(obj => obj.CNAB_ARQUIVOS_ITEM, conf => conf.Ignore());
            mCnabArquivos.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());
            mCnabArquivos.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mCnabArquivos.ForMember(obj => obj.CONTA_REF, conf => conf.Ignore());

            var mCnabArquivosR = mCnabArquivos.ReverseMap();
            mCnabArquivosR.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            mCnabArquivosR.ForMember(obj => obj.CNAB_ARQUIVOS_ITEM, conf => conf.Ignore());
            mCnabArquivosR.ForMember(obj => obj.PARCELAS, conf => conf.Ignore());
            mCnabArquivosR.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            mCnabArquivosR.ForMember(obj => obj.CONTA_REF, conf => conf.Ignore());

            var mCnabArquivositem = store.CreateMap<CNAB_ARQUIVOS_ITEM, CnabArquivosItemDTO>();
           // mCnabArquivositem.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            mCnabArquivositem.ForMember(obj => obj.CNAB_ARQUIVOS, conf => conf.Ignore());
            var mCnabArquivositemR = mCnabArquivositem.ReverseMap();
            mCnabArquivositemR.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            mCnabArquivositemR.ForMember(obj => obj.CNAB_ARQUIVOS, conf => conf.Ignore());

            //***************************( CNAB )************************************************\\
            //var cnab = store.CreateMap<CNAB, CnabDTO>();
            //cnab.ForMember(obj => obj.EMPRESA_REF, conf => conf.Ignore());
            //cnab.ForMember(obj => obj.BANCOS, conf => conf.Ignore());
            //***********************************************************************************\\

            // ----Config Sped Fiscal

            var mSemanaPremiacaoRepr = store.CreateMap<SEMANA_PREMIACAO_REPR, SemanaPremiacaoReprDTO>();
            mSemanaPremiacaoRepr.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mSemanaPremiacaoRepr.ForMember(obj => obj.SEMANA_PREMIACAO, conf => conf.Ignore());

            var mSemanaPremiacaoReprr = mSemanaPremiacaoRepr.ReverseMap();
            mSemanaPremiacaoRepr.ForMember(obj => obj.REPRESENTANTE, conf => conf.Ignore());
            mSemanaPremiacaoRepr.ForMember(obj => obj.SEMANA_PREMIACAO, conf => conf.Ignore());

        }

        public void Ignore(IMemberConfigurationExpression tes)
        {
            tes.Ignore();
        }
    }
}
