using System;
using System.Collections.Generic;
using System.Text;
using COAD.SEGURANCA.Service;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using BoletoNet;
using GenericCrud.Util;
using System.IO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.COBRANCA.Bancos.Service;
using System.Reflection;
using System.Globalization;
using System.Net;
using NReco.PdfGenerator;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Service;
using COAD.COBRANCA.Bancos.Config;
using COAD.SEGURANCA.Model;
using COAD.COBRANCA.Bancos.Model.DTO.Interfaces;

namespace COAD.CORPORATIVO.Service.Boleto
{

    public class BoletoSRV
    {
        public ContaSRV _serviceConta { get; set; }
        public ClienteSRV _serviceCliente { get; set; }
        public EmpresaSRV _serviceEmpresa { get; set; }
        public ParcelasSRV _serviceParcela { get; set; }
        public ParcelaAlocadaSRV _serviceParcelaAlocada { get; set; }
        public ClienteEnderecoSRV _serviceClienteEndereco { get; set; }
        public SequencialNossoNumeroSRV _seqNossoNumeroSRV { get; set; }
        public BoletoSRV()
        {
            ConfigBoleto.Configurar();
        }

        public BoletoDTO MontarBoleto(string idparcela)
        {
            int _cta_id = 0;
            bool _avulso = false;

            var parametro = ServiceFactory.RetornarServico<CnabSRV>().prepararParametro(idparcela, _cta_id, _avulso);
            BoletoDTO _boleto;

            if (parametro != null)
            {
                _boleto = this.MontarBoleto(parametro);
            }
            else
            {
                throw new Exception("O Cliente do título (" + idparcela + ") não foi localizado pelo gerador de [parâmetros] do Boleto!");
            }

            return _boleto;
        }

        public byte[] GerarBoletosPDF(string idTitulo)
        {

            string strSiteUrl = SysUtils.RetornarHostName() + "/CobrancaEscritural/BoletoHtml?idTitulo=" + idTitulo;

            //string data = "idTitulo =" + idTitulo;
            //Uri strSiteUrluri = new Uri(strSiteUrl);
            //var request = (HttpWebRequest)WebRequest.Create(strSiteUrluri);
            //request.Method = "GET";
            //request.ContentType = "text/xml";
            //var response = (HttpWebResponse)request.GetResponse();
            //var html = new StreamReader(response.GetResponseStream()).ReadToEnd();

            string curDir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
           
            string html = null;
            var assembly = Assembly.GetExecutingAssembly();

            if (assembly != null)
            {
                //var lstResource = assembly.GetManifestResourceNames();
                var htmlBoleto = assembly.GetManifestResourceStream("COAD.CORPORATIVO.Layout.BoletoHtml.cshtml"); // retorno a imagem como enbedded resource. Ou seja, o html do boleto fica inserido na própria dll.

                if(htmlBoleto != null)
                {
                    html = new StreamReader(htmlBoleto).ReadToEnd();
                }
                else
                {
                    curDir = curDir + "\\Views\\CobrancaEscritural\\BoletoHtml.cshtml";
                }
            }
            else
            {
                curDir = curDir + "\\Views\\CobrancaEscritural\\BoletoHtml.cshtml";
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                html = new StreamReader(curDir).ReadToEnd();
            }

            StringBuilder corpoemail = new StringBuilder();

            float ZoomPercent = 1.85f;
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();
            htmlToPdfConverter.CustomWkHtmlArgs = "";
            htmlToPdfConverter.Grayscale = false;
            htmlToPdfConverter.Zoom = ZoomPercent; // (ZoomPercent / 100);
            htmlToPdfConverter.Size = NReco.PdfGenerator.PageSize.A3;
            
            var _boleto = this.MontarBoleto(idTitulo);
            html = this.PrepararPDF(_boleto, html);

            byte[] outPdfBuffer = htmlToPdfConverter.GeneratePdf(html);

            return outPdfBuffer;
        }


        public byte[] GerarVariosBoletosPDF(List<ParametroDTO> parametro)
        {

            byte[] _retorno = null;
            var _boletoBancario = new BoletoBancario();

            var lstBoleto = new List<BoletoBancario>();

            foreach (var p in parametro)
            {
                var conta =  _serviceConta.FindById(p.idConta);

                if (conta.BAN_ID == "033" ||
                    conta.BAN_ID == "041" ||
                    conta.BAN_ID == "104" || 
                    conta.BAN_ID == "237" || 
                    conta.BAN_ID == "999" ||
                    conta.BAN_ID == "998" ||
                    conta.BAN_ID == "341" ||
                    conta.BAN_ID == "422" ||
                    conta.BAN_ID == "756" )
                {
                    _retorno = this.GerarBoletosPDF(p.idTitulo);
                }
                else
                {
                    var boleto = this.Gerar(p);

                    if (boleto != null)
                        lstBoleto.Add(boleto);

                    _retorno = _boletoBancario.MontaBytesListaBoletosPDF(lstBoleto, "Boletos Bancários");

                }
         
            }

            return _retorno;
        }

        public string retornarLinkBoletoAvulso(List<ParametroDTO> parametro, string pasta, string arquivo)
        {
            var lstBytes = this.GerarVariosBoletosPDF(parametro);

            string salvarEm = Path.Combine(pasta, "temp", arquivo);

            File.WriteAllBytes(salvarEm, lstBytes);

            return salvarEm;
        }

