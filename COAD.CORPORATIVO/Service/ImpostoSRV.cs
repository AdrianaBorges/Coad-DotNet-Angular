
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
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Models.Comparators;
using COAD.CORPORATIVO.Util;
using GenericCrud.Service;
using GenericCrud.Util;
using COAD.CORPORATIVO.Model.Dto.Custons.Impostos;
using COAD.FISCAL.Model.Enumerados;
using COAD.CORPORATIVO.Exceptions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IMP_ID")]
    public class ImpostoSRV : GenericService<IMPOSTO, ImpostoDTO, int>
    {
        private ImpostoDAO _dao { get; set; }
        public const decimal Porcentagem = 70m;
        public ConfigImpostoSRV _configImpostoSRV { get; set; }
        public NotaFiscalConfigSRV _notaFiscalConfigSRV { get; set; }
        public ClienteSRV _clienteSRV { get; set; }

        public ImpostoSRV()
        {
            this.Dao = new ImpostoDAO();
            this._configImpostoSRV = new ConfigImpostoSRV();
        }

        public ImpostoSRV(ImpostoDAO _dao)
        {
            this.Dao = _dao;
            this._dao = _dao;
            this._configImpostoSRV = new ConfigImpostoSRV();
        }

        /// <summary>
        /// Devolve a lista de impostos que de acordo com as configurações
        /// </summary>
        /// <returns></returns>
        public IList<ConfigImpostoDTO> ObterImpostos(
                RequisicaoConfigImpostoDTO requisicao)
        {
            var listConfigImposto = _configImpostoSRV.ObterConfiguracaoPorRegras(requisicao);
            return listConfigImposto;
        }


        public ResultadoCalculoImpostoDTO CalcularDescontosImposto(
                PropostaItemDTO propostaItem,
                PropostaDTO proposta)
        {
            ResultadoCalculoImpostoDTO resultado = null;
            if (propostaItem != null && propostaItem.CMP_ID != null)
            {

                if(proposta != null && proposta != null)
                {
                    var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(proposta.CLI_ID);

                    if (cliente != null 
                        //&&
                        //cliente.TIPO_CLI_ID != 2
                        )
                    {

                        var valorBruto = (propostaItem.PPI_VALOR_UNITARIO * propostaItem.PPI_QTD);
                        bool? calcularSobreTotal = null;
                        bool? arredondarParaBaixo = false;// (propostaItem.PPI_VALOR_BRUTO_ENTRADA == propostaItem.PPI_VALOR_BRUTO_PARCELA);


                        resultado = new ResultadoCalculoImpostoDTO();
                        var cmpId = propostaItem.CMP_ID;

                        if (propostaItem.PPI_VALOR_BRUTO_ENTRADA != null &&
                            propostaItem.PPI_VALOR_BRUTO_ENTRADA > 0)
                        {
                            calcularSobreTotal = false;
                            var calculoSobreTotal = CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = (proposta.PRT_POR_CENTO_FATURADO.HasValue) ? proposta.PRT_POR_CENTO_FATURADO.Value : false,
                                empresaDoSimples = (proposta.PRT_EMPRESA_DO_SIMPLES.HasValue) ? proposta.PRT_EMPRESA_DO_SIMPLES.Value : false,
                                CmpID = propostaItem.CmpId,
                                qtd = propostaItem.PPI_QTD,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = valorBruto,
                                sobreTotal = true
                            });


                            var infoFaturaEntrada = CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = (proposta.PRT_POR_CENTO_FATURADO.HasValue) ? proposta.PRT_POR_CENTO_FATURADO.Value : false,
                                empresaDoSimples = (proposta.PRT_EMPRESA_DO_SIMPLES.HasValue) ? proposta.PRT_EMPRESA_DO_SIMPLES.Value : false,
                                CmpID = propostaItem.CmpId,
                                qtd = propostaItem.PPI_QTD,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = propostaItem.PPI_VALOR_ENTRADA,
                                ImpostosParaSomar = calculoSobreTotal,
                                sobreTotal = false,
                                qtdParcelas = 1,
                                arredondarParaBaixo = arredondarParaBaixo
                            });
                            resultado.ResultadoEntrada = infoFaturaEntrada;
                        }

                        var infoFatura = CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                        {
                            cemPorCentoFaturado = (proposta.PRT_POR_CENTO_FATURADO.HasValue) ? proposta.PRT_POR_CENTO_FATURADO.Value : false,
                            empresaDoSimples = (proposta.PRT_EMPRESA_DO_SIMPLES.HasValue) ? proposta.PRT_EMPRESA_DO_SIMPLES.Value : false,
                            CmpID = propostaItem.CmpId,
                            qtd = propostaItem.PPI_QTD,
                            tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                            valorUnitario = propostaItem.PPI_VALOR_PARCELA,
                            sobreTotal = calcularSobreTotal,
                            qtdParcelas = propostaItem.PPI_QTD_PARCELAS
                        });

                        resultado.ResultadoParcela = infoFatura;
                    }

                }
            }
            return resultado;
        }

        public InfoFaturaDTO CalcularDescontosImposto(
               ItemPedidoDTO itemPedido,
               int? tipoCliId,
               int? rgId,
               decimal? valor,
               bool? empresaDoSimples = null,
               bool cemPorCentoFaturado = false)
        {
            if (itemPedido != null && itemPedido.CMP_ID != null)
            {
                var pedido = ServiceFactory.RetornarServico<PedidoCRMSRV>().FindById(itemPedido.PED_CRM_ID);

                if(pedido.CLIENTES.TIPO_CLI_ID != 2)
                {
                    var cmpId = itemPedido.CMP_ID;
                    return CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                    {
                        cemPorCentoFaturado = (pedido.PED_CEM_POR_CENTO_FATURADO.HasValue) ? pedido.PED_CEM_POR_CENTO_FATURADO.Value : false,
                        empresaDoSimples = (pedido.PED_EMPRESA_DO_SIMPLES.HasValue) ? pedido.PED_EMPRESA_DO_SIMPLES.Value : false,
                        CmpID = itemPedido.CmpId,
                        qtd = itemPedido.IPE_QTD,
                        rgId = rgId,
                        tipoCliId = (pedido.CLIENTES != null) ? pedido.CLIENTES.TIPO_CLI_ID : 0,
                        valorUnitario = itemPedido.IPE_VALOR_PRODUTO,
                    });
                }
            }
            return null;
        }

        public InfoFaturaDTO CalcularDescontosImposto(
                CalculoRequestImpostoDTO request)
        {
            //if (request.CmpID != null || request.EhServico == true)
            //{
            //    bool ehCurso = false;
            //    if(request.CmpID != null)
            //        ehCurso = new ProdutoComposicaoSRV().ChecaProdutoComposicaoEhCurso((int) request.CmpID);

            //    if (ehCurso || request.EhServico == true)
            //    {
            //        return _calcularDescontosImposto(request);
            //    }
            //    //else
            //    //{
            //    //    request.cemPorCentoFaturado = true;
            //    //    return _calcularDescontosImposto(request);
            //    //}
            //}
            //return null;
            request.cemPorCentoFaturado = true;
            return _calcularDescontosImposto(request);
        }

        public InfoFaturaDTO _calcularDescontosImposto(CalculoRequestImpostoDTO request)
        {
            var rgId = request.rgId;
            var valor = request.valorUnitario; //* request.qtd;
            var cemPorCentoFaturado = request.cemPorCentoFaturado;
            var tipoCliId = request.tipoCliId;
            var empresaDoSimples = request.empresaDoSimples;
            var cmpId = request.CmpID;
            var qtd = request.qtd;
            var sobreTotal = request.sobreTotal;
            var arredondarParaBaixo = request.arredondarParaBaixo;

            //if (tipoCliId != 2)
            {
                decimal? valorOriginal = valor;
                decimal? totalDescontoPercentual = 0;
                decimal? totalDesconto = 0;
                decimal? valorServico = 0;
                decimal? valorMaterial = 0;
                decimal? valorUnitario = 0;
                decimal? totalLiquido = 0;

                decimal? porcentagemService = 70;
                decimal? porcentagemMaterial = 100 - porcentagemService;

                InfoFaturaDTO calculo = null;


                // Se não é 100% faturado separo o valor de serviço para aplicar os impostos
                if (!cemPorCentoFaturado)
                {
                    valorServico = ((porcentagemService / 100) * valor);
                    valorMaterial = ((porcentagemMaterial / 100) * valor);
                    
                    valorServico = Math.Round((decimal)valorServico, 2);
                    valorMaterial = Math.Round((decimal)valorMaterial, 2);

                    totalLiquido = valorServico;
                }
                else // senão pego o valor total
                {
                    totalLiquido = valor;
                }

                totalLiquido = Math.Round((decimal) totalLiquido, 2);

                var listConfigImpostos = ObterImpostos(new RequisicaoConfigImpostoDTO() {

                    CmpID = cmpId,
                    TipoCliId = tipoCliId,
                    EmpresaDoSimples = empresaDoSimples,
                    SobreTotal = sobreTotal,
                    ClienteRetem = true
                }); //obtenho os impostos de acordo com as regras

                IList<ImpostoInfoFaturaDTO> lstImposto = new List<ImpostoInfoFaturaDTO>();

                if (listConfigImpostos != null && totalLiquido != null) // somo o total de impostos
                {

                    foreach (var configIm in listConfigImpostos)
                    {
                        IList<ImpostoInfoFaturaDTO> lstImpostoProcessado = new List<ImpostoInfoFaturaDTO>();
                        var valorMin = configIm.CFI_VLR_DESCONTO_MIM;
                        decimal? totalDesc = 0.00m;
                        decimal? percentual = 0;

                        foreach (var im in configIm.CONFIG_IMPOSTO_IMPOSTO)
                        {
                            if (im.IMPOSTO.IMP_ALIQUOTA != null)
                            {
                                var aliq = im.IMPOSTO.IMP_ALIQUOTA;
                                var desconto = ((aliq / 100) * totalLiquido);

                                if(arredondarParaBaixo == true)
                                    desconto = MathUtil.TruncarCasasDecimais(desconto, 2);
                                else
                                    desconto = Math.Round((decimal)desconto, 2);

                                lstImpostoProcessado.Add(new ImpostoInfoFaturaDTO() {

                                    DATA_ASSOCIACAO = DateTime.Now,
                                    IMPOSTO = im.IMPOSTO,
                                    IMP_ID = im.IMPOSTO.IMP_ID,
                                    IIF_VALOR_DESCONTO = desconto,
                                    IIF_PERCENTUAL_DESCONTO = aliq
                                });

                                totalDesc += desconto;
                                percentual += aliq;
                            }
                        }

                        if(totalDesc >= valorMin || configIm.CFI_QUALQUER_VALOR == true)
                        {
                            totalDescontoPercentual += percentual;
                            lstImposto = lstImposto.Concat(lstImpostoProcessado).ToList();
                            totalDesconto += totalDesc;
                        }
                    }

                    //totalDesconto = ((totalDescontoPercentual / 100) * totalLiquido); // obtenho o valor de desconto de acordo com a porcentagem total dos impostos
                    //totalDesconto = Math.Round((decimal)totalDesconto, 2);

                    if (request.ImpostosParaSomar != null)
                    {
                        totalDesconto += request.ImpostosParaSomar.IFF_TOTAL_DESCONTADO;
                        totalDescontoPercentual += request.ImpostosParaSomar.IFF_PERCENTUAL_TOTAL_DESCONTADO;

                        if(lstImposto != null && request.ImpostosParaSomar.IMPOSTO_INFO_FATURA != null)
                            lstImposto = lstImposto.Concat(request.ImpostosParaSomar.IMPOSTO_INFO_FATURA.ToList()).ToList();
                    }
                    //totalDesconto = MathUtil.TruncarCasasDecimais(totalDesconto, 2);

                    totalLiquido = (totalLiquido - totalDesconto); // desconto do valor do pedido o valor retido de imposto
                    
                    if (!cemPorCentoFaturado) // se não for 100% faturado o valor do material é reinserido no total junto com o valor do serviço recalculado já presente no total.
                    {
                        totalLiquido += valorMaterial;
                    }
                    
                    calculo = new InfoFaturaDTO()
                    {
                        IFF_PERCENTUAL_TOTAL_DESCONTADO = totalDescontoPercentual,
                        IFF_TOTAL_DESCONTADO = totalDesconto,
                        IFF_TOTAL_LIQUIDO = totalLiquido,
                        IFF_VALOR_BRUTO = valorOriginal,
                        IFF_VALOR_SERVICE = valorServico,
                        IFF_VALOR_MATERIAL = valorMaterial,
                        IFF_QTD_PRODUTO = qtd,
                        IFF_QTD_SERVICO = qtd,
                        IFF_VALOR_UNITARIO = valorUnitario,
                    };
                    
                    calculo.IMPOSTO_INFO_FATURA = lstImposto;
                }

                return calculo;
            }

            //return null;
        }

        public DivisaoServicoDTO DividirMaterialServico(decimal? valorBruto)
        {
            if(valorBruto != null)
            {
                var valorServico = (valorBruto * (Porcentagem / 100));
                var valorMaterial = valorBruto - valorServico;

                return new DivisaoServicoDTO()
                {
                    ValorServico = valorServico,
                    ValorProduto = valorMaterial
                };
            }

            return null;
        }

        public void CalcularDescontosImpostoNotaServico(
                NotaFiscalDTO notaFiscal)
        {
            ResultadoCalculoImpostoDTO resultado = null;
            if (notaFiscal != null)
            {

                if (notaFiscal != null)
                {
                    var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(notaFiscal.CLI_ID);

                    if (cliente != null
                        //&&
                        //cliente.TIPO_CLI_ID != 2
                        )
                    {
                        //if(notaFiscal != null &&
                        //    notaFiscal.NOTA_FISCAL_ITEM != null &&
                        //    notaFiscal.NOTA_FISCAL_ITEM.Count > 0)
                        //{
                        //    var cmpId = notaFiscal.NOTA_FISCAL_ITEM.ToList()[0].PRODUTOS
                        //}

                        var valorBruto = notaFiscal.NF_VLR_SERVICO;
                        bool? calcularSobreTotal = null;

                        resultado = new ResultadoCalculoImpostoDTO();
                        
                            calcularSobreTotal = false;
                            var calculoSobreTotal = CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = true,
                                empresaDoSimples =  false,
                                //CmpID = notaFiscal.CmpId,
                                EhServico = true,
                                qtd = 1,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = valorBruto,
                                sobreTotal = true
                            });


                            var infoFatura = CalcularDescontosImposto(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = true,
                                empresaDoSimples = false,
                                //CmpID = notaFiscal.CmpId,
                                EhServico = true,
                                qtd = 1,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = notaFiscal.NF_VLR_SERVICO,
                                ImpostosParaSomar = calculoSobreTotal,
                                sobreTotal = false,
                                qtdParcelas = 1,
                            });

                        resultado.ResultadoParcela = infoFatura;
                    }

                }
            }
            InserirImpostosNoServico(notaFiscal, resultado);
        }
        
        public void InserirImpostosNoServico(NotaFiscalDTO notaFiscal, ResultadoCalculoImpostoDTO resultado)
        {
            if (notaFiscal != null)
            {
                decimal? cofins = 0.00m;
                decimal? csll = 0.00m;
                decimal? inss = 0.00m;
                decimal? ir = 0.00m;
                decimal? pis = 0.00m;
                decimal? outros = 0.00m;
                decimal? iss = 5;
                iss = Math.Round((decimal)iss);

                if(resultado != null &&
                    resultado.ResultadoParcela != null &&
                    resultado.ResultadoParcela.IMPOSTO_INFO_FATURA != null &&
                    notaFiscal != null)
                {
                    var lstImpostos = resultado.ResultadoParcela.IMPOSTO_INFO_FATURA;

                    foreach (var im in lstImpostos)
                    {
                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.COFINS)
                        {
                            cofins += im.IIF_VALOR_DESCONTO;
                        }

                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.CSLL)
                        {
                            csll += im.IIF_VALOR_DESCONTO;
                        }

                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.INSS)
                        {
                            inss += im.IIF_VALOR_DESCONTO;
                        }

                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.IR)
                        {
                            ir += im.IIF_VALOR_DESCONTO;
                        }

                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.PIS)
                        {
                            pis += im.IIF_VALOR_DESCONTO;
                        }

                        if (im.IMPOSTO.TipoImposto == ImpostosEnum.OUTROS)
                        {
                            outros += im.IIF_VALOR_DESCONTO;
                        }
                    }
                }

                notaFiscal.NF_VLR_COFINS = cofins;
                notaFiscal.NF_VLR_CSLL = csll;
                notaFiscal.NF_VLR_INSS = inss;
                notaFiscal.NF_VLR_IR = ir;
                notaFiscal.NF_VLR_PIS = pis;
                notaFiscal.NF_VLR_OUTRAS = outros;
                notaFiscal.NF_VLR_ISS = iss;
                notaFiscal.NF_VLR_NOTA = resultado.ResultadoParcela.IFF_TOTAL_LIQUIDO;
            }
        }

        private IList<InfoFaturaItemDTO> CalcularInfoFaturaItm(ICollection<NotaFiscalConfigDTO> lstConfigNota, CalculoRequestImpostoDTO request)
        {
            var valor = request.valorUnitario;
            var cemPorCentoFaturado = request.cemPorCentoFaturado;
            var tipoCliId = request.tipoCliId;
            var empresaDoSimples = request.empresaDoSimples;
            var cmpId = request.CmpID;
            var sobreTotal = request.sobreTotal;
            decimal? porcentagemAcumulada = 0;

            IList<InfoFaturaItemDTO> lstCalculo = new List<InfoFaturaItemDTO>();
            if (lstConfigNota != null)
            {
                foreach (var nfConfig in lstConfigNota)
                {
                    bool naoRetevePorRegra = false;
                    decimal? totalDescontoPercentual = 0;
                    decimal? totalDesconto = 0;
                    var porcentagem = (decimal) nfConfig.NFC_PORCENTAGEM_VALOR;

                    if (cemPorCentoFaturado)
                        porcentagem = 100;

                    porcentagemAcumulada += porcentagem;

                    if(porcentagemAcumulada > 100)
                    {
                        throw new ImpostoException("O somatório de porcentagem da configuração de nota fiscal excedeu 100%." +
                                                    $" Verifique as configurações de nota fiscal do produto composição de ID {nfConfig.CMP_ID}");
                    }

                    decimal? totalBruto = ((porcentagem / 100) * valor);
                    var totalLiquido = totalBruto;

                    var listConfigImpostos = ObterImpostos(new RequisicaoConfigImpostoDTO()
                    {
                        CmpID = cmpId,
                        NfcId = nfConfig.NFC_ID,
                        TipoCliId = tipoCliId,
                        EmpresaDoSimples = empresaDoSimples,
                        SobreTotal = sobreTotal,
                        ClienteRetem = true
                    }); //obtenho os impostos de acordo com as regras

                    IList<ImpostoInfoFaturaItemDTO> lstImposto = new List<ImpostoInfoFaturaItemDTO>();

                    if (listConfigImpostos != null && totalLiquido != null) // somo o total de impostos
                    {

                        foreach (var configIm in listConfigImpostos)
                        {
                            IList<ImpostoInfoFaturaItemDTO> lstImpostoProcessado = new List<ImpostoInfoFaturaItemDTO>();
                            var valorMin = configIm.CFI_VLR_DESCONTO_MIM;
                            decimal? totalDesc = 0.00m;
                            decimal? percentual = 0;

                            foreach (var im in configIm.CONFIG_IMPOSTO_IMPOSTO)
                            {
                                if (im.IMPOSTO.IMP_ALIQUOTA != null)
                                {
                                    var aliq = im.CII_ALIQUOTA;
                                    var desconto = ((aliq / 100) * totalLiquido);
                                    desconto = Math.Round((decimal)desconto, 2);

                                    lstImpostoProcessado.Add(new ImpostoInfoFaturaItemDTO()
                                    {

                                        IMPOSTO = im.IMPOSTO,
                                        IMP_ID = im.IMPOSTO.IMP_ID,
                                        IFI_PERCENTUAL_DESCONTO = aliq,
                                        IFI_VALOR_DESCONTADO = desconto
                                    });

                                    totalDesc += desconto;
                                    percentual += aliq;
                                }
                            }

                            if (totalDesc >= valorMin || configIm.CFI_QUALQUER_VALOR == true)
                            {
                                totalDescontoPercentual += percentual;
                                lstImposto = lstImposto.Concat(lstImpostoProcessado).ToList();
                                totalDesconto += totalDesc;
                            }
                            else
                            {
                                naoRetevePorRegra = true;
                            }
                        }

                        if (request.ImpostosParaSomar != null)
                        {
                            var impostosItmParaSomar = RetornarFaturaItem(request.ImpostosParaSomar, porcentagem);

                            totalDesconto += impostosItmParaSomar.IFI_TOTAL_DESCONTADO;
                            totalDescontoPercentual += impostosItmParaSomar.IFI_PERCENTUAL_TOTAL_DESCONTADO;

                            if (lstImposto != null && impostosItmParaSomar.IMPOSTO_INFO_FATURA_ITEM != null)
                                lstImposto = lstImposto.Concat(impostosItmParaSomar.IMPOSTO_INFO_FATURA_ITEM.ToList()).ToList();
                        }

                        totalLiquido = (totalLiquido - totalDesconto); // desconto do valor do pedido o valor retido de imposto

                        var infoFaturaItm = new InfoFaturaItemDTO()
                        {
                            IFI_PERCENTUAL_REFERENCIA = porcentagem,
                            IFI_PERCENTUAL_TOTAL_DESCONTADO = totalDescontoPercentual,
                            IFI_TOTAL_DESCONTADO = Math.Round((decimal) totalDesconto, 2),
                            IFI_TOTAL_LIQUIDO = Math.Round((decimal) totalLiquido, 2),
                            IFI_VALOR_BRUTO = Math.Round((decimal)totalBruto, 2),
                            NFC_ID = nfConfig.NFC_ID,
                            NCT_ID = nfConfig.NCT_ID,
                            NOTA_FISCAL_CONFIG_TIPO = nfConfig.NOTA_FISCAL_CONFIG_TIPO,
                            IFF_N_RETEVE_POR_REGRA = naoRetevePorRegra
                        };

                        infoFaturaItm.IMPOSTO_INFO_FATURA_ITEM = lstImposto;

                        lstCalculo.Add(infoFaturaItm);
                    }
                }
            }

            return lstCalculo;
        }

        public InfoFaturaItemDTO RetornarFaturaItem(InfoFaturaDTO infoFatura, decimal? porcentagem)
        {
            if(infoFatura != null 
                && porcentagem != null 
                && infoFatura.INFO_FATURA_ITEM != null)
            {
                var infoFaturaItem = infoFatura
                    .INFO_FATURA_ITEM
                    .Where(x => x.IFI_PERCENTUAL_REFERENCIA == porcentagem)
                    .FirstOrDefault();

                return infoFaturaItem;
            }
            return null;
        }

        public InfoFaturaDTO CalcularImpostos(CalculoRequestImpostoDTO request)
        {
            var cmpId = request.CmpID;
            var lstNotaFiscalConfig = _notaFiscalConfigSRV.ListarNotaFiscalConfig(cmpId, null, request.cemPorCentoFaturado);
            var lstFaturaItem = CalcularInfoFaturaItm(lstNotaFiscalConfig, request);

            if (lstFaturaItem != null && lstFaturaItem.Count() > 0)
            {
                decimal? totalValorBruto = 0;
                decimal? totalValorLiquido = 0;
                decimal? totalDescontoPercentual = 0;
                decimal? totalDesconto = 0;

                var lstImpostosItmSomatorio = new List<ImpostoInfoFaturaItemDTO>();

                foreach (var itmFat in lstFaturaItem)
                {
                    totalValorBruto += itmFat.IFI_VALOR_BRUTO;
                    totalValorLiquido += itmFat.IFI_TOTAL_LIQUIDO;
                    totalDescontoPercentual += itmFat.IFI_PERCENTUAL_TOTAL_DESCONTADO;
                    totalDesconto += itmFat.IFI_TOTAL_DESCONTADO;

                    if (itmFat.IMPOSTO_INFO_FATURA_ITEM != null)
                    {
                        foreach (var imp in itmFat.IMPOSTO_INFO_FATURA_ITEM)
                        {
                            var impInfoFatuSoma = lstImpostosItmSomatorio.Where(x => x.IMP_ID == imp.IMP_ID).FirstOrDefault();

                            if (impInfoFatuSoma == null) {

                                impInfoFatuSoma = new ImpostoInfoFaturaItemDTO() {
                                    IFI_PERCENTUAL_DESCONTO = 0,
                                    IFI_VALOR_DESCONTADO = 0,
                                    IMP_ID = imp.IMP_ID,
                                    IMPOSTO = imp.IMPOSTO
                                };

                                lstImpostosItmSomatorio.Add(impInfoFatuSoma);
                            }


                            impInfoFatuSoma.IFI_VALOR_DESCONTADO += imp.IFI_VALOR_DESCONTADO;
                            impInfoFatuSoma.IFI_PERCENTUAL_DESCONTO += imp.IFI_PERCENTUAL_DESCONTO;

                        }
                    }
                }

                IList<ImpostoInfoFaturaDTO> lstImpostosInfoFatura = null;

                if (lstImpostosItmSomatorio != null)
                {
                    lstImpostosInfoFatura = lstImpostosItmSomatorio.Select(x => new ImpostoInfoFaturaDTO()
                    {
                        DATA_ASSOCIACAO = DateTime.Now,
                        IIF_VALOR_DESCONTO = x.IFI_VALOR_DESCONTADO,
                        IIF_PERCENTUAL_DESCONTO = x.IFI_PERCENTUAL_DESCONTO,
                        IMP_ID = x.IMP_ID,
                        IMPOSTO = x.IMPOSTO,
                    }).ToList();
                }

                var infoFatura = new InfoFaturaDTO()
                {
                    IFF_PERCENTUAL_TOTAL_DESCONTADO = totalDescontoPercentual,
                    IFF_VALOR_BRUTO = totalValorBruto,
                    IFF_TOTAL_LIQUIDO = totalValorLiquido,
                    IFF_TOTAL_DESCONTADO = totalDesconto,
                    IMPOSTO_INFO_FATURA = lstImpostosInfoFatura,
                    INFO_FATURA_ITEM = lstFaturaItem
                    
                };

                return infoFatura;
            }

            return null;
        }


        public ResultadoCalculoImpostoDTO CalcularDescontos(
                IPedidoItem pedidoItm,
                IPedido pedido)
        {
            ResultadoCalculoImpostoDTO resultado = null;
            if (pedidoItm != null && pedidoItm.CmpId != null)
            {

                if (pedido != null && pedido != null)
                {
                    var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(pedido.CliId);
                    if (cliente != null
                        //&&
                        //cliente.TIPO_CLI_ID != 2
                        )
                    {

                        var valorBruto = (pedidoItm.ValorUnitario * pedidoItm.Qtd);
                        bool? calcularSobreTotal = null;
                        
                        resultado = new ResultadoCalculoImpostoDTO();
                        var cmpId = pedidoItm.CmpId;

                        if (pedidoItm.ValorEntradaBruto != null &&
                            pedidoItm.ValorEntradaBruto > 0)
                        {
                            calcularSobreTotal = false;
                            var calculoSobreTotal = CalcularImpostos(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = (pedido.FaturadoCemPorCento.HasValue) ? pedido.FaturadoCemPorCento.Value : false,
                                empresaDoSimples = (pedido.EmpresaDoSimples.HasValue) ? pedido.EmpresaDoSimples.Value : false,
                                CmpID = pedidoItm.CmpId,
                                qtd = pedidoItm.Qtd,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = valorBruto,
                                sobreTotal = true
                            });


                            var infoFaturaEntrada = CalcularImpostos(new CalculoRequestImpostoDTO()
                            {
                                cemPorCentoFaturado = (pedido.FaturadoCemPorCento.HasValue) ? pedido.FaturadoCemPorCento.Value : false,
                                empresaDoSimples = (pedido.EmpresaDoSimples.HasValue) ? pedido.EmpresaDoSimples.Value : false,
                                CmpID = pedidoItm.CmpId,
                                qtd = pedidoItm.Qtd,
                                tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                                valorUnitario = pedidoItm.ValorEntradaBruto,
                                ImpostosParaSomar = calculoSobreTotal,
                                sobreTotal = false,
                                qtdParcelas = 1
                            });
                            resultado.ResultadoEntrada = infoFaturaEntrada;
                        }

                        var infoFatura = CalcularImpostos(new CalculoRequestImpostoDTO()
                        {
                            cemPorCentoFaturado = (pedido.FaturadoCemPorCento.HasValue) ? pedido.FaturadoCemPorCento.Value : false,
                            empresaDoSimples = (pedido.EmpresaDoSimples.HasValue) ? pedido.EmpresaDoSimples.Value : false,
                            CmpID = pedidoItm.CmpId,
                            qtd = pedidoItm.Qtd,
                            tipoCliId = (cliente != null) ? cliente.TIPO_CLI_ID : 0,
                            valorUnitario = pedidoItm.ValorParcelaBruto,
                            sobreTotal = calcularSobreTotal,
                            qtdParcelas = pedidoItm.QtdParcelas
                        });

                        resultado.ResultadoParcela = infoFatura;
                    }

                }
            }
            return resultado;
        }

        public InfoFaturaDTO CalcularDescontoProduto(ProdutoComposicaoDTO produtoComposicao, int? qtd = 1, int? cliId = null)
        {
            int? tipoCliente = 2;

            if(cliId != null)
            {
                ClienteDto cliente =_clienteSRV.FindById(cliId);
                if(cliente != null && cliente.TIPO_CLI_ID != null)
                {
                    tipoCliente = cliente.TIPO_CLI_ID;
                }
            }

            var infoFatura = CalcularImpostos(new CalculoRequestImpostoDTO()
            {
                cemPorCentoFaturado = false,
                empresaDoSimples = false,
                CmpID = produtoComposicao.CMP_ID,
                qtd = qtd,
                tipoCliId = tipoCliente,
                valorUnitario = produtoComposicao.CMP_VLR_VENDA,
                sobreTotal = true,
                qtdParcelas = 1
            });

            return infoFatura;

        }
    }
}
