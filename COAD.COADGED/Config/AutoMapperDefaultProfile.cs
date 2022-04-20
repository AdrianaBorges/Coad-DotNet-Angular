using AutoMapper;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Config
{
    public class AutoMapperDefaultProfile: GenericProfile
    {
        public override void Configure(ConfigurationStore store)
        {
            // Cargos...
            var mapeamentoCargos = store.CreateMap<CARGOS, CargosDTO>();
            var mapeamentoCargosReverso = mapeamentoCargos.ReverseMap();
            mapeamentoCargos.ForMember(obj => obj.COLABORADOR, conf => conf.Ignore());
            mapeamentoCargos.ForMember(obj => obj.PUBLICACAO_FLUXO_ETAPA, conf => conf.Ignore());

            // Colaboradores...
            var mapeamentoColaborador = store.CreateMap<COLABORADOR, ColaboradorDTO>();
            var mapeamentoColaboradorReverso = mapeamentoColaborador.ReverseMap();
            mapeamentoColaboradorReverso.ForMember(obj => obj.CARGOS, conf => conf.Ignore());
            mapeamentoColaborador.ForMember(obj => obj.PUBLICACAO_CICLO_APROVACAO, conf => conf.Ignore());
            mapeamentoColaborador.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoColaboradorReverso.ForMember(obj => obj.AREAS_CONSULTORIA, conf => conf.Ignore());

            // Areas...
            var mapeamentoAreas = store.CreateMap<AREAS_CONSULTORIA, AreasDTO>();
            mapeamentoAreas.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoAreas.ForMember(obj => obj.TITULACAO, conf => conf.Ignore());
            mapeamentoAreas.ForMember(obj => obj.COLABORADOR, conf => conf.Ignore());
        //    mapeamentoAreas.ForMember(obj => obj.AREAS_CONSULTORIA_NEWS, conf => conf.Ignore());

            var mapeamentoAreasReverso = mapeamentoAreas.ReverseMap();
            //mapeamentoAreasReverso.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoAreasReverso.ForMember(obj => obj.TITULACAO, conf => conf.Ignore());
            mapeamentoAreasReverso.ForMember(obj => obj.COLABORADOR, conf => conf.Ignore());
         //        mapeamentoAreasReverso.ForMember(obj => obj.AREAS_CONSULTORIA_NEWS, conf => conf.Ignore());


            // Capital...
            var mapeamentoCapital = store.CreateMap<CAPITAL, CapitalDTO>();
            var mapeamentoCapitalReverso = mapeamentoCapital.ReverseMap();
            mapeamentoCapital.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoCapitalReverso.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Labels...
            var mapeamentoLabels = store.CreateMap<LABELS, LabelsDTO>();
            var mapeamentoLabelsReverso = mapeamentoLabels.ReverseMap();
            mapeamentoLabels.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            // Periodicidade...
            var mapeamentoPeriodicidade = store.CreateMap<PERIODICIDADE, PeriodicidadeDTO>();
            var mapeamentoPeriodicidadeReverso = mapeamentoPeriodicidade.ReverseMap();
            mapeamentoPeriodicidade.ForMember(obj => obj.VEICULO, conf => conf.Ignore());

            // Veiculos...
            var mapeamentoVeiculo = store.CreateMap<VEICULO, VeiculoDTO>();
            var mapeamentoVeiculoReverso = mapeamentoVeiculo.ReverseMap();
            mapeamentoVeiculoReverso.ForMember(obj => obj.PERIODICIDADE, conf => conf.Ignore());
            mapeamentoVeiculo.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            // Tipo de Matéria...
            var mapeamentoTipoMateria = store.CreateMap<TIPO_MATERIA, TipoMateriaDTO>();
            var mapeamentoTipoMateriaReverso = mapeamentoTipoMateria.ReverseMap();
            mapeamentoTipoMateria.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            // Tipo de Ato...
            var mapeamentoTipoAto = store.CreateMap<TIPO_ATO, TipoAtoDTO>();
            var mapeamentoTipoAtoReverso = mapeamentoTipoAto.ReverseMap();
            mapeamentoTipoAto.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoTipoAto.ForMember(obj => obj.PUBLICACAO1, conf => conf.Ignore());

            // Seções...
            var mapeamentoSecoes = store.CreateMap<SECOES, SecoesDTO>();
            var mapeamentoSecoesReverso = mapeamentoSecoes.ReverseMap();
            mapeamentoSecoes.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            // Titulações...
            var mapeamentoTitulacao = store.CreateMap<TITULACAO, TitulacaoDTO>();
            mapeamentoTitulacao.ForMember(obj => obj.AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoTitulacao.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());
            mapeamentoTitulacao.ForMember(obj => obj.TITULACAO2, conf => conf.Ignore());
            mapeamentoTitulacao.ForMember(obj => obj.TITULACAO1, conf => conf.Ignore());
            mapeamentoTitulacao.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());



            var mapeamentoTitulacaoReverso = mapeamentoTitulacao.ReverseMap();
            mapeamentoTitulacaoReverso.ForMember(obj => obj.AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoTitulacaoReverso.ForMember(obj => obj.TAB_DINAMICA_CONFIG , conf => conf.Ignore());
            mapeamentoTitulacaoReverso.ForMember(obj => obj.TITULACAO2, conf => conf.Ignore());
            mapeamentoTitulacaoReverso.ForMember(obj => obj.TITULACAO1, conf => conf.Ignore());
            mapeamentoTitulacaoReverso.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());



            // Órgãos...
            var mapeamentoOrgao = store.CreateMap<ORGAO, OrgaoDTO>();
            var mapeamentoOrgaoReverso = mapeamentoOrgao.ReverseMap();
            mapeamentoOrgao.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoOrgao.ForMember(obj => obj.PUBLICACAO1, conf => conf.Ignore());


            // Informativos...
            var mapeamentoInformativo = store.CreateMap<INFORMATIVO, InformativoDTO>();
            var mapeamentoInformativoReverso = mapeamentoInformativo.ReverseMap();
            mapeamentoInformativo.ForMember(obj => obj.PUBLICACAO_UF, conf => conf.Ignore());

            // UFs...
            var mapeamentoUF = store.CreateMap<UF_REF, UfDTO>();
            var mapeamentoUFReverso = mapeamentoUF.ReverseMap();
            mapeamentoUF.ForMember(obj => obj.PUBLICACAO_UF, conf => conf.Ignore());

            // Publicações...
            var mapeamentoPublicacao = store.CreateMap<PUBLICACAO, PublicacaoDTO>();
            mapeamentoPublicacao.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoPublicacao.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());
        
    
            var mapeamentoPublicacaoReverso = mapeamentoPublicacao.ReverseMap();
            //mapeamentoPublicacao.ForMember(obj => obj.PUBLICACAO_ALTERACAO_REVOGACAO, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.LABELS, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.ORGAO, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.PLUBLICACAO_FLUXO, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.SECOES, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.TIPO_ATO, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.TIPO_MATERIA, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.VEICULO, conf => conf.Ignore());
            mapeamentoPublicacaoReverso.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());

            // Publicacao - alteração e revogação
            var mapeamentoPublicacaoAlteracaoRevogacao = store.CreateMap<PUBLICACAO_ALTERACAO_REVOGACAO, PublicacaoAlteracaoRevogacaoDTO>();
            var mapeamentoPublicacaoAlteracaoRevogacaoReverso = mapeamentoPublicacaoAlteracaoRevogacao.ReverseMap();
            mapeamentoPublicacaoAlteracaoRevogacaoReverso.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoPublicacaoAlteracaoRevogacao.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            // Publicacao - uf
            var mapeamentoPublicacaoUf = store.CreateMap<PUBLICACAO_UF, PublicacaoUfDTO>();
            var mapeamentoPublicacaoUfReverso = mapeamentoPublicacaoUf.ReverseMap();
            mapeamentoPublicacaoUf.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoPublicacaoUf.ForMember(obj => obj.INFORMATIVO, conf => conf.Ignore());
            mapeamentoPublicacaoUf.ForMember(obj => obj.UF_REF, conf => conf.Ignore());

            // Publicacao - titulacao
            var mapeamentoPublicacaoTitulacao = store.CreateMap<PUBLICACAO_TITULACAO, PublicacaoTitulacaoDTO>();
            var mapeamentoPublicacaoTitulacaoReverso = mapeamentoPublicacaoTitulacao.ReverseMap();
            mapeamentoPublicacaoTitulacao.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao - remissao
            var mapeamentoPublicacaoRemissao = store.CreateMap<PUBLICACAO_REMISSAO, PublicacaoRemissaoDTO>();
            var mapeamentoPublicacaoRemissaoReverso = mapeamentoPublicacaoRemissao.ReverseMap();
            mapeamentoPublicacaoRemissao.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoPublicacaoRemissaoReverso.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao - remissivo
            var mapeamentoPublicacaoRemissivo = store.CreateMap<PUBLICACAO_REMISSIVO, PublicacaoRemissivoDTO>();
            var mapeamentoPublicacaoRemissivoReverso = mapeamentoPublicacaoRemissivo.ReverseMap();
            mapeamentoPublicacaoRemissivo.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao - palavra chave
            var mapeamentoPublicacaoPalavraChave = store.CreateMap<PUBLICACAO_PALAVRA_CHAVE, PublicacaoPalavraChaveDTO>();
            var mapeamentoPublicacaoPalavraChaveReverso = mapeamentoPublicacaoPalavraChave.ReverseMap();
            mapeamentoPublicacaoPalavraChave.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao - config
            var mapeamentoPublicacaoConfig = store.CreateMap<PUBLICACAO_CONFIG, PublicacaoConfigDTO>();
            var mapeamentoPublicacaoConfigReverso = mapeamentoPublicacaoConfig.ReverseMap();
            mapeamentoPublicacaoConfig.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao por áreas da consultoria...
            var mapeamentoPublicacaoAreasConsultoria = store.CreateMap<PUBLICACAO_AREAS_CONSULTORIA, PublicacaoAreaConsultoriaDTO>();
            var mapeamentoPublicacaoAreasConsultoriaReverso = mapeamentoPublicacaoAreasConsultoria.ReverseMap();
            mapeamentoPublicacaoAreasConsultoriaReverso.ForMember(obj => obj.AREAS_CONSULTORIA1, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoriaReverso.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.AREAS_CONSULTORIA, conf => conf.Ignore());
            mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.AREAS_CONSULTORIA1, conf => conf.Ignore());
            mapeamentoPublicacaoAreasConsultoriaReverso.ForMember(obj => obj.CAPITAL, conf => conf.Ignore());
            mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.CAPITAL, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_CONFIG, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_PALAVRA_CHAVE, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_REMISSIVO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_REMISSAO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_TITULACAO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_UF, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_REVISAO, conf => conf.Ignore());
            //mapeamentoPublicacaoAreasConsultoria.ForMember(obj => obj.PUBLICACAO_REVISAO_COLABORADOR, conf => conf.Ignore());

            // Publicacao Revisão
            var mapeamentoPublicacaoRevisao = store.CreateMap<PUBLICACAO_REVISAO, PublicacaoRevisaoDTO>();
            var mapeamentoPublicacaoRevisaoReverso = mapeamentoPublicacaoRevisao.ReverseMap();
            mapeamentoPublicacaoRevisaoReverso.ForMember(obj => obj.COLABORADOR, conf => conf.Ignore());
            mapeamentoPublicacaoRevisaoReverso.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoPublicacaoRevisao.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // Publicacao Revisão Colaborador
            var mapeamentoPublicacaoRevisaoColaborador = store.CreateMap<PUBLICACAO_REVISAO_COLABORADOR, PublicacaoRevisaoColaboradorDTO>();
            var mapeamentoPublicacaoRevisaoColaboradorReverso = mapeamentoPublicacaoRevisaoColaborador.ReverseMap();
            mapeamentoPublicacaoRevisaoColaboradorReverso.ForMember(obj => obj.COLABORADOR, conf => conf.Ignore());
            mapeamentoPublicacaoRevisaoColaboradorReverso.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            mapeamentoPublicacaoRevisaoColaborador.ForMember(obj => obj.PUBLICACAO_AREAS_CONSULTORIA, conf => conf.Ignore());

            // fim do automapeamento do COADGED by ALT

