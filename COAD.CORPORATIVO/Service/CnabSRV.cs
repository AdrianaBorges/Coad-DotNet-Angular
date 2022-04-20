using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using COAD.CORPORATIVO.Service.Boleto;
using COAD.CORPORATIVO.Model.Dto.Boleto;
using GenericCrud.Util;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Service.Custons;
using COAD.SEGURANCA.Service.Custons;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Planilhas;
using System.Text.RegularExpressions;
using COAD.CORPORATIVO.Util;
using System.IO;
using GenericCrud.Excel.Impl;
using System.Web;
using COAD.SEGURANCA.Model.Custons;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CNB_ID")]
    public class CnabSRV : GenericService<CNAB, CnabDTO, int>
    {
        public ContaSRV _serviceConta { get; set; }
        public EmpresaSRV _serviceEmpresa { get; set; }
        public ClienteSRV _serviceCliente { get; set; }
        public ParcelasSRV _serviceParcela { get; set; }
        public BoletoSRV _serviceBoleto { get; set; }
        public CnabRegistrosSRV _serviceCnabRegistros { get; set; }
        public ItemPedidoSRV _serviceItemPedido { get; set; }
        public PedidoCRMSRV _servicePedidoCRM { get; set; }
        public AssinaturaSRV _serviceAssinatura { get; set; }
        public ParcelasRemessaSRV _parcelaRemessaSRV { get; set; }

        public PropostaSRV _serviceProposta { get; set; }
        public PropostaItemSRV _servicePropostaItem { get; set; }

        public BatchCustomSRV _batchSRV = new BatchCustomSRV();
        public CnabTipoDadosSRV _cnabTipoDadosSRV { get; set; }
        public CnabDAO _dao { get; set; }

        public CnabSRV(CnabDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public CnabSRV()
        {
            _dao = new CnabDAO();

            _serviceConta = new ContaSRV();
            _serviceEmpresa = new EmpresaSRV();
            _serviceCliente = new ClienteSRV();
            _serviceParcela = new ParcelasSRV();
            _serviceBoleto = new BoletoSRV();
            _serviceCnabRegistros = new CnabRegistrosSRV();
            _serviceItemPedido = new ItemPedidoSRV();
            _servicePedidoCRM = new PedidoCRMSRV();
            _serviceAssinatura = new AssinaturaSRV();

            _serviceProposta = new PropostaSRV(new PropostaDAO());
            _servicePropostaItem = new PropostaItemSRV(new PropostaItemDAO());

            Dao = _dao;
        }

        public ParametroDTO prepararParametro(string idTitulo, int idConta, bool preAlocado, string msg = "", DateTime? dtVencimento = null, Decimal? vlrBoleto = null)
        {
            try
            {
                var parcela = _serviceParcela.FindById(idTitulo);

                if (parcela == null)
                    throw new Exception("Parcela não encontrada (prepararParametro)!!");

                if (parcela.EMP_ID == null)
                    throw new Exception("Empresa não informada na parcela (prepararParametro)!!");

                var parametro = new ParametroDTO();

                parametro.BANCO = null;

                if (parcela.BAN_ID == "999")
                {
					
                    parcela.BAN_ID = "237";
					
                    parcela.CTA_ID = 94;
                    parametro.BANCO = parcela.BAN_ID;
                }

                if (parcela.BAN_ID == "998")
                {

                    parcela.BAN_ID = (parcela.CTA_ID != 125 ? "237" : "422");

                    parcela.CTA_ID = 123;
                    if (parcela.CTA_ID != 125) parcela.CTA_ID = 123;

                    parametro.BANCO = parcela.BAN_ID;

                }

                if (parcela.BAN_ID == "604")
                {
                    parcela.CTA_ID = 104;
                    parametro.BANCO = parcela.BAN_ID;
                }

                int idCliente = 0;

                var assinatura = _serviceAssinatura.BuscarAssinaturaPorContrato(parcela.CTR_NUM_CONTRATO);

                if (assinatura != null)
                {
                    idCliente = (int)assinatura.CLI_ID;
                }
                else
                {
                    var itemPedido = _serviceItemPedido.FindById(parcela.IPE_ID);
                    if (itemPedido != null)
                    {
                        var pedidoCRM = _servicePedidoCRM.FindById(itemPedido.PED_CRM_ID);
                        if (pedidoCRM != null)
                        {
                            idCliente = (int)pedidoCRM.CLI_ID;
                        }
                    }
                    else
                    {
                        var propostaItem = _servicePropostaItem.FindById(parcela.PPI_ID);
                        if (propostaItem != null)
                        {
                            var proposta = _serviceProposta.FindById(propostaItem.PRT_ID);
                            if (proposta != null)
                            {
                                idCliente = (int)proposta.CLI_ID;
                            }
                        }

                    }

                }

                idConta = (idConta > 0) ? idConta : (int)parcela.CTA_ID;

                if (idCliente == 0)
                    throw new Exception("Cliente não informado na parcela (prepararParametro)!!");
               
                if (idConta == 0)
                    throw new Exception("Conta não informada na parcela (prepararParametro)!!");

                var cta = _serviceConta.FindById(idConta);

                if (cta == null)
                    throw new Exception("Conta não informada na parcela (prepararParametro)!!");

                var banco = cta.CTA_CEDENTE_EMITE_BOLETO ? cta.BAN_ID : parcela.BAN_ID;
          
                parametro.idCliente = idCliente;
                parametro.idConta = idConta;
                parametro.idEmpresa = (int)parcela.EMP_ID;
                parametro.idTitulo = idTitulo;
                parametro.dtVencimento = dtVencimento;
                parametro.vlrBoleto = vlrBoleto;
                parametro.idRemessa = "01";
                parametro.preAlocado = preAlocado;
                parametro.msg = msg;

                return parametro;

            }
            catch (Exception e)
            {
                throw e; 
            }

        }

        public List<CnabDTO> BuscarDetalheCNAB(CnabDTO _Cnab)
        {
            return _dao.BuscarDetalheCNAB(_Cnab).ToList();
        }


        public List<CnabDTO> BuscarCNAB()
        {
            return _dao.BuscarCNAB();
        }

        private void AdicionarDadosSacadorAvalista(IList<CnabRegistrosDTO> lstRegistroRemessa)
        {
            if(lstRegistroRemessa != null && lstRegistroRemessa.Count > 0)
            {
                var registroRemessa = lstRegistroRemessa.FirstOrDefault();
                var remessa = _parcelaRemessaSRV.FindById(registroRemessa.REM_ID);
                if (remessa != null && remessa.REM_SACADOR_AVALISTA == true)
                {
                    var conta = _serviceConta.FindById(registroRemessa.CTA_ID);

                    if (conta != null && conta.EMP_ID_S_AVS != null)
                    {
                        var empresa = _serviceEmpresa.FindById(conta.EMP_ID_S_AVS);

                        foreach(var cnabRegistro in lstRegistroRemessa)
                        {
                            cnabRegistro.SACADOR_AVALISTA_CNPJ = empresa.EMP_CNPJ;
                            cnabRegistro.SACADOR_AVALISTA_RAZAO_SOCIAL = empresa.EMP_RAZAO_SOCIAL;
                        }
                    }
                }
            }
        }

        public List<CnabRegistrosDTO> SelecionarRegistrosCNAB(int _rem_id, bool preAlocado = false)
        {
            return _serviceCnabRegistros.SelecionarRegistrosCNAB(_rem_id,  preAlocado);
        }

        public string gerarArquivoCNAB( int? empresa_id = null
                                      , int? regs_empresa_id = null
                                      , string banco_id = null
                                      , string leiaute = "400"
                                      , int? remessa = null
                                      , bool preAlocado = false
                                      , string tipoRemessa = null)
        {
            /////////////////////
            // reg-0           //
            // loop-           //
            //      reg-1 a 8  //
            // -loop           //
            // reg-9           //
            /////////////////////


            string conteudo = "";
            int cnab;

            List<CnabRegistrosDTO> registroRemessa = this.SelecionarRegistrosCNAB((int)remessa, preAlocado);

            AdicionarDadosSacadorAvalista(registroRemessa);
            int qtdRegistros = registroRemessa.Count();
            bool temRegistros = (qtdRegistros > 0);
            try
            {

                Boolean CNAB_ok = int.TryParse(leiaute, out cnab);
                if (CNAB_ok && temRegistros)
                {
                    int nrSequencial = 0;
                    decimal totalTitulos = 0;

                    if (leiaute == "400")
                    {
                        // reg-0 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        CnabRegistrosDTO primeiroRegistro = registroRemessa.FirstOrDefault();
                        string arquivoModelo = "1REMESSA";

                        var campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "0", null); // pegando os campos do registro...
                        if (campos != null && campos.Count() > 0)
                        {
                            conteudo = "01REMESSA";
                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, primeiroRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;
                        }
                        else
                        {
                            return "CNAB " + leiaute + " do Banco [" + banco_id + "] Modelo [" + arquivoModelo + "] não encontrado!";
                        }

                        // reg-1 a 8 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        foreach (var registro in registroRemessa) // lendo enquanto há registros no BANCO DE DADOS...
                        {
                            for (int reg = 1; reg <= 8; reg++)
                            {
                                campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, reg.ToString(), null);

                                if (campos != null && campos.Count() > 0)
                                {
                                    conteudo = this.LerLinhaArquivoCNAB(campos, cnab, registro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                                    conteudo += Environment.NewLine;
                                    nrSequencial++;
                                }
                            }
                            totalTitulos += Convert.ToDecimal(registro.PAR_VLR_PARCELA);
                        }

                        // reg-9 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        CnabRegistrosDTO ultimoRegistro = registroRemessa.LastOrDefault();
                        campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "9", null); // pegando os campos do registro...

                        if (campos != null && campos.Count() > 0)
                        {
                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, ultimoRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;
                        }

                    }
                    else
                    {

                        string arquivoModelo = "1REMESSA";

                        // reg-0 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        CnabRegistrosDTO primeiroRegistro = registroRemessa[0];

                        var campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "0", null); // pegando os campos do registro...
                        if (campos != null && campos.Count() > 0)
                        {

                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, primeiroRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;

                        }
                        else
                        {

                            return "CNAB " + leiaute + " do Banco [" + banco_id + "] Modelo [" + arquivoModelo + "] não encontrado!";

                        }

                        // reg-3 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        //CnabRegistrosDTO segundoRegistro = registroRemessa[1];
                        campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "3", null); // pegando os campos do registro...
                        if (campos != null && campos.Count() > 0)
                        {

                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, primeiroRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;

                        }
                        else
                        {

                            return "CNAB " + leiaute + " do Banco [" + banco_id + "] Modelo [" + arquivoModelo + "] não encontrado!";

                        }


                        // reg-4 a 7 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        foreach (var registro in registroRemessa) // lendo enquanto há registros no BANCO DE DADOS...
                        {

                            for (int reg = 4; reg < 8; reg++)
                            {

                                campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, reg.ToString(), null);

                                if (campos != null && campos.Count() > 0)
                                {

                                    conteudo = this.LerLinhaArquivoCNAB(campos, cnab, registro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                                    conteudo += Environment.NewLine;
                                    nrSequencial++;

                                }

                            }

                            totalTitulos += Convert.ToDecimal(registro.PAR_VLR_PARCELA);

                        }

                        // reg-8 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        //CnabRegistrosDTO penultimoRegistro = registroRemessa[registroRemessa.Count() - 2];
                        CnabRegistrosDTO ultimoRegistro = registroRemessa[registroRemessa.Count() - 1];
                        campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "8", null); // pegando os campos do registro...

                        if (campos != null && campos.Count() > 0)
                        {

                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, ultimoRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;

                        }

                        // reg-9 ////////////////////////////////////////////////////////////////////////////////////////////////////
                        //CnabRegistrosDTO ultimoRegistro = registroRemessa[registroRemessa.Count() - 1];
                        campos = this.LerCNAB(empresa_id, banco_id, leiaute, arquivoModelo, "9", null); // pegando os campos do registro...

                        if (campos != null && campos.Count() > 0)
                        {

                            conteudo = this.LerLinhaArquivoCNAB(campos, cnab, ultimoRegistro, nrSequencial, conteudo, totalTitulos, preAlocado, tipoRemessa, qtdRegistros);
                            conteudo += Environment.NewLine;
                            nrSequencial++;

                        }

                    }

                }

                return conteudo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private string LerLinhaArquivoCNAB(IList<CnabDTO> campos
                                          ,int cnab
                                          ,CnabRegistrosDTO registroRemessa
                                          ,int nrSequencial
                                          ,string conteudo
                                          ,decimal totalTitulos
                                          ,bool preAlocado
                                          ,string tipoRemessa = null
                                          ,int qtdTotalTitulos = 0)
        {
            try
            {
                var campo = campos.ToArray();
                int nrCampo = 0;
                int coluna = 0;
                //int sequencialTitulo = 0;
                //int incrementei = 0;

                while (coluna <= cnab && nrCampo < campo.Length)
                {
                    string dado = campo[nrCampo].CNB_CONTEUDO;
                    if (dado.Substring(0, 1) == "#") 
                    {
                        int i = dado.IndexOf(".");
                        if (i > -1)
                        {
                            string cp = dado.Substring(i, dado.Length - i);
                            var informacao = this.informacaoDoCampo(registroRemessa, cp.Substring(1, cp.Length - 1));
                            dado = informacao == null ? "" : informacao.ToString();

                            if (campo[nrCampo].CNB_TIPO == "N")
                            {
                                int p = dado.IndexOf(".");
                                if (p >= 0)
                                {
                                    string a = dado.Substring(0, p);
                                    string d = this.Preencher(dado.Substring(p + 1, dado.Length - (p + 1)), "D", campo[nrCampo].CNB_DECIMAL, '0');
                                    dado = a + d;
                                }
                                dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            }
                            if (campo[nrCampo].CNB_TIPO == "T")
                                dado = this.Preencher(dado, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                            if (campo[nrCampo].CNB_TIPO == "D")
                            {
                                if (!String.IsNullOrWhiteSpace(dado))
                                {
                                    DateTime d = Convert.ToDateTime(dado);
                                    dado = d.ToString("ddMMyy");
                                }
                                else
                                {
                                    dado = this.Preencher(" ", "D", 6, ' ');
                                }
                            }
                        }
                        else 
                        {
                            if (dado == "#TipoRemessa")
                                dado = this.Preencher(tipoRemessa, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            if (dado == "#LerNossoNumero")
                            {
                                if (registroRemessa.CTA_CEDENTE_EMITE_BOLETO
                                    && registroRemessa.BAN_ID != "604")
                                {
                                    char str = campo[nrCampo].CNB_TIPO == "T" ? ' ' : '0';
                                    string lado = campo[nrCampo].CNB_TIPO == "T" ? "D" : "E";
                                    dado = registroRemessa.PAR_NOSSO_NUMERO;
                                    dado = this.Preencher(dado, lado, campo[nrCampo].CNB_TAMANHO, str);
                                }
                                else
                                {
                                    dado = this.Preencher("0", "E", campo[nrCampo].CNB_TAMANHO, '0');
                                }
                            }
                            if (dado == "#CalcularNossoNumeroSafra")
                                dado = PreencherNossoNumeroSafra( (int) registroRemessa.CTA_ID, registroRemessa.PAR_NUM_PARCELA );
                            if (dado == "#DataAtualCNAB")
                                dado = this.LerDataAtualCNAB();
                            if (dado == "#PreencheZero")
                                dado = this.Preencher("0", "E", campo[nrCampo].CNB_TAMANHO, '0');
                            if (dado == "#PreencheBranco")
                                dado = this.Preencher(" ", "E", campo[nrCampo].CNB_TAMANHO, ' ');
                            if (dado == "#SequencialArquivo")
                                dado = this.LerSequencial((int)registroRemessa.CTA_NR_ARQ_ENVIADO, campo[nrCampo].CNB_TAMANHO);
                            if (dado == "#SequencialRegistro")
                                dado = this.LerSequencial(nrSequencial, campo[nrCampo].CNB_TAMANHO);
                            if (dado == "#QuantidadeTitulos")
                                dado = this.Preencher(qtdTotalTitulos.ToString(), "E", campo[nrCampo].CNB_TAMANHO, '0');
                            if (dado == "#EmissorBoletos")
                                //   dado = registroRemessa.CTA_CEDENTE_EMITE_BOLETO ? this.Preencher("2", "E", campo[nrCampo].CNB_TAMANHO, '0') : this.Preencher("1", "E", campo[nrCampo].CNB_TAMANHO, '0');
                                dado = this.Preencher("1", "E", campo[nrCampo].CNB_TAMANHO, '0');
                            if (dado == "#TipoInscricaoCedente")
                                dado = registroRemessa.EMP_CNPJ.ToString().Length == 11 ? "01" : "02";
                            if (dado == "#TipoInscricaoCliente")
                                dado = registroRemessa.CLI_CPF_CNPJ.ToString().Length == 11 ? "01" : "02";
                            if (dado == "#MoraDiaria")
                            {
                                dado = registroRemessa.moraDiaria.ToString();

                                int p = dado.IndexOf(".");
                                if (p >= 0)
                                {
                                    string a = dado.Substring(0, p);
                                    string d = this.Preencher(dado.Substring(p + 1, dado.Length - (p + 1)), "D", campo[nrCampo].CNB_DECIMAL, '0');
                                    dado = a + d;
                                }

                                dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            }
                            if (dado == "#MultaApartirDe")
                            {
                                DateTime d = (DateTime)registroRemessa.PAR_DATA_VENCTO;
                                dado = this.LerDataCNAB(d.AddDays(1));
                            }
                            if (dado == "#MultaValor")
                            {
                                dado = (((registroRemessa.PAR_VLR_PARCELA) * 2) / 100).ToString();

                                int p = dado.IndexOf(".");
                                if (p >= 0)
                                {
                                    string a = dado.Substring(0, p);
                                    string d = this.Preencher(dado.Substring(p + 1, dado.Length - (p + 1)), "D", campo[nrCampo].CNB_DECIMAL, '0');
                                    dado = a + d;
                                }

                                dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            }
                            if (dado == "#ValorTotalTitulos")
                            {
                                dado = totalTitulos.ToString();

                                int p = dado.IndexOf(".");
                                if (p >= 0)
                                {
                                    string a = dado.Substring(0, p);
                                    string d = this.Preencher(dado.Substring(p + 1, dado.Length - (p + 1)), "D", campo[nrCampo].CNB_DECIMAL, '0');
                                    dado = a + d;
                                }

                                dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            }
                            if (dado == "#EnderecoSacado")
                            {
                                string endSacado = registroRemessa.ENDERECO_SACADO != null ? registroRemessa.ENDERECO_SACADO.ToString() : "";
                                dado = this.Preencher(endSacado, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                            }
                            if (dado == "#EnderecoCedente")
                            {
                                string endCedente = registroRemessa.ENDERECO_CEDENTE != null ? registroRemessa.ENDERECO_CEDENTE.ToString() : "";
                                dado = this.Preencher(endCedente, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                            }
                            if (dado == "#SacadorAvalista")
                            {
                                var _empresa = _serviceEmpresa.FindById(registroRemessa.CTA_ALOCAR_TITULO_DA_EMP_ID);
                                if (_empresa != null)
                                {
                                    dado = this.Preencher(_empresa.EMP_RAZAO_SOCIAL + " CNPJ:" + _empresa.EMP_CNPJ, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                                }
                            }
                            if (dado == "#SacadorAvalistaCNPJ")
                            {
                                string endSacado = registroRemessa.SACADOR_AVALISTA_CNPJ != null ? registroRemessa.SACADOR_AVALISTA_CNPJ.ToString() : "";
                                dado = this.Preencher(endSacado, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                            }
                            if (dado == "#SacadorAvalistaRazaoSocial")
                            {
                                string endSacado = registroRemessa.SACADOR_AVALISTA_RAZAO_SOCIAL != null ? registroRemessa.SACADOR_AVALISTA_RAZAO_SOCIAL.ToString() : "";
                                dado = this.Preencher(endSacado, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                            }
                            if (dado == "#DataAtualCNABLonga")
                                dado = this.LerDataAtualCNABLonga();
                            if (dado == "#SequencialTitulo")
                                dado = String.Format( "{0:00000}", nrSequencial - 1);
                            if (dado == "#Parcela")
                                dado = "0" + registroRemessa.PAR_NUM_PARCELA.Substring(registroRemessa.PAR_NUM_PARCELA.Length - 1, 1);
                            if (dado == "#DataVencimentoLonga")
                                dado = this.LerDataLonga( (DateTime) registroRemessa.PAR_DATA_VENCTO );
                            if (dado == "#Mora")
                            {
                                dado = registroRemessa.CTA_PERC_MORA_MES.ToString();

                                int p = dado.IndexOf(".");
                                if (p >= 0)
                                {
                                    string a = dado.Substring(0, p);
                                    string d = this.Preencher(dado.Substring(p + 1, dado.Length - (p + 1)), "D", campo[nrCampo].CNB_DECIMAL, '0');
                                    dado = a + d;
                                }

                                dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                            }
                            if (dado == "#MultaApartirDeLonga")
                            {

                                DateTime d = (DateTime)registroRemessa.PAR_DATA_VENCTO;
                                
                                dado = this.LerDataLonga(d.AddDays(1));

                            }
                            if (dado == "#TipoInscricaoClienteCurto")
                                dado = registroRemessa.CLI_CPF_CNPJ.ToString().Length == 11 ? "1" : "2";
                            if (dado == "#QuantidadeRegistros")
                                dado = String.Format("{0:00000}", nrSequencial - 2);

                        }
                    }
                    else 
                    {
                        if (campo[nrCampo].CNB_TIPO == "N")
                            dado = this.Preencher(dado, "E", campo[nrCampo].CNB_TAMANHO, '0');
                        if (campo[nrCampo].CNB_TIPO == "T")
                            dado = this.Preencher(dado, "D", campo[nrCampo].CNB_TAMANHO, ' ');
                        if (campo[nrCampo].CNB_TIPO == "D" && String.IsNullOrWhiteSpace(dado))
                            dado = this.Preencher(" ", "D", 6, ' ');
                    }
                    coluna += dado.Length;
                    conteudo += dado;
                    nrCampo++;
                }

                return conteudo;
            }
            catch (Exception ex)
            {
                //    throw new Exception(SysException.Show(ex));
                return conteudo + registroRemessa.PAR_NUM_PARCELA + " Erro: " + ex.Message;

            }
        }

        public Object informacaoDoCampo(Object DTO, string campo)
        {
            if (DTO.GetType().GetProperty(campo) != null)
            {
                System.Reflection.PropertyInfo cp = null;
                cp = DTO.GetType().GetProperty(campo);
                var texto = cp.GetValue(DTO, null) as object;
                return texto;
            }
            return null;
        }

        public string LerDataAtualCNAB()
        {
            DateTime d = DateTime.Now;
            return d.ToString("ddMMyy");
        }

        public string LerDataCNAB(DateTime dt)
        {
            return dt.ToString("ddMMyy");
        }

        public string Preencher(string texto = null, string onde = "D", int? tamanho = null, char com = ' ')
        {
            string retorno = "";

            texto = StringUtil.RetirarCaractereEspecial(texto);

            texto = texto == null ? "" : texto;
            tamanho = (tamanho == null) ? texto.Length : tamanho;

            if (onde == "E")
                retorno = texto.PadLeft((int)tamanho, com);
            else
                retorno = texto.PadRight((int)tamanho, com);

            return retorno.Substring(0, (int)tamanho);
        }

        public string LerSequencial(int numeroInicial = 0, int tamanho = 6)
        {
            numeroInicial++;
            return numeroInicial.ToString().PadLeft(tamanho, '0');
        }

        public IList<CnabDTO> LerCNAB(int? empresa = null, string banco = null, string leiaute = null, string arquivo = null, string registro = null, string campo = null)
        {
            return _dao.LerCNAB(empresa, banco, leiaute, arquivo, registro, campo);
        }
        public Pagina<CnabDTO> LerCNAB(int? empresa = null, string banco = null, string leiaute = null, string arquivo = null, string registro = null, string campo = null, int pagina = 1, int itensPorPagina = 10)
        {
            return _dao.LerCNAB(empresa, banco, leiaute, arquivo, registro, campo, pagina, itensPorPagina);
        }

        public void SalvarCNAB(IList<CnabDTO> cnab)
        {
            try
            {
                foreach (var cnb in cnab)
                {
                    // informa se a empresa é do Grupo COAD \\
                    cnb.EMP_GRP_COAD = SessionContext.empresa_do_grupo_coad;

                    if (cnb.CNB_ID != null)
                    {
                        cnb.DATA_ALTERACAO = DateTime.Now;
                        Merge(cnb, "CNB_ID");
                    }
                    else
                    {
                        cnb.DATA_CADASTRO = DateTime.Now;
                        Save(cnb);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }

        public CnabDTO DeletarCNAB(int cnabId)
        {
            var cnab = this.FindById(cnabId);
            cnab.DATA_EXCLUSAO = DateTime.Now;
            return Merge(cnab, "CNB_ID");
        }
        
        public void SalvarEExcluirCnab(CnabConfigArquivoDTO cnabConfigArq)
        {
            var cnabs = cnabConfigArq.CNAB;
            if (cnabs != null)
            {
                ExcluirCnab(cnabConfigArq);
                SalvarCnab(cnabs, cnabConfigArq);
            }

        }

        public void SalvarCnab(IEnumerable<CnabDTO> cnabs, CnabConfigArquivoDTO cnabConfigArq)
        {
            CheckAndAssignKeyFromParentToChildsList(cnabConfigArq, cnabs, "CCA_ID");

            if(cnabs != null)
            {
                foreach(var cnab in cnabs)
                {
                    if (cnab.CNB_ID == null)
                        cnab.DATA_CADASTRO = DateTime.Now;
                    else
                        cnab.DATA_ALTERACAO = DateTime.Now;
                }
            }
            SaveOrUpdateAll(cnabs);
        }


        public void ExcluirCnab(CnabConfigArquivoDTO cnabConfigArq)
        {
            if (cnabConfigArq.DATA_EXCLUSAO == null)
            {
                var ccaId = cnabConfigArq.CCA_ID;
                var nfConfigBanco = ServiceFactory.RetornarServico<CnabConfigArquivoSRV>().FindByIdFullLoaded(ccaId, true);
                var lstConfigImposto = GetMissinList(cnabConfigArq, nfConfigBanco, "CNAB");
                DeletarCnab(lstConfigImposto);
                
            }
        }

        public void DeletarCnab(IEnumerable<CnabDTO> cnabs)
        {
            if (cnabs != null)
            {
                foreach (var regTab in cnabs)
                {
                    regTab.DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(cnabs);
            }
        }

        public IList<CnabDTO> ListarCnabsDoCnabConfigArquivo(int? ccaId)
        {
            return _dao.ListarCnabsDoCnabConfigArquivo(ccaId);
        }

        public void PreencherCnabsNaConfig(CnabConfigArquivoDTO cnabConfig)
        {
            if (cnabConfig != null)
            {
                cnabConfig.CNAB = ListarCnabsDoCnabConfigArquivo(cnabConfig.CCA_ID);
            }
        }

        public ICollection<CnabDTO> InserirAtualizarPlanilhaCarga(string path, HttpPostedFileBase arquivo)
        {

            if (arquivo != null)
            {
                var fileName = string.Format(@"{0}temp\{1}", path, arquivo.FileName);

                arquivo.SaveAs(fileName);
                IList<PlanilhaCnab> lstDadosPla = new List<PlanilhaCnab>();
                using (var excel = new ExcelProxyOpenXML(fileName))
                {
                    lstDadosPla = excel.ToDTO<PlanilhaCnab>();
                }

                File.Delete(fileName);
                UploadUtil.LimparObjetoDeUpload();

                if (lstDadosPla != null)
                {
                    var result = _converterPlanilhaParaCnab(lstDadosPla);
                    return result;
                }
            }

            return new List<CnabDTO>();
        }

        private void _extrairTipoTamanhoDecimal(PlanilhaCnab pla, CnabDTO cnab)
        {
            if (string.IsNullOrWhiteSpace(pla.Tipo))
                return;

            string tipoPlanilha = pla.Tipo;
            string tipo = null;
            bool somarDecimal = false;
            int? decimais = null;
            int tamanho = 0;

            var regex = new Regex(@"(.)\((\d*)\)");
            if (regex.IsMatch(tipoPlanilha) && cnab != null)
            {
                var resultRegex = regex.Match(tipoPlanilha);

                if (resultRegex != null && resultRegex.Groups.Count >= 3)
                {
                    var valorTipo = resultRegex.Groups[1].Value;
                    var tamanhoStr = resultRegex.Groups[2].Value;
                    int.TryParse(tamanhoStr, out tamanho);

                    switch (valorTipo)
                    {
                        case "X":
                            tipo = "T";
                            break;
                        case "9":
                            tipo = "N";
                            break;
                        case "D":
                            tipo = "D";
                            break;
                        default:
                            tipo = "T";
                            break;
                    }
                    
                }
                // Buscando os decimais

                var regexDecimalPadrao1 = new Regex(@"(.)\((\d*)\)(([Vv]9)\((\d+)\))");

                if (regexDecimalPadrao1.IsMatch(tipoPlanilha))
                {
                    var busca = regexDecimalPadrao1.Match(tipoPlanilha);
                    if (busca != null && busca.Groups.Count >= 6)
                    {
                        var padraoDecimal = busca.Groups[5].Value;
                        int vlrDecimal = 0;
                        int.TryParse(padraoDecimal, out vlrDecimal);

                        if (vlrDecimal > 0)
                            decimais = vlrDecimal;
                    }
                }                
                else
                {
                    var regexDecimalPadrao2 = new Regex(@"(.)\((\d*)\)([Vv](9+))");

                    if (regexDecimalPadrao2.IsMatch(tipoPlanilha))
                    {
                        var busca = regexDecimalPadrao2.Match(tipoPlanilha);
                        if (busca != null && busca.Groups.Count >= 5)
                        {
                            var padraoDecimal = busca.Groups[4].Value;
                            decimais = padraoDecimal.Count();
                        }
                    }
                }

                if (decimais != null)
                    somarDecimal = true;

            }
            else
            {
                if (_cnabTipoDadosSRV.TipoExiste(tipoPlanilha))
                {
                    tipo = tipoPlanilha;
                }

                if(pla.Tamanho != null && pla.Tamanho > 0)
                {
                    tamanho = (int) pla.Tamanho;
                }
                else
                {
                    tamanho = (int) (pla.PosicaoInicial - pla.PosicaoFinal) + 1;
                }
                
                decimais = pla.Decimais;
            }

            cnab.CNB_TIPO = tipo;
            cnab.CNB_TAMANHO = tamanho;
            cnab.CNB_DECIMAL = decimais;

            if (somarDecimal && decimais != null)
                cnab.CNB_TAMANHO += (int)decimais;
        }

        private ICollection<CnabDTO> _converterPlanilhaParaCnab(ICollection<PlanilhaCnab> lstPlanilha)
        {
            ICollection<CnabDTO> lstCnab = new List<CnabDTO>();

            if (lstPlanilha != null && lstPlanilha.Count > 0)
            {
                foreach (var pla in lstPlanilha)
                {
                    CnabDTO cnab = null;

                    if (pla.Codigo != null)
                        cnab = FindById(pla.Codigo);
                    else
                        cnab = new CnabDTO();

                    cnab.CNB_CAMPO = pla.Campo;
                    cnab.CNB_CONTEUDO = pla.Conteudo;
                    cnab.CNB_INICIO = (pla.PosicaoInicial != null) ? (int)pla.PosicaoInicial : 0;
                    cnab.CNB_FINAL = (pla.PosicaoFinal != null) ? (int)pla.PosicaoFinal : 0;
                    cnab.DATA_CADASTRO = DateTime.Now;
                    _extrairTipoTamanhoDecimal(pla, cnab);
                    lstCnab.Add(cnab);
                }
            }

            return lstCnab;
        }

        private ICollection<PlanilhaCnab> _converterPraDTOPlanilha(ICollection<CnabDTO> lstCnab)
        {
            ICollection<PlanilhaCnab> lstDadosPlan = new List<PlanilhaCnab>();

            if(lstCnab != null && lstCnab.Count > 0)
            {
                lstDadosPlan = lstCnab.Select(x => new PlanilhaCnab()
                {
                    Codigo = x.CNB_ID,
                    Campo = x.CNB_CAMPO,
                    Conteudo = x.CNB_CONTEUDO,
                    PosicaoInicial = x.CNB_INICIO,
                    PosicaoFinal = x.CNB_FINAL,
                    Tamanho = x.CNB_TAMANHO,
                    Tipo = x.CNB_TIPO,
                    Decimais = x.CNB_DECIMAL
                    
                }).ToList();
            }

            return lstDadosPlan;
        }

        public FileInfoDTO RetornarPlanilhaCnab(int? ccaId, string path)
        {
            try
            {
                var lstCnabs = ListarCnabsDoCnabConfigArquivo(ccaId);

                if (lstCnabs != null && lstCnabs.Count > 0)
                {
                    var lstPlan = _converterPraDTOPlanilha(lstCnabs);

                    if (lstPlan != null && lstCnabs.Count > 0)
                    {
                        var fileName = string.Format(@"{0}download\cnabs-update-{1:yyyy-MM-dd hh-mm-ss}.xlsx", path, DateTime.Now);

                        using (ExcelProxyOpenXML excelLoad = new ExcelProxyOpenXML())
                        {
                            excelLoad.ToSheet(fileName, lstPlan);
                        };

                        var bytes = File.ReadAllBytes(fileName);
                        var downloadInfo = new FileInfoDTO()
                        {
                            Path = fileName,
                            Bytes = bytes
                        };

                        File.Delete(fileName);
                        return downloadInfo;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Não é possível gerar a planilha", e);
            }
        }

        public byte[] GerarBoleto2aVia(string idTitulo)
        {

            int _cta_id = 0;
            bool _avulso = false;

            var parametro = prepararParametro(idTitulo, _cta_id, _avulso);
            var parcela = _serviceParcela.FindById(idTitulo);

            if(parcela != null)
            {
                parcela.PAR_BOLETO_AVULSO = false;
                _serviceParcela.Merge(parcela);
            }

            if (parametro != null)
            {
                parametro.segVia = true;
                List<ParametroDTO> lstParametro = new List<ParametroDTO>();
                lstParametro.Add(parametro);
                var bytes = _serviceBoleto.GerarVariosBoletosPDF(lstParametro);
                return bytes;
            }
            else
            {
                throw new Exception("O Cliente do título (" + idTitulo + ") não foi localizado pelo gerador de [parâmetros] do Boleto!");
            }
        }

        public string PreencherNossoNumeroSafra (int CTA_ID, string PAR_NUM_PARCELA)
        {

            string retorno = "000000019";

            var cta = _serviceConta.FindById(CTA_ID);

            long? ultimoNumero = cta.CTA_ULTIMO_NOSSO_NUMERO + 1;

            if ((ultimoNumero != null) && (_serviceConta.incrementarUltimoNossoNumeroDeConta(CTA_ID)))
            {

                retorno = String.Format("{0:00000000}", ultimoNumero);

                int numeroCalculado = 0;

                for (int contador = 2; contador < 10; contador++)
                    numeroCalculado += int.Parse(retorno[9 - contador].ToString())*contador;

                int resto = numeroCalculado % 11;

                int verificador = 0;

                if (resto == 0)
                    verificador = 1;
                else if (resto == 1)
                    verificador = 0;
                else
                    verificador= 11 - resto;

                retorno += verificador.ToString();

            }

            ParcelasDTO parcela = _serviceParcela.BuscarParcela(PAR_NUM_PARCELA);

            parcela.PAR_NOSSO_NUMERO = retorno;

            _serviceParcela.Salvar(parcela);

            return retorno;

        }

        public string LerDataAtualCNABLonga()
        {
            DateTime d = DateTime.Now;
            return d.ToString("ddMMyyyy");
        }

        public string LerDataLonga( DateTime data )
        {
            return data.ToString("ddMMyyyy");
        }


    }

}
