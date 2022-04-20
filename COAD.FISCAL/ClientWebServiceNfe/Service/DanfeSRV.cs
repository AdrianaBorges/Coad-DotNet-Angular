using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using COAD.FISCAL.Model.Danfe.Anotacao;
using COAD.FISCAL.Model.Danfe.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using Coad.Reflection;
using GenericCrud.Util;

namespace COAD.FISCAL.Service
{
    public class DanfeSRV
    {
        string tpAmb;
        string verAplic;
        string chNFe;
        string dhRecbto;
        string nProt;
        string digVal;
        string cStat;
        string xMotivo;

        public void ImprimirNFE()
        {
        }
        public void ImprimirNFSE()
        {

        }
        public BlocoCanhoto BuscarCanhoto(TNFe _nfe)
        {
            var dadoscanhoto = new BlocoCanhoto();

            var _razaosocial = _nfe.infNFe.emit.xNome;

            if (_nfe.infNFe.emit.xNome.Length > 50)
                _razaosocial = _nfe.infNFe.emit.xNome.Substring(0, 50);

            dadoscanhoto.Recebemos = "RECEBEMOS DE " + _razaosocial + " OS PRODUTOS/SERVIÇOS CONSTANTES NA NOTA FISCAL INDICADA AO LADO";
            dadoscanhoto.NumeroNfe = "Nº " + _nfe.infNFe.ide.nNF;
            dadoscanhoto.SerieNfe = "SÉRIE: " + _nfe.infNFe.ide.serie;

            return dadoscanhoto;
        }
        public BlocoFaturas BuscarFaturas(TNFe _nfe)
        {
            var dadosFaturas = new BlocoFaturas();

            return dadosFaturas;
        }
        public BlocoDestinatarioRemetente BuscarDestinatario(TNFe _nfe)
        {

            var dadosDestRem = new BlocoDestinatarioRemetente();
            dadosDestRem.CNPJ = _nfe.infNFe.dest.Item;
            dadosDestRem.DataEmissao = Convert.ToDateTime(_nfe.infNFe.ide.dhEmi).Day.ToString().PadLeft(2, '0') + "/" +
                                       Convert.ToDateTime(_nfe.infNFe.ide.dhEmi).Month.ToString().PadLeft(2, '0') + "/" +
                                       Convert.ToDateTime(_nfe.infNFe.ide.dhEmi).Year.ToString();
            dadosDestRem.DataEntradaSaida = Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Day.ToString().PadLeft(2, '0') + "/" +
                                            Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Month.ToString().PadLeft(2, '0') + "/" +
                                            Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Year.ToString();
            dadosDestRem.HoraEntradaSaida = Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Hour.ToString().PadLeft(2, '0') + ":" +
                                            Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Minute.ToString().PadLeft(2, '0') + ":" +
                                            Convert.ToDateTime(_nfe.infNFe.ide.dhSaiEnt).Second.ToString().PadLeft(2, '0');

            

            dadosDestRem.RazaoSocial = _nfe.infNFe.dest.xNome;
            dadosDestRem.CEP = _nfe.infNFe.dest.enderDest.CEP;
            dadosDestRem.Endereco = _nfe.infNFe.dest.enderDest.xLgr + "," +
                                    _nfe.infNFe.dest.enderDest.nro +
                                    _nfe.infNFe.dest.enderDest.xCpl;

            var _endereco = dadosDestRem.Endereco;

            if (_endereco.Length > 48)
                _endereco = _endereco.Substring(0, 48);

            dadosDestRem.Endereco = _endereco;


            dadosDestRem.Municipio = _nfe.infNFe.dest.enderDest.xMun;
            dadosDestRem.UF = _nfe.infNFe.dest.enderDest.UF.ToString();
            dadosDestRem.BairroDistrito = _nfe.infNFe.dest.enderDest.xBairro;
            dadosDestRem.FoneFax = _nfe.infNFe.dest.enderDest.fone;
            dadosDestRem.InscricaoEstadual = _nfe.infNFe.dest.IE;

            return dadosDestRem;

        }
        public BlocoCalculoImposto BuscarImpostos(TNFe _nfe)
        {

            var dadosImposto = new BlocoCalculoImposto();

            dadosImposto.BaseCalcIcms = _nfe.infNFe.total.ICMSTot.vBC;
            dadosImposto.ValorIcms = _nfe.infNFe.total.ICMSTot.vICMS;
            dadosImposto.BaseCalcIcmsST = _nfe.infNFe.total.ICMSTot.vBCST;
            dadosImposto.ValorIcmsST = _nfe.infNFe.total.ICMSTot.vST;
            dadosImposto.ValorProdutos = _nfe.infNFe.total.ICMSTot.vProd;
            dadosImposto.ValorFrete = _nfe.infNFe.total.ICMSTot.vFrete;
            dadosImposto.ValorSeguro = _nfe.infNFe.total.ICMSTot.vSeg;
            dadosImposto.ValorDesconto = _nfe.infNFe.total.ICMSTot.vDesc;
            dadosImposto.ValorOutrasDesp = _nfe.infNFe.total.ICMSTot.vOutro;
            dadosImposto.ValorIPI = _nfe.infNFe.total.ICMSTot.vIPI;
            dadosImposto.ValorTotalNota = _nfe.infNFe.total.ICMSTot.vNF;


            return dadosImposto;
        }
        public BlocoTransportador BuscarTransportador(TNFe _nfe)
        {
            var _FretePorConta = "";

            var dadosTransportador = new BlocoTransportador();

            if (_nfe.infNFe.transp != null)
            {
                var _modo = this.ConvertToString((Enum)(_nfe.infNFe.transp.modFrete));

                switch (Convert.ToInt32(_modo))
                {
                    case 0:
                        _FretePorConta = "(0) Emitente (CIF)";
                        break;

                    case 1:
                        _FretePorConta = "(1) Destinatário(FOB)";
                        break;

                    case 2:
                        _FretePorConta = "(2) Terceiros";
                        break;

                    case 3:
                        _FretePorConta = "(3) Próprio (Remetente)";
                        break;

                    case 4:
                        _FretePorConta = "(4) Próprio (Destinatário)";
                        break;

                    case 9:
                        _FretePorConta = "(9) Sem Frete";
                        break;
                }

                dadosTransportador.FretePorContaDe = _FretePorConta;
            }

            if (_nfe.infNFe.transp.transporta != null)
            {
                dadosTransportador.tranRazaoSocial = _nfe.infNFe.transp.transporta.xNome;
          

                if (_nfe.infNFe.transp.Items != null)
                {
                    dadosTransportador.CodigoAntt = ((TVeiculo)_nfe.infNFe.transp.Items[0]).RNTC;
                    dadosTransportador.Placa = ((TVeiculo)_nfe.infNFe.transp.Items[0]).placa;
                    dadosTransportador.PlacaUF = ((TVeiculo)_nfe.infNFe.transp.Items[0]).UF.ToString();
                }

                dadosTransportador.tranCNPJ = _nfe.infNFe.transp.transporta.Item;
                dadosTransportador.tranEndereco = _nfe.infNFe.transp.transporta.xEnder;
                dadosTransportador.tranMunicipio = _nfe.infNFe.transp.transporta.xMun;
                dadosTransportador.tranUF = _nfe.infNFe.transp.transporta.UF.ToString();
                dadosTransportador.tranInscEstadual = _nfe.infNFe.transp.transporta.IE;

                if (_nfe.infNFe.transp.vol != null)
                {
                    dadosTransportador.QtdeVolumes = _nfe.infNFe.transp.vol[0].nVol;
                    dadosTransportador.Especie = _nfe.infNFe.transp.vol[0].esp;
                    dadosTransportador.Marca = _nfe.infNFe.transp.vol[0].marca;
                    dadosTransportador.PesoBruto = _nfe.infNFe.transp.vol[0].pesoB;
                    dadosTransportador.PesoLiquido = _nfe.infNFe.transp.vol[0].pesoL;

                    if (_nfe.infNFe.transp.vol[0].lacres != null)
                        dadosTransportador.Numeracao = _nfe.infNFe.transp.vol[0].lacres[0].nLacre;
                }
            }

            return dadosTransportador;
        }
        public BlocoDadosNFE BuscarDadosNFE(TNFe _nfe)
        {

            var dadosnfe = new BlocoDadosNFE();
            var _item = 0;
            var _chNFe = "";
            while (_item <= 10)
            {
                _chNFe = _chNFe+" "+chNFe.Substring(_item * 4, 4);
                _item += 1;
            }

            dadosnfe.emiOpcao = this.ConvertToString((Enum)(_nfe.infNFe.ide.tpNF));  
            dadosnfe.CodigoBarras = chNFe;
            dadosnfe.ChaveAcesso = "       "+_chNFe;
            dadosnfe.InformacaoNF = "";
            dadosnfe.NaturezaOperacao = _nfe.infNFe.ide.natOp;
            dadosnfe.ProtocoloAutorizacao = "         " + nProt +" - "+Convert.ToDateTime(dhRecbto).ToString("dd/MM/yyyy HH:mm:ss");
            dadosnfe.InscricaoEstadual = _nfe.infNFe.emit.IE;
            dadosnfe.InscricaoEstadualST = _nfe.infNFe.emit.IEST;
            dadosnfe.CNPJEmitente = _nfe.infNFe.emit.CNPJ;

            dadosnfe.emiRazaoSocial = _nfe.infNFe.emit.xNome;
            dadosnfe.emiEndereco = _nfe.infNFe.emit.enderEmit.xLgr + "," +
                                   _nfe.infNFe.emit.enderEmit.nro + " " +
                                   _nfe.infNFe.emit.enderEmit.xCpl;
            
            dadosnfe.emiMunicipio = _nfe.infNFe.emit.enderEmit.xBairro + " - " +
                                    Convert.ToUInt64(_nfe.infNFe.emit.enderEmit.CEP).ToString(@"00000\-000");
            dadosnfe.emiFoneFax   = _nfe.infNFe.emit.enderEmit.xMun + " - " +
                                    _nfe.infNFe.emit.enderEmit.UF.ToString() +
                                    " Fone: " + _nfe.infNFe.emit.enderEmit.fone;
            
            dadosnfe.emiNumNF = "Nº " + _nfe.infNFe.ide.nNF;
            dadosnfe.emiSerieNF = "SÉRIE: " + _nfe.infNFe.ide.serie;
            dadosnfe.emiPagina = "      Página 1 de 1";

            return dadosnfe;

        }
        public BlocoDadosAdicionais BuscarDadosAdicionais(TNFe _nfe)
        {

            var dadosDadosAdicionais = new BlocoDadosAdicionais();
            dadosDadosAdicionais.InformacoesComplementares = _nfe.infNFe.infAdic.infCpl;
            dadosDadosAdicionais.InformacoesComplementares = _nfe.infNFe.infAdic.infAdFisco;
            //dadosDadosAdicionais.InformacoesComplementares = _nfe.infNFe.obsCont[0];
            //dadosDadosAdicionais.InformacoesComplementares = _nfe.infNFe.obsFisco[0];
            //dadosDadosAdicionais.InformacoesComplementares = _nfe.infNFe.procRef[0];
    
            return dadosDadosAdicionais;
        }
        public BlocoISSQN BuscarDadosISSQN(TNFe _nfe)
        {

            var dadosISSQN = new BlocoISSQN();

            return dadosISSQN;
        }
        public string ConvertToString(Enum e)
        {
            Type t = e.GetType();
            FieldInfo info = t.GetField(e.ToString("G"));
            if (!info.IsDefined(typeof(XmlEnumAttribute), false))
            {
                return e.ToString("G");
            }

            object[] o = info.GetCustomAttributes(typeof(XmlEnumAttribute), false);
            XmlEnumAttribute att = (XmlEnumAttribute)o[0];
            return att.Name;
        }
        public List<BlocoProdutoServico> BuscarProdutoServico(TNFe _nfe)
        {

            var _lista = new List<BlocoProdutoServico>();

            for (var i = 0; i <= _nfe.infNFe.det.Count() - 1; i++)
            {

                var _prod = _nfe.infNFe.det[i].prod;
                var _imposto = _nfe.infNFe.det[i].imposto;
                var _cst = "";
                var _icms = "";
                var _orig = "";

                var dadosprodutoservico = new BlocoProdutoServico();
                dadosprodutoservico.Codigo = _prod.cProd;
                dadosprodutoservico.Descricao = _prod.xProd;
                dadosprodutoservico.NcmSh = _prod.NCM;
                dadosprodutoservico.Cfop = _prod.CFOP;
                dadosprodutoservico.Unid = _prod.uCom;
                dadosprodutoservico.Qtd = _prod.qCom;
                dadosprodutoservico.VlrUnit = _prod.vUnCom;
                dadosprodutoservico.VlrTotal = _prod.vProd;
                dadosprodutoservico.VlrIpi = null;
                dadosprodutoservico.AliqIpi = null;


                if (_nfe.infNFe.det[i].imposto.Items != null)
                {
                    var tipo = (TNFeInfNFeDetImpostoICMS)(_nfe.infNFe.det[i].imposto.Items[0]);
                    var d = ((TNFeInfNFeDetImpostoICMS)(_nfe.infNFe.det[i].imposto.Items[0])).Item;
                    
                    switch (tipo.Item.ToString())
                    {
                        case "TNFeInfNFeDetImpostoICMSICMS00":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS00)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS00)(d)).orig);