        public string retornarAgencia(string ag)
        {
            return ag.Substring(0, 4);
        }
        public string retornarDvAgencia(string ag, string bco)
        {
            string dvAg = ag.IndexOf('-') >= 0 ? ag.Substring(5, 1) : "";
            return bco == "041" ? "0" : dvAg;
        }
        public string retornarConta(string cc)
        {
            if (cc.IndexOf('-') >= 0)
            {
                cc = cc.Substring(0, cc.Length - 2);
            }
            return cc;
        }
        public string retornarDvConta(string cc)
        {
            string dvCc = "";
            if (cc.IndexOf('-') >= 0)
            {
                dvCc = cc.Substring(cc.Length - 1, 1);
            }
            return dvCc;
        }
        public BoletoDTO MontarBoleto(ParametroDTO parametro)
        {
			
			var bl = new BoletoDTO();
																		  

            try
            {
                var titulo = _serviceParcela.FindById(parametro.idTitulo);

                var conta = new ContaDTO();

                if ((titulo.BAN_ID != "998") && (titulo.BAN_ID != "999"))
                    conta = _serviceConta.FindById(titulo.CTA_ID);
                else if (titulo.BAN_ID == "998") // ajuste para a FactorMix
                    conta = ( parametro.idConta != 125 ? _serviceConta.ListarPorEmpresa(10)[0] : _serviceConta.FindById(125));
                else if (titulo.BAN_ID == "999") // ajuste para a Lira
                    conta = _serviceConta.ListarPorEmpresa(8)[0];

                var empresa = _serviceEmpresa.FindById((parametro.idConta != 125 ? conta.EMP_ID : _serviceConta.ListarPorEmpresa(10)[0].EMP_ID));
                var empresaSacadorAvalista = _serviceEmpresa.FindById(parametro.idEmpresa);
                var cliente = _serviceCliente.FindById(parametro.idCliente);
                var endereco = _serviceClienteEndereco.FindById(parametro.idCliente, 2);
                if (endereco == null)
                    endereco = _serviceClienteEndereco.FindById(parametro.idCliente, 1);

                if (titulo.PAR_VENC_BOLETO != null)
                    titulo.PAR_DATA_VENCTO = (DateTime)titulo.PAR_VENC_BOLETO;

                if (titulo.PAR_VLR_BOLETO != null)
                    titulo.PAR_VLR_PARCELA = titulo.PAR_VLR_BOLETO;

                ConfigBoleto.Configurar();

                var banco = ConfigBoleto.GetBanco((parametro.idConta != 125 ? conta.BAN_ID : parametro.BANCO) );
											  
                var ag = this.retornarAgencia(conta.CTA_AGENCIA.Substring(0, 4));
                var dvAg = this.retornarDvAgencia(conta.CTA_AGENCIA, conta.BAN_ID);
                var cc = this.retornarConta(conta.CTA_CONTA.Substring(0, 5));
                var dvCc = this.retornarDvConta(conta.CTA_CONTA);
                var carteira = conta.CTA_CARTEIRA_BOLETO;

                if ((titulo.PAR_BOLETO_AVULSO == null ||
                    titulo.PAR_BOLETO_AVULSO == false) &&
                    (titulo.PAR_SEQ_PARCELA > 0 &&
                    conta.BAN_ID == "237" &&
                    empresa.EMP_ID == 9))
                {
                    carteira = "02";
                    conta.CTA_CARTEIRA_REMESSA = "02";
                    conta.CTA_CARTEIRA_BOLETO = "02";
                }

                conta.CTA_PERC_MORA_MES = conta.CTA_PERC_MORA_MES == null ? 4 : conta.CTA_PERC_MORA_MES;
                var vlrMora = (((decimal)conta.CTA_PERC_MORA_MES / 30) * Convert.ToDecimal(titulo.PAR_VLR_PARCELA)) / 100;

                //var bl = new BoletoDTO();
                string[] instrucoes = null;
													
                if (conta.CTA_INSTRUCOES_BOLETO != null)
                    instrucoes = conta.CTA_INSTRUCOES_BOLETO.Split(';');

                /*
                if (!parametro.preAlocado)
                {
                    if (conta.CTA_PERC_MORA_MES != null)
                    {

                        var _morames = decimal.Round((((decimal)conta.CTA_PERC_MORA_MES / 30 / 100) * (decimal)titulo.PAR_VLR_PARCELA), 2);
                        bl.Instrucoes01 = "APÓS O VENCIMENTO COBRAR R$ " + _morames.ToString("0.00").Replace('.', ',') + " AO DIA";
                    }

                    if (titulo.BAN_ID == "999")
                    {
						
						bool avalistaIncluido = false;
                        bl.SacadorAvalista = "Sacador / Avalista:" + empresa.EMP_RAZAO_SOCIAL + " CNPJ:" + empresa.EMP_CNPJ;
						
                        if (instrucoes != null)
                        {

                            if (instrucoes.Length > 0)
                                bl.Instrucoes02 = instrucoes[0].ToString();
                            else if (!avalistaIncluido)
                            {
                                bl.Instrucoes02 = bl.SacadorAvalista;
                                avalistaIncluido = true;
                            }

                            if (instrucoes.Length > 1)
                                bl.Instrucoes03 = instrucoes[1].ToString();
                            else if (!avalistaIncluido)
                            {
                                bl.Instrucoes03 = bl.SacadorAvalista;
                                avalistaIncluido = true;
                            }

                            if (instrucoes.Length > 2) 
                                bl.Instrucoes04 = instrucoes[2].ToString();
                            else if (!avalistaIncluido)
                            {
                                bl.Instrucoes04 = bl.SacadorAvalista;
                                avalistaIncluido = true;
                            }

                            if (instrucoes.Length > 3)
                                bl.Instrucoes05 = instrucoes[3].ToString();
                            else if (!avalistaIncluido)
                            {
                                bl.Instrucoes05 = bl.SacadorAvalista;
                                avalistaIncluido = true;
                            }
                        }

                    }
                    else
                    if (titulo.BAN_ID == "604")
                    {

                        bl.SacadorAvalista = "Sacador / Avalista:" + empresa.EMP_RAZAO_SOCIAL + " CNPJ:" + empresa.EMP_CNPJ;

                        if (instrucoes != null)
                        {
                            bl.Instrucoes02 = (instrucoes.Length > 1) ? instrucoes[0].ToString() : null;
                            bl.Instrucoes03 = (instrucoes.Length > 2) ? instrucoes[1].ToString() : null;
                            bl.Instrucoes04 = (instrucoes.Length > 3) ? instrucoes[2].ToString() : null;
                            bl.Instrucoes05 = (instrucoes.Length > 4) ? instrucoes[3].ToString() : null;
                        }

                    }
                    else
                    {
                        if (instrucoes != null)
                        {
                            bl.Instrucoes02 = (instrucoes.Length > 1) ? instrucoes[0].ToString() : null;
                            bl.Instrucoes03 = (instrucoes.Length > 2) ? instrucoes[1].ToString() : null;
                            bl.Instrucoes04 = (instrucoes.Length > 3) ? instrucoes[2].ToString() : null;
                            bl.Instrucoes05 = (instrucoes.Length > 4) ? instrucoes[3].ToString() : null;
                        }
                    }
                }
                */

                if (conta.CTA_PERC_MORA_MES != null)
                {

                    var _morames = decimal.Round((((decimal)conta.CTA_PERC_MORA_MES / 30 / 100) * (decimal)titulo.PAR_VLR_PARCELA), 2);
                    bl.Instrucoes01 = "APÓS O VENCIMENTO COBRAR R$ " + _morames.ToString("0.00").Replace('.', ',') + " AO DIA";

                }

                if ((titulo.BAN_ID == "999") || (titulo.BAN_ID == "604") 
                    || (titulo.BAN_ID == "998"))
                {

                    bl.SacadorAvalista = "Sacador / Avalista:" + empresaSacadorAvalista.EMP_RAZAO_SOCIAL;
                    bl.SacadorAvalista += " - CNPJ:" + Convert.ToUInt64(empresaSacadorAvalista.EMP_CNPJ).ToString(@"00\.000\.000\/0000\-00");

                    if (instrucoes != null)
                    {

                        bool avalistaIncluido = false;

                        if (instrucoes.Length > 0)
                            bl.Instrucoes02 = instrucoes[0].ToString();
                        else if (!avalistaIncluido)
                        {
                            bl.Instrucoes02 = bl.SacadorAvalista;
                            avalistaIncluido = true;
                        }

                        if (instrucoes.Length > 1)
                            bl.Instrucoes03 = instrucoes[1].ToString();
                        else if (!avalistaIncluido)
                        {
                            bl.Instrucoes03 = bl.SacadorAvalista;
                            avalistaIncluido = true;
                        }

                        if (instrucoes.Length > 2)
                            bl.Instrucoes04 = instrucoes[2].ToString();
                        else if (!avalistaIncluido)
                        {
                            bl.Instrucoes04 = bl.SacadorAvalista;
                            avalistaIncluido = true;
                        }

                        if (instrucoes.Length > 3)
                            bl.Instrucoes05 = instrucoes[3].ToString();
                        else if (!avalistaIncluido)
                        {
                            bl.Instrucoes05 = bl.SacadorAvalista;
                            avalistaIncluido = true;
                        }

                    }

                }
                else if (instrucoes != null)
                {

                    bl.Instrucoes02 = (instrucoes.Length > 1) ? instrucoes[0].ToString() : null;
                    bl.Instrucoes03 = (instrucoes.Length > 2) ? instrucoes[1].ToString() : null;
                    bl.Instrucoes04 = (instrucoes.Length > 3) ? instrucoes[2].ToString() : null;
                    bl.Instrucoes05 = (instrucoes.Length > 4) ? instrucoes[3].ToString() : null;

                }
                

                string nossonumsemdig = titulo.PAR_NOSSO_NUMERO.Substring(0, (titulo.PAR_NOSSO_NUMERO.Length - 1));
                string digitoNossoNumero = titulo.PAR_NOSSO_NUMERO.Substring((titulo.PAR_NOSSO_NUMERO.Length - 1), 1);

                if (banco != null && banco.UsarConfigDoBanco)
                {

                    if (banco.CodigoBanco == "422")
                        bl.AgenciaCodBeneficiario = conta.CTA_AGENCIA.Substring(0, 4) + "/" + conta.CTA_CONTA.Substring(0, 7);
                    else
                        bl.AgenciaCodBeneficiario = banco.FormatarContaAgencia(conta);

                    if (!banco.ExcluirDigitoNossoNumero)
                    {

                        nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                        digitoNossoNumero = null;

                    }
                }
                else
                {
                    switch (conta.BAN_ID)
                    {
                        case "104":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONVENIO;
																 
						 
                            nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                            digitoNossoNumero = null;																				 
                            carteira = null;
                            break;
                        case "033":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONVENIO;
																
						 
                            nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                            digitoNossoNumero = null;
                            break;
                        case "237":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONTA;
                            if (titulo.PAR_NOSSO_NUMERO.Length == 11)
                            {
                                nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                                digitoNossoNumero = null;
                            }
                            break;
                        case "041":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONVENIO;
                            nossonumsemdig = titulo.PAR_NOSSO_NUMERO.Substring(0, 8);
                            digitoNossoNumero = null;
                            break;
                        case "341":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONTA;
                            if (titulo.PAR_NOSSO_NUMERO.Length == 9)
                            {
                                nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                                digitoNossoNumero = null;
                            }
                            break;
                        case "422":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONTA;
                            if (titulo.PAR_NOSSO_NUMERO.Length == 9)
                            {
                                nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                                digitoNossoNumero = null;
                            }
                            break;
                        case "756":
                            bl.AgenciaCodBeneficiario = ag + "/" + conta.CTA_CONVENIO;
                            if (titulo.PAR_NOSSO_NUMERO.Length == 9)
                            {
                                nossonumsemdig = titulo.PAR_NOSSO_NUMERO;
                                digitoNossoNumero = null;
                            }
                            break;
                    }
                }
				

            bl.Beneficiario = empresa.EMP_RAZAO_SOCIAL + " - CNPJ: " + Convert.ToUInt64(empresa.EMP_CNPJ).ToString(@"00\.000\.000\/0000\-00");
            bl.CodBeneficiario = conta.CTA_CONVENIO;
            bl.Especie = conta.CTA_ESPECIE;
            bl.EspecieDoc = conta.CTA_ESPECIE_DOC;
            bl.Aceite = conta.CTA_ACEITE;
            bl.Quantidade = null;
            bl.Carteira = carteira;

            if (conta.BAN_ID!="237")
                bl.NossoNumero = titulo.PAR_NOSSO_NUMERO;
            else
                bl.NossoNumero = carteira + "/"+ titulo.PAR_NOSSO_NUMERO;

            bl.NumeroDocumento = titulo.PAR_NUM_PARCELA;
            bl.CpfCnpj = empresa.EMP_CNPJ;
            bl.DtVencimento = titulo.PAR_DATA_VENCTO;
            bl.DtProcessamento = DateTime.Now;
            bl.ValorDocumento = (titulo.PAR_VLR_BOLETO != null && titulo.PAR_VLR_BOLETO > 0) ? titulo.PAR_VLR_BOLETO : titulo.PAR_VLR_PARCELA;

       
            bl.NomeCpfCnpj = cliente.CLI_NOME;

            if ( cliente.CLI_CPF_CNPJ.Length > 11 )
                bl.NomeCpfCnpj += " - CPF/CNPJ: "+ Convert.ToUInt64(cliente.CLI_CPF_CNPJ).ToString(@"00\.000\.000\/0000\-00");
            else
                bl.NomeCpfCnpj += " - CPF/CNPJ: "+ Convert.ToUInt64(cliente.CLI_CPF_CNPJ).ToString(@"000\.000\.000\-00");

            bl.Endereco = endereco.END_LOGRADOURO + " " + endereco.END_NUMERO + " " + endereco.END_COMPLEMENTO;
            bl.BairroCidadeCep = endereco.END_BAIRRO + " - " + endereco.END_MUNICIPIO +" - CEP: " + endereco.END_CEP;

            bl.VlrDescAbatimetos = null;
            bl.VlrOutrasDeducoes = null;
            bl.VlrMoraMulta = null;
            bl.VlrOutraAcrecimos = null;
            bl.ValorCobrado = null;

            var boletoSRV = new CodigoBarraSRV(banco);
            var cdbarras = boletoSRV.GerarCodigoBarras(bl.DtVencimento, nossonumsemdig, bl.ValorDocumento, conta);
            var lndigitavel = boletoSRV.GerarLinhaDigitavel(cdbarras);
            bl.BarcodeImage = boletoSRV.GerarImagemBarcode(cdbarras.CodigoBarras);
            bl.CodigoBarras = cdbarras;
            bl.LinhaDigivel = lndigitavel;
            

            string _curDir = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

                if (banco != null && banco.UsarConfigDoBanco)
                {
                    bl.logo = RetornarImagem($"/Images/bancos/{banco.Logo}", _curDir);
                    bl.Bancologo = banco.BancoDesc;
                }
                else
                {
                    switch (conta.BAN_ID)
                    {
                        case "033":
                            bl.logo = _curDir + "/Images/bancos/santander-logo.jpg";
                            bl.Bancologo = "033-7";
                            break;
                        case "041":
                            bl.logo = _curDir + "/Images/bancos/banrisul-logo.jpg";
                            bl.Bancologo = "041-8";
                            break;
                        case "104":
                            bl.logo = _curDir + "/Images/bancos/caixa-logo.jpg";
                            bl.Bancologo = "104-0";
                            break;
                        case "237":
                            bl.logo = _curDir + "/Images/bancos/bradesco-logo.jpg";
                            bl.Bancologo = "237-2";
                            break;
                        case "341":
                            bl.logo = RetornarImagem("/Images/bancos/itau-logo.jpg", _curDir);
                            bl.Bancologo = "341-7";
                            break;
                        case "422":
                            bl.logo = RetornarImagem("/Images/bancos/safra-logo.jpg", _curDir);
                            bl.Bancologo = "422-7";
                            break;
                        case "756":
                            bl.logo = _curDir + "/Images/bancos/sicoob-logo.jpg";
                            bl.Bancologo = "756-0";
                            break;
                    }
                    if (bl.BarcodeImage == null)
                        bl.BarcodeImage = bl.logo;
                }
            }
            catch (Exception e)
            {

                throw new  Exception(string.Format("Erro ao gerar boleto."), e);

            }
            return bl;
        }

