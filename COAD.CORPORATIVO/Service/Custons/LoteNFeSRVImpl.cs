using COAD.CORPORATIVO.Model.Dto;
using COAD.FISCAL.Service.Integracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using GenericCrud.Service;
using COAD.FISCAL.Model.Enumerados;
using GenericCrud.Util;
using COAD.SEGURANCA.Model;
using COAD.CORPORATIVO.Exceptions;
using COAD.SEGURANCA.Service;
using COAD.FISCAL.Model.Integracoes;
using System.IO;
using COAD.FISCAL.Model.Servicos.Retornos;
using System.Transactions;
using COAD.FISCAL.Model.NFSe;
using COAD.SEGURANCA.Model.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Impostos;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class LoteNFeSRVImpl : IntegrLoteNFeSRV<NotaFiscalLoteDTO, NotaFiscalLoteItemDTO>
    {
        public NotaFiscalLoteSRV NotaFiscalLoteSRV { get; set; }
        public PedidoCRMSRV PedidoSRV { get; set; }
        public ImpostoSRV ImpostoSRV { get; set; }
        public LoteNFeSRVImpl()
        {

            this.IntegrLoteItemNFeSRV = new LoteItemNFeSRVImpl();
            this.PedidoSRV = new PedidoCRMSRV();
            
        }


        public LoteNFeSRVImpl(LoteItemNFeSRVImpl loteItemNFeSRV)
        {
            this.IntegrLoteItemNFeSRV = loteItemNFeSRV;
            this.PedidoSRV = new PedidoCRMSRV();
        }

        public override INFeLote RetornarLote(int? LoteID)
        {
            var lote = NotaFiscalLoteSRV.FindById(LoteID);
            return lote;
        }

        public override INFeLote RetornarLotePorCodRecibo(string CodRecibo)
        {
            if (!string.IsNullOrWhiteSpace(CodRecibo))
            {
                var lote = NotaFiscalLoteSRV.RetornarLotePorRecibo(CodRecibo);
                return lote;
            }
            return null;
        }

        public FISCAL.Model.DTO.EmpresaDTO GerarDadosDoEmitente(EmpresaModel empresa)
        {
            string codigoIBGE = empresa.IBGE_COD_COMPLETO;
            string municipio = null;
            string UF = null;
            int codUf = 0;
            int? seqEmpresa = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarSequencialNFEEmpresa(empresa.EMP_ID);

            if (string.IsNullOrWhiteSpace(empresa.EMP_IE))
                throw new FaturamentoException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui inscrição estadual cadastrada.", empresa.EMP_NOME_FANTASIA));

            int? codMunicipio = null;

            if (!string.IsNullOrWhiteSpace(codigoIBGE))
            {
                codMunicipio = int.Parse(codigoIBGE);
                var municipioDTO = ServiceFactory.RetornarServico<MunicipioSRV>().BuscarPorIBGE(codigoIBGE);

                if (municipioDTO != null)
                {
                    municipio = StringUtil.LimparAcentuacao(municipioDTO.MUN_DESCRICAO);
                    UF = municipioDTO.UF;
                }
            }

            var uf = ServiceFactory.RetornarServico<UFSRV>().FindById(UF);
            if (uf.UF_COD != null)
            {
                int.TryParse(uf.UF_COD, out codUf);
            }

            var empresaNFe = new FISCAL.Model.DTO.EmpresaDTO()
            {
                CNPJ = empresa.EMP_CNPJ,
                IE = empresa.EMP_IE,
                IM = empresa.EMP_IM,
                NomeFantasia = empresa.EMP_NOME_FANTASIA,
                RazaoSocial = empresa.EMP_RAZAO_SOCIAL,
                SequencialNFe = seqEmpresa,
                Endereco = new EnderecoDTO()
                {
                    Bairro = empresa.EMP_BAIRRO,
                    CEP = empresa.EMP_CEP,
                    CodMunicipio = codMunicipio,
                    Complemento = empresa.EMP_COMPLEMENTO,
                    Logradouro = empresa.EMP_LOGRADOURO,
                    Municipio = municipio,
                    Numero = empresa.EMP_NUMERO,
                    Pais = "Brasil",
                    Telefone = empresa.EMP_TEL1,
                    UF = new FISCAL.Model.DTO.UFDTO()
                    {
                        CodigoUF = (codUf > 0) ? new int?(codUf) : null,
                        Nome = UF
                    }
                }
            };

            return empresaNFe;
        }

        private ClienteDTO GerarDadosDoDestinatario(ClienteDto cliente, string email = null)
        {
            string telefone = null;
            string cpf = null;
            string cnpj = null;
            int codUf = 0;
            int? MUN_ID = null;
            int? COD_IBGE = null;
            string nomeMunicipio = null;
            var codigoPaisDestino = 1058;

            var telefoneObj = ServiceFactory.RetornarServico<AssinaturaTelefoneSRV>().RetornarTelefoneContato(cliente.CLI_ID);
            var endereco = ServiceFactory.RetornarServico<ClienteEnderecoSRV>().BuscarEnderecoDeFaturamentoOuEnderecoPadrao(cliente);

            if(endereco == null)
            {
                throw new Exception("Não é possível encontrar o endereço do cliente");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                var emailObj = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(cliente.CLI_ID);
                if (emailObj != null)
                {
                    email = emailObj.AEM_EMAIL;
                }
            }

            if (telefoneObj != null)
            {
                telefone = telefoneObj.ATE_DDD + telefoneObj.ATE_TELEFONE;
            }

            string inscricaoEstadual = cliente.CLI_INSCRICAO;

            if (!string.IsNullOrWhiteSpace(inscricaoEstadual))
            {
                decimal inscricao = 0;
                if (!decimal.TryParse(inscricaoEstadual, out inscricao))
                {
                    inscricaoEstadual = null;
                }
            }

            TipoClienteEnum tipoCliente = TipoClienteEnum.Fisica;
            if (cliente.TIPO_CLI_ID != null)
            {

                if (cliente.TIPO_CLI_ID == 2)
                {
                    cpf = StringUtil.PreencherZeroEsquerda(cliente.CLI_CPF_CNPJ, 11);
                    cpf = StringUtil.Truncate(cpf, 11);
                    tipoCliente = TipoClienteEnum.Fisica;
                }
                else
                {
                    tipoCliente = (cliente.TIPO_CLI_ID == 3) ? TipoClienteEnum.Juridica : TipoClienteEnum.OrgaoPublico;
                    cnpj = StringUtil.PreencherZeroEsquerda(cliente.CLI_CPF_CNPJ, 14);
                    cnpj = StringUtil.Truncate(cnpj, 14);
                }

            }

            MUN_ID = endereco.MUN_ID;
            var objMunicipio = ServiceFactory.RetornarServico<MunicipioSRV>().FindById(MUN_ID);

            if (objMunicipio != null)
            {
                if (!string.IsNullOrWhiteSpace(objMunicipio.IBGE_COD_COMPLETO))
                    COD_IBGE = int.Parse(objMunicipio.IBGE_COD_COMPLETO);
                nomeMunicipio = StringUtil.LimparAcentuacao(objMunicipio.MUN_DESCRICAO);
            }

            var uf = ServiceFactory.RetornarServico<UFSRV>().FindById(endereco.END_UF);
            if(uf.UF_COD != null)
            {
                int.TryParse(uf.UF_COD, out codUf);
            }

            var clienteNFe = new ClienteDTO()
            {
                CodCliente = cliente.CLI_ID,
                CPF = cpf,
                CNPJ = cnpj,
                Email = email,
                IE = inscricaoEstadual,
                Nome = cliente.CLI_NOME,
                TipoCliente = tipoCliente,
                Endereco = new EnderecoDTO()
                {
                    Bairro = endereco.END_BAIRRO,
                    CEP = endereco.END_CEP,
                    CodMunicipio = COD_IBGE,
                    Logradouro = endereco.END_LOGRADOURO,
                    Numero = endereco.END_NUMERO,
                    Municipio = nomeMunicipio,
                    Pais = "Brasil",
                    Telefone = telefone,
                    Complemento = endereco.END_COMPLEMENTO,
                    UF = new FISCAL.Model.DTO.UFDTO()
                    {
                        Nome = uf.UF,
                        CodigoUF = (codUf > 0) ? new int?(codUf) : null
                    }
                }
            };

            return clienteNFe;
        }

        public PedidoItemDTO GerarItensDaNota(ItemPedidoDTO itemPedido, ContratoDTO contrato = null, string endereco = null, int? nfcId = null)
        {
            var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(itemPedido.CMP_ID);
            var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);
            var lstParcela = ServiceFactory.RetornarServico<ParcelasSRV>().ListarParcelaPorPedido(itemPedido.IPE_ID);
            var configNotaFiscal = ServiceFactory.RetornarServico<NotaFiscalConfigSRV>().FindById(nfcId);

            if (produto == null)
                throw new Exception("Não é possível gerar os itens da nota. O produto não foi encontrado");

            if (string.IsNullOrWhiteSpace(produto.PRO_NOME))
                throw new Exception("Não é possível gerar os itens da nota. O produto não possui nenhum nome.");

            if (itemPedido.PRODUTO_COMPOSICAO == null)
            {
                throw new Exception("Não é possível gerar os itens da nota. O produto composto não foi carregado.");
            }

            if (configNotaFiscal == null)
            {
                throw new Exception("Não é possível gerar os itens da nota. A configuração de Nota Fiscal não foi encontrada.");
            }

            ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaEMarcaProdutoCurso(itemPedido.PRODUTO_COMPOSICAO);
            
            var produtoDesc = (itemPedido.PRODUTO_COMPOSICAO.EhCurso) ? itemPedido.PRODUTO_COMPOSICAO.CMP_DESCRICAO : produto.PRO_NOME;
            string nomeDoProduto = (!string.IsNullOrWhiteSpace(configNotaFiscal.NFC_DESCRICAO_PRODUTO)) ? 
                configNotaFiscal.NFC_DESCRICAO_PRODUTO :
                produtoDesc.ToUpper();

            string codProduto = null;
            decimal quantidadeDecimal = 1.00m;
            int quantidade = (int)itemPedido.IPE_QTD;
            decimal valorUnitario = 0.00m;

            decimal valorTotal = (contrato != null) ? 
                (decimal) contrato.CTR_VLR_BRUTO : 
                (decimal) (itemPedido.IPE_PRECO_UNITARIO * itemPedido.IPE_QTD);

            if(configNotaFiscal.NFC_PORCENTAGEM_VALOR != null &&
                configNotaFiscal.NFC_PORCENTAGEM_VALOR > 0)
            {
                var porcentagem = (decimal) configNotaFiscal.NFC_PORCENTAGEM_VALOR;

                if(itemPedido.PEDIDO_CRM.FaturadoCemPorCento == true && configNotaFiscal.NFC_APLICAR_100_POR_CENTO_FAT == true)
                {
                    porcentagem = 100;
                }

                valorTotal = ((porcentagem / 100) * valorTotal);
                valorTotal = Math.Round(valorTotal, 2);
            }
               
            valorUnitario = (decimal)valorTotal / quantidade;
            quantidadeDecimal = quantidadeDecimal * quantidade;

            valorUnitario = Math.Round(valorUnitario, 8);

            if (produto.PRO_ID != null)
            {
                codProduto = produto.PRO_ID.ToString();
            }

            var pedidoItem = new PedidoItemDTO()
            {
                Produto = new ProdutoDTO()
                {
                    CodProduto = codProduto,
                    NCM = "49019900",
                    Nome = nomeDoProduto,
                    UnidadeComercial = "UNID",
                    UnidadeTributavel = "UNID",
                },
                QtdComercial = quantidadeDecimal,
                QtdTributavel = quantidadeDecimal,
                ValorTotal = valorTotal,
                ValorUnitario = valorUnitario
            };

            GerarDadosDaParcela(pedidoItem, lstParcela, nfcId);

            if (configNotaFiscal.NCT_ID == 2)
            {
                LocalizacaoCursoDTO localizacao = null;

                if (itemPedido.LOC_ID != null)
                {

                    if (itemPedido.LOCALIZACAO_CURSO == null)
                        localizacao = ServiceFactory.RetornarServico<LocalizacaoCursoSRV>().FindById(itemPedido.LOC_ID);
                    else
                        localizacao = itemPedido.LOCALIZACAO_CURSO;
                }

                if(localizacao != null)
                {
                    nomeDoProduto = $"{nomeDoProduto} - {localizacao.LOC_DESCRICAO}";
                }

                var valorServico = valorTotal;
                var valorLiquidoServico = valorTotal = (contrato != null) ? (decimal) contrato.CTR_VLR_CONTRATO : 
                (decimal) (itemPedido.IPE_TOTAL);


                pedidoItem.Servico = new Servico()
                {
                    Aliquota = (5m / 100),
                    BaseCalculo = valorServico,
                    CodListaServico = configNotaFiscal.NFC_COD_LISTA_SERVICO,
                    ValorServicos = valorServico,
                    DescricaoServico = $"{nomeDoProduto} / Consumidor da ALERJ Tel:0800 2827060, conforme Lei 5817/10 ",
                    ValorLiquidoNfse = valorLiquidoServico,
                    CodigoTributacaoMunicipio = configNotaFiscal.NFC_CODIGO_TRIBUTACAO_MUNICIPIO,
                };

                InserirImpostosDaEmpresa(pedidoItem, itemPedido, nfcId);
                InserirImpostosNoServico(pedidoItem);
                BuscarBuscarCodTributacaoMunicipioConfig(pedidoItem, itemPedido, nfcId);
            }
            return pedidoItem;
        }
        
        public void BuscarBuscarCodTributacaoMunicipioConfig(PedidoItemDTO pedidoItem, ItemPedidoDTO itemPedido, int? nfcId)
        {
            var lstConfigImpostos = RetornarConfiguracao(pedidoItem, itemPedido, nfcId);
            if (lstConfigImpostos != null)
            {
                if (lstConfigImpostos != null && lstConfigImpostos.Count > 0)
                {
                    foreach (var conf in lstConfigImpostos)
                    {
                        if (!string.IsNullOrWhiteSpace(conf.CFI_CODIGO_TRIBUTACAO_MUNICIPIO))
                        {
                            pedidoItem.Servico.CodigoTributacaoMunicipio = conf.CFI_CODIGO_TRIBUTACAO_MUNICIPIO;
                            break;
                        }
                    }
                }
            }
        }

        public void InserirImpostosNoServico(PedidoItemDTO pedidoItem)
        {
            if(pedidoItem != null && 
                pedidoItem.Servico != null &&
                pedidoItem.Parcelas != null)
            {
                decimal? cofins = 0.00m;
                decimal? csll = 0.00m;
                decimal? inss = 0.00m;
                decimal? ir = 0.00m;
                decimal? pis = 0.00m;
                decimal? outros = 0.00m;
                decimal? iss = (pedidoItem.Servico.ValorServicos * (pedidoItem.Servico.Aliquota));
                iss = Math.Round((decimal)iss);

                foreach (var par in pedidoItem.Parcelas)
                {
                    if(par.Impostos != null)
                    {
                        foreach(var im in par.Impostos)
                        {
                            if(im.TipoImposto == ImpostosEnum.COFINS)
                            {
                                cofins += im.ValorDesconto;
                            }

                            if (im.TipoImposto == ImpostosEnum.CSLL)
                            {
                                csll += im.ValorDesconto;
                            }

                            if (im.TipoImposto == ImpostosEnum.INSS)
                            {
                                inss += im.ValorDesconto;
                            }

                            if (im.TipoImposto == ImpostosEnum.IR)
                            {
                                ir += im.ValorDesconto;
                            }

                            if (im.TipoImposto == ImpostosEnum.PIS)
                            {
                                pis += im.ValorDesconto;
                            }

                            if (im.TipoImposto == ImpostosEnum.OUTROS)
                            {
                                outros += im.ValorDesconto;
                            }
                        }
                    }
                }

                pedidoItem.Servico.ValorCofins = cofins;
                pedidoItem.Servico.ValorCsll = csll;
                pedidoItem.Servico.ValorInss = inss;
                pedidoItem.Servico.ValorIr = ir;
                pedidoItem.Servico.ValorPis = pis;
                pedidoItem.Servico.OutrasRetencoes = outros;
                pedidoItem.Servico.ValorIss = iss;
            }
        }

        public ICollection<ConfigImpostoDTO> RetornarConfiguracao(PedidoItemDTO pedidoItem, ItemPedidoDTO itemPedido, int? nfcId)
        {
            ICollection<ConfigImpostoDTO> lstConfigImpostos = null;

            ServiceFactory.RetornarServico<PedidoCRMSRV>().ChecarEPreencherPedido(itemPedido);

            if (itemPedido.PEDIDO_CRM != null &&
               itemPedido.PEDIDO_CRM.CLIENTES != null)
            {
                var tipoCliente = itemPedido.PEDIDO_CRM.CLIENTES.TIPO_CLI_ID;

                lstConfigImpostos = ImpostoSRV.ObterImpostos(new RequisicaoConfigImpostoDTO()
                {

                    ClienteRetem = false,
                    CmpID = itemPedido.CmpId,
                    NfcId = nfcId,
                    SobreTotal = true,
                    TipoCliId = tipoCliente,
                    EmpresaDoSimples = itemPedido.PEDIDO_CRM.PED_EMPRESA_DO_SIMPLES
                });
            }

            return lstConfigImpostos;
        }


        /// <summary>
        /// Obtém todos os impostos que são de responsabilidade da empresa emitente pagar
        /// </summary>
        /// <param name="servico"></param>
        public void InserirImpostosDaEmpresa(PedidoItemDTO pedidoItem, ItemPedidoDTO itemPedido, int? nfcId)
        {
            var lstConfigImpostos = RetornarConfiguracao(pedidoItem, itemPedido, nfcId);
            if (lstConfigImpostos != null)
            {
                if(lstConfigImpostos != null && lstConfigImpostos.Count > 0)
                {
                    foreach(var conf in lstConfigImpostos)
                    {
                        var iss = conf
                            .CONFIG_IMPOSTO_IMPOSTO
                            .Where(x => x.IMPOSTO.TipoImposto == ImpostosEnum.ISS)
                            .FirstOrDefault();

                        if (iss != null && 
                            iss.CII_ALIQUOTA != null &&
                            iss.CII_ALIQUOTA > 0)
                        {
                            pedidoItem.Servico.Aliquota = (iss.CII_ALIQUOTA / 100);
                        }
                    }
                }
            }
        }


        public void GerarDadosDaParcela(PedidoItemDTO pedidoItem, ICollection<ParcelasDTO> lstParcelas, int? nfcID)
        {
            if(lstParcelas != null)
            {
                var lstParcelasParcela = new List<ParcelaDTO>();
                var _infoFaturaItemSRV = ServiceFactory.RetornarServico<InfoFaturaItemSRV>();
                var _tipoPagamentoSRV = ServiceFactory.RetornarServico<TipoPagamentoSRV>();
                foreach (var par in lstParcelas)
                {
                    if (par.IFF_ID != null)
                    {
                        var infoFaturaItm = _infoFaturaItemSRV.ListarInfoFaturaItemPorConfig(par.IFF_ID, nfcID);

                        if (infoFaturaItm == null)
                            throw new Exception($"Não é possível gerar as informações de imposto de parcela. Não existe informações de retenção para {par.IFF_ID} config de nota fical {nfcID}. Isso é um bug. Verifique se o pedido foi faturado 100%. Se sim informe o Adm do sistema.");
                        var tipoPagamentoObj = _tipoPagamentoSRV.FindById(par.TPG_ID);
                        TipoPagamentoEnum tipoPagamento = (par != null && tipoPagamentoObj != null) ? tipoPagamentoObj.TipoPagamento : TipoPagamentoEnum.OUTROS;
                        var parPedido = new ParcelaDTO()
                        {
                            ValorLiquido = par.PAR_VLR_PARCELA,
                            ValorParcela = infoFaturaItm.IFI_VALOR_BRUTO,
                            BaseCalculo = infoFaturaItm.IFI_VALOR_BRUTO,
                            NumeroParcela = par.PAR_SEQ_PARCELA,
                            TipoPagamentoParcela = tipoPagamento,
                            ValorLiquidoServico = infoFaturaItm.IFI_TOTAL_LIQUIDO
                        };

                        parPedido.NaoRetevePorRegra = infoFaturaItm.IFF_N_RETEVE_POR_REGRA;

                        if (infoFaturaItm.IMPOSTO_INFO_FATURA_ITEM != null && infoFaturaItm.IMPOSTO_INFO_FATURA_ITEM.Count > 0)
                        {
                            foreach (var im in infoFaturaItm.IMPOSTO_INFO_FATURA_ITEM)
                            {
                                parPedido.Impostos.Add(new ImpostosDTO()
                                {   Aliguota = im.IMPOSTO.IMP_ALIQUOTA,
                                    TipoImposto = im.IMPOSTO.TipoImposto,
                                    ValorDesconto = im.IFI_VALOR_DESCONTADO
                                });
                            }
                        }
                        pedidoItem.Parcelas.Add(parPedido);
                    }
                }
            }
        }

        public PedidoItemDTO GerarItensDaNota(PropostaItemDTO propostaItem)
        {
            var proId = ServiceFactory.RetornarServico<ProdutoComposicaoItemSRV>().ObterProIdParaGerarAssinatura(propostaItem.CMP_ID);
            var produto = ServiceFactory.RetornarServico<ProdutosSRV>().FindById(proId);

            if (produto == null)
                throw new Exception("Não é possível gerar os itens da nota. O produto não foi encontrado");

            if (string.IsNullOrWhiteSpace(produto.PRO_NOME))
                throw new Exception("Não é possível gerar os itens da nota. O produto não possui nenhum nome.");


            string nomeDoProduto = "LIVRO TÉC " + produto.PRO_NOME.ToUpper();
            string codProduto = null;
            decimal quantidadeDecimal = 1.00m;
            int quantidade = (int)propostaItem.PPI_QTD;
            decimal valorUnitario = (decimal)propostaItem.PPI_VALOR_UNITARIO;

            decimal valorTotal = (decimal)propostaItem.PPI_TOTAL;

            //if (quantidade != null)
            //{
            //    quantidadeDecimal = ((int)quantidade);
            //    quantidadeDecimal = decimal.Round(quantidadeDecimal, 2, MidpointRounding.AwayFromZero);

            //}

            if (produto.PRO_ID != null)
            {
                codProduto = produto.PRO_ID.ToString();
            }

            var pedidoItem = new PedidoItemDTO()
            {
                Produto = new ProdutoDTO()
                {
                    CodProduto = codProduto,
                    NCM = "49019900",
                    Nome = nomeDoProduto,
                    UnidadeComercial = "UNID",
                    UnidadeTributavel = "UNID",
                },
                QtdComercial = quantidadeDecimal,
                QtdTributavel = quantidadeDecimal,
                ValorTotal = valorUnitario,
                ValorUnitario = valorTotal
            };

            return pedidoItem;
        }

        public override PedidoDTO RetornarPedido(int? CodPedido, string CodContrato = null, int? NfConfigID = null)
        {
            ContratoDTO contrato = null;

            if (!string.IsNullOrWhiteSpace(CodContrato))
            {
                contrato = ServiceFactory.RetornarServico<ContratoSRV>().FindById(CodContrato);
            }

            var itemPedido = ServiceFactory.RetornarServico<ItemPedidoSRV>().FindById(CodPedido);

            if (itemPedido == null)
                throw new Exception("O Item de Pedido. não foi encontrado.");

            var pedido = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindByIdFullLoaded(itemPedido.PED_CRM_ID, true, true);

            if (pedido == null)
                throw new Exception("O pedido não foi encontrado.");

            if (contrato == null)
                contrato = ServiceFactory.RetornarServico<PedidoCRMSRV>()._contratoSRV.ListarContratosDoItemPedido(itemPedido.IPE_ID).FirstOrDefault();

            var empId = (contrato != null && contrato.EMP_ID != null) ? contrato.EMP_ID : pedido.EMP_ID;
            var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(empId);

            if (empresa == null)
                throw new Exception("A empresa não foi encontrada.");

            var cliente = pedido.CLIENTES;

            if (cliente == null)
                throw new Exception("O cliente não foi encontrada.");

            var email = (!string.IsNullOrWhiteSpace(pedido.PED_CRM_EMAIL_NOTA_FISCAL)) ? pedido.PED_CRM_EMAIL_NOTA_FISCAL : pedido.PED_CRM_EMAIL_CONTATO;

            var empresaNFe = GerarDadosDoEmitente(empresa);
            var clienteNFe = GerarDadosDoDestinatario(cliente, email);

            var endereco = $"{empresaNFe.Endereco.Municipio} - {empresaNFe.Endereco.UF.Nome}";
            var pedidoItem = GerarItensDaNota(itemPedido, contrato, endereco, NfConfigID);


            var tipoPagamento = ServiceFactory.RetornarServico<ItemPedidoSRV>().RetornarTipoPagamentoEntrada(CodPedido);

            var pedidoNfe = new PedidoDTO()
            {
                Cliente = clienteNFe,
                CodPedido = CodPedido,
                DataFaturamento = DateTime.Now,
                Empresa = empresaNFe,
                TipoPagamento = tipoPagamento,
                Items = new List<PedidoItemDTO>()
                {
                    pedidoItem
                },
                ObservacoesNotaFiscal = pedido.PED_OBSERVACOES_NOTA_FISCAL
            };

            if (pedidoItem.Servico != null)
            {
                pedidoNfe.Empresa.SequencialNFSe = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarSequencialNFSEEmpresa(empresa.EMP_ID);
            }

            return pedidoNfe;
        }
        
        public override PedidoDTO RetornarProposta(int? CodProposta)
        {
            var propostaItem = ServiceFactory.RetornarServico<PropostaItemSRV>().FindById(CodProposta);

            if (propostaItem == null)
                throw new Exception("O Item de Proposta. não foi encontrado.");

            var proposta = ServiceFactory.RetornarServico<PropostaSRV>().FindById(propostaItem.PRT_ID);

            if (proposta == null)
                throw new Exception("A proposta não foi encontrado.");

            var empresa = ServiceFactory.RetornarServico<EmpresaSRV>().FindById(proposta.EMP_ID);

            if (empresa == null)
                throw new Exception("A empresa não foi encontrada.");

            var cliente = proposta.CLIENTES;

            if (cliente == null)
                throw new Exception("O cliente não foi encontrada.");

            var email = (!string.IsNullOrWhiteSpace(proposta.PRT_EMAIL_NOTA_FISCAL)) ? proposta.PRT_EMAIL_NOTA_FISCAL : proposta.PRT_EMAIL_CONTATO;
            var empresaNFe = GerarDadosDoEmitente(empresa);
            var clienteNFe = GerarDadosDoDestinatario(cliente, email);
            var pedidoItem = GerarItensDaNota(propostaItem);


            var tipoPagamento = ServiceFactory.RetornarServico<PropostaItemSRV>().RetornarTipoPagamentoEntrada(CodProposta);

            var pedidoNfe = new PedidoDTO()
            {
                Cliente = clienteNFe,
                CodPedido = CodProposta,
                DataFaturamento = DateTime.Now,
                Empresa = empresaNFe,
                TipoPagamento = tipoPagamento,
                Items = new List<PedidoItemDTO>()
                {
                    pedidoItem
                },
                ObservacoesNotaFiscal = proposta.PRT_OBSERVACOES_NOTA_FISCAL
            };

            if(pedidoItem.Servico != null)
            {
                pedidoNfe.Empresa.SequencialNFSe = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarSequencialNFSEEmpresa(empresa.EMP_ID);
            }

            return pedidoNfe;
        }    

        public override INFeLote SalvarOuAtualizarLote(INFeLote Lote)
        {
           return NotaFiscalLoteSRV.SaveOrUpdate((NotaFiscalLoteDTO) Lote);
        }

        public override INFeLote RetornarLoteVigente(int? EmpresaID)
        {
            return NotaFiscalLoteSRV.RetornarLoteVigente(EmpresaID);
        }       

        public override INFeLote RetornarProximoLoteParaEnviar()
        {
            return NotaFiscalLoteSRV.RetornarLoteParaEnviar();
        }

        public override INFeLote RetornarProximoLoteParaProcessar()
        {
            return NotaFiscalLoteSRV.RetornarLoteParaProcessar();
        }

        public override ICollection<INFeLote> ListarLotesPendentesDeEnvio()
        {
            var list = NotaFiscalLoteSRV.RetornarLotesParaEnviar();

            if (list != null)
                return list.Cast<INFeLote>().ToList();
            return new List<INFeLote>();
        }

        public override ICollection<INFeLote> ListarLotesNFsePendentesDeEnvio()
        {
            var list = NotaFiscalLoteSRV.RetornarLoteNFseParaEnviar();

            if (list != null)
                return list.Cast<INFeLote>().ToList();
            return new List<INFeLote>();
        }

        public override ICollection<INFeLote> ListarLotesPendentesDeProcessamento()
        {
            var list = NotaFiscalLoteSRV.RetornarLotesParaProcessar();

            if (list != null)
                return list.Cast<INFeLote>().ToList();
            return new List<INFeLote>();
        }

        public override ICollection<INFeLote> ListarLotesNFsePendentesDeProcessamento()
        {
            var list = NotaFiscalLoteSRV.RetornarLotesNFseParaProcessar();

            if (list != null)
                return list.Cast<INFeLote>().ToList();
            return new List<INFeLote>();
        }

        public override void NotaFiscalAutorizadaCallBack(NFeAutorizadaContext context)
        {
            if(context != null && context.LoteItem != null)
            {
                var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();
                var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();
                if (context.LoteItem.CodProposta != null || context.LoteItem.CodPedido != null || context.LoteItem.ClienteID != null || context.LoteItem.FornecedorID != null)
                {
                    var cliID = context.LoteItem.ClienteID;
                    var empID = context.LoteItem.EmpresaID;
                    var forID = context.LoteItem.FornecedorID;
                    string email = null;

                    using (var scope = new TransactionScope())
                    {
                        if (context.LoteItem.CodPedido != null)
                        {
                            var itemPedido = itemPedidoSRV.FindById(context.LoteItem.CodPedido);

                            if (cliID == null)
                            {
                                cliID = itemPedido.PEDIDO_CRM.CLI_ID;
                            }

                            if (itemPedido != null)
                            {
                                itemPedidoSRV.AlterarStatusPedidoNotaFiscalAutorizada(itemPedido, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno, context.LoteItem.CodContrato);
                            }
                        }
                        else if (context.LoteItem.CodProposta != null)
                        {
                            var proposaItem = propostaItemSRV.FindById(context.LoteItem.CodProposta);

                            if (cliID == null)
                            {
                                cliID = proposaItem.PROPOSTA.CLI_ID;
                            }

                            if (proposaItem != null)
                            {
                                propostaItemSRV.RegistrarHistPedidoNotaFiscalAutorizada(proposaItem, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno);
                            }
                        }

                        var notaFiscalSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
                        var notaFiscal = notaFiscalSRV.CadastrarNotaFiscal(context.proNFe, context.LoteItem, context.filePath, context.bytesNFe);
                        context.LoteItem.CodNotaFiscal = notaFiscal.NF_ID;

                        var ntRefs = context.LoteItem.NotaFiscalReferenciados;

                        if (ntRefs != null)
                        {
                            foreach (var ntRef in ntRefs)
                            {
                                ntRef.CodNotaFiscal = notaFiscal.NF_ID;
                            }

                            context.LoteItem.NotaFiscalReferenciados = ntRefs;
                        }

                        if(context.LoteItem.NotaAntecipada == true)
                        {
                            ServiceFactory.RetornarServico<RepresentanteSRV>().NotificarDeNotaAntecipada(context.LoteItem.CodProposta);
                        }

                        if (!string.IsNullOrWhiteSpace(notaFiscal.NF_EMAIL))
                        {
                            notaFiscalSRV.EnviarEmailNotaFiscal(notaFiscal.NF_EMAIL, notaFiscal.NF_ID, 1, path: context.filePath);
                            notaFiscal.NST_ID = 2;
                            notaFiscalSRV.SaveOrUpdate(notaFiscal);
                        }
                        scope.Complete();
                    }

                }
            }
        }

        public override void NotaFiscalRejeitadaCallBack(NFeRejeitadaContext context)
        {
            var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();
            var itemPedido = itemPedidoSRV.FindById(context.LoteItem.CodPedido);

            if(itemPedido != null)
            {
                itemPedidoSRV.AlterarStatusPedidoNotaFiscalRejeitada(itemPedido, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno);
            }
        }        

        public override void NFseAutorizadaCallBack(NFseAutorizadaContext context)
        {
            if (context != null && context.LoteItem != null)
            {
                var propostaItemSRV = ServiceFactory.RetornarServico<PropostaItemSRV>();
                var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();
                if (context.LoteItem.CodProposta != null || context.LoteItem.CodPedido != null || context.LoteItem.ClienteID != null || context.LoteItem.FornecedorID != null)
                {
                    var cliID = context.LoteItem.ClienteID;
                    var empID = context.LoteItem.EmpresaID;
                    var forID = context.LoteItem.FornecedorID;
                    string email = null;

                    using (var scope = new TransactionScope())
                    {
                        if (context.LoteItem.CodPedido != null)
                        {
                            var itemPedido = itemPedidoSRV.FindById(context.LoteItem.CodPedido);

                            if (cliID == null)
                            {
                                cliID = itemPedido.PEDIDO_CRM.CLI_ID;
                            }

                            if (itemPedido != null)
                            {
                                itemPedidoSRV.AlterarStatusPedidoNotaFiscalAutorizada(itemPedido, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno, context.LoteItem.CodContrato, true);
                            }
                        }
                        else if (context.LoteItem.CodProposta != null)
                        {
                            var proposaItem = propostaItemSRV.FindById(context.LoteItem.CodProposta);

                            if (cliID == null)
                            {
                                cliID = proposaItem.PROPOSTA.CLI_ID;
                            }

                            if (proposaItem != null)
                            {
                                propostaItemSRV.RegistrarHistPedidoNotaFiscalAutorizada(proposaItem, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno);
                            }
                        }

                        var notaFiscalSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
                        var notaFiscal = notaFiscalSRV.CadastrarNotaFiscalServico(context.compNfse, context.LoteItem, context.filePath, context.bytesNFe);
                        context.LoteItem.CodNotaFiscal = notaFiscal.NF_ID;

                        var ntRefs = context.LoteItem.NotaFiscalReferenciados;

                        if (ntRefs != null)
                        {
                            foreach (var ntRef in ntRefs)
                            {
                                ntRef.CodNotaFiscal = notaFiscal.NF_ID;
                            }

                            context.LoteItem.NotaFiscalReferenciados = ntRefs;
                        }

                        if (!string.IsNullOrWhiteSpace(notaFiscal.NF_EMAIL))
                        {
                            notaFiscalSRV.EnviarEmailNotaFiscalDeServico(notaFiscal.NF_EMAIL, notaFiscal.NF_ID, 1, path: context.filePath);
                            notaFiscal.NST_ID = 2;
                            notaFiscalSRV.SaveOrUpdate(notaFiscal);
                        }
                        scope.Complete();
                    }

                }
            }
        }

        public override void NFseRejeitadaCallBack(NFeRejeitadaContext context)
        {

            var itemPedidoSRV = ServiceFactory.RetornarServico<ItemPedidoSRV>();
            var itemPedido = itemPedidoSRV.FindById(context.LoteItem.CodPedido);

            if (itemPedido != null)
            {
                itemPedidoSRV.AlterarStatusPedidoNotaFiscalRejeitada(itemPedido, context.LoteItem.NumeroNota, context.LoteItem.CodRetorno, context.LoteItem.MensagemRetorno);
            }
        }

        public override IEmpresa RetornarEmpresa(int? EmpresaID)
        {
            var empresa = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarEmpresa(EmpresaID);

            return empresa;
        }
        
        public override INotaFiscal RetornarNotaFiscal(int? notaFiscalID)
        {
            var notaFiscalSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
            var notaFiscal = notaFiscalSRV.FindById(notaFiscalID);

            return notaFiscal;
        }

        public override ICollection<INotaFiscal> SalvarNotas(ICollection<INotaFiscal> lstNotaFiscal)
        {
            if(lstNotaFiscal != null && lstNotaFiscal.Count > 0)
            {
                var lstNotaSalvar = lstNotaFiscal.Cast<NotaFiscalDTO>(); 
                var retorno = ServiceFactory.RetornarServico<NotaFiscalSRV>().SaveOrUpdateAll(lstNotaSalvar);
                if(retorno != null)
                {
                    return retorno
                        .Cast<INotaFiscal>()
                        .ToList();
                }
            }
            return new List<INotaFiscal>();
        }


        public override void EnviarEmailCancelamentoDeServico(ICollection<INotaFiscal> lstNotas)
        {
            if(lstNotas != null)
            {
                foreach(var nota in lstNotas)
                {
                    EnviarEmailNotaServicoCancelada(new ProcessamentoEventoItem()
                    {
                        Arquivo = nota.Arquivo,
                        NomeArquivo = nota.NomeArquivo,
                        NotaFiscal = nota
                    });
                }
            }
        }

        public override void EnviarEmailEventoNota(ProcessamentoEventoItem procEvent)
        {
            if(procEvent != null)
            {
                var notaFiscal = ServiceFactory.RetornarServico<NotaFiscalSRV>().FindById(procEvent.NotaFiscal.CodNotaFiscal);
                if(notaFiscal != null)
                {
                    var nomeArquivo = procEvent.NomeArquivo;
                    var arquivo = procEvent.Arquivo;

                    var email = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(notaFiscal.CLI_ID);
                    if(email != null && arquivo != null && arquivo.Count() > 0)
                    {
                        var path = SysUtils.DefaultPath + SysUtils.RetornarPathNFeXML() + @"\notas-eventos";
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        var fullPath = string.Format(@"{0}\{1}", path, nomeArquivo);
                        File.WriteAllBytes(fullPath, arquivo);

                        ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarEmailEvento(email.AEM_EMAIL, notaFiscal.NF_ID, procEvent.LoteItem.Tipo, 1, fullPath);
                    }
                }
            }
        }


        public override void EnviarEmailNotaServicoCancelada(ProcessamentoEventoItem procEvent)
        {
            if (procEvent != null)
            {
                var notaFiscal = ServiceFactory.RetornarServico<NotaFiscalSRV>().FindById(procEvent.NotaFiscal.CodNotaFiscal);
                if (notaFiscal != null)
                {
                    var nomeArquivo = procEvent.NomeArquivo;
                    var arquivo = procEvent.Arquivo;

                    var fileInfo = new FileInfoDTO(nomeArquivo);

                    var email = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().RetornarEmailDeContato(notaFiscal.CLI_ID);
                    if (email != null && arquivo != null && arquivo.Count() > 0)
                    {
                        var path = SysUtils.DefaultPath + SysUtils.RetornarPathNFeXML() + @"\notas-eventos";
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        var fullPath = string.Format(@"{0}\{1}", path, fileInfo.FileName);
                        File.WriteAllBytes(fullPath, arquivo);

                        if (File.Exists(fullPath))
                            ServiceFactory.RetornarServico<NotaFiscalSRV>().EnviarEmailCancNotaServico(email.AEM_EMAIL, notaFiscal.NF_ID, 1, fullPath);
                    }
                }
            }
        }

        public override int? RetornarSequencialEmpresa(int? EmpresaID)
        {
            var sequencial = ServiceFactory.RetornarServico<CustomEmpresaSRV>().RetornarSequencialNFEEmpresa(EmpresaID);
            return sequencial;
        }

        public override ICollection<INotaFiscalReferenciada> ListarNotasReferenciadas(int? loteItm)
        {
            var lstNotasRef = ServiceFactory.RetornarServico<NotaFiscalReferenciadaSRV>().ListarNotasReferenciadasPorLoteItem(loteItm);
            if (lstNotasRef != null)
                return lstNotasRef.Cast<INotaFiscalReferenciada>().ToList();
            return new List<INotaFiscalReferenciada>();
        }

        public override void NotaFiscalDevolvidaCallBack(NFeDevolucaoContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.ChaveNota))
            {
                var notaSRV = ServiceFactory.RetornarServico<NotaFiscalSRV>();
                var notaFiscal = notaSRV.BuscarPorChave(context.ChaveNota);

                if(notaFiscal != null)
                {
                    notaFiscal.AdicionarStatus(4);
                    notaSRV.SaveOrUpdate(notaFiscal);
                }
            }
        }

        public override ClienteDTO RetornarClientePorNota(int? codNota)
        {
            var notaFiscal = ServiceFactory.RetornarServico<NotaFiscalSRV>().FindById(codNota);

            if (notaFiscal != null && notaFiscal.CLI_ID != null)
            {
                var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(notaFiscal.CLI_ID);
                var clienteNFe = GerarDadosDoDestinatario(cliente);
                return clienteNFe;
            }
            return null;
        }

        public override INotaFiscalEventoDTO CriarInstanciaNotaFiscalEvento()
        {
            return new NotaFiscalEventoDTO();
        }

        public override INotaFiscalEventoDTO SalvarEvento(INotaFiscalEventoDTO notaFiscalEvento)
        {
            var servNotaEv = ServiceFactory.RetornarServico<NotaFiscalEventoSRV>();

            if(notaFiscalEvento is NotaFiscalEventoDTO)
            {
                var obj = servNotaEv.SaveOrUpdate((NotaFiscalEventoDTO) notaFiscalEvento);
                return obj as NotaFiscalEventoDTO;
            }
            return null;
        }

        public override ICollection<INotaFiscalItemMSG> ConverterNotaFiscalItemMSG(ListaMensagemRetornoNfse listaMensagemRetornoNfse)
        {

            if(listaMensagemRetornoNfse != null &&
                listaMensagemRetornoNfse.MensagemRetorno != null)
            {
                var lst = listaMensagemRetornoNfse.MensagemRetorno.Select(x => new NotaFiscalLoteItemMsgDTO() {

                    NLM_COD = x.Codigo,
                    NLM_MSG = x.Mensagem,
                    NLM_CORRECAO = x.Correcao
                }).Cast<INotaFiscalItemMSG>()
                .ToList();
                return lst;
            }
            return new List<INotaFiscalItemMSG>();
        }

        public override IList<ParcelaDTO> RetornarParcelasPorPedido(int? IdPedido, int? IdItemPedido)
        {

            var _infoFaturaItemSRV = ServiceFactory.RetornarServico<InfoFaturaItemSRV>();

            ItemPedidoDTO itemPedido = new ItemPedidoDTO();

            if ( IdPedido != null )
                itemPedido = PedidoSRV._itemPedidoSRV.ListarItemPedidoDoPedido(IdPedido).FirstOrDefault();

            if ((IdPedido == null) && (IdItemPedido != null))
                itemPedido = PedidoSRV._itemPedidoSRV.dao.ToDTO(PedidoSRV._itemPedidoSRV.BuscarPorId(IdItemPedido));

            var parcelas = ServiceFactory.RetornarServico<ParcelasSRV>().ListarParcelaPorPedido(itemPedido.IPE_ID);

            IList<ParcelaDTO> parcelasRetorno = new List<ParcelaDTO>();

            foreach (ParcelasDTO parcela in parcelas)
            {

                ParcelaDTO parcelaRetorno = new ParcelaDTO();

                var infoFaturaItm = _infoFaturaItemSRV.ListarInfoFaturaPorItem(parcela.IFF_ID);

                parcelaRetorno.Parcela = parcela.PAR_NUM_PARCELA;
                parcelaRetorno.Vencimento = parcela.PAR_DATA_VENCTO;
                parcelaRetorno.ValorParcela = parcela.PAR_VLR_PARCELA;
                parcelaRetorno.Contrato = parcela.CTR_NUM_CONTRATO;
                parcelaRetorno.ValorLiquidoServico = infoFaturaItm.IFI_TOTAL_LIQUIDO;

                parcelasRetorno.Add(parcelaRetorno);

            }

            return parcelasRetorno;

        }

        public override IList<ParcelaDTO> RetornarParcelasPorProposta(int? CodProposta)
        {

            var parcelas = ServiceFactory.RetornarServico<ParcelasSRV>().ListarParcelaPorProposta(CodProposta);

            IList<ParcelaDTO> parcelasRetorno = new List<ParcelaDTO>();

            foreach (ParcelasDTO parcela in parcelas)
                parcelasRetorno.Add(new ParcelaDTO()
                {

                    Parcela = parcela.PAR_NUM_PARCELA,
                    Vencimento = (DateTime)parcela.PAR_DATA_VENCTO,
                    ValorParcela = parcela.PAR_VLR_PARCELA,
                    Contrato = parcela.CTR_NUM_CONTRATO

                });

            return parcelasRetorno;

        }

    }
}