                            dadosprodutoservico.Cst = ((TNFeInfNFeDetImpostoICMSICMS00)(d)).CST.ToString();
                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS00)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS00)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS00)(d)).pICMS;

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS10":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS10)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS10)(d)).orig);

                            dadosprodutoservico.Cst = ((TNFeInfNFeDetImpostoICMSICMS10)(d)).CST.ToString();
                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS10)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS10)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS10)(d)).pICMS;

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS20":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS20)(d)).CST);
                            _icms = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS20)(d)).motDesICMS);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS20)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS20)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS20)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS20)(d)).pICMS;

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS30":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS30)(d)).CST);
                            _icms = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS30)(d)).motDesICMS);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS30)(d)).orig);

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS40":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS40)(d)).CST);
                            _icms = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS40)(d)).motDesICMS);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS40)(d)).orig);
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS51":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS51)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS51)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS51)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS51)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS51)(d)).pICMS;

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS60":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS60)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS60)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS60)(d)).vBCSTRet;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS60)(d)).vICMSSTRet;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS60)(d)).pST;
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS70":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS70)(d)).CST);
                            _icms = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS70)(d)).motDesICMS);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS70)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS70)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS70)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS70)(d)).pICMS;
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMS90":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS90)(d)).CST);
                            _icms = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS90)(d)).motDesICMS);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMS90)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMS90)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMS90)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMS90)(d)).pICMS;
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSPart":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSPart)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSPart)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMSPart)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMSPart)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMSPart)(d)).pICMS;
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSSN101":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN101)(d)).CSOSN);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN101)(d)).orig);

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSSN102":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN102)(d)).CSOSN);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN102)(d)).orig);
                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSSN201":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN201)(d)).CSOSN);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN201)(d)).orig);

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSSN202":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN202)(d)).CSOSN);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN202)(d)).orig);

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSSN900":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN900)(d)).CSOSN);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSSN900)(d)).orig);

                            dadosprodutoservico.BcIcms = ((TNFeInfNFeDetImpostoICMSICMSSN900)(d)).vBC;
                            dadosprodutoservico.VlrIcms = ((TNFeInfNFeDetImpostoICMSICMSSN900)(d)).vICMS;
                            dadosprodutoservico.AliqIcms = ((TNFeInfNFeDetImpostoICMSICMSSN900)(d)).pICMS;

                            break;
                        case "TNFeInfNFeDetImpostoICMSICMSST":
                            _cst = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSST)(d)).CST);
                            _orig = this.ConvertToString((Enum)((TNFeInfNFeDetImpostoICMSICMSST)(d)).orig);

                            break;
                    }

                    dadosprodutoservico.Cst = _orig + _cst;

                    _lista.Add(dadosprodutoservico);


                    return _lista;

                }
            }

            return _lista;
        }
        public string RetornaStringDoCampo(XmlDocument _doc, string cNo, string cCampo, string cDefault = null)
        {
            // variável de retorno...
            string cRetorno = "";

            // foram informados os dois parâmetros?...
            if ((cNo != "") && (cCampo != ""))
            {
                // preparando a leitura do Nó no XML...
                XmlNodeList no = _doc.DocumentElement.GetElementsByTagName(cNo);

                // retornando com a informação do campo...
                if (no.Count > 0)
                {
                    if (((XmlElement)no[0]).GetElementsByTagName(cCampo).Count > 0)
                    {
                        cRetorno = ((XmlElement)no[0]).GetElementsByTagName(cCampo)[0].InnerText;
                    }
                }
            }

            if (cRetorno == "" && (cDefault != "" && cDefault != null))
            {
                cRetorno = cDefault;
            }

            return cRetorno;
        }
        public string GerarPdf(string _nfid)
        {

            string _path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            var curDir = _path + "\\temp\\" + _nfid + ".xml";
            var caminho = _path + "\\temp\\" + _nfid + ".pdf";

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(curDir);
            var _node = xmldoc.DocumentElement.SelectSingleNode("*");
            XmlNodeReader reader = new XmlNodeReader(_node);

            var _ser = new XmlSerializer(typeof(TNFe));

            var _nfe = (TNFe)_ser.Deserialize(reader);

            //-------------

            tpAmb = this.RetornaStringDoCampo(xmldoc, "infProt", "tpAmb");
            verAplic = this.RetornaStringDoCampo(xmldoc, "infProt", "verAplic");
            chNFe = this.RetornaStringDoCampo(xmldoc, "infProt", "chNFe");
            dhRecbto = this.RetornaStringDoCampo(xmldoc, "infProt", "dhRecbto");
            nProt = this.RetornaStringDoCampo(xmldoc, "infProt", "nProt");
            digVal = this.RetornaStringDoCampo(xmldoc, "infProt", "digVal");
            cStat = this.RetornaStringDoCampo(xmldoc, "infProt", "cStat");
            xMotivo = this.RetornaStringDoCampo(xmldoc, "infProt", "xMotivo");

            //-------------
            
            var bloco = new BlocoCanhoto();
            var _ListaPropriedades = this.BuscarPropriedade(typeof(BlocoCanhoto));
            var doc = new Document(PageSize.A4, 56, 56, 56, 56);

            doc.AddCreationDate();//adicionando as configuracoes

            //--------
            //var imagemDoTopo = iTextSharp.text.Image.GetInstance("logo.jpg");
            //imagemDoTopo.SetAbsolutePosition(50, 653);
            //doc.Add(imagemDoTopo);
            //--------

            try
            {

                var dadoscanhoto = this.BuscarCanhoto(_nfe);
                var dadosadicionais = this.BuscarDadosAdicionais(_nfe);
                var dadosissqn = this.BuscarDadosISSQN(_nfe);
                var dadosnfe = this.BuscarDadosNFE(_nfe);
                var dadosDestRem = this.BuscarDestinatario(_nfe);
                var dadosfaturas = this.BuscarFaturas(_nfe);
                var dadosimpostos = this.BuscarImpostos(_nfe);
                var dadostransportador = this.BuscarTransportador(_nfe);
                var dadosprodutoservico = this.BuscarProdutoServico(_nfe);


                var writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

                doc.Open();

                //----------
                var sair = 1;
                var contentByte = writer.DirectContent;


                while (sair <= 9)
                {
                    switch (sair)
                    {
                        case 1:
                            _ListaPropriedades = this.BuscarPropriedade(dadosfaturas.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosfaturas, contentByte, doc);
                            break;
                        case 2:
                            _ListaPropriedades = this.BuscarPropriedade(dadoscanhoto.GetType());
                            this.GerarDoc(_ListaPropriedades, dadoscanhoto, contentByte, doc);
                            break;
                        case 3:
                            _ListaPropriedades = this.BuscarPropriedade(dadosDestRem.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosDestRem, contentByte, doc);
                            break;
                        case 4:
                            _ListaPropriedades = this.BuscarPropriedade(dadosimpostos.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosimpostos, contentByte, doc);
                            break;
                        case 5:
                            _ListaPropriedades = this.BuscarPropriedade(dadostransportador.GetType());
                            this.GerarDoc(_ListaPropriedades, dadostransportador, contentByte, doc);
                            break;
                        case 6:
                            _ListaPropriedades = this.BuscarPropriedade(dadosnfe.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosnfe, contentByte, doc);
                            break;
                        case 7:
                            _ListaPropriedades = this.BuscarPropriedade(dadosadicionais.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosadicionais, contentByte, doc);
                            break;
                        case 8:
                            _ListaPropriedades = this.BuscarPropriedade(dadosissqn.GetType());
                            this.GerarDoc(_ListaPropriedades, dadosissqn, contentByte, doc);
                            break;
                        case 9:
                            //var top = 0;
                            //foreach (var item in dadosprodutoservico)
                            //{
                            //    _ListaPropriedades = this.BuscarPropriedade(item.GetType());

                            //    this.GerarDoc(_ListaPropriedades, item, contentByte, doc, top);

                            //    top += 10;
                            //}
                            this.GerarDoc(dadosprodutoservico, contentByte, doc);
                            break;
                    }



                    sair += 1;
                }

                //---------

                writer.DirectContent.Add(contentByte);

                //----------

            }
            finally
            {
                doc.Close();

                //System.Diagnostics.Process.Start(caminho);
            }

            return caminho;
        }
        public void GerarDoc(List<BlocoProdutoServico> dadosprodutoservico, PdfContentByte contentByte, Document doc)
        {
            var papel = 842;
            double altura = 0;
            double largura = 0;
            float margEsq = 0;
            float margTop = 0;
            string texto = "";
            string conteudo = "";
            float fontsize = 0;
            bool box = true;
            var bc = new Barcode128();
            var top = 1;
            float linhaprod = 25.48f;

            foreach (var item in dadosprodutoservico)
            {
                var _ListaPropriedades = this.BuscarPropriedade(item.GetType());
                var _ListaAlinhamento = this.BuscarAlinhamento(item.GetType());

                foreach (var propriedades in _ListaPropriedades)
                {

                    papel = 842;
                    PropertyInfo property;
                    property = item.GetType().GetProperty(propriedades.Nome);
                    var alinhamento = _ListaAlinhamento.Where(x => x.Nome == propriedades.Nome).FirstOrDefault();

                    if (property != null)
                    {
                        if (property.PropertyType == typeof(System.String))
                        {
                            conteudo = (string)property.GetValue(item, null);
                            if (!String.IsNullOrWhiteSpace(conteudo) && alinhamento != null)
                            {
                                if (property.Name == "VlrTotal" ||
                                    property.Name == "BcIcms"   ||
                                    property.Name == "VlrIcms"  ||
                                    property.Name == "VlrIpi"   ||
                                    property.Name == "AliqIcms" ||
                                    property.Name == "AliqIpi"  ||
                                    property.Name == "VlrUnit"  ||
                                    property.Name == "Qtd")
                                {
                                       conteudo = StringUtil.FormatarDinheiro(decimal.Parse(conteudo), false, false);
                                }

                                if (alinhamento.Alinhamento == "D")
                                    conteudo = conteudo.PadLeft(alinhamento.Tamanho, alinhamento.Conteudo);
                                else
                                    conteudo = conteudo.PadRight(alinhamento.Tamanho, alinhamento.Conteudo);
                            }

                        }
                        else if (property.PropertyType == typeof(System.DateTime))
                            conteudo = ((DateTime)property.GetValue(item, null)).ToString("dd/MM/yyyy");
                        else if (property.PropertyType == typeof(System.Double))
                        {
                            conteudo = ((Double)property.GetValue(item, null)).ToString("#.00");
                            conteudo = conteudo.PadLeft(8, '0');
                        }
                        else
                            conteudo = "";

                    }
                    else
                        conteudo = "";


                    if (conteudo == null)
                        conteudo = "";

                    conteudo = conteudo.ToUpper();

                    altura = (double)iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.Altura * 10));
                    largura = (double)iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.Largura * 10));
                    margEsq = iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.MargemEsquerda * 10));
                    margTop = papel - iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.MargemSuperior * 10));

                    texto = propriedades.Texto;
                    fontsize = (float)propriedades.Tamanho;
                    box = propriedades.Box;
                    var tipo = propriedades.Tipo;

                    if (top == 1)
                    {
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        if (property.Name == "AliqIcms")
                        {
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                            contentByte.ShowText(" ALIQ.");
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 12));
                            contentByte.ShowText(" ICMS");
                        }
                        else if (property.Name == "AliqIpi")
                        {
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                            contentByte.ShowText(" ALIQ.");
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 12));
                            contentByte.ShowText("   IPI");
                        }
                        else if (property.Name == "ProdutoServico")
                        {
                            contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false), fontsize);
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                            contentByte.ShowText(texto);
                          
                        }
                        else
                        {
                            contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                            contentByte.ShowText(texto);

                        }
                        contentByte.EndText();

                    }

                    contentByte.BeginText();
                    contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 7);
                    
                    if (property.Name == "Descricao")
                    {
                        var tam = conteudo.Length;
                        if (tam > 50)
                        {
                            var texto01 = conteudo.Substring(0, 40);
                            var texto02 = conteudo.Substring(40, (tam - 40));
                            if (top == 1)
                            {
                                contentByte.SetTextMatrix((margEsq + 2), (margTop - 21));
                                contentByte.ShowText(texto01);
                                contentByte.SetTextMatrix((margEsq + 2), (margTop - 29));
                                contentByte.ShowText(texto02);
                            }
                            else
                            {
                                contentByte.SetTextMatrix((margEsq + 2), (margTop - (linhaprod - 5)));
                                contentByte.ShowText(texto01);
                                contentByte.SetTextMatrix((margEsq + 2), (margTop - (linhaprod + 2)));
                                contentByte.ShowText(texto02);
                            }
                        }
                        else
                        {
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - linhaprod));
                            contentByte.ShowText(conteudo);
                        }
                    }
                    else
                    {
                        contentByte.SetTextMatrix((margEsq + 2), (margTop - linhaprod));
                        contentByte.ShowText(conteudo);
                    }

                    contentByte.EndText();


                    if (box)
                    {
                        if (top == 1)
                        {
                            contentByte.Rectangle(margEsq, (margTop - altura), largura, altura);
                            contentByte.Stroke();
                            contentByte.Rectangle(margEsq, ((margTop - (top * altura)) - altura), largura, altura);
                            contentByte.Stroke();
                        }
                        else
                            contentByte.Rectangle(margEsq, ((margTop - (top * altura)) - altura), largura, altura);

                        contentByte.Stroke();
                    }

                }

                top += 1;
                linhaprod += 14.74f;
            }
        }
        public void GerarDoc(List<CustomFieldPDF> _ListaPropriedades, object classe, PdfContentByte contentByte, Document doc)
        {
            var papel = 842;
            double altura = 0;
            double largura = 0;
            float margEsq = 0;
            float margTop = 0;
            string texto = "";
            string conteudo = "";
            float fontsize = 0;
            bool box = true;
            var bc = new Barcode128();
            iTextSharp.text.Image img = null;

            var _ListaAlinhamento = this.BuscarAlinhamento(classe.GetType());

            foreach (var propriedades in _ListaPropriedades)
            {

                papel = 842;
                PropertyInfo property;
                property = classe.GetType().GetProperty(propriedades.Nome);
                var alinhamento = _ListaAlinhamento.Where(x => x.Nome == propriedades.Nome).FirstOrDefault();

                if (property != null)
                {
                    if (property.PropertyType == typeof(System.String))
                    {
                        conteudo = (string)property.GetValue(classe, null);
                        if (!String.IsNullOrWhiteSpace(conteudo) && alinhamento != null)
                        {
                            if (property.Name == "CalculoImposto" ||
                                property.Name == "BaseCalcIcms"   ||
                                property.Name == "ValorIcms"      ||
                                property.Name == "BaseCalcIcmsST" ||
                                property.Name == "ValorIcmsST"    ||
                                property.Name == "ValorProdutos"  ||
                                property.Name == "ValorFrete"     ||
                                property.Name == "ValorSeguro"    ||
                                property.Name == "ValorDesconto"  ||
                                property.Name == "ValorOutrasDesp" ||
                                property.Name == "ValorIPI"       ||
                                property.Name == "ValorTotalNota")
                            {
                                conteudo = StringUtil.FormatarDinheiro(decimal.Parse(conteudo), false, false);
                            }

                            if (alinhamento.Alinhamento == "D")
                                conteudo = conteudo.PadLeft(alinhamento.Tamanho, alinhamento.Conteudo);
                            else
                                conteudo = conteudo.PadRight(alinhamento.Tamanho, alinhamento.Conteudo);
                        }
                    }
                    else if (property.PropertyType == typeof(System.DateTime))
                        conteudo = ((DateTime)property.GetValue(classe, null)).ToString("dd/MM/yyyy");
                    else if (property.PropertyType == typeof(System.Double))
                        conteudo = ((Double)property.GetValue(classe, null)).ToString("#.00");
                    else
                        conteudo = "";

                }
                else
                    conteudo = "";


                if (conteudo == null)
                    conteudo = "";

                //conteudo = conteudo.ToUpper();

                altura = (double)iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.Altura * 10));
                largura = (double)iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.Largura * 10));
                margEsq = iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.MargemEsquerda * 10));
                margTop = papel - iTextSharp.text.Utilities.MillimetersToPoints((float)(propriedades.MargemSuperior * 10));
                texto = propriedades.Texto;
                fontsize = (float)propriedades.Tamanho;
                box = propriedades.Box;
                var tipo = propriedades.Tipo;

                if (propriedades.Nome == "Recebemos")
                {
                    texto = conteudo;
                    conteudo = "";
                }
                      

                if (property.Name == "CalculoImposto")
                {
                    contentByte.BeginText();
                    contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false), fontsize);
                    contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                    contentByte.ShowText(texto);
                    contentByte.EndText();

                }
                else if (property.Name == "emiEntrada" ||
                         property.Name == "emiSaida"   ||
                         property.Name == "emiDanfe02" ||
                         property.Name == "emiDanfe03"  )
                {
                    contentByte.BeginText();
                    contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                    contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                    contentByte.ShowText(texto);
                    contentByte.EndText();
                }
                else if (property.Name == "emiDanfe01" )
                {
                    contentByte.BeginText();
                    contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false), fontsize);
                    contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                    contentByte.ShowText(texto);
                    contentByte.EndText();
                }
                else
                {
                    contentByte.BeginText();

                    if (box)
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 6);
                    else
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false), 7);

                    contentByte.SetTextMatrix((margEsq + 2), (margTop - 6));
                    contentByte.ShowText(texto);
                    contentByte.EndText();
                }

                if (property.Name == "CNPJ" || property.Name == "CNPJEmitente")
                {
                    if (conteudo.Length == 11)
                        conteudo = Convert.ToUInt64(conteudo).ToString(@"000\.000\.000\-00");
                    else
                        conteudo = Convert.ToUInt64(conteudo).ToString(@"00\.000\.000\/0000\-00");
                }

                if (property.Name == "CEP")
                {
                    conteudo = Convert.ToUInt64(conteudo).ToString(@"00000\-000"); 
                }

                switch (tipo)
                {
                    case 0:
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        if (property.Name == "emiOpcao")
                        {
                            contentByte.SetTextMatrix((margEsq + 5), (margTop - 10));
                            contentByte.ShowText(conteudo);
                        }
                        else
                        {
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 21));
                            contentByte.ShowText(conteudo);
                        }
                        contentByte.EndText();
                        break;
                    case 1:
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        if (property.Name == "InformacoesComplementares")
                        {
                            var pos = 0;
                            var tam = 16;

                            while (pos < conteudo.Length)
                            {
                                var texto01 = "";

                                if ((pos + 101) > conteudo.Length)
                                    texto01 = conteudo.Substring(pos, (conteudo.Length - pos));
                                else
                                    texto01 = conteudo.Substring(pos, 101);

                                contentByte.SetTextMatrix((margEsq + 2), (margTop - tam));
                                contentByte.ShowText(texto01);


                                pos += 101;
                                tam += 10;
                            }

                        }
                        contentByte.EndText();
                        break;
                    case 2:
                        bc.Font = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                        bc.TextAlignment = Element.ALIGN_CENTER;
                        bc.StartStopText = false;
                        bc.Code = ((BlocoDadosNFE)classe).CodigoBarras;
                        bc.CodeType = Barcode128.CODE128;
                        bc.Extended = true;
                        img = bc.CreateImageWithBarcode(contentByte, iTextSharp.text.BaseColor.BLACK, iTextSharp.text.BaseColor.WHITE);
                        img.SetAbsolutePosition((margEsq + 3), (margTop - 41));
                        doc.Add(img);
                        break;
                    case 3:
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        contentByte.SetTextMatrix((margEsq + 2), (margTop - 21));
                        contentByte.ShowText(conteudo);
                        contentByte.EndText();
                        break;
                    case 4:
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        contentByte.SetTextMatrix((margEsq + 5), (margTop - 12));
                        contentByte.ShowText("Consulta   de   autenticidade  no  portal   nacional  da ");
                        contentByte.EndText();
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        contentByte.SetTextMatrix((margEsq + 5), (margTop - 23));
                        contentByte.ShowText("NF-e  www.nfe.fazenda.gov.br/portal   ou    no    site");
                        contentByte.EndText();
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        contentByte.SetTextMatrix((margEsq + 5), (margTop - 34));
                        contentByte.ShowText("da Sefaz Autorizadora.");
                        contentByte.EndText();
                        break;
                    case 7:
                        var _alinh = (60 - conteudo.Length);
                        string _str = "";
                        _str = _str.PadLeft(_alinh, ' ');
                        contentByte.BeginText();
                        if (property.Name == "emiRazaoSocial")
                        {
                            if (conteudo.Length < 45)
                               fontsize = 10;
                            else
                               fontsize = 7;
                            conteudo = conteudo.ToUpper();
                            contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false), fontsize);
                            contentByte.SetTextMatrix((margEsq + 2), (margTop - 21));
                        }
                        else
                        {
                            conteudo = conteudo.ToUpper();
                            conteudo = _str+conteudo;
                            contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                            contentByte.SetTextMatrix((margEsq + 5), (margTop - 21));
                        }
                        contentByte.ShowText(conteudo);
                        contentByte.EndText();
                        break;
                    case 8:
                        contentByte.BeginText();
                        contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                        contentByte.SetTextMatrix((margEsq + 2), (margTop - 21));
                        contentByte.ShowText(conteudo);
                        contentByte.EndText();
                        break;
                }

                if (box)
                {
                    contentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), fontsize);
                    contentByte.Rectangle(margEsq, (margTop - altura), largura, altura);
                    contentByte.Stroke();
                }

            }
        }

        public PropertyInfo BuscarValorPropriedade(System.Type t)
        {
            return null;
        }
        public List<CustomFieldPDF> BuscarPropriedade(System.Type t)
        {
            var annotedProperties = ReflectionProvider.GetPropertyByAttributes<CustomFieldPDF>(t);
            List<CustomFieldPDF> _lista = new List<CustomFieldPDF>();

            foreach (var pro in annotedProperties)
            {
                var _filterDef = new CustomFieldPDF();
                _filterDef = ReflectionProvider.GetMemberAttribute<CustomFieldPDF>(pro);
                _filterDef.Nome = pro.Name;
                _lista.Add(_filterDef);

            }

            return _lista;
        }
        public List<CustomFieldAlinPDF> BuscarAlinhamento(System.Type t)
        {
            var annotedProperties = ReflectionProvider.GetPropertyByAttributes<CustomFieldAlinPDF>(t);
            List<CustomFieldAlinPDF> _lista = new List<CustomFieldAlinPDF>();

            foreach (var pro in annotedProperties)
            {
                var _filterDef = new CustomFieldAlinPDF();
                _filterDef = ReflectionProvider.GetMemberAttribute<CustomFieldAlinPDF>(pro);
                _filterDef.Nome = pro.Name;
                _lista.Add(_filterDef);

            }

            return _lista;
        }

    }

}