        public string RetornarImagem(string path, string basePath)
        {
            var url = basePath + path;

            url = url.Replace("/", "\\");
            var originalPath = url;

            if (File.Exists(url))
            {
                return url;
            }

            path = path.Replace("\\", ".").Replace("/", ".");
            var assembly = Assembly.GetExecutingAssembly();
            var name = "COAD.CORPORATIVO";
            var fileInfo = assembly.GetManifestResourceStream(name + path);

            if(fileInfo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fileInfo.CopyTo(ms);
                    File.WriteAllBytes(originalPath, ms.ToArray());
                }
                return url;
            }
            return originalPath;
        }

        public string PrepararPDF(BoletoDTO bol,  string html)
        {
            PropertyInfo[] campos = typeof(BoletoDTO).GetProperties();
            var _valor = "";
            var _campo = "";

            foreach (PropertyInfo mInfo in campos)
            {
                try
                {
                    _valor = null;
                    _campo = "{{boleto." + mInfo.Name + "}}";


                    if (mInfo.Name == "LinhaDigivel")
                        _campo = "{{boleto.LinhaDigivel.ValorFormatado}}";

                    
                    if (mInfo.GetValue(bol) != null && mInfo.Name != "LinhaDigivel")
                        _valor = mInfo.GetValue(bol).ToString();

                    if (mInfo.Name == "DtProcessamento")
                    {
                        string formattedDateValue = String.Format("{0:dd/MM/yyyy}", bol.DtProcessamento);
                        html = html.Replace(_campo, formattedDateValue);
                    }
                    else if (mInfo.Name == "DtVencimento")
                    {
                        string formattedDateValue = String.Format("{0:dd/MM/yyyy}", bol.DtVencimento);
                        html = html.Replace(_campo, formattedDateValue);

                    }
                    else if (mInfo.Name == "LinhaDigivel")
                    {
                        html = html.Replace(_campo, bol.LinhaDigivel.ValorFormatado);

                    }
                    else if (mInfo.Name == "ValorDocumento")
                    {
                        //string formattedMoneyValue = String.Format("{0:#.##}", bol.ValorDocumento);
                        string formattedMoneyValue = String.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", bol.ValorDocumento);
                        html = html.Replace(_campo, formattedMoneyValue);
                    }
                    else
                        html = html.Replace(_campo, _valor);
                }
                catch 
                {

                }
            }
        
            return html;
        }