//******************************************************************************************************************************************************//            
            
            // Tabela Dinamica

            var tabdinamica = store.CreateMap<TAB_DINAMICA, TabDinamicaDTO>();
            tabdinamica.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            var tabdinamicar = tabdinamica.ReverseMap();
            tabdinamicar.ForMember(obj => obj.TAB_DINAMICA_CONFIG , conf => conf.Ignore());
            tabdinamicar.ForMember(obj => obj.TAB_DINAMICA_ITEM, conf => conf.Ignore());
            
            // Tabela Dinamica Item

            var tabdinamicaitem = store.CreateMap<TAB_DINAMICA_ITEM, TabDinamicaItemDTO>();
            var tabdinamicaitemr = tabdinamicaitem.ReverseMap();
            tabdinamicaitemr.ForMember(obj => obj.TAB_DINAMICA, conf => conf.Ignore());
            
            // Tabela Dinamica Config

            var tabdinamicaconfig = store.CreateMap<TAB_DINAMICA_CONFIG, TabDinamicaConfigDTO>();
            tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA, conf => conf.Ignore());
            //tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_CONFIG1, conf => conf.Ignore());
            //tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_CONFIG2, conf => conf.Ignore());
            tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_PUBLICACAO, conf => conf.Ignore());
            tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_GRUPO, conf => conf.Ignore());
            // tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_LINK, conf => conf.Ignore());
            // tabdinamicaconfig.ForMember(obj => obj.TITULACAO , conf => conf.Ignore());
            // tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_ORIGEM , conf => conf.Ignore());
            // tabdinamicaconfig.ForMember(obj => obj.TAB_DINAMICA_CONFIG_ITEM, conf => conf.Ignore());


            var tabdinamicaconfigr = tabdinamicaconfig.ReverseMap();
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA , conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_CONFIG_ITEM , conf => conf.Ignore());
            //tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_CONFIG1, conf => conf.Ignore());
            //tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_CONFIG2, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_PUBLICACAO, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_GRUPO, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_LINK, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TITULACAO, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_ORIGEM, conf => conf.Ignore());
            tabdinamicaconfigr.ForMember(obj => obj.TAB_DINAMICA_CONFIG_ITEM, conf => conf.Ignore());
         

            // Tabela Dinamica Config Item

            var tabdinamicaconfigitem = store.CreateMap<TAB_DINAMICA_CONFIG_ITEM, TabDinamicaConfigItemDTO>();
            tabdinamicaconfigitem.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            var tabdinamicaconfigitemr = tabdinamicaconfigitem.ReverseMap();
            tabdinamicaconfigitemr.ForMember(obj => obj.TAB_DINAMICA_CONFIG , conf => conf.Ignore());


            // Tabela Dinamica Publicacao
            var tabdinamicapublicacao = store.CreateMap<TAB_DINAMICA_PUBLICACAO, TabDinamicaPublicacaoDTO>();
            tabdinamicapublicacao.ForMember(obj => obj.TAB_DINAMICA_CONFIG , conf => conf.Ignore());

            var tabdinamicapublicacaor = tabdinamicapublicacao.ReverseMap();
            tabdinamicapublicacaor.ForMember(obj => obj.TAB_DINAMICA_CONFIG , conf => conf.Ignore());


            // Tabela Dinamica Grupo
            var tabdinamicaGrupo = store.CreateMap<TAB_DINAMICA_GRUPO, TabDinamicaGrupoDTO>();
            //tabdinamicaGrupo.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            var tabdinamicaGrupor = tabdinamicaGrupo.ReverseMap();
            tabdinamicaGrupor.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            // Publicacao Revisão Colaborador
            var mapeamentoCaderno = store.CreateMap<CADERNO, CadernoDTO>();
            var mapeamentoCadernoR = mapeamentoCaderno.ReverseMap();
            mapeamentoCadernoR.ForMember(obj => obj.CLIENTES_REF, conf => conf.Ignore());
            mapeamentoCadernoR.ForMember(obj => obj.CADERNO_COMPARTILHADO, conf => conf.Ignore());
            mapeamentoCadernoR.ForMember(obj => obj.CADERNO_CONTEUDO, conf => conf.Ignore());
            mapeamentoCadernoR.ForMember(obj => obj.CADERNO_NOTA, conf => conf.Ignore());

            // Publicacao Revisão Colaborador
            var mapeamentoCadernoCompartilhado = store.CreateMap<CADERNO_COMPARTILHADO, CadernoCompartilhadoDTO>();
            var mapeamentoCadernoCompartilhadoR = mapeamentoCadernoCompartilhado.ReverseMap();
            mapeamentoCadernoCompartilhadoR.ForMember(obj => obj.CLIENTES_REF, conf => conf.Ignore());
            mapeamentoCadernoCompartilhadoR.ForMember(obj => obj.CADERNO, conf => conf.Ignore());

            // Publicacao Revisão Colaborador
            var mapeamentoClientesRef = store.CreateMap<CLIENTES_REF, ClientesRefDTO>();
            var mapeamentoClientesRefR = mapeamentoClientesRef.ReverseMap();
            mapeamentoClientesRefR.ForMember(obj => obj.CADERNO, conf => conf.Ignore());
            mapeamentoClientesRefR.ForMember(obj => obj.CADERNO_COMPARTILHADO, conf => conf.Ignore());

            // Publicacao Revisão Colaborador
            var mapeamentoCadernoConteudo = store.CreateMap<CADERNO_CONTEUDO, CadernoConteudoDTO>();
            var mapeamentoCadernoConteudoR = mapeamentoCadernoConteudo.ReverseMap();
            mapeamentoCadernoConteudoR.ForMember(obj => obj.CADERNO, conf => conf.Ignore());

            // Caderno Nota
            var mapeamentoCadernoNota = store.CreateMap<CADERNO_NOTA, CadernoNotaDTO>();
            var mapeamentoCadernoNotaR = mapeamentoCadernoNota.ReverseMap();
            mapeamentoCadernoNotaR.ForMember(obj => obj.CADERNO, conf => conf.Ignore());

            //----------------------
            // Origem Acesso
            var origemAcessoRef = store.CreateMap<ORIGEM_ACESSO_REF, OrigemAcessoRefDTO>();
            origemAcessoRef.ForMember(obj => obj.ORIGEM_FUNCIONALIDADE, conf => conf.Ignore());
            origemAcessoRef.ForMember(obj => obj.TAB_DINAMICA_ORIGEM, conf => conf.Ignore());
            var origemAcessoRefR = origemAcessoRef.ReverseMap();
            origemAcessoRefR.ForMember(obj => obj.ORIGEM_FUNCIONALIDADE, conf => conf.Ignore());
            origemAcessoRefR.ForMember(obj => obj.TAB_DINAMICA_ORIGEM, conf => conf.Ignore());

            // Tabela Dinamica Origem
            var tabDinamicaOrigem = store.CreateMap<TAB_DINAMICA_ORIGEM, TabDinamicaOrigemDTO>();
            tabDinamicaOrigem.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            tabDinamicaOrigem.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            var tabDinamicaOrigemR = tabDinamicaOrigem.ReverseMap();
            tabDinamicaOrigemR.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            tabDinamicaOrigemR.ForMember(obj => obj.TAB_DINAMICA_CONFIG, conf => conf.Ignore());

            // Origem Funcionalidade
            var origemFuncionalidade = store.CreateMap<ORIGEM_FUNCIONALIDADE, OrigemFuncionalidadeDTO>();
           // origemFuncionalidade.ForMember(obj => obj.FUNCIONALIDADE, conf => conf.Ignore());
            origemFuncionalidade.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            var origemFuncionalidadeR = origemFuncionalidade.ReverseMap();
            origemFuncionalidadeR.ForMember(obj => obj.FUNCIONALIDADE, conf => conf.Ignore());
            origemFuncionalidadeR.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());


            // Funcionalidade
            var funcionalidade = store.CreateMap<FUNCIONALIDADE, FuncionalidadeDTO>();
            funcionalidade.ForMember(obj => obj.ORIGEM_FUNCIONALIDADE, conf => conf.Ignore());
            funcionalidade.ForMember(obj => obj.FUNCIONALIDADE_NIVEL_ACESSO, conf => conf.Ignore());
            funcionalidade.ForMember(obj => obj.PRODUTO_REF, conf => conf.Ignore());
            var funcionalidadeR = funcionalidade.ReverseMap();
            funcionalidadeR.ForMember(obj => obj.ORIGEM_FUNCIONALIDADE, conf => conf.Ignore());
            funcionalidadeR.ForMember(obj => obj.FUNCIONALIDADE_NIVEL_ACESSO, conf => conf.Ignore());
            funcionalidadeR.ForMember(obj => obj.PRODUTO_REF, conf => conf.Ignore());


            // Funcionalidade Nivel Acesso
            var funcionalidadeNivelAcesso = store.CreateMap<FUNCIONALIDADE_NIVEL_ACESSO, FuncionalidadeNivelAcessoDTO>();
            funcionalidadeNivelAcesso.ForMember(obj => obj.FUNCIONALIDADE, conf => conf.Ignore());
            var funcionalidadeNivelAcessoR = funcionalidadeNivelAcesso.ReverseMap();
            funcionalidadeNivelAcessoR.ForMember(obj => obj.FUNCIONALIDADE, conf => conf.Ignore());


            // Produto Composição REF
            var produtoComposicaoRef = store.CreateMap<PRODUTO_REF, ProdutoRefDTO>();
            produtoComposicaoRef.ForMember(obj => obj.FUNCIONALIDADE, conf => conf.Ignore());
            var produtoComposicaoRefR = produtoComposicaoRef.ReverseMap();

            // Notícia
            var noticia = store.CreateMap<NOTICIA, NoticiaDTO>();
            noticia.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            //noticia.ForMember(obj => obj.TITULACAO, conf => conf.Ignore());
            //noticia.ForMember(obj => obj.NOTICIA_GRUPO, conf => conf.Ignore());
            var noticiaR = noticia.ReverseMap();
            noticiaR.ForMember(obj => obj.PUBLICACAO , conf => conf.Ignore());
            noticiaR.ForMember(obj => obj.TITULACAO, conf => conf.Ignore());
            noticiaR.ForMember(obj => obj.NOTICIA_GRUPO, conf => conf.Ignore());

            
            // Notícia Grupo
            var noticiagrupo = store.CreateMap<NOTICIA_GRUPO, NoticiaGrupoDTO>();
            noticiagrupo.ForMember(obj => obj.NOTICIA , conf => conf.Ignore());
            var noticiagrupoR = noticiagrupo.ReverseMap();
            noticiagrupoR.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());

            // Origem Acesso Menu
            var origemacessomenu = store.CreateMap<ORIGEM_ACESSO_MENU, OrigemAcessoMenuDTO>();
            origemacessomenu.ForMember(obj => obj.ORIGEM_ACESSO_REF , conf => conf.Ignore());
            origemacessomenu.ForMember(obj => obj.ORIGEM_ACESSO_SUBMENU , conf => conf.Ignore());
            var origemacessomenuR = origemacessomenu.ReverseMap();
            origemacessomenuR.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            origemacessomenuR.ForMember(obj => obj.ORIGEM_ACESSO_SUBMENU, conf => conf.Ignore());

            // Origem Acesso Submenu
            var origemacessosubmenu = store.CreateMap<ORIGEM_ACESSO_SUBMENU, OrigemAcessoSubMenuDTO>();
            origemacessosubmenu.ForMember(obj => obj.ORIGEM_ACESSO_MENU, conf => conf.Ignore());
            var origemacessosubmenuR = origemacessosubmenu.ReverseMap();
            origemacessosubmenuR.ForMember(obj => obj.ORIGEM_ACESSO_MENU, conf => conf.Ignore());

            // Noticias salvas
            var noticiasSalvas = store.CreateMap<NOTICIAS_GUARDADAS, NoticiasGuardadasDTO>();
            var noticiasSalvasR = origemacessosubmenu.ReverseMap();

            var manualdp = store.CreateMap<MANUAL_DP,ManualDPDTO>();
            manualdp.ForMember(obj => obj.MENU_DOC, conf => conf.Ignore());
            manualdp.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());
            manualdp.ForMember(obj => obj.MANUAL_DP_MODULO, conf => conf.Ignore());
         
            var manualdpr = manualdp.ReverseMap();
            manualdpr.ForMember(obj => obj.MENU_DOC, conf => conf.Ignore());
            manualdpr.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());
            manualdpr.ForMember(obj => obj.MANUAL_DP_MODULO, conf => conf.Ignore());
        
            var manualdpitem = store.CreateMap<MANUAL_DP_ITEM,ManualDPItemDTO>();
            manualdpitem.ForMember(obj => obj.ORGAO, conf => conf.Ignore());
            manualdpitem.ForMember(obj => obj.TIPO_ATO, conf => conf.Ignore());
            manualdpitem.ForMember(obj => obj.MANUAL_DP_ITEM1, conf => conf.Ignore());
            manualdpitem.ForMember(obj => obj.MANUAL_DP_ITEM2, conf => conf.Ignore());
          //  manualdpitem.ForMember(obj => obj.MANUAL_DP_LINK, conf => conf.Ignore());
           // manualdpitem.ForMember(obj => obj.FUNDAMENTACAO , conf => conf.Ignore());

            var manualdpitemr = manualdpitem.ReverseMap();
            manualdpitemr.ForMember(obj => obj.ORGAO, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.TIPO_ATO, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.MANUAL_DP, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.MANUAL_DP_ITEM1, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.MANUAL_DP_ITEM2, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.MANUAL_DP_LINK, conf => conf.Ignore());
            manualdpitemr.ForMember(obj => obj.FUNDAMENTACAO, conf => conf.Ignore());
            


            var menudoc = store.CreateMap<MENU_DOC,MenuDocDTO>();
            menudoc.ForMember(obj => obj.MANUAL_DP , conf => conf.Ignore());
            menudoc.ForMember(obj => obj.MENU_DOC_ITEM , conf => conf.Ignore());
            var menudocr = menudoc.ReverseMap();
            menudocr.ForMember(obj => obj.MANUAL_DP, conf => conf.Ignore());
            menudocr.ForMember(obj => obj.MENU_DOC_ITEM, conf => conf.Ignore());

            var mendocitem = store.CreateMap<MENU_DOC_ITEM, MenuDocItemDTO>();
            mendocitem.ForMember(obj => obj.MENU_DOC , conf => conf.Ignore());
            mendocitem.ForMember(obj => obj.MENU_DOC_ITEM1 , conf => conf.Ignore());
            mendocitem.ForMember(obj => obj.MENU_DOC_ITEM2 , conf => conf.Ignore());

            var mendocitemr = mendocitem.ReverseMap();
            mendocitemr.ForMember(obj => obj.MENU_DOC, conf => conf.Ignore());
            mendocitemr.ForMember(obj => obj.MENU_DOC_ITEM1, conf => conf.Ignore());
            mendocitemr.ForMember(obj => obj.MENU_DOC_ITEM2, conf => conf.Ignore());


            var manualdpModulo = store.CreateMap<MANUAL_DP_MODULO, ManualDPModuloDTO>();
           // manualdpModulo.ForMember(obj => obj.MANUAL_DP , conf => conf.Ignore());
            var manualdpModulor = manualdpModulo.ReverseMap();
            manualdpModulor.ForMember(obj => obj.MANUAL_DP, conf => conf.Ignore());


            var fundamentacao = store.CreateMap<FUNDAMENTACAO, FundamentacaoDTO>();
            fundamentacao.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());
            var fundamentacaor = fundamentacao.ReverseMap();
            fundamentacaor.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());


            var manualdplink = store.CreateMap<MANUAL_DP_LINK, ManualDPLinkDTO>();
            manualdplink.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());
            var manualdplinkr = manualdplink.ReverseMap();
            manualdplinkr.ForMember(obj => obj.MANUAL_DP_ITEM, conf => conf.Ignore());

            var logacessoportal = store.CreateMap<LOG_ACESSO_PORTAL, LogAcessoPortalDTO>();
            logacessoportal.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());
            logacessoportal.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            logacessoportal.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());
            var logacessoportalr = logacessoportal.ReverseMap();
            logacessoportalr.ForMember(obj => obj.NOTICIA, conf => conf.Ignore());
            logacessoportalr.ForMember(obj => obj.ORIGEM_ACESSO_REF, conf => conf.Ignore());
            logacessoportalr.ForMember(obj => obj.PUBLICACAO, conf => conf.Ignore());

            #region Consultoria por email do portais
            var consultaemailverbete = store.CreateMap<CONSULTA_EMAIL_VERBETE, ConsultaEmailVerbeteDTO>();
            consultaemailverbete.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailverbete.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());
            var consultaemailverbeter = consultaemailverbete.ReverseMap();
            consultaemailverbeter.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailverbeter.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());

            var consultaemailstatus = store.CreateMap<CONSULTA_EMAIL_STATUS, ConsultaEmailStatusDTO>();
            consultaemailstatus.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            var consultaemailstatusr = consultaemailstatus.ReverseMap();
            consultaemailstatusr.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());

            var consultaemailperfilcolec = store.CreateMap<CONSULTA_EMAIL_PERFIL_COLEC, ConsultaEmailPerfilColecDTO>();
            consultaemailperfilcolec.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());
            var consultaemailperfilcolecr = consultaemailperfilcolec.ReverseMap();
            consultaemailperfilcolecr.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());

            var consultaemailgrandegrupo = store.CreateMap<CONSULTA_EMAIL_GRANDE_GRUPO, ConsultaEmailGrandeGrupoDTO>();
            consultaemailgrandegrupo.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailgrandegrupo.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());
            var consultaemailgrandegrupor = consultaemailgrandegrupo.ReverseMap();
            consultaemailgrandegrupor.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailgrandegrupor.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());

            var consultaemailcolecionador = store.CreateMap<CONSULTA_EMAIL_COLECIONADOR, ConsultaEmailColecionadorDTO>();
            consultaemailcolecionador.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailcolecionador.ForMember(obj => obj.CONSULTA_EMAIL_GRANDE_GRUPO, conf => conf.Ignore());
            consultaemailcolecionador.ForMember(obj => obj.CONSULTA_EMAIL_PERFIL_COLEC, conf => conf.Ignore());
            consultaemailcolecionador.ForMember(obj => obj.CONSULTA_EMAIL_VERBETE, conf => conf.Ignore());
            var consultaemailcolecionadorr = consultaemailcolecionador.ReverseMap();
            consultaemailcolecionadorr.ForMember(obj => obj.CONSULTA_EMAIL, conf => conf.Ignore());
            consultaemailcolecionadorr.ForMember(obj => obj.CONSULTA_EMAIL_GRANDE_GRUPO, conf => conf.Ignore());
            consultaemailcolecionadorr.ForMember(obj => obj.CONSULTA_EMAIL_PERFIL_COLEC, conf => conf.Ignore());
            consultaemailcolecionadorr.ForMember(obj => obj.CONSULTA_EMAIL_VERBETE, conf => conf.Ignore());

            var consultaemail = store.CreateMap<CONSULTA_EMAIL, ConsultaEmailDTO>();
            consultaemail.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());
            consultaemail.ForMember(obj => obj.CONSULTA_EMAIL_VERBETE, conf => conf.Ignore());
            consultaemail.ForMember(obj => obj.CONSULTA_EMAIL_GRANDE_GRUPO, conf => conf.Ignore());
            consultaemail.ForMember(obj => obj.CONSULTA_EMAIL_STATUS, conf => conf.Ignore());
            var consultaemailr = consultaemail.ReverseMap();
            consultaemailr.ForMember(obj => obj.CONSULTA_EMAIL_COLECIONADOR, conf => conf.Ignore());
            consultaemailr.ForMember(obj => obj.CONSULTA_EMAIL_VERBETE, conf => conf.Ignore());
            consultaemailr.ForMember(obj => obj.CONSULTA_EMAIL_GRANDE_GRUPO, conf => conf.Ignore());
            consultaemailr.ForMember(obj => obj.CONSULTA_EMAIL_STATUS, conf => conf.Ignore());
            #endregion
        }
    }
}
