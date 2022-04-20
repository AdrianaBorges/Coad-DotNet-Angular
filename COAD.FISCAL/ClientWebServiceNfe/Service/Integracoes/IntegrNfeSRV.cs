using COAD.FISCAL.Exceptions;
using COAD.FISCAL.Model;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Enumerados;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Enumerados;
using Coad.GenericCrud.Exceptions;
using COAD.FISCAL.XmlUtils;
using GenericCrud.Util;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Service;
using COAD.FISCAL.Model.Servicos.Retornos;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using System.Transactions;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using System.Xml;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using GenericCrud.Exceptions;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.FISCAL.Model.DTO.Enumerados;
using COAD.FISCAL.Model.DTO.Requests;
using System.Text.RegularExpressions;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Model.NFSe.Enumerados;
using COAD.FISCAL.Model.NFSe.Retornos;

namespace COAD.FISCAL.Service.Integracoes
{
    public class IntegrNfeSRV
    {
        public ILoteNFeSRV LoteSRV { get; set; }

        public string defaultPath { get; set; }

        public IntegrNfeSRV()
        {
            defaultPath = SysUtils.DefaultPath;
        }

        public void TestarCriacaoDeNfe(int cfop)
        {
            Random rand = new Random();
            DateTime dataAtual = DateTime.Now;

            NotaFiscal nfe = new NotaFiscal();

            InfoNfeDTO infoNotaFiscal = new InfoNfeDTO()
            {
                Versao = "3.10"
            };

            nfe.lstInfNFe.Add(infoNotaFiscal);

            var numeroAleatorio = rand.Next(100, 99999999);
            var codigoNf = StringUtil.PreencherZeroEsquerda(numeroAleatorio, 8);
            var codigoPaisDestino = 1058;
            var codigoUFEmitente = 33;
            var codigoUFDestino = 33;

            TipoLocalDestinoOperacaoEnum tipoLocalOperacao;

            if (codigoPaisDestino != 1058)
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoComExterior;
            }
            else
            if (codigoUFEmitente == codigoUFDestino)
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterna;
            }
            else
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterestadual;
            }

            NfeIdentificacaoDTO nfeIdent = new NfeIdentificacaoDTO()
            {
                CodigoNumerico = codigoNf,
                CodigoDoMunicipio = 3304557,
                IndicadorLocalDestino = tipoLocalOperacao,
                CodigoUFEmitente = codigoUFEmitente,
                DataDeEmissao = dataAtual,
                DataEntradaSaida = dataAtual,
                finNFe = FinalidadeNotaFiscalEnum.NfeNormal,
                //IndicacaoPagamento = IndPagEnum.PagamentoAPrazo,
                IndicacaoPrensenca = IndicacaoPresencaEnum.OPERACAO_NAO_PRESENCIAL_OUTROS,
                NaturezaOperacao = "Venda",
                NumeroNotaFiscal = 160,
                Serie = 55,
                TipoAmbiente = TipoAmbienteEnum.Homologacao,
                TipoEmissao = TipoEmissaoEnum.Normal,
                FormatoImpressao = FormatoImpressaoEnum.Retrato,
                TipoOperacao = TipoNotaFiscalEnum.Saida,
                VersaoProcesso = "5.0.17.9395"
            };

            infoNotaFiscal.Identificacao = nfeIdent;

            NfeEmitenteDTO emitente = new NfeEmitenteDTO()
            {
                CNPJ = "27922913000111",
                InscricaoEstadual = "86122774",
                xNome = "ATUALIZACAO PROFISSIONAL CONTINUADA LTDA",
                NomeFantasia = "ATUALIZACAO",
                EnderecoEmitente = new NfeEnderecoEmitenteDTO()
                {
                    CEP = "22640020",
                    CodigoMunicipio = 3304557,
                    Telefone = "2121565900",
                    Numero = "455",
                    UF = "RJ",
                    Bairro = "PECHINCHA",
                    Complemento = "LOJA A SALA 101 PARTE",
                    Logradouro = "ESTRADA DO TINDIBA",
                    Municipio = "Rio de Janeiro"
                }
            };

            infoNotaFiscal.Emitente = emitente;

            NfeDestinoDTO destino = new NfeDestinoDTO()
            {
                CPF = "13718478781",
                Email = SysUtils.RetornaEmailDeTeste(),
                xNome = "Diego Andrade da Silva",
                Endereco = new NfeEnderecoDestinatarioDTO()
                {
                    CEP = "25575414",
                    CodigoPais = codigoPaisDestino,
                    CodigoMunicipio = 3304557,
                    Telefone = "21979807161",
                    Numero = "029",
                    UF = "RJ",
                    Bairro = "São João de Meriti",
                    Complemento = "Próximo ao transformador de luz",
                    Logradouro = "Av. Portugal",
                    Municipio = "Rio de Janeiro",
                    Pais = "Brasil",
                },
                TipoIEDestinatario = TipoIEDestinatarioEnum.ContribuinteIsentoDoICMS
            };

            infoNotaFiscal.Destino = destino;


            NfeItemDTO item = new NfeItemDTO()
            {
                CodigoProduto = "841",
                NomeProduto = "LIVRO TÉC",
                CFOP = cfop,
                NCM = "49019900",
                Quantidade = 1.00m,
                Unidade = "UNID",
                ValorUnitario = 56.00m,
                QuantidadeTributavel = 1.00m,
                UnidadeTributavel = "UNID",
                ValorUnitarioTributacao = 56.00m,
                ValorTotal = 56.00m,
                IndicacaoTotal = 1,
            };

            NFeImpostoICMSGrupo40 icms40 = new NFeImpostoICMSGrupo40();
            icms40.CST = 41;

            NFeImposto imposto = new NFeImposto();
            imposto.ICMS = new NFeImpostoICMS()
            {
                ICMS40 = icms40
            };

            imposto.PIS = new NFeImpostoPIS()
            {
                PISGrupoNaoTributado = new NFeImpostoPISGrupoNaoTributadoDTO()
            };

            imposto.COFINS = new NFeImpostoCOFINS()
            {
                COFINSGrupoNaoTributado = new NFeImpostoCOFINSGrupoNaoTributadoDTO()
            };

            NFeDetalhamentoItem det = new NFeDetalhamentoItem()
            {
                NumeroItem = 1,
                Produto = item,
                Imposto = imposto
            };

            infoNotaFiscal.Detalhamento.Add(det);

            NFeTotalDTO total = new NFeTotalDTO()
            {
                ICMSTotal = new NFeICMSTotalDTO()
                {
                    BaseCalculoICMS = 0.00m,
                    ValorICMS = 0.00m,
                    ICMSDesoneracao = 0.00m,
                    BaseCalculoST = 0.00m,
                    ValorST = 0.00m,
                    TotalProduto = 56.00m,
                    ValorFrete = 0.00m,
                    TotalSeguro = 0.00m,
                    TotalDesconto = 0.00m,
                    ValorII = 0.00m,
                    ValorIPI = 0.00m,
                    ValorPIS = 0.00m,
                    ValorCOFINS = 0.00m,
                    ValorOutrasDespesas = 0.00m,
                    TotalNotaFiscal = 56.00m,
                }
            };

            infoNotaFiscal.Total = total;

            GerarCodigoDaNFe(nfe);
            //SerializarNotaFiscal(nfe);


            string fileName = string.Format(@"\nfe_teste.xml", DateTime.Now);
            string downloadPath = Path.Combine(defaultPath, Path.GetFileName(fileName));
            string certPath = @"C:\Users\dasilva\Documents\certificados\13623007_ATUALIZACAO_PROFISSIONAL_CONTINUADA_LTDA27922913000111matriz.p12";

            XmlUtil.SerializeAsXmlWithSignature(nfe, downloadPath, certPath,
                "279229",
                "infNFe",
                "..",
                true);

        }

        public ResultadoGeracaoNFeDTO SerializarNotaFiscal(NotaFiscal nfe,
            TipoQualificacaoNFeEnum tipoQualificacao = TipoQualificacaoNFeEnum.PRODUTO, string identificacao = null, int? empID = null)
        {
            var codigoDaNota = "nota-fisca";
            int? numeroDaNota = null;


            DateTime? dataFaturamento = DateTime.Now;
            if (nfe.lstInfNFe != null && nfe.lstInfNFe.Count > 0)
            {
                codigoDaNota = nfe.lstInfNFe[0].Id;
                codigoDaNota = codigoDaNota.Replace("NFe", "");

                if (nfe.lstInfNFe[0].Identificacao == null)
                {
                    throw new ArgumentException("Não é possível localizar a identificação da nota fiscal.");
                }

                if (nfe.lstInfNFe[0].Identificacao.NumeroNotaFiscal == null)
                {
                    throw new ArgumentException("Não é possível encontrar o número da nota fiscal, ele não foi gerado.");
                }

                numeroDaNota = nfe.lstInfNFe[0].Identificacao.NumeroNotaFiscal;
                dataFaturamento = nfe.lstInfNFe[0].Identificacao.DataDeEmissao;
            }

            string fileName = string.Format(@"\nfe_numero_({0})_chave_({1})_{2:yyyy-MM-ddTH-mm}.xml", numeroDaNota, codigoDaNota, dataFaturamento);
            string downloadPath = Path.Combine(defaultPath, "nfe", Path.GetFileName(fileName));
            var certificado = CertificateUtil.RetornarCertificado(empID);
            var xmlDoc = XmlUtil.AssinarXml(nfe, certificado, "infNFe", "..");

            XmlUtil.SerializeXml(xmlDoc, downloadPath);
            //XmlUtil.SerializeAsXmlWithSignature(
            //    nfe,
            //    downloadPath,
            //    certPath,
            //    "279229",
            //    "infNFe",
            //    "..",
            //    false);

            var retorno = new ResultadoGeracaoNFeDTO()
            {
                NumeroDaNotaFiscal = numeroDaNota,
                ChaveNotaFiscal = codigoDaNota,
                Path = downloadPath,
                TipoQualificacao = tipoQualificacao,
                FileName = fileName,
            };

            return retorno;

        }

        public void GerarCodigoDaNFe(NotaFiscal nfe)
        {
            if (nfe != null && nfe.lstInfNFe != null)
            {
                var notas = nfe.lstInfNFe;

                foreach (var nf in notas)
                {
                    GerarCodigoDaNFe(nf);
                }
            }
        }

        public void GerarCodigoDaNFe(InfoNfeDTO nf)
        {
            if (nf != null && nf.Identificacao != null && nf.Emitente != null)
            {
                var idenficacao = nf.Identificacao;
                var emitente = nf.Emitente;

                var codigoUf = idenficacao.CodigoUFEmitente;

                var dataEmissao = idenficacao.DataDeEmissao.ToString("yyMM");
                //var cnpj = StringUtil.PreencherZeroEsquerda(emitente.CNPJ, 14);
                var cnpj = emitente.CNPJ;
                var modelo = idenficacao.ModeloDocumentoFiscal;
                var serie = StringUtil.PreencherZeroEsquerda((int)idenficacao.Serie, 3);
                var numero = StringUtil.PreencherZeroEsquerda((int)idenficacao.NumeroNotaFiscal, 9);
                var tipoEmissao = (int)idenficacao.TipoEmissao;
                var codigoNumerico = StringUtil.PreencherZeroEsquerda(idenficacao.CodigoNumerico, 8);

                StringBuilder sb = new StringBuilder();

                sb.Append(codigoUf);
                sb.Append(dataEmissao);
                sb.Append(cnpj);
                sb.Append(modelo);
                sb.Append(serie);
                sb.Append(numero);
                sb.Append(tipoEmissao);
                sb.Append(codigoNumerico);

                var codigo43Digitos = sb.ToString();
                var dv = CalcularDigitoVerificador(codigo43Digitos);
                var codigo44Digitos = codigo43Digitos + dv;

                nf.Id = "NFe" + codigo44Digitos;
                idenficacao.DigitoVerificador = dv;
            }
        }

        public int? CalcularDigitoVerificador(string codigo43Digitos)
        {
            if (string.IsNullOrWhiteSpace(codigo43Digitos))
            {
                throw new ArgumentException("O código de 43 dígitos não pode ser nullo ou vazio.");
            }

            if (codigo43Digitos.Length != 43)
            {
                var msg = "O código passado deve conter 43 dígitos.  Tamanho do código passado ({0}).";
                var tamanho = codigo43Digitos.Length;
                msg = string.Format(msg, tamanho);

                throw new ArgumentException(msg);
            }

            char[] arrayChar = codigo43Digitos.ToCharArray();
            int length = arrayChar.Length;

            int multiplicador = 2;
            int somatorio = 0;

            for (var index = (length - 1); index >= 0; index--)
            {
                int valorInteiro = int.Parse(arrayChar[index].ToString());
                int valorMultiplicado = valorInteiro * multiplicador;

                somatorio += valorMultiplicado;

                multiplicador++;
                if (multiplicador > 9)
                {
                    multiplicador = 2;
                }
            }

            var modulo = somatorio % 11;

            if (modulo == 0 || modulo == 1)
            {
                return 0;
            }

            var digitoVerificador = 11 - modulo;

            if (digitoVerificador > 9)
            {
                throw new Exception("Ocorreu um erro ao gerar o dígito verificador, o resultado obtido possui mais de 1 dígito");
            }

            return digitoVerificador;

        }

        public NotaFiscal GerarNfeDTO(PedidoDTO pedido, IList<ParcelaDTO> parcelas = null)
        {
            if (pedido == null)
            {
                throw new ArgumentNullException("O Pedido não pode ser nulo");
            }

            PedidoItemDTO itemPed = pedido.Items.FirstOrDefault();

            if (itemPed.Parcelas.Count() == 0)
                itemPed.Parcelas = parcelas;

            if (pedido.Empresa == null)
            {
                throw new ArgumentNullException("A Empresa não pode ser nulo");
            }

            if (pedido.DataFaturamento == null)
            {
                string mensagem = "Não é possível gerar o xml da nota fiscal. Não é possível encontrar a data de faturamento no Item de Pedido {0}";
                mensagem = string.Format(mensagem, pedido.CodPedido);
                throw new Exception(mensagem);
            }

            var validacao = ValidatorProxy.RecursiveValidate(pedido);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre um problema ao gerar a nota fiscal.", validacao);
            }

            int cfop = 0;
            int? sequencialEmpresa = pedido.Empresa.SequencialNFe;
            int codigoMunicipioIBGE = 0;

            Random rand = new Random();
            DateTime dataEntradaSaida = DateTime.Now;
            TipoAmbienteEnum tipoAmbiente;
            TipoNotaFiscalEnum tipoNota = TipoNotaFiscalEnum.Saida;
            TipoLocalDestinoOperacaoEnum tipoLocalOperacao;
            FinalidadeNotaFiscalEnum finalidadeNFe = FinalidadeNotaFiscalEnum.NfeNormal;
            List<NFeReferenciadaDTO> lstReferencias = new List<NFeReferenciadaDTO>();
            
            var numeroAleatorio = rand.Next(100, 99999999);
            var codigoNf = StringUtil.PreencherZeroEsquerda(numeroAleatorio, 8);
            var codigoPaisDestino = 1058;
            var codigoUFEmitente = ( pedido.Empresa.Endereco.UF.Nome == "RJ" ? 33 : 31);
            var codigoUFDestino = 33;
            var codigoDoPedido = pedido.CodPedido;

            if (pedido.TipoOperacaoNota == TipoOperacaoEnum.DEVOLUCAO)
            {
                tipoNota = TipoNotaFiscalEnum.Entrada;
                finalidadeNFe = FinalidadeNotaFiscalEnum.DevolucaoMercadoria;
            }

            if (pedido.Empresa.Endereco.CodMunicipio != null)
            {
                codigoMunicipioIBGE = pedido.Empresa.Endereco.CodMunicipio.Value;
            }

            NotaFiscal nfe = new NotaFiscal();

            InfoNfeDTO infoNotaFiscal = new InfoNfeDTO()
            {
                Versao = ConstantsNFe.VERSAO
            };

            nfe.lstInfNFe.Add(infoNotaFiscal);

            tipoAmbiente = (SysUtils.InHomologation()) ? TipoAmbienteEnum.Homologacao : TipoAmbienteEnum.Producao;

            var emitente = GerarDadosDoEmitente(pedido.Empresa);
            var destino = GerarDadosDoDestinatario(pedido.Cliente);

            if (emitente != null)
            {
                if (pedido.Empresa.Endereco != null &&
                    pedido.Empresa.Endereco.UF != null &&
                    pedido.Empresa.Endereco.UF.CodigoUF == null)
                    throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui UF cadastrada.", pedido.Empresa.NomeFantasia));
                codigoUFEmitente = pedido.Empresa.Endereco.UF.CodigoUF.Value;
            }

            if (destino != null)
            {
                if (pedido.Cliente.Endereco != null &&
                    pedido.Cliente.Endereco.UF != null &&
                    pedido.Cliente.Endereco.UF.CodigoUF == null)
                    throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal. O endereço do cliente {0} não possui UF cadastrada.", pedido.Cliente.Nome));
                codigoUFDestino = pedido.Cliente.Endereco.UF.CodigoUF.Value;
            }

            infoNotaFiscal.Emitente = emitente;
            infoNotaFiscal.Destino = destino;


            if (codigoPaisDestino != 1058)
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoComExterior;
            }
            else
                if (codigoUFEmitente == codigoUFDestino)
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterna;
                cfop = (pedido.TipoOperacaoNota == TipoOperacaoEnum.DEVOLUCAO) ? 1202 : 5101;
            }
            else
            {
                tipoLocalOperacao = TipoLocalDestinoOperacaoEnum.OperacaoInterestadual;
                cfop = (pedido.TipoOperacaoNota == TipoOperacaoEnum.DEVOLUCAO) ? 2202 : 6101;
            }

            if (pedido.ChaveNotasReferenciadas != null)
            {

                foreach (var chave in pedido.ChaveNotasReferenciadas)
                {
                    lstReferencias.Add(new NFeReferenciadaDTO()
                    {
                        ChaveNotaRefenciada = chave
                    });
                }
            }

            NfeIdentificacaoDTO nfeIdent = new NfeIdentificacaoDTO()
            {
                CodigoNumerico = codigoNf,
                CodigoDoMunicipio = codigoMunicipioIBGE,
                IndicadorLocalDestino = tipoLocalOperacao,
                CodigoUFEmitente = codigoUFEmitente,
                DataDeEmissao = (DateTime)pedido.DataFaturamento,
                DataEntradaSaida = (DateTime)dataEntradaSaida,
                finNFe = finalidadeNFe,
                //IndicacaoPagamento = pedido.IndicacaoDePagamento,
                IndicacaoPrensenca = IndicacaoPresencaEnum.OPERACAO_NAO_PRESENCIAL_OUTROS,
                NaturezaOperacao = pedido.NaturezaDaOperacao,
                NumeroNotaFiscal = sequencialEmpresa,
                Serie = 1,
                TipoAmbiente = tipoAmbiente,
                TipoEmissao = TipoEmissaoEnum.Normal,
                FormatoImpressao = FormatoImpressaoEnum.Retrato,
                TipoOperacao = tipoNota,
                VersaoProcesso = "3.10.86",
                NotasRefenciadas = lstReferencias
            };

            infoNotaFiscal.Identificacao = nfeIdent;

            var items = GerarItensDaNota(pedido, cfop);
            infoNotaFiscal.Detalhamento = items;

            var total = GerarTotaisDaNotaFiscal(infoNotaFiscal.Detalhamento);
            infoNotaFiscal.Total = total;

            var informacoesPagamento = new InformacoesPagamento()
            {
                Detalhamentos = new List<InfPagamentoDetalhamento>()
                {
                    new InfPagamentoDetalhamento()
                    {
                        FormaPagamento = pedido.TipoPagamento,
                        ValorPagamento = (pedido.TipoPagamento != TipoPagamentoEnum.SEM_PAGAMENTO) ? total.ICMSTotal.TotalNotaFiscal : (decimal) 0.00,

                    }
                }
            };

            infoNotaFiscal.InformacoesPagamento = informacoesPagamento;

            infoNotaFiscal.Transporte = new NfeInfoTransporteDTO()
            {
                ModalidadeFrete = TipoModalidadeTransporteEnum.SEM_CORRENCIA_FRETE
            };

            string parcelasEmInfo = "";

            if (itemPed.Parcelas != null)
            {

                TNFeInfNFeCobr cobranca = new TNFeInfNFeCobr();

                cobranca.dup = new TNFeInfNFeCobrDup[itemPed.Parcelas.Count];

                parcelasEmInfo += itemPed.Parcelas.Count.ToString() + " Parcela(s) do contrato " + pedido.Contrato + " ( ";

                decimal valor = 0;

                for (int contador = 0; contador < itemPed.Parcelas.Count; contador++)
                {
                    
                    ParcelaDTO parcela = itemPed.Parcelas.ToList()[contador];

                    decimal valorProduto = (decimal)parcela.ValorLiquidoServico; //(parcela.ValorLiquido - parcela.ValorLiquidoServico);

                    TNFeInfNFeCobrDup dup = new TNFeInfNFeCobrDup();

                    DateTime dataParcela = itemPed.Parcelas.ToList()[contador].Vencimento;

                    dataParcela = ( dataParcela < DateTime.Now ? DateTime.Now : dataParcela );

                    dup.nDup = String.Format("{0:000}", contador + 1); //parcela.Parcela;
                    dup.dVenc = String.Format("{0:yyyy-MM-dd}", dataParcela);
                    dup.vDup = valorProduto.ToString().Replace(",", ".");

                    parcelasEmInfo += String.Format("{0:000}", contador + 1) + " - " + itemPed.Parcelas.ToList()[contador].Parcela + ":R$ " + valorProduto.ToString();

                    parcelasEmInfo += (contador + 1 == parcelas.Count ? " )." : " - ");

                    valor += valorProduto;

                    cobranca.dup[contador] = dup;

                }

                TNFeInfNFeCobrFat fat = new TNFeInfNFeCobrFat();

                fat.nFat = pedido.Contrato;  //pedido.CodPedido.ToString();
                fat.vOrig = valor.ToString().Replace(",", ".");
                fat.vDesc = "0.00";
                fat.vLiq = valor.ToString().Replace(",", ".");

                cobranca.fat = fat;

                infoNotaFiscal.Cobranca = cobranca;

            }

            string infAdFisco = "Informações Adicionais de Interesse do Fisco: Produto Imune Dec.7212/10-RIPI;CF/88 Art. 150 Inc.VI Alínea d. - Não Incidência do ICMS Art.40, inc.I Lei 2657/96.PROCON-RJ Tel. 151 End.Rua da ajuda, 05 - S.Solo-Centro/RJ - Comissão de Defesa do Consumidor da ALERJ Tel.:0800 282706,Conforme Lei 5817/10.- Não incidência de PIS e COFINS Art.5º,parágrafo único,inc.I, alínea C da IN 1234.";

            infAdFisco += ";" + parcelasEmInfo;

            infoNotaFiscal.InformacoesAdicionais = new NfeInformacoesAdicionaisDTO()
            {
                infAdFisco = infAdFisco,
                InformacoesComplementares = pedido.ObservacoesNotaFiscal

            };

            GerarCodigoDaNFe(nfe);

            var result = ValidatorProxy.RecursiveValidate<NotaFiscal>(nfe);

            if (validacao.IsValid)
            {

                //var resp = EnviarLote(lote);
                return nfe;
            }
            else
            {
                throw new ValidacaoException("Ocorre um problema ao gerar o xml da nota fiscal.", validacao);
            }
        }

        private NotaFiscal _gerarDadosDevolucao(int? CodNota, INFeLoteItem loteItem)
        {
            if (CodNota != null && loteItem != null)
            {
                var notaFiscal = LoteSRV.RetornarNotaFiscal(CodNota);
                var notasReferenciadas = LoteSRV.ListarNotasReferenciadas(loteItem.ItemLoteID);

                if (notaFiscal != null && notaFiscal.Arquivo != null)
                {
                    var codigoPaisDestino = 1058;
                    var empresa = LoteSRV.RetornarEmpresa(notaFiscal.CodEmpresa);
                    var cliente = LoteSRV.RetornarClientePorNota(CodNota);
                    var sequencialEmpresa = LoteSRV.RetornarSequencialEmpresa(notaFiscal.CodEmpresa);

                    int codMun = 0;
                    int.TryParse(empresa.CodIBGE, out codMun);

                    ICollection<string> lstChaveNotasRef = new HashSet<string>();
                    NFeProcessada nota = XmlUtil.LoadFromXMLBytes<NFeProcessada>(notaFiscal.Arquivo);

                    if (notasReferenciadas != null)
                    {
                        lstChaveNotasRef = notasReferenciadas.Select(x => x.ChaveNota).ToList();
                    }

                    if (cliente == null)
                    {
                        throw new Exception(string.Format("Não é possível localizar o clienbte para a nota {0}", CodNota));
                    }

                    loteItem.ClienteID = cliente.CodCliente;

                    EmpresaDTO empresaDTO = new EmpresaDTO()
                    {
                        CNPJ = empresa.CNPJ,
                        IE = empresa.IE,
                        NomeFantasia = empresa.NomeFantasia,
                        RazaoSocial = empresa.RazaoSocial,
                        SequencialNFe = sequencialEmpresa,
                        Endereco = new EnderecoDTO()
                        {
                            Bairro = empresa.Bairro,
                            CEP = empresa.CEP,
                            CodMunicipio = codMun,
                            Complemento = empresa.Complemento,
                            Logradouro = empresa.Logradouro,
                            Municipio = empresa.Municipio,
                            Numero = empresa.Numero,
                            Pais = codigoPaisDestino.ToString(),
                            Telefone = empresa.Telefone,
                            UF = empresa.UFDTO
                        }
                    };

                    
                    ICollection<PedidoItemDTO> pedidosItens = new HashSet<PedidoItemDTO>();

                    if (nota != null && nota.NFe != null &&
                        nota.NFe.Count > 0 &&
                        nota.NFe[0].lstInfNFe != null
                        && nota.NFe[0].lstInfNFe.Count > 0)
                    {

                        var itens = nota.NFe[0].lstInfNFe[0].Detalhamento;

                        foreach (var item in itens)
                        {
                            pedidosItens.Add(new PedidoItemDTO()
                            {
                                Produto = new ProdutoDTO()
                                {
                                    CodProduto = item.Produto.CodigoProduto,
                                    NCM = item.Produto.NCM,
                                    Nome = item.Produto.NomeProduto,
                                    UnidadeComercial = item.Produto.Unidade,
                                    UnidadeTributavel = item.Produto.UnidadeTributavel,
                                },
                                QtdComercial = item.Produto.Quantidade,
                                QtdTributavel = item.Produto.QuantidadeTributavel,
                                ValorTotal = item.Produto.ValorTotal,
                                ValorUnitario = item.Produto.ValorUnitario
                            });
                        }

                        PedidoDTO pedido = new PedidoDTO()
                        {
                            Cliente = cliente,
                            DataFaturamento = DateTime.Now,
                            Empresa = empresaDTO,
                            TipoPagamento = TipoPagamentoEnum.SEM_PAGAMENTO,
                            //IndicacaoDePagamento = IndPagEnum.PagamentoAVista,
                            TipoOperacaoNota = TipoOperacaoEnum.DEVOLUCAO,
                            Items = pedidosItens,
                            ChaveNotasReferenciadas = lstChaveNotasRef
                        };

                        pedido.Contrato = loteItem.CodContrato;

                        IList<ParcelaDTO> parcelas = LoteSRV.RetornarParcelasPorPedido(null, loteItem.CodPedido);

                        return GerarNfeDTO(pedido, parcelas);
                    }
                }
            }
            return null;
        }

        public NfeEmitenteDTO GerarDadosDoEmitente(EmpresaDTO empresa)
        {

            if (string.IsNullOrWhiteSpace(empresa.IE))
                throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui inscrição estadual cadastrada.", empresa.NomeFantasia));

            if (SysUtils.InHomologation())
            {
                empresa.RazaoSocial = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            if (string.IsNullOrWhiteSpace(empresa.Endereco.Municipio))
            {
                empresa.Endereco.Municipio = StringUtil.LimparAcentuacao(empresa.Endereco.Municipio);
            }

            NfeEmitenteDTO emitente = new NfeEmitenteDTO()
            {
                CNPJ = empresa.CNPJ,
                InscricaoEstadual = empresa.IE,
                xNome = empresa.RazaoSocial,
                NomeFantasia = empresa.NomeFantasia,
                EnderecoEmitente = new NfeEnderecoEmitenteDTO()
                {
                    CEP = empresa.Endereco.CEP,
                    CodigoMunicipio = empresa.Endereco.CodMunicipio,
                    Telefone = empresa.Endereco.Telefone,
                    Numero = StringUtil.PreencherZeroEsquerda(empresa.Endereco.Numero, 3),
                    UF = empresa.Endereco.UF.Nome,
                    Bairro = empresa.Endereco.Bairro,
                    Complemento = empresa.Endereco.Complemento,
                    Logradouro = empresa.Endereco.Logradouro,
                    Municipio = empresa.Endereco.Municipio
                }
            };

            if (!string.IsNullOrWhiteSpace(emitente.NomeFantasia))
                emitente.NomeFantasia = emitente.NomeFantasia.Trim();
            if (!string.IsNullOrWhiteSpace(emitente.xNome))
                emitente.xNome = emitente.xNome.Trim();
            return emitente;

        }

        public DadosPrestadorRps GerarDadosDoPrestadorNfse(EmpresaDTO empresa)
        {

            if (string.IsNullOrWhiteSpace(empresa.IM))
                throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal. A empresa {0} não possui inscrição municipal cadastrada.", empresa.NomeFantasia));

            if (SysUtils.InHomologation())
            {
                empresa.RazaoSocial = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            if (string.IsNullOrWhiteSpace(empresa.Endereco.Municipio))
            {
                empresa.Endereco.Municipio = StringUtil.LimparAcentuacao(empresa.Endereco.Municipio);
            }

            DadosPrestadorRps prestador = new DadosPrestadorRps()
            {
                IdentificacaoPrestador = new IdentificacaoPrestadorRps()
                {
                    Cnpj = empresa.CNPJ,
                    InscricaoMunicipal = empresa.IM,
                },
                NomeFantasia = empresa.NomeFantasia,
                RazaoSocial = empresa.RazaoSocial,
                Endereco = new EnderecoRps()
                {
                    Endereco = empresa.Endereco.Logradouro,
                    Bairro = empresa.Endereco.Bairro,
                    Cep = empresa.Endereco.CEP,
                    CodigoMunicipio = empresa.Endereco.CodMunicipio,
                    Complemento = empresa.Endereco.Complemento,
                    Numero = empresa.Endereco.Numero,
                    Uf = empresa.Endereco.UF.Nome
                },
                Contato = new ContatoRps()
                {
                    Telefone = empresa.Endereco.Telefone
                }
            };

            if (!string.IsNullOrWhiteSpace(prestador.NomeFantasia))
                prestador.NomeFantasia = prestador.NomeFantasia.Trim();
            if (!string.IsNullOrWhiteSpace(prestador.RazaoSocial))
                prestador.RazaoSocial = prestador.RazaoSocial.Trim();
            return prestador;

        }

        public NfeDestinoDTO GerarDadosDoDestinatario(ClienteDTO cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException("O cliente não pode ser nulo");
            }

            if (cliente.Endereco == null)
            {
                throw new GeracaoNotaException("O endereço do cliente pode ser nulo");
            }

            if (SysUtils.InHomologation())
            {
                cliente.Nome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            if (cliente.TipoCliente == TipoClienteEnum.Fisica)
            {
                cliente.CPF = StringUtil.PreencherZeroEsquerda(cliente.CPF, 11);
                cliente.CPF = StringUtil.Truncate(cliente.CPF, 11);
            }
            else
            {
                cliente.CNPJ = StringUtil.PreencherZeroEsquerda(cliente.CNPJ, 14);
                cliente.CNPJ = StringUtil.Truncate(cliente.CNPJ, 14);
            }

            var codigoPaisDestino = 1058;

            NfeDestinoDTO destino = new NfeDestinoDTO()
            {
                CPF = cliente.CPF,
                CNPJ = cliente.CNPJ,
                Email = (!string.IsNullOrWhiteSpace(cliente.Email)) ? cliente.Email : null,
                xNome = cliente.Nome,
                Endereco = new NfeEnderecoDestinatarioDTO()
                {
                    CEP = cliente.Endereco.CEP,
                    CodigoPais = codigoPaisDestino,
                    CodigoMunicipio = cliente.Endereco.CodMunicipio,
                    Telefone = (!string.IsNullOrWhiteSpace(cliente.Endereco.Telefone)) ? cliente.Endereco.Telefone : null,
                    Numero = StringUtil.PreencherZeroEsquerda(cliente.Endereco.Numero, 3),
                    UF = cliente.Endereco.UF.Nome,
                    Bairro = StringUtil.RetirarCaractereEspecialComTrim(cliente.Endereco.Bairro),
                    Complemento = StringUtil.RetirarCaractereEspecialComTrim(cliente.Endereco.Complemento),
                    Logradouro = StringUtil.RetirarCaractereEspecialComTrim(cliente.Endereco.Logradouro),
                    Municipio = cliente.Endereco.Municipio,
                    Pais = cliente.Endereco.Pais,
                },
                TipoIEDestinatario = (cliente.IE != null) ?
                    TipoIEDestinatarioEnum.ContribuinteICMS :
                    TipoIEDestinatarioEnum.ContribuinteIsentoDoICMS,
                IncricaoEstadual = cliente.IE
            };

            if (!string.IsNullOrWhiteSpace(destino.xNome))
                destino.xNome = destino.xNome.Trim();

            return destino;
        }


        public DadosTomadorRps GerarDadosDoTomadorNfse(ClienteDTO cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException("O cliente não pode ser nulo");
            }

            if (cliente.Endereco == null)
            {
                throw new GeracaoNotaException("O endereço do cliente pode ser nulo");
            }

            if (SysUtils.InHomologation())
            {
                cliente.Nome = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL";
            }

            if (cliente.TipoCliente == TipoClienteEnum.Fisica)
            {
                cliente.CPF = StringUtil.PreencherZeroEsquerda(cliente.CPF, 11);
                cliente.CPF = StringUtil.Truncate(cliente.CPF, 11);
            }
            else
            {
                cliente.CNPJ = StringUtil.PreencherZeroEsquerda(cliente.CNPJ, 14);
                cliente.CNPJ = StringUtil.Truncate(cliente.CNPJ, 14);
            }
            

            DadosTomadorRps tomador = new DadosTomadorRps()
            {
                IdentificacaoTomador = new IdentificacaoTomadorRps()
                {
                    CpfCnpj = new CnpjCpfRps()
                    {
                        Cnpj = cliente.CNPJ,
                        Cpf = cliente.CPF
                    },
                    InscricaoMunicipal = cliente.IE
                },
                RazaoSocial = cliente.Nome,
                Endereco = new EnderecoRps()
                {
                    Bairro = cliente.Endereco.Bairro,
                    Cep = cliente.Endereco.CEP,
                    CodigoMunicipio = cliente.Endereco.CodMunicipio,
                    Complemento = cliente.Endereco.Complemento,
                    Endereco = cliente.Endereco.Logradouro,
                    Numero = cliente.Endereco.Numero,
                    Uf = cliente.Endereco.UF.Nome
                },
                Contato = new ContatoRps()
                {
                    Email = cliente.Email,
                    Telefone = cliente.Endereco.Telefone
                }                
            };            

            if (!string.IsNullOrWhiteSpace(tomador.RazaoSocial))
                tomador.RazaoSocial = tomador.RazaoSocial.Trim();

            return tomador;
        }

        public List<NFeDetalhamentoItem> GerarItensDaNota(PedidoDTO pedido, int? cfop)
        {
            List<NFeDetalhamentoItem> itensNota = new List<NFeDetalhamentoItem>();

            if (pedido.Items == null || pedido.Items.Count <= 0)
            {
                throw new GeracaoNotaException("Não existem itens da nota.");
            }

            var index = 0;
            foreach (var itemPed in pedido.Items)
            {
                if (itemPed.Produto == null)
                    throw new Exception(string.Format("Não é possível gerar os itens da nota. O produto no item {0} não foi encontrado", index));

                if (string.IsNullOrWhiteSpace(itemPed.Produto.Nome))
                    throw new Exception(string.Format("Não é possível gerar os itens da nota. O produto no item {0} não possui nenhum nome.", index));

                NfeItemDTO item = new NfeItemDTO()
                {
                    CodigoProduto = StringUtil.PreencherZeroEsquerda(itemPed.Produto.CodProduto, 3),
                    NomeProduto = itemPed.Produto.Nome,
                    CFOP = cfop,
                    NCM = itemPed.Produto.NCM,
                    Quantidade = (decimal)itemPed.QtdComercial,
                    Unidade = itemPed.Produto.UnidadeComercial,
                    ValorUnitario = (decimal)itemPed.ValorUnitario,
                    QuantidadeTributavel = (decimal)itemPed.QtdTributavel,
                    UnidadeTributavel = itemPed.Produto.UnidadeTributavel,
                    ValorUnitarioTributacao = (decimal)itemPed.ValorUnitario,
                    ValorTotal = (decimal)itemPed.ValorTotal,
                    IndicacaoTotal = 1
                };

                NFeImpostoICMSGrupo40 icms40 = new NFeImpostoICMSGrupo40();
                icms40.CST = 41;

                NFeImposto imposto = new NFeImposto();
                imposto.ICMS = new NFeImpostoICMS()
                {
                    ICMS40 = icms40
                };

                imposto.PIS = new NFeImpostoPIS()
                {
                    PISGrupoNaoTributado = new NFeImpostoPISGrupoNaoTributadoDTO()
                };

                imposto.COFINS = new NFeImpostoCOFINS()
                {
                    COFINSGrupoNaoTributado = new NFeImpostoCOFINSGrupoNaoTributadoDTO()
                };

                imposto.IPI = new NfeImpostoIPI()
                {
                    CodigoEnquadramento = "999",
                    IPINaoTributado = new NfeImpostoIPIGrupoNaoTributado()
                    {
                        CST = TipoTributacaoIPIEnum.SaidaNaoTributada
                    }
                };

                NFeDetalhamentoItem det = new NFeDetalhamentoItem()
                {
                    NumeroItem = 1,
                    Produto = item,
                    Imposto = imposto
                };
                itensNota.Add(det);
                index++;
            }
            return itensNota;
        }

        private string GerarInfoParcelaDiscriminacaoServico(PedidoItemDTO pedidoItem, PedidoDTO pedido, IList<ParcelaDTO> parcelas = null)
        {

            if (pedidoItem != null)
            {

                StringBuilder sb = new StringBuilder();

                if (pedidoItem.Parcelas != null) //( parcelas != null )
                {

                    IList<ParcelaDTO> parcelasLista = pedidoItem.Parcelas.ToList();

                    StringBuilder sbDesc = new StringBuilder();

                    sbDesc.Append("\n");
                    sbDesc.Append($"{parcelasLista.Count()} Parcela(s) do contrato: " + pedido.Contrato + " ( "); //parcelas[0].Contrato

                    sb.Append(sbDesc.ToString());

                    for (int contador = 0; contador < parcelasLista.Count(); contador ++)
                    {

                        ParcelaDTO par = parcelasLista[contador];  //parcelas[contador];

                        sbDesc = new StringBuilder();

                        sbDesc.Append($"{String.Format("{0:000}", contador + 1)} - " + parcelas[contador].Parcela + ":R$ " + par.ValorLiquidoServico + ( (contador + 1 == parcelasLista.Count()) ? " ) \n" : " - " )); //par.Parcela

                        sb.Append(sbDesc.ToString());

                    }

                }
                
                if(pedidoItem.Parcelas != null)
                {
                    bool exibirTextoDesconto = false;

                    foreach(var par in pedidoItem.Parcelas.ToList().Take(2))
                    {

                        var valorParcela = StringUtil.FormatarDinheiro(par.ValorParcela);
                        var valorLiquido = StringUtil.FormatarDinheiro(par.ValorLiquidoServico);
                        var baseCalculo = StringUtil.FormatarDinheiro(par.BaseCalculo);
                        var baseCalculoIR = StringUtil.FormatarDinheiro(pedidoItem.Servico.BaseCalculo);

                        decimal? aliquotaIRRF = 0;
                        decimal? aliquotaFederal = 0;

                        decimal? totalIRRF = 0;
                        decimal? totalImpFederal = 0;

                        string totalIRRFFormatado = null;
                        string totalImpFederalFormatado = null;

                        if (par.Impostos != null)
                        {
                            var impostos = par.Impostos.OrderBy(x => x.Ordem);
                            
                            foreach (var im in impostos)
                            {
                                    
                                if(im.TipoImposto == ImpostosEnum.IR)
                                {
                                    aliquotaIRRF += im.Aliguota;
                                    totalIRRF += im.ValorDesconto;
                                }
                                else
                                {
                                    aliquotaFederal += im.Aliguota;
                                    totalImpFederal += im.ValorDesconto;
                                }
                            }

                            

                            totalIRRFFormatado = StringUtil.FormatarDinheiro(totalIRRF);
                            totalImpFederalFormatado = StringUtil.FormatarDinheiro(totalImpFederal);
                            StringBuilder sbDesc = new StringBuilder();

                            if (par.NumeroParcela == 0)
                            {
                                if ((totalImpFederal <= 0 || totalIRRF <= 0) && par.NaoRetevePorRegra == true)
                                {
                                    exibirTextoDesconto = true;
                                }
                                sbDesc.Append("\n");
                                sbDesc.Append($"Entrada: ");
                                sbDesc.Append($"Base de Cálculo para IRRF - {baseCalculoIR}, ({aliquotaIRRF}% - {totalIRRFFormatado}). ");
                                sbDesc.Append("\n");
                                sbDesc.Append($"Base de Cálculo para Contribuição Federal - {baseCalculo}, ({aliquotaFederal}% - {totalImpFederalFormatado}), Valor Líquido: {valorLiquido}. ");
                                sbDesc.Append("\n");
                                sb.Append(sbDesc.ToString());
                            }
                            else
                            {
                                if (totalImpFederal <= 0 && par.NaoRetevePorRegra == true)
                                {
                                    exibirTextoDesconto = true;
                                }

                                sbDesc.Append("\n");
                                sbDesc.Append($"{pedidoItem.Parcelas.Count - 1} Parcela(s) onde: ");
                                sbDesc.Append($"Base de Cálculo por Parcela para Contribuição Federal - {baseCalculo}, ({aliquotaFederal}% - {totalImpFederalFormatado}), Valor Líquido: {valorLiquido}. ");
                                sbDesc.Append("\n");

                                sb.Append(sbDesc.ToString());
                            }
                        }
                    }

                    if (exibirTextoDesconto)
                    {
                        sb.Append("\n");
                        sb.Append("Abaixo de R$ 10,00 não Reter");
                        sb.Append("\n");
                    }

                    if (pedidoItem.Servico.CodigoTributacaoMunicipio == "010901")
                    {
                        sb.Append("\n ");
                        sb.Append($"Código dos Serviços Prestados: 01.09.01 - Serviço digital - Resolução 2617/2010 Art.10 §4° Inciso XIII. ");
                        sb.Append("\n ");
                    }

                    if (pedido != null && !string.IsNullOrWhiteSpace(pedido.ObservacoesNotaFiscal))
                    {
                        sb.Append("\n");
                        sb.Append(pedido.ObservacoesNotaFiscal);
                        sb.Append("\n");
                    }
                }
                return sb.ToString();
            }
            return null;
        }

        public DadosServicoRps GerarDadosServicoNfse(PedidoDTO pedido, IList<ParcelaDTO> parcelas = null)
        {
            if (pedido.Items == null || pedido.Items.Count <= 0)
            {
                throw new GeracaoNotaException("Não existem itens da nota.");
            }

            var itemPed = pedido.Items.Where(x => x.Servico != null).FirstOrDefault();
            if(itemPed.Servico == null)
                throw new Exception("Não é possível gerar os itens da nota. Não foi encontrado nenhum dado do serviço.");

            if (string.IsNullOrWhiteSpace(itemPed.Servico.DescricaoServico))
                    throw new Exception("Não é possível gerar os itens da nota. Não foi encontrado nenhuma descrição do serviço.");

            if (string.IsNullOrWhiteSpace(itemPed.Servico.DescricaoServico))
                    throw new Exception("Não é possível gerar os itens da nota. O Serviço não foi discriminado.");

            var discriminacaoParcela = GerarInfoParcelaDiscriminacaoServico(itemPed, pedido, parcelas);
            itemPed.Servico.DescricaoServico += discriminacaoParcela;

            DadosServicoRps Servico = new DadosServicoRps()
            {
                Valores = new ValoresRps()
                {
                    ValorServicos = itemPed.Servico.ValorServicos,
                    ValorPis = itemPed.Servico.ValorPis,
                    Aliquota = itemPed.Servico.Aliquota,
                    BaseCalculo = itemPed.Servico.BaseCalculo,
                    DescontoCondicionado = itemPed.Servico.DescontoCondicionado,
                    DescontoIncondicionado = itemPed.Servico.DescontoIncondicionado,
                    IssRetido = (itemPed.Servico.IssRetido) ? IdenSimNaoEnum.SIM : IdenSimNaoEnum.NAO,
                    OutrasRetencoes = itemPed.Servico.OutrasRetencoes,
                    ValorCofins = itemPed.Servico.ValorCofins,
                    ValorCsll = itemPed.Servico.ValorCsll,
                    ValorDeducoes = itemPed.Servico.ValorDeducoes,
                    ValorInss = itemPed.Servico.ValorInss,
                    ValorIr = itemPed.Servico.ValorIr,
                    ValorIss = itemPed.Servico.ValorIss,
                    ValorIssRetido = itemPed.Servico.ValorIssRetido,
                    ValorLiquidoNfse = itemPed.Servico.ValorLiquidoNfse,

                },
                CodigoMunicipio = pedido.Empresa.Endereco.CodMunicipio,
                
                ItemListaServico = itemPed.Servico.CodListaServico,
                Discriminacao = itemPed.Servico.DescricaoServico,
                CodigoTributacaoMunicipio = itemPed.Servico.CodigoTributacaoMunicipio,
            };
        
            return Servico;
        }

        public NFeTotalDTO GerarTotaisDaNotaFiscal(IEnumerable<NFeDetalhamentoItem> detalhamentos)
        {

            decimal desconto = 0.00m;
            decimal totalDosProdutos = 0.00m;
            decimal totalDaNota = 0.00m;

            foreach (var det in detalhamentos)
            {
                //desconto += det.prod.desconto;
                totalDosProdutos += det.Produto.ValorTotal;
            }

            totalDaNota = totalDosProdutos; //- desconto;

            NFeTotalDTO total = new NFeTotalDTO()
            {
                ICMSTotal = new NFeICMSTotalDTO()
                {
                    BaseCalculoICMS = 0.00m,
                    ValorICMS = 0.00m,
                    ICMSDesoneracao = 0.00m,
                    BaseCalculoST = 0.00m,
                    ValorST = 0.00m,
                    TotalProduto = totalDosProdutos,
                    ValorFrete = 0.00m,
                    TotalSeguro = 0.00m,
                    TotalDesconto = desconto,
                    ValorII = 0.00m,
                    ValorIPI = 0.00m,
                    ValorPIS = 0.00m,
                    ValorCOFINS = 0.00m,
                    ValorOutrasDespesas = 0.00m,
                    ValorIPIDevolvido = 0.00m,
                    ValorTotalFCP = 0.00m,
                    ValorTotalFCPST = 0.00m,
                    ValorTotalFCPSTRetido = 0.00m,
                    TotalNotaFiscal = totalDaNota,
                }
            };

            return total;

        }

        /// <summary>
        /// Recebe um lote de notas fiscais e transmite para a SEFAZ. Esse método é assincrono. O retorno é uma chave para ser utilizada para ser consultada por outro serviço.
        /// </summary>
        /// <param name="lote"></param>
        /// <returns></returns>
        public LoteRetorno EnviarLote(LoteNFE lote, int? empID)
        {
            //string fileName = string.Format(@"\lote_envio_{0:yyyy-MM-ddTH-mm}.xml", DateTime.Now);
            //string downloadPath = Path.Combine(defaultPath, "nfe", Path.GetFileName(fileName));

            var certificado = CertificateUtil.RetornarCertificado(empID);
            var srvAutorizacao = ServiceFactory.RetornarServico<ClienteLoteNfeSRV>();
            var empresa = LoteSRV.RetornarEmpresa(empID);

            var resp = srvAutorizacao.EnviarLoteNotaFiscal(lote, certificado, empresa.UFDTO.Nome);
            return resp;
        }

        /// <summary>
        /// Recebe um lote notas fiscais de serviço
        /// </summary>
        /// <param name="lote"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        public EnviarLoteRpsResposta EnviarLoteNfse(EnviarLoteRpsEnvio lote, int? empID)
        {
            var certificado = CertificateUtil.RetornarCertificado(empID);
            var srvEnvioNfse = ServiceFactory.RetornarServico<ClienteNfseSRV>();

            var resp = srvEnvioNfse.EnviarLoteNotaFiscal(lote, certificado);
            return resp;
        }

        /// <summary>
        /// Esse método consulta o resultado do processamento do lote de NFe por parte do  SEFAZ Recebe um objeto de consulta do lote contendo, principalmente, a chave de recibo para consultar no SEFAZ
        /// </summary>
        /// <param name="consultaLote"></param>
        /// <returns></returns>
        public ConsultaLoteRetorno ConsultarProcessamentoLote(ConsultaLote consultaLote, int? empID)
        {

            var validacao = ValidatorProxy.RecursiveValidate(consultaLote);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre consultar o resultado do processamento do lote.", validacao);
            }

            var certificado = CertificateUtil.RetornarCertificado(empID);
            var srvRetAutorizacao = ServiceFactory.RetornarServico<ClienteRetornoLoteSRV>();

            var resp = srvRetAutorizacao.ProcessarRetornoNotaFiscal(consultaLote, certificado);
            return resp;
        }

        public ConsultarSituacaoLoteRpsResposta ChecarSituacaoDoLote(INFeLote lote)
        {
            var prestador = LoteSRV.RetornarDadosPrestador(lote.EmpresaID);
            ConsultarSituacaoLoteRpsEnvio consultaLoteSituacao = new ConsultarSituacaoLoteRpsEnvio()
            {
                Prestador = prestador,
                Protocolo = lote.CodRecibo
            };

            var validacao = ValidatorProxy.RecursiveValidate(consultaLoteSituacao);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre consultar o resultado do processamento do lote.", validacao);
            }

            var certificado = CertificateUtil.RetornarCertificado(lote.EmpresaID);
            var srvNFse = ServiceFactory.RetornarServico<ClienteNfseSRV>();

            var resp = srvNFse.ChecarSituacaoDoLote(consultaLoteSituacao, certificado);
            return resp;
        }
        
        public INFeLote AdicionarPedidoLoteVigente(int? CodPedido, int? EmpresaID, string CodContrato = null)
        {
            try
            {
                if (CodPedido != null)
                {
                    var resp = LoteSRV.AdicionarPedidoLoteVigente(CodPedido, EmpresaID, CodContrato);
                    return resp;
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Não é possível gerar a nota fiscal para o pedido '{0}'.", CodPedido), e);
            }

        }

        public void EnviarLoteVigente(int? numeroDeTentativas = 0)
        {
            var loteVigente = LoteSRV.RetornarProximoLoteParaEnvioCompleto();

            if (loteVigente != null)
            {
                using (var scope = new TransactionScope())
                {
                    EnviarLote(loteVigente);
                    scope.Complete();
                }

                for (int tentativa = 1; tentativa <= numeroDeTentativas; tentativa++)
                {
                    //Tentativa de pegar a resposta no momento
                    Thread.Sleep(15000);

                    using (var scope = new TransactionScope())
                    {

                        ProcessarRetornoDoLote(loteVigente);
                        scope.Complete();
                    }

                    if (loteVigente.Status == StatusLoteEnum.LOTE_EM_PROCESSAMENTO)
                    {
                        tentativa++;
                    }
                    if (loteVigente.Status == StatusLoteEnum.PROCESSADA_COM_EXITO)
                    {
                        return;
                    }
                }
            }
        }

        public void EnviarLote(INFeLote lote)
        {
            try
            {
                var notas = new List<NotaFiscal>();

                if (lote != null && lote.Itens != null)
                {
                    foreach (var item in lote.Itens)
                    { 
                        NotaFiscal nfe = null;
                        switch (item.Tipo)
                        {
                            case TipoLoteItemEnum.ENVIO:
                                {
                                    PedidoDTO pedido = null;
                                    IList<ParcelaDTO> parcelas = null;

                                    if (item.CodPedido != null)
                                    {

                                        pedido = LoteSRV.RetornarPedido(item.CodPedido, item.CodContrato, item.NfConfigID);
                                        parcelas = LoteSRV.RetornarParcelasPorPedido(item.CodPedido, null);

                                    }
                                    else if (item.CodProposta != null)
                                    {

                                        pedido = LoteSRV.RetornarProposta(item.CodProposta);
                                        parcelas = LoteSRV.RetornarParcelasPorProposta(item.CodProposta);

                                    }

                                    pedido.TipoOperacaoNota = TipoOperacaoEnum.VENDA;
                                    pedido.Contrato = item.CodContrato;

                                    nfe = GerarNfeDTO(pedido, parcelas);
                                    break;

                                }
                            case TipoLoteItemEnum.DEVOLUCAO:
                                {

                                    nfe = _gerarDadosDevolucao(item.CodNotaFiscal, item);
                                    break;
                                }
                        }

                        notas.Add(nfe);

                        LoteSRV.LoteItemNFeSRV.InserirChaveENumeroDaNota(item, nfe);
                        item.Status = StatusLoteItemEnum.AGUARDANDO_RETORNO;
                    }
                    LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);


                    var loteNFe = new LoteNFE()
                    {
                        idLote = lote.LoteID,
                        indSinc = IndicacaoDeSincroniaEnum.ASSINCRONO,
                        versao = ConstantsNFe.VERSAO,
                        NFe = notas
                    };

                    LoteRetorno retorno = EnviarLote(loteNFe, lote.EmpresaID);

                    if (retorno.cStat == 103 && retorno.infRec != null)
                    {
                        var codRecibo = retorno.infRec.nRec;
                        lote.CodRecibo = codRecibo;
                        lote.CodRetorno = retorno.cStat;
                        lote.MensagemRetorno = retorno.xMotivo;
                        lote.Status = StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO;
                        LoteSRV.SalvarOuAtualizarLote(lote);
                        InserirXmlNotaNoLoteItem(lote.Itens, retorno.XmlLote);
                        LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);

                    }
                    else
                    {
                        InutilizarNotas(lote);
                        throw new RetornoProcessamentoException("O processamento do Lote não retornou o código esperado.", retorno.cStat, retorno.xMotivo);
                    }
                }
            }
            catch (RetornoProcessamentoException e)
            {
                if (lote != null)
                {
                    lote.CodRetorno = e.CodRetorno;
                    lote.MensagemRetorno = e.MensagemRetorno;
                    lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                    LoteSRV.SalvarOuAtualizarLote(lote);
                }
            }
        }

        public void EnviarLoteNfse(INFeLote lote)
        {
            try
            {
                var notas = new List<Rps>();

                if (lote != null && lote.Itens != null)
                {

                    int? index = 0;
                    foreach (var item in lote.Itens)
                    {
                        Rps rps = null;
                        switch (item.Tipo)
                        {
                            case TipoLoteItemEnum.ENVIO:
                                {

                                    PedidoDTO pedido = null;
                                    
                                    IList<ParcelaDTO> parcelas = null;

                                    if (lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_RPS_NFSE)
                                    {

                                        if (item.CodPedido != null)
                                        {

                                            pedido = LoteSRV.RetornarPedido(item.CodPedido, item.CodContrato, item.NfConfigID);
                                            parcelas = LoteSRV.RetornarParcelasPorPedido(item.CodPedido, null);

                                        }
                                        else if (item.CodProposta != null)
                                        {

                                            pedido = LoteSRV.RetornarProposta(item.CodProposta);
                                            parcelas = LoteSRV.RetornarParcelasPorProposta(item.CodProposta);

                                        }

                                    }
                                    else if (lote.TipoLote == TipoLoteEnum.ENVIO_NFSE_AVULSA)
                                    {

                                    }

                                    /*
                                    if (lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_RPS_NFSE)
                                        if (item.CodPedido != null)
                                            pedido = LoteSRV.RetornarPedido(item.CodPedido, item.CodContrato, item.NfConfigID);
                                        else if (item.CodProposta != null)
                                            pedido = LoteSRV.RetornarProposta(item.CodProposta);
                                    */

                                    pedido.Contrato = item.CodContrato;

                                    rps = GerarNfseDTO(pedido, parcelas);
                                    break;

                                }
                        }

                        notas.Add(rps);

                        LoteSRV.LoteItemNFeSRV.InserirChaveENumeroDaNotaFiscalServico(item, rps);
                        item.Status = StatusLoteItemEnum.AGUARDANDO_RETORNO;
                        index++;
                    }

                    LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
                    var empresa = LoteSRV.RetornarEmpresa(lote.EmpresaID);

                    if (empresa != null)
                    {
                        EnviarLoteRpsEnvio enviarLote = new EnviarLoteRpsEnvio()
                        {
                            LoteRps = new LoteRps()
                            {
                                Cnpj = empresa.CNPJ,
                                InscricaoMunicipal = empresa.IM,
                                NumeroLote = lote.LoteID,
                                QuantidadeRps = lote.Itens.Count,
                                Id = $"R{lote.LoteID}",
                                ListaRps = notas
                            }
                        };

                        EnviarLoteRpsResposta retorno = EnviarLoteNfse(enviarLote, lote.EmpresaID);

                        if (!string.IsNullOrWhiteSpace(retorno.Protocolo))
                        {
                            var codRecibo = retorno.Protocolo;
                            lote.CodRecibo = codRecibo;
                            lote.Status = StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO;
                            LoteSRV.SalvarOuAtualizarLote(lote);
                            LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
                        }
                        else
                        {
                            var mensagem = ExceptionFormatterNfse.FormatarMensagem("O processamento do Lote não retornou o código esperado.", retorno.ListaMensagemRetorno);
                            throw new RetornoProcessamentoNfseException(mensagem);
                        }
                    }
                }
            }
            catch (RetornoProcessamentoException e)
            {
                if (lote != null)
                {
                    lote.CodRetorno = e.CodRetorno;
                    lote.MensagemRetorno = e.MensagemRetorno;
                    lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                    LoteSRV.SalvarOuAtualizarLote(lote);
                }
            }
        }

        public void InutilizarNotas(INFeLote lote)
        {
            if (lote != null && lote.Itens != null)
            {
                foreach (var loteItm in lote.Itens)
                {
                    if (loteItm.NumeroNota != null && loteItm.BinarioNFeXml != null && loteItm.BinarioNFeXml.Count() > 0)
                    {
                        InutilizarNFe(loteItm);
                    }
                }
            }
        }

        public void ProcessarRetornoLoteEnviado()
        {

            using (var scope = new TransactionScope())
            {

                var lote = LoteSRV.RetornarProximoLoteParaProcessarCompleto();
                if (lote != null)
                {
                    ProcessarRetornoDoLote(lote);
                    scope.Complete();
                }
            }

        }


        public INFeLote CriarNovoLote(RequisicaoNovoLote requisicaoCriacao)
        {
            return LoteSRV.CriarNovoLote(requisicaoCriacao);
        }

        public void ExecutarCallBacksLoteItem(INFeLoteItem item, BatchContext batchContext = null, bool levantarErro = false)
        {
            try
            {
                if (batchContext == null)
                    batchContext = new BatchContext();

                if (item.Status == StatusLoteItemEnum.AUTORIZADA)
                {
                    if (item.BinarioNFeXml == null || item.BinarioNFeXml.Count() <= 0)
                    {
                        throw new Exception("Não é possível encontrar o binário do arquivo.");
                    }

                    if (item.BinarioNFeXml == null || item.BinarioNFeXml.Count() <= 0)
                    {
                        throw new Exception("Não é possível encontrar o binário do arquivo.");
                    }

                    if (string.IsNullOrWhiteSpace(item.PathArquivoNFeXml))
                    {
                        throw new Exception("O Path não foi encontrado.");
                    }

                    File.WriteAllBytes(item.PathArquivoNFeXml, item.BinarioNFeXml);
                    var xmlContent = File.ReadAllText(item.PathArquivoNFeXml);
                    var procNFe = XmlUtil.LoadFromXMLString<NFeProcessada>(xmlContent);


                    var contextCallBack = new NFeAutorizadaContext()
                    {
                        fileName = Path.GetFileName(item.PathArquivoNFeXml),
                        filePath = item.PathArquivoNFeXml,
                        LoteItem = item,
                        Lote = item.Lote,
                        proNFe = procNFe,
                        bytesNFe = item.BinarioNFeXml
                    };
                    LoteSRV.NotaFiscalAutorizadaCallBack(contextCallBack);
                    item.Status = StatusLoteItemEnum.AUTORIZADA_E_ENVIADA;
                    batchContext.AdicionarContagemSucesso();


                    if (item.Tipo == TipoLoteItemEnum.DEVOLUCAO)
                    {
                        if (item.NotaFiscalReferenciados != null)
                        {
                            foreach (var ntRef in item.NotaFiscalReferenciados)
                            {
                                var devolucaoCallBack = new NFeDevolucaoContext()
                                {
                                    LoteItem = item,
                                    Lote = item.Lote,
                                    ChaveNota = ntRef.ChaveNota
                                };

                                LoteSRV.NotaFiscalDevolvidaCallBack(devolucaoCallBack);
                            }
                        }
                    }
                }
                else
               if (item.Status == StatusLoteItemEnum.REJEITADA)
                {
                    var contextCallBack = new NFeRejeitadaContext()
                    {
                        LoteItem = item,
                        Lote = item.Lote
                    };

                    InutilizarNFe(item);
                    LoteSRV.NotaFiscalRejeitadaCallBack(contextCallBack);

                    item.Status = StatusLoteItemEnum.REJEITADA_E_INUTILIZADA;
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception("Não é possível executar as tarefas pós autorização da NFe", e);
                var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(ex);
                item.MsgErroSistema = mensagem;
                batchContext.AdicionarContagemFalha();
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = item.NumeroNota + "",
                    Mensagem = string.Format("A nota de número {0} no item de lote Código {1} reportou os seguintes erros. {2}", item.NumeroNota, item.ItemLoteID, mensagem)
                });

                if (levantarErro)
                {
                    throw ex;
                }
            }
        }

        public void ExecutarCallBacksLoteItemNFse(INFeLoteItem item, BatchContext batchContext = null, bool levantarErro = false)
        {
            try
            {
                if (batchContext == null)
                    batchContext = new BatchContext();

                if (item.Status == StatusLoteItemEnum.AUTORIZADA)
                {
                    if (item.BinarioNFeXml == null || item.BinarioNFeXml.Count() <= 0)
                    {
                        throw new Exception("Não é possível encontrar o binário do arquivo.");
                    }

                    if (item.BinarioNFeXml == null || item.BinarioNFeXml.Count() <= 0)
                    {
                        throw new Exception("Não é possível encontrar o binário do arquivo.");
                    }

                    if (string.IsNullOrWhiteSpace(item.PathArquivoNFeXml))
                    {
                        throw new Exception("O Path não foi encontrado.");
                    }

                    File.WriteAllBytes(item.PathArquivoNFeXml, item.BinarioNFeXml);
                    var xmlContent = File.ReadAllText(item.PathArquivoNFeXml);
                    var compNfse = XmlUtil.LoadFromXMLString<CompNfse>(xmlContent);


                    var contextCallBack = new NFseAutorizadaContext()
                    {
                        fileName = Path.GetFileName(item.PathArquivoNFeXml),
                        filePath = item.PathArquivoNFeXml,
                        LoteItem = item,
                        Lote = item.Lote,
                        compNfse = compNfse,
                        bytesNFe = item.BinarioNFeXml
                    };
                    LoteSRV.NFseAutorizadaCallBack(contextCallBack);
                    item.Status = StatusLoteItemEnum.AUTORIZADA_E_ENVIADA;
                    batchContext.AdicionarContagemSucesso();
                }
                else
               if (item.Status == StatusLoteItemEnum.REJEITADA)
                {
                    var contextCallBack = new NFeRejeitadaContext()
                    {
                        LoteItem = item,
                        Lote = item.Lote
                    };
                    LoteSRV.NFseRejeitadaCallBack(contextCallBack);
                    item.Status = StatusLoteItemEnum.REJEITADA;
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception("Não é possível executar as tarefas pós autorização da NFe", e);
                var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(ex);
                item.MsgErroSistema = mensagem;
                batchContext.AdicionarContagemFalha();
                batchContext.ListErros.Add(new ErroReportItemDTO()
                {
                    Contexto = item.NumeroNota + "",
                    Mensagem = string.Format("A nota de número {0} no item de lote Código {1} reportou os seguintes erros. {2}", item.NumeroNota, item.ItemLoteID, mensagem)
                });

                if(levantarErro)
                {
                    throw ex;
                }
            }
        }

        public void ExecutarCallBacksLoteItem(INFeLote lote, BatchContext batchContext = null, bool levantarErro = false)
        {
            if (lote != null && lote.Itens != null)
            {
                if (lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_NFE)
                {
                    foreach (var item in lote.Itens)
                    {
                        ExecutarCallBacksLoteItem(item, batchContext, levantarErro);
                    }
                }

                if (lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_RPS_NFSE)
                {
                    foreach (var item in lote.Itens)
                    {
                        ExecutarCallBacksLoteItemNFse(item, batchContext, levantarErro);
                    }
                }
                LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
            }
        }

        public void ExecutarCallBacksLoteItemNFse(INFeLote lote, BatchContext batchContext = null)
        {
            if (lote != null && lote.Itens != null)
            {
                foreach (var item in lote.Itens)
                {
                    ExecutarCallBacksLoteItemNFse(item, batchContext);
                }
                LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
            }
        }

        private void ProcessarRetornoDoLote(INFeLote lote)
        {
            if (lote != null)
            {
                try
                {
                    var consultaLote = new ConsultaLote()
                    {
                        nRec = lote.CodRecibo,
                        tpAmb = (SysUtils.InHomologation()) ? TipoAmbienteEnum.Homologacao : TipoAmbienteEnum.Producao,
                        versao = ConstantsNFe.VERSAO
                    };

                    using (var scope = new TransactionScope())
                    {

                        var retorno = ConsultarProcessamentoLote(consultaLote, lote.EmpresaID);
                        LoteSRV.DefaultPath = defaultPath;

                        if ( retorno.cUF == 31 )
                            LoteSRV.ProcessarRetornoLoteMG(retorno, lote);
                        else
                            LoteSRV.ProcessarRetornoLote(retorno, lote);

                        scope.Complete();
                    }
                    ExecutarCallBacksLoteItem(lote);

                }
                catch (RetornoProcessamentoException e)
                {
                    if (lote != null)
                    {
                        lote.CodRetornoProcessamento = e.CodRetorno;
                        lote.MsgErroSistema = e.MensagemRetorno;
                        lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                        LoteSRV.SalvarOuAtualizarLote(lote);
                    }
                }
            }
        }

        private void ProcessarRetornoDoLoteNFse(INFeLote lote)
        {
            if (lote != null)
            {
                try
                {
                   using (var scope = new TransactionScope())
                    {

                        var retorno = ChecarSituacaoDoLote(lote);
                        LoteSRV.DefaultPath = defaultPath;
                        LoteSRV.ProcessarRetornoLoteNFse(retorno, lote);
                        scope.Complete();
                    }
                    ExecutarCallBacksLoteItemNFse(lote);

                }
                catch (RetornoProcessamentoException e)
                {
                    if (lote != null)
                    {
                        lote.CodRetornoProcessamento = e.CodRetorno;
                        lote.MsgErroSistema = e.MensagemRetorno;
                        lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                        LoteSRV.SalvarOuAtualizarLote(lote);
                    }
                }
            }
        }

        private void InserirXmlNotaNoLoteItem(ICollection<INFeLoteItem> lstLoteItem, XmlDocument xmlNFe)
        {
            if (lstLoteItem != null && xmlNFe != null)
            {
                var xmlNotas = xmlNFe.GetElementsByTagName("NFe");
                if (xmlNotas != null && xmlNotas.Count > 0)
                {
                    foreach (XmlNode node in xmlNotas)
                    {
                        var NFe = XmlUtil.LoadFromXMLDocument<NotaFiscal>(node);
                        if (NFe != null &&
                            NFe.lstInfNFe != null &&
                            NFe.lstInfNFe.Count > 0)
                        {
                            var chaveNota = NFe.lstInfNFe[0].Id;

                            if (!string.IsNullOrWhiteSpace(chaveNota))
                            {
                                chaveNota = chaveNota.Replace("NFe", "");
                                var loteItem = lstLoteItem
                                    .Where(x => x.ChaveNota == chaveNota)
                                    .FirstOrDefault();
                                var fileName = string.Format(@"\{0}-procNFe.xml", chaveNota);
                                var nfePath = SysUtils.RetornarPathNFeXML();
                                var path = defaultPath + nfePath;
                                var filePath = path + fileName;

                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);

                                var xmlBinary = XmlUtil.SerializeAsXmlBinary(node, filePath, false);
                                loteItem.BinarioNFeXml = xmlBinary;
                                loteItem.PathArquivoNFeXml = fileName;
                            }
                        }
                    }
                }
            }
        }

        public void ExecutarTarefaEnviarLoteVigente(BatchContext batchContext = null)
        {
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 10;
                    //ServiceFactory.RetornarServico<JobAgendamentoSRV>().MarcarInicioExecucao(8);
                }

                var lstLotes = LoteSRV.ListarLotesPendentesDeEnvio();

                if (lstLotes != null && lstLotes.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Transmitindo a nota...", true, lstLotes.Count);
                    foreach (var lote in lstLotes)
                    {
                        try
                        {
                            LoteSRV.PreecherLoteItem(lote);
                            //if (lote.Itens.Count >= 10 || lote.EnvioImediato == true || (DateTime.Now.Hour == 17))
                            //{
                            using (var scope = new TransactionScope())
                            {
                                EnviarLote(lote);
                                scope.Complete();
                            }
                            //}
                            batchContext.IncrementarPassoBatch();
                            batchContext.AdicionarContagemSucesso();
                        }
                        catch (Exception e)
                        {
                            var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                            if (lote != null)
                            {
                                lote.MsgErroSistema = mensagem;
                                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                                LoteSRV.SalvarOuAtualizarLote(lote);
                                batchContext.AdicionarContagemFalha();
                            }
                        }

                        batchContext.IniciarPassoBatch("Notas Transmitidas... Aguardando Processamento do Retorno", false);
                    }
                }


            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao importar os suspects.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Envio de Lote de NFe",
                    projeto = "CORPORATIVO",
                    servico = "ImportacaoSRV",
                    tipoJob = 7,
                    descricaoCodigoReferencia = "Não existe",
                    codReferencia = 0,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,

                });

            }
            finally
            {
                //_jobAgendamento.MarcarFimExecucao(8);
            }
        }

        public void ExecutarTarefaProcessarRetornoLote(BatchContext batchContext = null)
        {
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 11;
                }

                var lstLotes = LoteSRV.ListarLotesPendentesDeProcessamento();

                if (lstLotes != null && lstLotes.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Processar o retorno da Nota Fiscal...", true, lstLotes.Count);
                    foreach (var lote in lstLotes)
                    {
                        try
                        {
                            LoteSRV.PreecherLoteItem(lote);
                            ProcessarRetornoDoLote(lote);
                            batchContext.IncrementarPassoBatch();
                            batchContext.AdicionarContagemSucesso();
                        }
                        catch (Exception e)
                        {
                            var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                            if (lote != null)
                            {
                                lote.MsgErroSistema = mensagem;
                                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                                LoteSRV.SalvarOuAtualizarLote(lote);
                                batchContext.AdicionarContagemFalha();
                            }
                        }

                    }
                }


            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao importar os suspects.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Envio de Lote de NFe",
                    projeto = "CORPORATIVO",
                    servico = "ImportacaoSRV",
                    tipoJob = 7,
                    descricaoCodigoReferencia = "Não existe",
                    codReferencia = 0,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,

                });

            }
            finally
            {
                //_jobAgendamento.MarcarFimExecucao(8);
            }
        }


        public void ExecutarTarefaProcessarRetornoLoteNfse(BatchContext batchContext = null)
        {
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 13;
                }

                var lstLotes = LoteSRV.ListarLotesNFsePendentesDeProcessamento();

                if (lstLotes != null && lstLotes.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Processar o retorno da Nota Fiscal...", true, lstLotes.Count);
                    foreach (var lote in lstLotes)
                    {
                        try
                        {
                            LoteSRV.PreecherLoteItem(lote);
                            ProcessarRetornoDoLoteNFse(lote);
                            batchContext.IncrementarPassoBatch();
                            batchContext.AdicionarContagemSucesso();
                        }
                        catch (Exception e)
                        {
                            var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                            if (lote != null)
                            {
                                lote.MsgErroSistema = mensagem;
                                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                                LoteSRV.SalvarOuAtualizarLote(lote);
                                batchContext.AdicionarContagemFalha();
                            }
                        }

                    }
                }


            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao importar os suspects.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Envio de Lote de NFe",
                    projeto = "CORPORATIVO",
                    servico = "ImportacaoSRV",
                    tipoJob = 7,
                    descricaoCodigoReferencia = "Não existe",
                    codReferencia = 0,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,

                });

            }
            finally
            {
                //_jobAgendamento.MarcarFimExecucao(8);
            }
        }

        public void InutilizarNFe(INFeLoteItem item)
        {
            try
            {
                if (item != null)
                {
                    var certificado = CertificateUtil.RetornarCertificado(item.Lote.EmpresaID);
                    if (item.BinarioNFeXml == null || item.BinarioNFeXml.Count() <= 0)
                    {
                        throw new Exception("Não é possível encontrar o binário da NFe");
                    }
                    var binarioXml = item.BinarioNFeXml;
                    int? numero = (item != null && item.NumeroNota != null) ? item.NumeroNota : new Random().Next(1000) * -1;
                    string path = @"C:\saidaNfe";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    path += string.Format(@"\nfeInuti({0})", numero);
                    File.WriteAllBytes(path, item.BinarioNFeXml);
                    var content = File.ReadAllText(path);
                    File.Delete(path);

                    var nfeDTO = XmlUtil.LoadFromXMLString<NotaFiscal>(content);

                    if (nfeDTO != null && nfeDTO.lstInfNFe != null && nfeDTO.lstInfNFe.Count > 0)
                    {
                        var infNFe = nfeDTO.lstInfNFe[0];
                        var ide = infNFe.Identificacao;
                        var emi = infNFe.Emitente;
                        var serie = StringUtil.PreencherZeroEsquerda((int)ide.Serie, 3);
                        var numeroStr = StringUtil.PreencherZeroEsquerda((int)numero, 9);
                        var codUF = StringUtil.PreencherZeroEsquerda((int)ide.CodigoUFEmitente, 2);
                        var ano = ide.DataDeEmissao.ToString("yy");

                        var id = string.Format("ID{0}{1}{2}{3}{4}{5}{6}", codUF, (object)ano, emi.CNPJ, (object)ide.ModeloDocumentoFiscal, serie, numeroStr, numeroStr);

                        var reqInutilizacao = new RequisicaoInutilizacao()
                        {
                            versao = ConstantsNFe.VERSAO,
                            infInut = new InformacoesInutilizacao()
                            {
                                Id = id,
                                ano = ano,
                                CNPJ = emi.CNPJ,
                                cUF = codUF,
                                mod = ide.ModeloDocumentoFiscal,
                                serie = ide.Serie,
                                tpAmb = ide.TipoAmbiente,
                                xServ = "INUTILIZAR",
                                xJust = "A Nota Fiscal foi Rejeitada pelo Sistema do SEFAZ. Por isso, a nota teve seu número inutilizado.",
                                nNFIni = numero,
                                nNFFin = numero,
                            }
                        };

                        var clienteInutilizacao = ServiceFactory.RetornarServico<ClienteNfeInutilizacaoSRV>();
                        var retorno = clienteInutilizacao.InutilizarNFe(reqInutilizacao, certificado);
                        ProcessarRetornoInutilizacao(retorno, item);
                    }

                }
            }
            catch (Exception e)
            {
                int? numero = (item != null && item.NumeroNota != null) ? item.NumeroNota : null;
                throw new Exception(string.Format("Não é possível initilizar a nota número {0}.", numero), e);
            }
        }

        public void ProcessarRetornoInutilizacao(InutilizacaoRetorno retorno, INFeLoteItem item)
        {
            if (retorno != null)
            {

                if (retorno.infInut.cStat == 102)
                {
                    item.Status = StatusLoteItemEnum.REJEITADA_E_INUTILIZADA;
                    item.NumeroProtocolo = retorno.infInut.nProt;
                    item.DataAutorizacaoRejeicao = retorno.infInut.dhRecbto;
                }
                else
                {
                    string mensagem = $"Não é possível rejeitar a nota. Código de retorno {retorno.infInut.cStat}. Mensagem de Retorno. {retorno.infInut.xMotivo}";
                    item.MsgErroSistema = mensagem;
                }
            }
        }


        public ProcessamentoEventoRetorno EnviarEvento(RequisicaoNovoLote requisicaoCriacao)
        {
            var lote = LoteSRV.CriarNovoLote(requisicaoCriacao);
            if (lote != null)
            {
                lote.TipoLote = TipoLoteEnum.ENVIO_EVENTO;
            }
            var retorno = LoteSRV.EnviarEvento(lote);

            return retorno;
        }


        public void CancelarNotaFiscal(int? nfID)
        {
            if (nfID != null)
            {
                var nota = LoteSRV.RetornarNotaFiscal(nfID);

                if (nota != null)
                {
                    if(nota.Tipo == TipoNFEnum.SAIDA)
                        CancelarNotaFiscal(new List<int>() { nfID.Value });
                    if (nota.Tipo == TipoNFEnum.SAIDA_SERVICO)
                        CancelarNotaFiscalServico(new List<int>() { nfID.Value });
                }
            }
        }
        

        public void CancelarNotaFiscal(ICollection<int> lstNotasFiscais)
        {
            ICollection<ErroReportItemDTO> lstErros = new HashSet<ErroReportItemDTO>();
            using (var scope = new TransactionScope())
            {
                foreach (var codNf in lstNotasFiscais)
                {
                    try
                    {
                        var notaFiscal = LoteSRV.RetornarNotaFiscal(codNf);

                        if (notaFiscal == null)
                        {
                            throw new Exception(string.Format("A nota de código {0} não pode ser encontrada", codNf));
                        }

                        if(notaFiscal.Tipo != TipoNFEnum.ENTRADA && notaFiscal.Tipo != TipoNFEnum.SAIDA)
                        {
                            throw new Exception($"Não é possível cancelar a nfe. A nota {codNf} é uma nota de serviço");
                        }

                        if (string.IsNullOrWhiteSpace(notaFiscal.ProtocoloAutorizacao))
                        {
                            throw new Exception("A nota não possui o protocolo de autorização cadastrado.");
                        }

                        RequisicaoNovoLote requisicaoCriacao = new RequisicaoNovoLote()
                        {
                            TipoLote = TipoLoteEnum.ENVIO_EVENTO,
                            EmpresaID = notaFiscal.CodEmpresa,

                        };

                        requisicaoCriacao.LstRequisicoes.Add(new RequisicaoNovoLoteItem()
                        {
                            ChaveNotaFiscal = notaFiscal.ChaveNota,
                            NumeroProtocolo = notaFiscal.ProtocoloAutorizacao,
                            CodNotaFiscal = notaFiscal.CodNotaFiscal,
                            Tipo = TipoLoteItemEnum.CANCELAMENTO
                        });

                        if (requisicaoCriacao.LstRequisicoes.Count > 0)
                        {
                            var retorno = EnviarEvento(requisicaoCriacao);
                            if (retorno != null && retorno.BatchContext != null && retorno.BatchContext.ListErros != null)
                                lstErros = lstErros.Concat(retorno.BatchContext.ListErros).ToList();
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Não é possível cancelar a nota de código {0}.", codNf), e);
                    }

                }
                scope.Complete();
            }

            if (lstErros != null && lstErros.Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                foreach (var msg in lstErros)
                {
                    sb.Append(" \n\r");
                    sb.Append(msg.Mensagem);
                    sb.Append(" \n\r");
                }
                throw new Exception(string.Format("Uma ou mais notas não foram canceladas com êxito: {0}", sb.ToString()));
            }

        }

        public void CancelarNotaFiscalServico(ICollection<int> lstNotasFiscais)
        {
            ICollection<ErroReportItemDTO> lstErros = new HashSet<ErroReportItemDTO>();
            ICollection<INotaFiscal> lstNotas = new HashSet<INotaFiscal>();

            using (var scope = new TransactionScope())
            {
                foreach (var codNf in lstNotasFiscais)
                {
                    try
                    {
                        var notaFiscal = LoteSRV.RetornarNotaFiscal(codNf);

                        if (notaFiscal == null)
                        {
                            throw new Exception(string.Format("A nota de código {0} não pode ser encontrada", codNf));
                        }
                        if (string.IsNullOrWhiteSpace(notaFiscal.NomeArquivo))
                        {
                            throw new Exception("A nota não possui o protocolo de autorização cadastrado.");
                        }

                        if (notaFiscal.Tipo != TipoNFEnum.ENTRADA_SERVICO && notaFiscal.Tipo != TipoNFEnum.SAIDA_SERVICO)
                        {
                            throw new Exception($"Não é possível cancelar a Nfse. A nota {codNf} é uma nota de produto.");
                        }

                        var empresa = LoteSRV.RetornarEmpresa(notaFiscal.CodEmpresa);
                        var compNfse = XmlUtil.LoadFromXMLBytes<CompNfse>(notaFiscal.Arquivo);

                        if (empresa != null &&
                            compNfse != null && 
                            compNfse.Nfse.InfNfse != null &&
                            compNfse.Nfse.InfNfse.IdentificacaoRps != null)
                        {

                            var numero = compNfse.Nfse.InfNfse.Numero;
                            var cnpj = compNfse.Nfse.InfNfse.PrestadorServico.IdentificacaoPrestador.Cnpj;
                            var IM = compNfse.Nfse.InfNfse.PrestadorServico.IdentificacaoPrestador.InscricaoMunicipal;
                            var codMun = empresa.CodIBGE;

                            var id = $"CAN{numero}{cnpj}{IM}";
                            CancelarNfseEnvio canc = new CancelarNfseEnvio()
                            {
                                Pedido = new PedidoCancelamentoNfse()
                                {
                                    
                                    InfPedidoCancelamento = new InfPedidoCancelamentoNfse()
                                    {   Id = id,
                                        CodigoCancelamento = "00",
                                        IdentificacaoNfse = new IdentificacaoNfse
                                        {
                                            Numero = numero,
                                            Cnpj = cnpj,
                                            InscricaoMunicipal = IM,
                                            CodigoMunicipio = codMun
                                        }
                                    },
                                }
                            };
                            var certificado = CertificateUtil.RetornarCertificado(notaFiscal.CodEmpresa);

                            var srvNFse = ServiceFactory.RetornarServico<ClienteNfseSRV>();
                            var respCanc = srvNFse.CancelarNotaFiscalServico(canc, certificado);

                            if(respCanc != null && respCanc.Cancelamento != null)
                            {
                                compNfse.NfseCancelamento = respCanc.Cancelamento;
                                var bytes = XmlUtil.SerializeAsXmlBinary(compNfse, true, "xmlCan.xml");
                                notaFiscal.Arquivo = bytes;
                                notaFiscal.Status = StatusNotaFiscalEnum.CANCELADA;
                                lstNotas.Add(notaFiscal);

                                File.WriteAllBytes(notaFiscal.NomeArquivo, notaFiscal.Arquivo);

                                var ltItem = LoteSRV.LoteItemNFeSRV.RetornarLoteItemDaNotaAutorizada(notaFiscal.CodNotaFiscal);
                                ltItem.BinarioNFeXml = bytes;
                                LoteSRV.LoteItemNFeSRV.SalvarOuAtualizarLoteItem(ltItem);
                            }
                            else
                            {
                                if(respCanc.ListaMensagemRetorno != null)
                                {
                                    foreach(var msg in respCanc.ListaMensagemRetorno.MensagemRetorno)
                                    {
                                        lstErros.Add(new ErroReportItemDTO()
                                        {
                                            Mensagem = $"Código Retorno: {msg.Codigo} Mensagem: {msg.Mensagem} Correção: {msg.Correcao}"
                                        });
                                    }
                                }
                            }
                        }
                        
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Não é possível cancelar a nota de código {0}.", codNf), e);
                    }

                }
                LoteSRV.SalvarNotas(lstNotas);
                scope.Complete();
            }

            if(lstNotas != null && lstNotas.Count > 0)
            {
                LoteSRV.EnviarEmailCancelamentoDeServico(lstNotas);
            }            

            if (lstErros != null && lstErros.Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                foreach (var msg in lstErros)
                {
                    sb.Append(" \n\r");
                    sb.Append(msg.Mensagem);
                    sb.Append(" \n\r");
                }
                throw new Exception(string.Format("Uma ou mais notas não foram canceladas com êxito: {0}", sb.ToString()));
            }
        }

        public void GerarDevolucao(int? codNota)
        {
            var nota = LoteSRV.RetornarNotaFiscal(codNota);

            if (nota.Status == StatusNotaFiscalEnum.DEVOLVIDA)
                throw new Exception("A nota fiscal já foi devolvida.");

            if (nota.Status == StatusNotaFiscalEnum.CANCELADA)
                throw new Exception("Não é possível enviar uma nota cancelada.");
            GerarDevolucao(new List<int>() { codNota.Value }, nota.CodEmpresa);
        }

        public void GerarDevolucao(ICollection<int> lstCodNota, int? codEmpresa)
        {
            if (lstCodNota != null)
            {
                RequisicaoNovoLote novoLote = new RequisicaoNovoLote()
                {
                    EmpresaID = codEmpresa,
                    TipoLote = TipoLoteEnum.ENVIO_LOTE_NFE
                };

                var lstRequisicoes = new List<RequisicaoNovoLoteItem>();

                foreach (var cod in lstCodNota)
                {
                    var nota = LoteSRV.RetornarNotaFiscal(cod);

                    if (nota.Status != StatusNotaFiscalEnum.DEVOLVIDA && nota.Status != StatusNotaFiscalEnum.CANCELADA)
                    {
                        lstRequisicoes.Add(new RequisicaoNovoLoteItem()
                        {
                            CodNotaFiscal = cod,
                            CodPedido = nota.CodPedido,
                            CodContrato = nota.CodContrato,
                            CodCliente = nota.CodCliente,
                            CodEmpresa = nota.CodEmpresa,
                            Tipo = TipoLoteItemEnum.DEVOLUCAO,
                            CodNotaReferencia = new List<int>()
                            {
                                cod
                            },
                        });
                    }
                }
                if (lstRequisicoes != null && lstRequisicoes.Count > 0)
                {
                    novoLote.LstRequisicoes = lstRequisicoes;
                    CriarNovoLote(novoLote);
                }
            }

        }

        public void GerarCartaDeCorrecao(CartaCorrecaoRequestDTO cartaCorrecaoRequest)
        {

            if (cartaCorrecaoRequest == null)
                throw new ArgumentNullException("Informe o parâmetro cartaCorrecaoRequest para poder enviar a carta de correção");

            var valiResult = ValidatorProxy.RecursiveValidate(cartaCorrecaoRequest);

            if (!valiResult.IsValid)
                throw new ValidacaoException("Não é possível enviar a carta de correção", valiResult);

            ICollection<ErroReportItemDTO> lstErros = null;
            using (var scope = new TransactionScope())
            {
                var notaFiscal = LoteSRV.RetornarNotaFiscal(cartaCorrecaoRequest.NotaFiscalID);

                if (notaFiscal == null)
                {
                    throw new Exception(string.Format("A nota de código {0} não pode ser encontrada", cartaCorrecaoRequest.NotaFiscalID));
                }
                if (string.IsNullOrWhiteSpace(notaFiscal.ProtocoloAutorizacao))
                {
                    throw new Exception("A nota não possui o protocolo de autorização cadastrado.");
                }

                RequisicaoNovoLote requisicaoCriacao = new RequisicaoNovoLote()
                {
                    TipoLote = TipoLoteEnum.ENVIO_EVENTO,
                    EmpresaID = notaFiscal.CodEmpresa,

                };

                cartaCorrecaoRequest.CartaCorrecao = new Regex("\n").Replace(cartaCorrecaoRequest.CartaCorrecao, @"\n");
                cartaCorrecaoRequest.CartaCorrecao = new Regex("\t").Replace(cartaCorrecaoRequest.CartaCorrecao, @"\t");
                cartaCorrecaoRequest.CartaCorrecao = new Regex("\r").Replace(cartaCorrecaoRequest.CartaCorrecao, @"\r");

                requisicaoCriacao.LstRequisicoes.Add(new RequisicaoNovoLoteItem()
                {
                    ChaveNotaFiscal = notaFiscal.ChaveNota,
                    NumeroProtocolo = notaFiscal.ProtocoloAutorizacao,
                    CodNotaFiscal = notaFiscal.CodNotaFiscal,
                    CartaCorrecao = cartaCorrecaoRequest.CartaCorrecao,
                    Tipo = TipoLoteItemEnum.CARTA_CORRECAO
                });

                if (requisicaoCriacao.LstRequisicoes.Count > 0)
                {
                    var retorno = EnviarEvento(requisicaoCriacao);
                    lstErros = retorno.BatchContext.ListErros;
                }
                scope.Complete();
            }

            if (lstErros != null && lstErros.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var msg in lstErros)
                {
                    sb.Append(" \n\r");
                    sb.Append(msg.Mensagem);
                    sb.Append(" \n\r");
                }
                throw new Exception(string.Format("Uma ou mais notas não adicionaram a carta de correção com êxito: {0}", sb.ToString()));
            }
        }

        public void MarcarLoteComErrosParaReenvio(int? loteID)
        {
            if (loteID != null)
            {
                var lote = LoteSRV.RetornarLote(loteID);

                if (lote.Status == StatusLoteEnum.ERRO_AO_PROCESSAR)
                {
                    if (lote.CodRetorno == 103 && !string.IsNullOrWhiteSpace(lote.CodRecibo))
                    {
                        lote.Status = StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO;
                    }
                    else
                    if (lote.CodRetorno == null)
                    {

                    }

                    LoteSRV.SalvarOuAtualizarLote(lote);
                }
                else
                {
                    throw new Exception("O lote não está marcado com erro.");
                }
            }
        }

        public List<InfCadastroRetornoDados> ConsultarCadastro(string cpfCnpj, string ie, bool usarCPF = false)
        {
            string cpf = null;
            string cnpj = null;

            if (usarCPF)
            {
                cpf = cpfCnpj;
            }
            else
            {
                cnpj = cpfCnpj;
            }

            var requisicao = new RequisicaoConsultaCadastro()
            {

                versao = "2.00",
                InfoConsulta = new InformacaoConsulta()
                {
                    CPF = cpf,
                    CNPJ = cnpj,
                    IE = ie,
                    xServ = "CONS-CAD",
                    UF = "RJ"
                }
            };
            var clientConsulta = ServiceFactory.RetornarServico<ClienteConsultaCadastroSRV>();
            var certificado = CertificateUtil.RetornarCertificado(2);

            var resposta = clientConsulta.ConsultarCadastro(requisicao, certificado);

            if (resposta.InfCadRetorno.CodigoRetorno == 111 ||
                resposta.InfCadRetorno.CodigoRetorno == 112)
            {
                if (resposta.InfCadRetorno != null)
                {
                    return resposta.InfCadRetorno.lstInfCadRetDados;
                }
            }

            return new List<InfCadastroRetornoDados>();
        }

        public Rps GerarNfseDTO(PedidoDTO pedido, IList<ParcelaDTO> parcelas = null)
        {
            if (pedido == null)
            {
                throw new ArgumentNullException("O Pedido não pode ser nulo");
            }

            if (pedido.Empresa == null)
            {
                throw new ArgumentNullException("A Empresa não pode ser nulo");
            }

            if (pedido.DataFaturamento == null)
            {
                string mensagem = "Não é possível gerar o xml da nota fiscal. Não é possível encontrar a data de faturamento no Item de Pedido {0}";
                mensagem = string.Format(mensagem, pedido.CodPedido);
                throw new Exception(mensagem);
            }

            var validacao = ValidatorProxy.RecursiveValidate(pedido);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre um problema ao gerar a nota fiscal.", validacao);
            }

            var numeroNfse = pedido.Empresa.SequencialNFSe;
            Random rand = new Random();
            DateTime dataEntradaSaida = DateTime.Now;
            List<NFeReferenciadaDTO> lstReferencias = new List<NFeReferenciadaDTO>();

            var numeroAleatorio = rand.Next(100, 99999999);
            var codigoNf = StringUtil.PreencherZeroEsquerda(numeroAleatorio, 8);
            var codigoDoPedido = pedido.CodPedido;
            int? codigoUFEmitente = null;
            int? codigoUFDestino = null;

            Rps nfse = new Rps()
            {
                InfRps = new InfRps()
                {
                    Id = $"R{numeroNfse}",
                    IdentificacaoRps = new IdentificacaoRps()
                    {
                        Numero = numeroNfse,
                        Serie = "ABC",
                        Tipo = TipoRPSEnum.RPS
                    },
                    DataEmissaoDateTime = DateTime.Now,
                    NaturezaOperacao = TipoNaturezaOperacaoEnum.TRIBUTACAO_NO_MUNICIPIO,
                    OptanteSimplesNacional = IdenSimNaoEnum.NAO,
                    IncentivadorCultural = IdenSimNaoEnum.NAO,
                    Status = StatusRpsEnum.NORMAL,

                }
            };         
            
            var prestador = GerarDadosDoPrestadorNfse(pedido.Empresa);
            var tomador = GerarDadosDoTomadorNfse(pedido.Cliente);

            if (prestador != null)
            {
                if (pedido.Empresa.Endereco != null &&
                    pedido.Empresa.Endereco.UF != null &&
                    pedido.Empresa.Endereco.UF.CodigoUF == null)
                    throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal de serviço. A empresa {0} não possui UF cadastrada.", pedido.Empresa.NomeFantasia));
                codigoUFEmitente = pedido.Empresa.Endereco.UF.CodigoUF.Value;
            }

            if (tomador != null)
            {
                if (pedido.Cliente.Endereco != null &&
                    pedido.Cliente.Endereco.UF != null &&
                    pedido.Cliente.Endereco.UF.CodigoUF == null)
                    throw new GeracaoNotaException(string.Format("Não é possível gerar a nota fiscal de serviço. O endereço do cliente {0} não possui UF cadastrada.", pedido.Cliente.Nome));
                codigoUFDestino = pedido.Cliente.Endereco.UF.CodigoUF.Value;
            }

            nfse.InfRps.Prestador = prestador.IdentificacaoPrestador;
            nfse.InfRps.Tomador = tomador;

            var servicos = GerarDadosServicoNfse(pedido, parcelas);
            nfse.InfRps.Servico = servicos;

            var result = ValidatorProxy.RecursiveValidate(nfse);

            if (validacao.IsValid)
            {
                return nfse;
            }
            else
            {
                throw new ValidacaoException("Ocorre um problema ao gerar o xml da nota fiscal.", validacao);
            }
        }

        public EnviarLoteRpsEnvio GerarNotaDeServico(int? CodItemPedido, string contrato)
        {
            var pedido = LoteSRV.RetornarPedido(CodItemPedido, contrato);
            var nfse = GerarNfseDTO(pedido);

            EnviarLoteRpsEnvio enviarLote = new EnviarLoteRpsEnvio()
            {
                LoteRps = new LoteRps()
                {
                    Cnpj = pedido.Empresa.CNPJ,
                    InscricaoMunicipal = pedido.Empresa.IM,
                    NumeroLote = 5,
                    QuantidadeRps = 1,
                    Id = "R5",
                    ListaRps = new List<Rps>()
                    {
                        nfse
                    }
                }
            };
            return enviarLote;
        }


        public void ExecutarTarefaEnviarLoteVigenteNfSe(BatchContext batchContext = null)
        {
            try
            {
                if (batchContext == null)
                {
                    batchContext = new BatchContext();
                    batchContext.JobID = 12;
                    //ServiceFactory.RetornarServico<JobAgendamentoSRV>().MarcarInicioExecucao(8);
                }

                var lstLotes = LoteSRV.ListarLotesNFsePendentesDeEnvio();

                if (lstLotes != null && lstLotes.Count > 0)
                {
                    batchContext.IniciarPassoBatch("Transmitindo a nota...", true, lstLotes.Count);
                    foreach (var lote in lstLotes)
                    {
                        try
                        {
                            LoteSRV.PreecherLoteItem(lote);
                            //if (lote.Itens.Count >= 10 || lote.EnvioImediato == true || (DateTime.Now.Hour == 17))
                            //{
                            using (var scope = new TransactionScope())
                            {
                                EnviarLoteNfse(lote);
                                scope.Complete();
                            }
                            //}
                            batchContext.IncrementarPassoBatch();
                            batchContext.AdicionarContagemSucesso();
                        }
                        catch (Exception e)
                        {
                            var mensagem = ExceptionFormatter.RecursiveFindExceptionsMessage(e);
                            if (lote != null)
                            {
                                lote.MsgErroSistema = mensagem;
                                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                                LoteSRV.SalvarOuAtualizarLote(lote);
                                batchContext.AdicionarContagemFalha();
                            }
                        }

                        batchContext.IniciarPassoBatch("Notas Transmitidas... Aguardando Processamento do Retorno", false);
                    }
                }


            }
            catch (Exception e)
            {
                string chaveErro = string.Format("Erro ao importar os suspects.");

                ServiceFactory.RetornarServico<BatchCustomSRV>().RegistrarNotificacaoErroBatch(new RegistroErroBatchDTO()
                {
                    batchEx = batchContext,
                    context = chaveErro,
                    e = e,
                    nomeDaExecucao = "Envio de Lote de NFe",
                    projeto = "CORPORATIVO",
                    servico = "ImportacaoSRV",
                    tipoJob = 7,
                    descricaoCodigoReferencia = "Não existe",
                    codReferencia = 0,
                    contabilizarFalha = false,
                    qtdOcorrenciaEnvioEmail = 60,

                });

            }
            finally
            {
                //_jobAgendamento.MarcarFimExecucao(8);
            }
        }

    }
}