        public BoletoBancario Gerar(ParametroDTO parametro)
        {
            MontagemDTO montado = new MontagemDTO();
            var titulo = _serviceParcela.FindById(parametro.idTitulo);

            var conta = new ContaDTO();

            if ((titulo.BAN_ID != "998") && (titulo.BAN_ID != "999"))
                conta = _serviceConta.FindById(titulo.CTA_ID);
            else if (titulo.BAN_ID == "998") // ajuste para a FactorMix
                conta = _serviceConta.ListarPorEmpresa(10)[0];
            else if (titulo.BAN_ID == "999") // ajuste para a Lira
                conta = _serviceConta.ListarPorEmpresa(8)[0];

            var empresa = _serviceEmpresa.FindById(conta.EMP_ID);
            var empresaSacadorAvalista = _serviceEmpresa.FindById(parametro.idEmpresa);

            //var empresa = _serviceEmpresa.FindById(parametro.idEmpresa);
            //var conta = _serviceConta.FindById(parametro.idConta);
            var cliente = _serviceCliente.FindById(parametro.idCliente);
            var endereco = _serviceClienteEndereco.FindById(parametro.idCliente, 2);
            if (endereco == null)
                endereco = _serviceClienteEndereco.FindById(parametro.idCliente, 1);

            var _ban_id = conta.BAN_ID;

            if (titulo != null && empresa != null && conta != null && cliente != null && endereco != null)
            {
                if (titulo.PAR_VENC_BOLETO != null)
                    titulo.PAR_DATA_VENCTO = (DateTime)titulo.PAR_VENC_BOLETO;

                if (titulo.PAR_VLR_BOLETO != null)
                    titulo.PAR_VLR_PARCELA = titulo.PAR_VLR_BOLETO;

                string ag = this.retornarAgencia(conta.CTA_AGENCIA);
                string dvAg = this.retornarDvAgencia(conta.CTA_AGENCIA, _ban_id);

                string cc = this.retornarConta(conta.CTA_CONTA);
                string dvCc = this.retornarDvConta(conta.CTA_CONTA);

                string nossoNumero = titulo.PAR_NOSSO_NUMERO.Substring(0, (titulo.PAR_NOSSO_NUMERO.Length - 1));
                string digitoNossoNumero = titulo.PAR_NOSSO_NUMERO.Substring((titulo.PAR_NOSSO_NUMERO.Length - 1), 1);

                switch (_ban_id)
                {
                    case "237":
                        if (titulo.PAR_NOSSO_NUMERO.Length == 11)
                        {
                            nossoNumero = titulo.PAR_NOSSO_NUMERO;
                            digitoNossoNumero = null;
                        }
                        break;
                    case "041":
                        nossoNumero = titulo.PAR_NOSSO_NUMERO.Substring(0, 8);
                        digitoNossoNumero = null;
                        break;
                }
                
                string beneficiario = empresa.EMP_RAZAO_SOCIAL;

                if (_ban_id == "237")
                {

                    beneficiario = empresa.EMP_RAZAO_SOCIAL;
                    beneficiario += " - CNPJ:" + Convert.ToUInt64(empresa.EMP_CNPJ).ToString(@"00\.000\.000\/0000\-00");
                    beneficiario += " - " + empresa.EMP_LOGRADOURO + " " + empresa.EMP_NUMERO;

                }

                CedenteDTO cedente = new CedenteDTO(empresa.EMP_CNPJ, beneficiario, ag, dvAg, cc, dvCc);
                SacadoDTO sacado = new SacadoDTO(cliente.CLI_CPF_CNPJ, cliente.CLI_NOME);
                InstrucaoDTO instrucao = new InstrucaoDTO();
                EspecieDocumentoDTO especie = new EspecieDocumentoDTO(_ban_id, "");

                if (_ban_id == "237")
                    especie.Sigla = "DM";

                conta.CTA_PERC_MORA_MES = conta.CTA_PERC_MORA_MES == null ? 4 : conta.CTA_PERC_MORA_MES;
                decimal vlrMora = (((decimal)conta.CTA_PERC_MORA_MES / 30) * Convert.ToDecimal(titulo.PAR_VLR_PARCELA)) / 100;

                /*
                if (parametro.preAlocado)
                {
                    instrucao.Descricao = "";
                }
                else
                {
                    if ((titulo.BAN_ID == "999") || (titulo.BAN_ID == "998"))
                    {

                        instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                                              "<li>Sacador/Avalista:" + empresa.EMP_RAZAO_SOCIAL + " CNPJ:" + empresa.EMP_CNPJ + "</li>" +
                                              "<li>APOS O VENCIMENTO COBRAR 2% DE MULTA </li>" +
                                              "<li>APÓS O VENCIMENTO COBRAR R$ 0.62 AO DIA </li>" +
                                              "<li>PAGÁVEL EM QUALQUER AGÊNCIA BANCARIA ATÉ A DATA DO VENCIMENTO </li>" +
                                              "<li>APÓS O VENCIMENTO PAGÁVEL SOMENTE NAS AGÊNCIAS DO BRADESCO </li>" +
                                              "</ul>";

                    }
                    else if (titulo.BAN_ID == "604")
                    {

                        instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                                              "<li>Sacador/Avalista:" + empresa.EMP_RAZAO_SOCIAL + " CNPJ:" + empresa.EMP_CNPJ + "</li>" +
                                              "<li>"+empresa.EMP_LOGRADOURO + ", " + empresa.EMP_NUMERO +" - " +empresa.EMP_COMPLEMENTO+" - "+ endereco.END_MUNICIPIO+"/" + endereco.END_UF + " - CEP: " + endereco.END_CEP+"<li>" +
                                              "<li>APOS O VENCIMENTO COBRAR 2% DE MULTA </li>" +
                                              "<li>APÓS O VENCIMENTO COBRAR R$ 0.13 AO DIA </li>" +
                                              "<li>TITULO CEDIDO AO BANCO INDUSTRIAL DO BRASIL. </li>" +
                                              "</ul>";

                    }

                    else
                    {
                        instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                            "                  <li>APOS O VENCIMENTO COBRAR 2% DE MULTA E MORA DE R$ " + string.Format("{0:0,0.00}", vlrMora) + " POR DIA DE ATRASO. </li>" +
                                              "<li>TITULO NEGOCIADO. SUJEITO A PROTESTO. " + parametro.msg + " </li></ul>";

                    }

                }
                */

                if ((titulo.BAN_ID == "999") || (titulo.BAN_ID == "998"))
                {

                    instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                                          "<li>Sacador/Avalista:" + empresaSacadorAvalista.EMP_RAZAO_SOCIAL 
                                          + " - CNPJ:" + Convert.ToUInt64(empresaSacadorAvalista.EMP_CNPJ).ToString(@"00\.000\.000\/0000\-00") + "</li>" +
                                          "<li>APOS O VENCIMENTO COBRAR 2% DE MULTA </li>" +
                                          "<li>APÓS O VENCIMENTO COBRAR R$ 0.62 AO DIA </li>" +
                                          "<li>PAGÁVEL EM QUALQUER AGÊNCIA BANCARIA ATÉ A DATA DO VENCIMENTO </li>" +
                                          "<li>APÓS O VENCIMENTO PAGÁVEL SOMENTE NAS AGÊNCIAS DO BRADESCO </li>" +
                                          "</ul>";

                }
                else if (titulo.BAN_ID == "604")
                {

                    instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                                          "<li>Sacador/Avalista:" + empresaSacadorAvalista.EMP_RAZAO_SOCIAL 
                                            + " - CNPJ:" + Convert.ToUInt64(empresaSacadorAvalista.EMP_CNPJ).ToString(@"00\.000\.000\/0000\-00") + "</li>" +
                                          "<li>" + empresaSacadorAvalista.EMP_LOGRADOURO + ", " + empresaSacadorAvalista.EMP_NUMERO + " - " + empresaSacadorAvalista.EMP_COMPLEMENTO 
                                            + " - " + empresaSacadorAvalista.MUN_DESCRICAO + "/" + empresaSacadorAvalista.UF + " - CEP: " + empresaSacadorAvalista.EMP_CEP + "<li>" +
                                          "<li>APOS O VENCIMENTO COBRAR 2% DE MULTA </li>" +
                                          "<li>APÓS O VENCIMENTO COBRAR R$ 0.13 AO DIA </li>" +
                                          "<li>TITULO CEDIDO AO BANCO INDUSTRIAL DO BRASIL. </li>" +
                                          "</ul>";

                }
                else
                {

                    instrucao.Descricao = "<ul style='font-size:8px;border:0px;border-style: none !important;'>" +
                        "                  <li>APOS O VENCIMENTO COBRAR 2% DE MULTA E MORA DE R$ " + string.Format("{0:0,0.00}", vlrMora) + " POR DIA DE ATRASO. </li>" +
                                          "<li>TITULO NEGOCIADO. SUJEITO A PROTESTO. " + parametro.msg + " </li></ul>";

                }

                cedente.Codigo = _ban_id == "041" ? StringUtil.PreencherZeroEsquerda(conta.CTA_CONVENIO, 13) : conta.CTA_CONVENIO;
                cedente.Convenio = Convert.ToInt32(cedente.Codigo);

                CorpoDTO boleto;

                if (_ban_id == "341")
                    boleto = new CorpoDTO(Convert.ToDateTime(titulo.PAR_DATA_VENCTO), Convert.ToDecimal(titulo.PAR_VLR_PARCELA), conta.CTA_CARTEIRA_BOLETO, nossoNumero, cedente, new EspecieDocumento(341, "1"));
                else
                    boleto = new CorpoDTO(Convert.ToDateTime(titulo.PAR_DATA_VENCTO), Convert.ToDecimal(titulo.PAR_VLR_PARCELA), conta.CTA_CARTEIRA_BOLETO, nossoNumero, digitoNossoNumero, cedente);

                boleto.NumeroDocumento = parametro.idTitulo;

                boleto.Sacado = sacado;
                boleto.Sacado.Endereco.End = endereco.END_LOGRADOURO + " " + endereco.END_NUMERO + " " + endereco.END_COMPLEMENTO;
                boleto.Sacado.Endereco.Bairro = endereco.END_BAIRRO;
                boleto.Sacado.Endereco.Cidade = endereco.END_MUNICIPIO;
                boleto.Sacado.Endereco.CEP = endereco.END_CEP;
                boleto.Sacado.Endereco.UF = endereco.END_UF;
                boleto.Instrucoes.Add(instrucao);
                boleto.EspecieDocumento = especie;
                

                if (_ban_id == "237")
                    boleto.LocalPagamento = "Pagável preferencialmente na Rede Bradesco ou Bradesco Expresso"; //+= " " + boleto.EspecieDocumento.Banco.Nome;
                else
                    boleto.LocalPagamento += " "; // + boleto.EspecieDocumento.Banco.Nome;

                montado.CodigoBanco = (short)Convert.ToDouble(_ban_id);
                montado.Boleto = boleto;
                montado.MostrarCodigoCarteira = (_ban_id == "033");
                montado.Boleto.Valida();
                montado.MostrarComprovanteEntrega = true;
            }

            return montado;
        }

        public string ConverterNossoNumero(string idTitulo)
        {
            string nn = "";
            int quantasLetrasNoTitulo = 0;

            foreach (char s in idTitulo)
            {
                if (quantasLetrasNoTitulo < 2) 
                {
                    if (Char.IsDigit(s))
                    {
                        nn += s;
                    }
                    else if (Char.IsLetter(s))
                    {
                        nn += Convert.ToInt32(s).ToString();
                        quantasLetrasNoTitulo++;
                    }
                }
            }

            int nrEnvio = _serviceParcelaAlocada.ObterNumeroEnvioBoleto(idTitulo);

            return nn + nrEnvio.ToString();
        }

        public string GerarNossoNumero(string bco, string idTitulo, bool comDV = true, bool avulso = true)
        {
            var _nossonumero = "";

            if (avulso)
            {
                _nossonumero = ConverterNossoNumero(idTitulo);
            }
            else
            {
                var titulo = _serviceParcela.FindById(idTitulo);

                if (titulo != null)
                {
                    if (!String.IsNullOrWhiteSpace(titulo.PAR_NOSSO_NUMERO) && titulo.PAR_NOSSO_NUMERO != idTitulo)
                        _nossonumero = Convert.ToInt64(this.tirarDV(titulo.PAR_NOSSO_NUMERO.Trim(), titulo.BAN_ID)).ToString();
                    else
                        _nossonumero = ConverterNossoNumero(idTitulo);
                }
            }

            _nossonumero = this.formatarNossoNumero(bco, _nossonumero, comDV);

            return _nossonumero;

        }

        private string tirarDV(string nn, string bc)
        {
            if (bc != "104")
                nn = nn.Substring(0, nn.Length - 1);

            return nn;
        }

        public string formatarNossoNumero(string bco, string nossoNumero, bool comDV = true, BoletoNet.Boleto boleto = null)
        {
            switch (bco)
            {
                case "033":
                        if (comDV)
                            nossoNumero = nossoNumero + Mod11Santander(nossoNumero.PadLeft(12, '0'), 9).ToString();
                        nossoNumero = nossoNumero.PadLeft(12, '0').Substring(0, 12);
                        break;
                case "237":
                        if (comDV)
                            nossoNumero = nossoNumero + CalcularDigitoNossoNumeroBradesco("09", nossoNumero.PadLeft(11, '0'));
                        nossoNumero = nossoNumero.PadLeft(11, '0').Substring(0, 11);
                        break;
                case "041":
                        if (comDV)
                            nossoNumero = NNDVBanrisul(nossoNumero);
                        nossoNumero = nossoNumero.PadLeft(08, '0').Substring(0, 8);
                        break;
                case "104":
                        if (comDV)
                            nossoNumero = FormataNossoNumeroCaixa(boleto);
                        nossoNumero = nossoNumero.PadLeft(09, '0').Substring(0, 9);
                        break;
                case "341":
                        if (comDV)
                        {
                            if (boleto.Carteira == "112")
                                nossoNumero += Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.Carteira + boleto.NossoNumero);
                            else if (boleto.Carteira != "126" && boleto.Carteira != "131"
                                 &&  boleto.Carteira != "146" && boleto.Carteira != "150"
                                 &&  boleto.Carteira != "168")
                                nossoNumero += Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
                            else
                                nossoNumero += Mod10(boleto.Carteira + boleto.NossoNumero);
                        }
                        nossoNumero = nossoNumero.PadLeft(08, '0').Substring(0, 8);
                        break;
            }

            return nossoNumero.Trim();
        }

        public string formatarNossoNumero(CnabRegistrosDTO CNABdto, string nossoNumero, bool comDV = true, BoletoNet.Boleto boleto = null)
        {
            string bco = CNABdto.BAN_ID;

            if (boleto == null)
            {
                string ag = this.retornarAgencia(CNABdto.CTA_AGENCIA);
                string dvAg = this.retornarDvAgencia(CNABdto.CTA_AGENCIA, bco);
                string cc = this.retornarConta(CNABdto.CTA_CONTA);
                string dvCc = this.retornarDvConta(CNABdto.CTA_CONTA);

                CedenteDTO cedente = new CedenteDTO(CNABdto.EMP_CNPJ, CNABdto.EMP_RAZAO_SOCIAL, ag, dvAg, cc, dvCc);
                cedente.Codigo = bco == "041" ? StringUtil.PreencherZeroEsquerda(CNABdto.CTA_CONVENIO, 13) : CNABdto.CTA_CONVENIO;
                cedente.Convenio = Convert.ToInt32(cedente.Codigo);
                boleto = new CorpoDTO(Convert.ToDateTime(CNABdto.PAR_DATA_VENCTO), Convert.ToDecimal(CNABdto.PAR_VLR_PARCELA), CNABdto.CTA_CARTEIRA_BOLETO, nossoNumero, cedente);
            }

            comDV = boleto == null ? false : comDV;
            if (bco == "033")
            {
                if (comDV)
                    nossoNumero = nossoNumero + Mod11Santander(nossoNumero.PadLeft(12, '0'), 9).ToString();
                nossoNumero = nossoNumero.PadLeft(12, '0');
            }
            if (bco == "237")
            {
                if (comDV)
                    nossoNumero = nossoNumero + CalcularDigitoNossoNumeroBradesco(boleto);
                nossoNumero = nossoNumero.PadLeft(08, '0');
            }
            if (bco == "041")
            {
                if (comDV)
                    nossoNumero = NNDVBanrisul(nossoNumero);
                nossoNumero = nossoNumero.PadLeft(08, '0');
            }
            if (bco == "104")
            {
                if (comDV)
                    nossoNumero = FormataNossoNumeroCaixa(boleto);
                nossoNumero = nossoNumero.PadLeft(09, '0');
            }
            if (bco == "341")
            {
                if (comDV)
                {
                    if (boleto.Carteira == "112")
                        nossoNumero += Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.Carteira + boleto.NossoNumero);
                    else if (boleto.Carteira != "126" && boleto.Carteira != "131"
                        && boleto.Carteira != "146" && boleto.Carteira != "150"
                        && boleto.Carteira != "168")
                        nossoNumero += Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
                    else
                        nossoNumero += Mod10(boleto.Carteira + boleto.NossoNumero);
                }
                nossoNumero = nossoNumero.PadLeft(08, '0');
            }
            return nossoNumero.Trim();
        }


        public static string BBModulo11(string codigoSemDv)
        {

            int pesoMaximo = 9, soma = 0, peso = 2;

            for (int i = (codigoSemDv.Length - 1); i >= 0; i--)
            {
                soma = soma + (Convert.ToInt32(codigoSemDv.Substring(i, 1)) * pesoMaximo);
                if (peso == pesoMaximo)
                    pesoMaximo = 9;
                else
                    pesoMaximo--;
            }

            var resto = (soma % 11);

            if (resto < 10)
                return resto.ToString();
            if (resto == 10)
                return "X";

            //if (resto <= 1 || resto > 9)
            //    return "1";

            return (11 - resto).ToString();
        }

        public static int Mod10(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, r;

            for (int i = seq.Length; i > 0; i--)
            {
                r = (Convert.ToInt32(Mid(seq, i, 1)) * p);

                if (r > 9)
                    r = (r / 10) + (r % 10);

                s += r;

                if (p == 2)
                    p = 1;
                else
                    p = p + 1;
            }
            d = ((10 - (s % 10)) % 10);
            return d;
        }

        //------------------------------------------------------------------
        public string FormataNossoNumeroCaixa(BoletoNet.Boleto boleto)
        {
            string NN = "";
            if (boleto != null)
            {
                if (boleto.Carteira.Equals("SR"))
                {
                    if (boleto.NossoNumero.Length == 14)
                    {
                        NN = "8" + boleto.NossoNumero;
                    }
                }
            }
            return string.Format("{0}-{1}", NN, Mod11Base9(NN)); //
        }
        public static int Mod11Base9(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int d, s = 0, p = 2, b = 9;


            for (int i = seq.Length - 1; i >= 0; i--)
            {
                string aux = Convert.ToString(seq[i]);
                s += (Convert.ToInt32(aux) * p);
                if (p >= b)
                    p = 2;
                else
                    p = p + 1;
            }

            if (s < 11)
            {
                d = 11 - s;
                return d;
            }
            else
            {
                d = 11 - (s % 11);
                if ((d > 9) || (d == 0))
                    d = 0;

                return d;
            }
        }


        //---------------------------------------------------------------------------------------\\
        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumeroBradesco(BoletoNet.Boleto boleto)
        {
            return boleto == null ? "" : Mod11Bradesco(boleto.Carteira + boleto.NossoNumero, 7);
        }
        public string CalcularDigitoNossoNumeroBradesco(string Carteira, string NossoNumero)
        {
            return Mod11Bradesco(Carteira + NossoNumero, 7);
        }
        private string Mod11Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO
            /* 
            Para o cálculo do dígito, será necessário acrescentar o número da carteira à esquerda antes do Nosso Número, 
            e aplicar o módulo 11, com base 7.
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma deverá ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferença entre o divisor e o resto, será o dígito de autoconferência: 11 - 3 = 8 (dígito de auto-conferência)
            
            Se o resto da divisão for “1”, desprezar o cálculo de subtração e considerar o dígito como “P”. 
            Se o resto da divisão for “0”, desprezar o cálculo de subtração e considerar o dígito como “0”.
            */
            #endregion

            /* Variáveis
             * -------------
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (int i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(Mid(seq, i, 1)) * p);
                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            int r = (s % 11);

            if (r == 0)
                return "0";
            else if (r == 1)
                return "P";
            else
                return (11 - r).ToString();
        }

        //--------------------------------------------------------------------------------------\\
        /// <summary>
        /// Cálculo do DVNN apenas do Banrisul.
        /// </summary>
        /// <param name="nossoNumero"></param>
        /// <returns></returns>
        public string NNDVBanrisul(String nossoNumero)
        {
            int dv1 = Mod10Banri(nossoNumero);
            int dv1e2 = Mod11Banri(nossoNumero, dv1); // O módulo 11 sempre devolve os dois Dígitos, pois, as vezes o dígito calculado no mod10 será incrementado em 1
            return nossoNumero + dv1e2.ToString("00");
        }
        private int Mod10Banri(string seq)
        {
            /* (N1*1-9) + (N2*2-9) + (N3*1-9) + (N4*2-9) + (N5*1-9) + (N6*2-9) + (N7*1-9) + (N8*2-9)
             * Observação:
             * a) a subtração do 9 somente será feita se o produto obtido da multiplicação individual for maior do que 9. 
             * b) quando o somatório for menor que 10, o resto da divisão por 10 será o próprio somatório. 
             * c) quando o resto for 0, o primeiro DV é igual a 0.
             */
            int soma = 0, resto, dv1, peso = 2, n, result;

            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                result = n * peso > 9 ? (n * peso) - 9 : n * peso;
                soma += result;
                if (peso == 2)
                    peso = 1;
                else
                    peso = 2;
            }

            if (soma < 10)
                resto = soma;
            else
                resto = soma % 10;
            dv1 = resto == 0 ? 0 : 10 - resto;
            return dv1;
        }
        private int Mod11Banri(string seq, int dv1)
        {
            /* Obter somatório (peso de 2 a 7), sempre da direita para a esquerda (N1*4)+(N2*3)+(N3*2)+(N4*7)+(N5*6)+(N6*5)+(N7*4)+(N8*3)+(N9*2)
             * Caso o somatório obtido seja menor que "11", considerar como resto da divisão o próprio somatório.
             * Caso o ''resto'' obtido no cálculo do módulo ''11'' seja igual a ''1'', considera-se o DV inválido. 
             * Soma-se, então, "1" ao DV obtido do módulo "10" e refaz-se o cálculo do módulo 11 . 
             * Se o dígito obtido pelo módulo 10 era igual a "9", considera-se então (9+1=10) DV inválido. 
             * Neste caso, o DV do módulo "10" automaticamente será igual a "0" e procede-se assim novo cálculo pelo módulo "11". 
             * Caso o ''resto'' obtido no cálculo do módulo "11" seja ''0'', o segundo ''NC'' será igual ao próprio ''resto''
             */
            int peso = 2, mult, sum = 0, rest, dv2, b = 7, n;
            seq += dv1.ToString();
            bool dvInvalido;
            for (int i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));
                mult = n * peso;
                sum += mult;
                if (peso < b)
                    peso++;
                else
                    peso = 2;
            }
            seq = seq.Substring(0, seq.Length - 1);
            rest = sum < 11 ? sum : sum % 11;
            if (rest == 1)
                dvInvalido = true;
            else
                dvInvalido = false;

            if (dvInvalido)
            {
                int novoDv1 = dv1 == 9 ? 0 : dv1 + 1;
                dv2 = Mod11Banri(seq, novoDv1);
            }
            else
            {
                dv2 = rest == 0 ? 0 : 11 - rest;
            }
            if (!dvInvalido)
            {
                string digitos = dv1.ToString() + dv2;
                return Convert.ToInt32(digitos);
            }
            else
            {
                return dv2;
            }
        }


        //-------------------------------------------------------------------------------------\\
        /// <summary>
        /// Cálculo do DVNN apenas do Santander.
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="lim"></param>
        /// <returns></returns>
        public static int Mod11Santander(string seq, int lim)
        {
            int ndig = 0;
            int nresto = 0;
            int total = 0;
            int multiplicador = 5;

            while (seq.Length > 0)
            {
                int valorPosicao = Convert.ToInt32(seq.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador--;

                if (multiplicador == 1)
                {
                    multiplicador = 9;
                }

                seq = seq.Remove(0, 1);
            }

            nresto = total - ((total / 11) * 11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }

        public string GerarProximoNossoNumero(ContaDTO conta)
        {
            if(conta != null && conta.CTA_GERA_NOSSO_NUMERO == true)
            {
                var banco = ConfigBoleto.GetBanco(conta.BAN_ID);
                
                if(banco != null && banco is INossoNumeroGenerator)
                {
                    var seqNossoNumero = _seqNossoNumeroSRV.RetornarSequencialDoNossoNumero(conta.BAN_ID, conta.EMP_ID);
                    var nossoNumero = ((INossoNumeroGenerator)banco).GerarNossoNumero(conta, seqNossoNumero);

                    return nossoNumero;
                }

            }

            return null;
        }
    }
}
