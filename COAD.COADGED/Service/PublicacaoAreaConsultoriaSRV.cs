using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.PORTAL.DAO.PortalBusca;
using COAD.PORTAL.DAO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Model.DTO.PortalBusca;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using COAD.PORTAL.Service.PortalCoad;
using COAD.PORTAL.Service.PortalBusca;
using System.Data.OleDb;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using GenericCrud.Service;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoAreaConsultoriaSRV : GenericService<PUBLICACAO_AREAS_CONSULTORIA, PublicacaoAreaConsultoriaDTO, int>
    {
        private TipoMateriaSRV _serviceTpMateria = new TipoMateriaSRV();
        private TipoAtoSRV _serviceTpAto = new TipoAtoSRV();
        private TitulacaoSRV _serviceTitulacao = new TitulacaoSRV();
        private VeiculoSRV _serviceVeiculo = new VeiculoSRV();
        private OrgaoSRV _serviceOrgao = new OrgaoSRV();
        private AreasSRV _serviceAreas = new AreasSRV();
        private LabelsSRV _serviceLabel = new LabelsSRV();
        private SecoesSRV _serviceSecao = new SecoesSRV();
        private ColaboradorSRV _serviceColaborador = new ColaboradorSRV();
        private PublicacaoSRV _servicePublicacao = new PublicacaoSRV();
        private PublicacaoTitulacaoSRV _servicePublicacaoTitulacao = new PublicacaoTitulacaoSRV();
        private PublicacaoUfSRV _servicePublicacaoUf = new PublicacaoUfSRV();
        private PublicacaoPalavraChaveSRV _servicePublicacaoPalavraChave = new PublicacaoPalavraChaveSRV();
        private PublicacaoConfigSRV _servicePublicacaoConfig = new PublicacaoConfigSRV();
        private PublicacaoRemissaoSRV _servicePublicacaoRemissao = new PublicacaoRemissaoSRV();
        private PublicacaoRevisaoColaboradorSRV _servicePublicacaoRevisaoColaborador = new PublicacaoRevisaoColaboradorSRV();
        private PublicacaoRemissivoSRV _servicePublicacaoRemissivo = new PublicacaoRemissivoSRV();
        private PublicacaoAlteracaoRevogacaoSRV _servicePublicacaoAlteracaoRevogacao = new PublicacaoAlteracaoRevogacaoSRV();
        private PublicacaoAreaConsultoriaDAO _dao = new PublicacaoAreaConsultoriaDAO();

        // portal...
        private Tab_30SRV _serviceTab30 = new Tab_30SRV();
        private Tab_30_htmlSRV _serviceTab30Html = new Tab_30_htmlSRV();

        private Tab_31SRV _serviceTab31 = new Tab_31SRV();
        private Tab_31_htmlSRV _serviceTab31Html = new Tab_31_htmlSRV();
        private Busca_completa_tributarioSRV _serviceBusca = new Busca_completa_tributarioSRV();

        public PublicacaoAreaConsultoriaSRV()
        {
            Dao = _dao;
        }

        public String RetornarPalavraPorPalavra(string texto)
        {
            string primeiroCaracter = texto.Substring(0, 1);
            if (System.Convert.ToChar(primeiroCaracter) != '"')
            {
                string[] aPalavra = texto.Split(' ');

                texto = "";

                foreach (var palavra in aPalavra)
                {
                    texto += "+" + palavra + " ";
                }
            }
            return texto.Trim();
        }

        public String Substring(string texto = null, int? i = null, int? t = null)
        {
            texto = texto == null ? "" : texto; // muda texto nulo para branco
            i = i == null ? 0 : i;              // muda posição (i)nícial nula para ZERO
            t = t == null ? texto.Length : t;   // muda (t)otal de caracteres nulo para o tamanho do texto
            var r = texto.Length - i;             // (r)estante de caracteres apartir da posição inicial
            t = t > r ? r : t;                  // se o total de caracteres é maior que o restante, então, muda para restante
            return texto.Substring((int)i, (int)t);
        }

        public bool ImportouExcel(string arquivoExcel = "C:\\COADGED\\Importacao\\Andrea limpou\\Tab_31.xlsx")
        {
            try
            {
                if (File.Exists(arquivoExcel))
                {
                    string con = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + arquivoExcel + ";" +
                                 @"Extended Properties='Excel 12.0 Xml;HDR=YES;'";

                    using (OleDbConnection connection = new OleDbConnection(con))
                    {
                        connection.Open();

                        DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                        string[] colecionadores = { "IR", "ICMS", "LTPS", "DINAMICO", "LC", "IPI" };
                        string[] tipo = { "G", "V", "S" };
                        int?[] pai = { null };

                        foreach (DataRow aba in dt.Rows)
                        {
                            string planilha = aba["TABLE_NAME"].ToString(); // obtem o nome da planilha corrente
                            OleDbCommand command = new OleDbCommand("SELECT * FROM [" + planilha + "]", connection); // obtem todos as linhas da planilha corrente
                            using (OleDbDataReader dr = command.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    for (var i = 1; i <= 3; i++) // lendo na ordem: 1=gg, 2=vb, 3=svb
                                    {
                                        if (!String.IsNullOrWhiteSpace(dr[i].ToString()))
                                        {
                                            TitulacaoDTO titulacao = new TitulacaoDTO();

                                            titulacao.TIT_DESCRICAO = dr[i].ToString();
                                            titulacao.UF_ID = null;
                                            titulacao.TIT_TIPO = tipo[i - 1];
                                            titulacao.TIT_ATIVO = 1;
                                            titulacao.TIT_ID_REFERENCIA = null;
                                            titulacao.ARE_CONS_ID = Array.IndexOf(colecionadores, dr[0].ToString()) + 1;

                                            if (titulacao.ARE_CONS_ID == 0)
                                            {
                                                titulacao.ARE_CONS_ID = 2; // se não encontrou, então é ICMS...
                                                titulacao.UF_ID = String.IsNullOrWhiteSpace(dr[0].ToString()) ? null : dr[0].ToString();
                                            }

                                            if (i > 1) // é vb ou svb, então, pegue o ID da sua Titulação
                                                titulacao.TIT_ID_REFERENCIA = pai[0];

                                            var achou = _serviceTitulacao.Titulacoes(null, titulacao.ARE_CONS_ID, titulacao.TIT_DESCRICAO, titulacao.TIT_ATIVO, titulacao.TIT_TIPO, titulacao.TIT_ID_REFERENCIA, titulacao.UF_ID, true).lista.FirstOrDefault();
                                            if (achou != null)
                                                pai[0] = achou.TIT_ID;
                                            else
                                                pai[0] = _serviceTitulacao.SalvarTitulacao(titulacao, false).TIT_ID;
                                        }
                                    }
                                }
                                dr.Close();
                            }
                        }
                        connection.Close();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        // importar matéria do portal 30 \\
        public int[] ImportarMateriaTab30(Tab_30DTO tb)
        {
            int[] retorno = null;

            PublicacaoAreaConsultoriaDTO pub = new PublicacaoAreaConsultoriaDTO();

            // buscando o texto...
            var html = _serviceTab30Html.Coad(null, null, Convert.ToInt32(tb.modulo)).lista;
            if (html.Count() > 0)
            {
                // publicacao area consultoria...
                pub.ARE_CONS_ID = 4;
                pub.PUB_PAGINA_INDICE = tb.pagina;
                pub.PUB_MANCHETE_PORTAL = tb.titulo;
                pub.lPublicar = false;
                pub.lIncluir = true;
                pub.revisao = "0";


                // palavra chave...
                pub.PUBLICACAO_PALAVRA_CHAVE.Add(new PublicacaoPalavraChaveDTO());
                pub.PUBLICACAO_PALAVRA_CHAVE.FirstOrDefault().ARE_CONS_ID = 4;
                pub.PUBLICACAO_PALAVRA_CHAVE.FirstOrDefault().PPC_TEXTO = tb.complemento;

                // publicacao...
                pub.PUBLICACAO = new PublicacaoDTO();
                pub.PUBLICACAO.PUB_ATIVO = 0;
                pub.PUBLICACAO.PUB_NUMERO_ATO = tb.num;
                pub.PUBLICACAO.DATA_CADASTRO = DateTime.Now;
                pub.PUBLICACAO.PUB_DATA_ATO = tb.dataCadastro;
                pub.PUBLICACAO.PUB_CONTEUDO = html.FirstOrDefault().html;
                var mat = _serviceTpMateria.TiposMaterias(null, tb.tipo_materia).lista;
                if (mat.Count() > 0)
                {
                    pub.PUBLICACAO.TIP_MAT_ID = mat.FirstOrDefault().TIP_MAT_ID;
                }
                var ato = _serviceTpAto.TiposAtos(null, tb.expressao_ato).lista;
                if (ato.Count() > 0)
                {
                    pub.PUBLICACAO.TIP_ATO_ID = ato.FirstOrDefault().TIP_ATO_ID;
                }
                var org = _serviceOrgao.Orgaos(null, tb.org).lista;
                if (org.Count() > 0)
                {
                    pub.PUBLICACAO.ORG_ID = org.FirstOrDefault().ORG_ID;
                }

                // publicacao uf...
                pub.PUBLICACAO_UF.Add(new PublicacaoUfDTO());
                pub.PUBLICACAO_UF.FirstOrDefault().ARE_CONS_ID = 4;
                pub.PUBLICACAO_UF.FirstOrDefault().INF_NUMERO = String.IsNullOrWhiteSpace(tb.informativo) ? (int?)Convert.ToInt32(tb.informativo) : null;
                pub.PUBLICACAO_UF.FirstOrDefault().INF_ANO = tb.ano;

                retorno = this.SalvarPublicacaoAreaConsultoria(pub, tb, html.FirstOrDefault());
            }

            return retorno;
        }

        // importar matéria do portal 31 \\
        public int[] ImportarMateriaTab31(Tab_31DTO tb, int colecionadorId)
        {
            int[] retorno = null;

            PublicacaoAreaConsultoriaDTO pub = new PublicacaoAreaConsultoriaDTO();

            // buscando o texto...
            var html = _serviceTab31Html.Coad(null, null, Convert.ToInt32(tb.modulo)).lista;
            if (html.Count() > 0)
            {
                // publicacao area consultoria...
                pub.ARE_CONS_ID = colecionadorId;
                pub.PUB_PAGINA_INDICE = tb.pagina;
                pub.PUB_MANCHETE_PORTAL = tb.titulo;
                pub.lPublicar = false;
                pub.lIncluir = true;
                pub.revisao = "0";

                // palavra chave...
                pub.PUBLICACAO_PALAVRA_CHAVE.Add(new PublicacaoPalavraChaveDTO());
                pub.PUBLICACAO_PALAVRA_CHAVE.FirstOrDefault().ARE_CONS_ID = colecionadorId;
                pub.PUBLICACAO_PALAVRA_CHAVE.FirstOrDefault().PPC_TEXTO = tb.complemento;

                // publicacao...
                pub.PUBLICACAO = new PublicacaoDTO();
                pub.PUBLICACAO.PUB_ATIVO = 0;
                pub.PUBLICACAO.PUB_NUMERO_ATO = tb.num;
                pub.PUBLICACAO.DATA_CADASTRO = DateTime.Now;
                pub.PUBLICACAO.PUB_DATA_ATO = tb.datadoato;
                pub.PUBLICACAO.PUB_CONTEUDO = html.FirstOrDefault().html;
                var mat = _serviceTpMateria.TiposMaterias(null, tb.tipo_materia).lista;
                if (mat.Count() > 0)
                {
                    pub.PUBLICACAO.TIP_MAT_ID = mat.FirstOrDefault().TIP_MAT_ID;
                }
                var ato = _serviceTpAto.TiposAtos(null, tb.expressao_ato).lista;
                if (ato.Count() > 0)
                {
                    pub.PUBLICACAO.TIP_ATO_ID = ato.FirstOrDefault().TIP_ATO_ID;
                }
                var org = _serviceOrgao.Orgaos(null, tb.org).lista;
                if (org.Count() > 0)
                {
                    pub.PUBLICACAO.ORG_ID = org.FirstOrDefault().ORG_ID;
                }

                // publicacao uf...
                pub.PUBLICACAO_UF.Add(new PublicacaoUfDTO());
                pub.PUBLICACAO_UF.FirstOrDefault().ARE_CONS_ID = colecionadorId;
                pub.PUBLICACAO_UF.FirstOrDefault().INF_NUMERO = tb.informativo;
                pub.PUBLICACAO_UF.FirstOrDefault().INF_ANO = tb.ano;

                retorno = this.SalvarPublicacaoAreaConsultoria(pub, null, null, tb, html.FirstOrDefault());
            }

            return retorno;
        }

        // Sumário Portal
        public String SumarioPortal(int inf, string ano, int colecionadorId, string uf = null)
        {
            string gg = "";
            string vb = "";
            string svb = "";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS<br>";
            if (colecionadorId == 4)
                txt += "<br>DP<br>";
            if (colecionadorId == 5)
                txt += "<br>LC<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI<br>";

            txt += "Informativo: " + inf.ToString() + "/" + ano + "<br>";

            txt = "@T = Sumário<br>";

            bool temRegistro = false;

            string antes = "";

            if (colecionadorId == 4)
            {
                Pagina<Tab_30DTO> tab30 = _serviceTab30.Sumario(ano, inf.ToString(), uf, 1, 999999);
                foreach (var lst in tab30.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb + " - " + lst.expressao_ato + " " + lst.num + " " + lst.org;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(uf))
                {
                    if (colecionadorId == 1)
                        uf = "IR";
                    if (colecionadorId == 2)
                        uf = "ICMS";
                    if (colecionadorId == 3)
                        uf = "LTPS";
                    if (colecionadorId == 5)
                        uf = "LC";
                    if (colecionadorId == 6)
                        uf = "IPI";
                }

                Pagina<Tab_31DTO> tab31 = _serviceTab31.Sumario(ano, inf.ToString(), uf, colecionadorId, 1, 999999);
                foreach (var lst in tab31.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb + " - " + lst.expressao_ato + " " + lst.num + " " + lst.org;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }

            if (!temRegistro)
            {
                txt += "Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            return txt;
        }

        // Sumário...
        public String Sumario(int inf, string ano, int colecionadorId, string uf = null, string baseDados = "COADGED", int pagina = 1, int itensPorPagina = 999999)
        {
            // PROVISÓRIO ATÉ SURGIR NOVO PORTAL /////////////////////
            if (baseDados != "COADGED")
                return this.SumarioPortal(inf, ano, colecionadorId, uf);
            //////////////////////////////////////////////////////////

            // pegando todas as titulações do [colecionadorId] no cadastro...
            Pagina<TitulacaoDTO> titulacao = _serviceTitulacao.Titulacoes(null, colecionadorId);

            // pegando os grandes grupos
            var ggs = titulacao.lista.Where(w => w.TIT_TIPO == "G").OrderBy(o => o.TIT_DESCRICAO);

            // montando o índice...
            //String txt = "Sumário<br><br><br>";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS<br>";
            if (colecionadorId == 4)
                txt += "<br>DP<br>";
            if (colecionadorId == 5)
                txt += "<br>LC<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI<br>";

            txt += "Informativo: " + inf.ToString() + "/" + ano + "<br>";

            txt = "@T = Sumário<br>";

            bool temRegistro = false;


            foreach (var gg in ggs)
            {
                // verificando se houve matéria para esse gg...
                var ggr = _dao.PublicacoesAreaConsultoria(uf, null, null, null, null, null, null, null, null, inf, ano, colecionadorId, null, gg.TIT_ID).lista;
                if (ggr.Count() > 0)
                {
                    temRegistro = true;

                    // adicionando ggs...
                    txt += "@GA = " + gg.TIT_DESCRICAO.ToString() + "<br>";

                    // pegando verbetes...
                    var vbs = titulacao.lista.Where(w => w.TIT_TIPO == "V" && w.TIT_ID_REFERENCIA == gg.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                    foreach (var vb in vbs)
                    {
                        // verificando se houve matéria para esse verbete...
                        var vbr = _dao.PublicacoesAreaConsultoria(uf, null, null, null, null, null, null, null, null, inf, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID).lista;
                        if (vbr.Count() > 0)
                        {
                            // adicionando vbs...
                            txt += "@VB = " + vb.TIT_DESCRICAO.ToString() + "<br>";

                            // pegando subverbetes...
                            var svbs = titulacao.lista.Where(w => w.TIT_TIPO == "S" && w.TIT_ID_REFERENCIA == vb.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                            foreach (var svb in svbs)
                            {
                                // verificando se houve matéria para esse subverbete...
                                var svbr = _dao.PublicacoesAreaConsultoria(uf, null, null, null, null, null, null, null, null, inf, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID, svb.TIT_ID).lista;
                                if (svbr.Count() > 0)
                                {
                                    // pegando o tipo e número do ato...
                                    var pub = _servicePublicacao.FindById(svbr.First().PUB_ID);
                                    var ato = _serviceTpAto.FindById(pub.TIP_ATO_ID);
                                    var org = _serviceOrgao.FindById(pub.ORG_ID);
                                    var mostrarAto = (ato == null) ? "".ToString() : ato.TIP_ATO_DESCRICAO.ToString() + " " + pub.PUB_NUMERO_ATO.ToString() + org == null ? "" : " " + org.ORG_DESCRICAO;

                                    // adicionando  svbs...
                                    var lin = " " + svb.TIT_DESCRICAO.ToString() + " - " + mostrarAto;
                                    var pag = svbr.First().PUB_PAGINA_SUMARIO.ToString();
                                    txt += lin + "@PAGINA = " + pag + "<br>";
                                }
                            }
                            //txt += "<br>";
                        }
                    }
                    //txt += "<br>";
                }
            }

            if (!temRegistro)
            {
                txt += "Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            // retornando o sumário...
            return txt;
        }

        // Indice Orientacoes Lembretes Portal
        public String IndiceOrientacoesLembretesPortal(string ano, int colecionadorId, string uf = null)
        {
            string gg = "";
            string vb = "";
            string svb = "";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE DE ORIENTAÇÕES E LEMBRETES<br>";

            bool temRegistro = false;

            string antes = "";

            if (colecionadorId == 4)
            {
                Pagina<Tab_30DTO> tab30 = _serviceTab30.IndiceOrientacoesLembretes(ano, uf, 1, 999999);
                foreach (var lst in tab30.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(uf))
                {
                    if (colecionadorId == 1)
                        uf = "IR";
                    if (colecionadorId == 2)
                        uf = "ICMS";
                    if (colecionadorId == 3)
                        uf = "LTPS";
                    if (colecionadorId == 5)
                        uf = "LC";
                    if (colecionadorId == 6)
                        uf = "IPI";
                }

                Pagina<Tab_31DTO> tab31 = _serviceTab31.IndiceOrientacoesLembretes(ano, uf, colecionadorId, 1, 999999);
                foreach (var lst in tab31.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            return txt;
        }

        // índice de Orientações e Lembretes...
        public String IndiceOrientacoesLembretes(string ano, int colecionadorId, string uf = null, string baseDados = "COADGED", int pagina = 1, int itensPorPagina = 999999)
        {
            // PROVISÓRIO ATÉ SURGIR NOVO PORTAL /////////////////////////////////
            if (baseDados != "COADGED")
                return this.IndiceOrientacoesLembretesPortal(ano, colecionadorId, uf);
            //////////////////////////////////////////////////////////////////////

            // pegando todas as titulações do [colecionadorId] no cadastro...
            Pagina<TitulacaoDTO> titulacao = _serviceTitulacao.Titulacoes(null, colecionadorId);

            // pegando os grandes grupos
            var ggs = titulacao.lista.Where(w => w.TIT_TIPO == "G").OrderBy(o => o.TIT_DESCRICAO);

            // montando o índice...
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE DE ORIENTAÇÕES E LEMBRETES<br>";

            bool temRegistro = false;

            foreach (var gg in ggs)
            {
                // filtrando apenas Lembretes(1) e Orientações(2)...
                int?[] tpMat = new int?[] { 1, 2 };

                // verificando se houve matéria para esse gg...
                var ggr = _dao.PublicacoesAreaConsultoria(null, null, null, null, tpMat, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID).lista;
                if (ggr.Count() > 0)
                {
                    temRegistro = true;

                    // adicionando ggs...
                    txt += "@GA = " + gg.TIT_DESCRICAO.ToString() + "<br>";

                    // pegando verbetes...
                    var vbs = titulacao.lista.Where(w => w.TIT_TIPO == "V" && w.TIT_ID_REFERENCIA == gg.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                    foreach (var vb in vbs)
                    {
                        // verificando se houve matéria para esse verbete...
                        var vbr = _dao.PublicacoesAreaConsultoria(null, null, null, null, tpMat, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID).lista;
                        if (vbr.Count() > 0)
                        {
                            // adicionando vbs...
                            txt += "@VB = " + vb.TIT_DESCRICAO.ToString() + "<br>";

                            // pegando subverbetes...
                            var svbs = titulacao.lista.Where(w => w.TIT_TIPO == "S" && w.TIT_ID_REFERENCIA == vb.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                            foreach (var svb in svbs)
                            {
                                // verificando se houve matéria para esse subverbete...
                                var svbr = _dao.PublicacoesAreaConsultoria(null, null, null, null, tpMat, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID, svb.TIT_ID).lista;
                                if (svbr.Count() > 0)
                                {
                                    // adicionando  svbs...
                                    var lin = svb.TIT_DESCRICAO.ToString();
                                    if (String.IsNullOrWhiteSpace(svbr.First().PUB_EXPRESSAO))
                                    {
                                        lin += " - " + svbr.First().PUB_EXPRESSAO;
                                    }
                                    if (svbr.First().CAP_ID != null)
                                    {
                                        var cap = new CapitalSRV().FindById(svbr.First().CAP_ID);
                                        lin += " - " + cap.CAP_NOME;
                                    }
                                    var pag = svbr.First().PUB_PAGINA_INDICE.ToString();
                                    txt += lin + "@PAGINA = " + pag + "<br>";
                                }
                            }
                            //txt += "<br>";
                        }
                    }
                    //txt += "<br>";
                }
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            // retornando o índice...
            return txt;
        }

        // índice Alfabético e Remissivo...
        public String IndiceAlfabeticoRemissivoPortal(string ano, int colecionadorId, string uf = null)
        {
            string gg = "";
            string vb = "";
            string svb = "";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE ALFABÉTICO E REMISSIVO<br>";

            bool temRegistro = false;

            string antes = "";

            if (colecionadorId == 4)
            {
                Pagina<Tab_30DTO> tab30 = _serviceTab30.IndiceAlfabeticoRemissivo(ano, uf, 1, 999999);
                foreach (var lst in tab30.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb + " - " + lst.expressao_ato + " " + lst.num + " " + lst.org;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(uf))
                {
                    if (colecionadorId == 1)
                        uf = "IR";
                    if (colecionadorId == 2)
                        uf = "ICMS";
                    if (colecionadorId == 3)
                        uf = "LTPS";
                    if (colecionadorId == 5)
                        uf = "LC";
                    if (colecionadorId == 6)
                        uf = "IPI";
                }

                Pagina<Tab_31DTO> tab31 = _serviceTab31.IndiceOrientacoesLembretes(ano, uf, colecionadorId, 1, 999999);
                foreach (var lst in tab31.lista)
                {
                    temRegistro = true;

                    if (lst.gg != gg)
                    {
                        txt += "@GA = " + lst.gg + "<br>";
                        gg = lst.gg;
                    }
                    if (lst.vb != vb)
                    {
                        txt += "@VB = " + lst.vb + "<br>";
                        vb = lst.vb;
                    }
                    var lin = lst.svb + " - " + lst.expressao_ato + " " + lst.num + " " + lst.org;
                    lin = lin + "@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }

                    svb = lst.svb;
                }
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            return txt;
        }

        // índice Alfabético e Remissivo...
        public String IndiceAlfabeticoRemissivo(string ano, int colecionadorId, string uf = null, string baseDados = "COADGED", int pagina = 1, int itensPorPagina = 999999)
        {
            // PROVISÓRIO ATÉ SURGIR NOVO PORTAL /////////////////////////////////
            if (baseDados != "COADGED")
                return this.IndiceAlfabeticoRemissivoPortal(ano, colecionadorId, uf);
            //////////////////////////////////////////////////////////////////////

            // pegando todas as titulações do [colecionadorId] no cadastro...
            Pagina<TitulacaoDTO> titulacao = _serviceTitulacao.Titulacoes(null, colecionadorId);

            // pegando os grandes grupos
            var ggs = titulacao.lista.Where(w => w.TIT_TIPO == "G").OrderBy(o => o.TIT_DESCRICAO);

            // montando o índice...
            //String txt = "ÍNDICE ALFABÉTICO E REMISSIVO<br><br><br>";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE ALFABÉTICO E REMISSIVO<br>";

            bool temRegistro = false;

            foreach (var gg in ggs)
            {
                // verificando se houve matéria para esse gg...
                var ggr = _dao.PublicacoesAreaConsultoria(null, null, null, null, null, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID).lista;
                if (ggr.Count() > 0)
                {
                    temRegistro = true;

                    // adicionando ggs...
                    txt += "@GA = " + gg.TIT_DESCRICAO.ToString() + "<br>";

                    // pegando verbetes...
                    var vbs = titulacao.lista.Where(w => w.TIT_TIPO == "V" && w.TIT_ID_REFERENCIA == gg.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                    foreach (var vb in vbs)
                    {
                        // verificando se houve matéria para esse verbete...
                        var vbr = _dao.PublicacoesAreaConsultoria(null, null, null, null, null, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID).lista;
                        if (vbr.Count() > 0)
                        {
                            // adicionando vbs...
                            txt += "@VB = " + vb.TIT_DESCRICAO.ToString() + "<br>";

                            // pegando subverbetes...
                            var svbs = titulacao.lista.Where(w => w.TIT_TIPO == "S" && w.TIT_ID_REFERENCIA == vb.TIT_ID).OrderBy(o => o.TIT_DESCRICAO);
                            foreach (var svb in svbs)
                            {
                                // verificando se houve matéria para esse subverbete...
                                var svbr = _dao.PublicacoesAreaConsultoria(null, null, null, null, null, null, null, null, null, null, ano, colecionadorId, null, gg.TIT_ID, vb.TIT_ID, svb.TIT_ID).lista;
                                if (svbr.Count() > 0)
                                {
                                    // pegando o tipo e número do ato...
                                    var pub = _servicePublicacao.FindById(svbr.First().PUB_ID);
                                    var mostrarAto = "";
                                    if (pub.TIP_ATO_ID != null)
                                    {
                                        var ato = _serviceTpAto.FindById(pub.TIP_ATO_ID);
                                        mostrarAto = (ato == null) ? "".ToString() : ato.TIP_ATO_DESCRICAO.ToString() + " " + pub.PUB_NUMERO_ATO.ToString();
                                    }
                                    else
                                    {
                                        mostrarAto = new TipoMateriaSRV().FindById(svbr.First().PUBLICACAO.TIP_MAT_ID).TIP_MAT_DESCRICAO;
                                    }
                                    // adicionando  svbs...
                                    var lin = svb.TIT_DESCRICAO.ToString();
                                    if (String.IsNullOrWhiteSpace(svbr.First().PUB_EXPRESSAO))
                                    {
                                        lin += " - " + svbr.First().PUB_EXPRESSAO;
                                    }
                                    if (svbr.First().CAP_ID != null)
                                    {
                                        var cap = new CapitalSRV().FindById(svbr.First().CAP_ID);
                                        lin += " - " + cap.CAP_NOME;
                                    }
                                    var org = _serviceOrgao.FindById(pub.ORG_ID);
                                    lin += " - " + mostrarAto + org == null ? "" : " " + org.ORG_DESCRICAO;
                                    var pag = svbr.First().PUB_PAGINA_INDICE.ToString();
                                    txt += lin + "@PAGINA = " + pag + "<br>";
                                }
                            }
                            txt += "<br>";
                        }
                    }
                    txt += "<br>";
                }
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            // retornando o índice...
            return txt;
        }

        // índice Numérico dos Atos...
        public String IndiceNumericoAtosPortal(string ano, int colecionadorId, string uf = null)
        {
            string ato = "";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE NUMÉRICO DOS ATOS<br>";

            bool temRegistro = false;

            string antes = "";

            if (colecionadorId == 4)
            {
                Pagina<Tab_30DTO> tab30 = _serviceTab30.IndiceNumericoAtos(ano, uf, 1, 999999);
                foreach (var lst in tab30.lista)
                {
                    temRegistro = true;

                    if (lst.expressao_ato != ato)
                    {
                        txt += "<br><br>" + lst.expressao_ato + "<br><br>";
                        ato = lst.expressao_ato;
                    }
                    DateTime d = (DateTime)lst.dataCadastro;
                    var dt = d.ToString("dd-MM-yyyy");
                    var lin = "- " + lst.num + " - " + lst.org + " - " + dt + " @GA = " + lst.gg + " @VB = " + lst.vb;
                    lin = lin + "<br>@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(uf))
                {
                    if (colecionadorId == 1)
                        uf = "IR";
                    if (colecionadorId == 2)
                        uf = "ICMS";
                    if (colecionadorId == 3)
                        uf = "LTPS";
                    if (colecionadorId == 5)
                        uf = "LC";
                    if (colecionadorId == 6)
                        uf = "IPI";
                }

                Pagina<Tab_31DTO> tab31 = _serviceTab31.IndiceNumericoAtos(ano, uf, colecionadorId, 1, 999999);
                foreach (var lst in tab31.lista)
                {
                    temRegistro = true;

                    if (lst.expressao_ato != ato)
                    {
                        txt += "<br><br>" + lst.expressao_ato + "<br><br>";
                        ato = lst.expressao_ato;
                    }
                    DateTime d = (DateTime)lst.datadoato;
                    var dt = d.ToString("dd-MM-yyyy");
                    var lin = "- " + lst.num + " - " + lst.org + " - " + dt + " @GA = " + lst.gg + " @VB = " + lst.vb;
                    lin = lin + "<br>@PAGINA = " + lst.pagina.ToString() + "<br>";

                    if (antes != lin)
                    {
                        antes = lin;
                        txt += lin;
                    }
                }
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            return txt;
        }

        // índice Numérico dos Atos...
        public String IndiceNumericoAtos(string ano, int colecionadorId, string uf = null, string baseDados = "COADGED", int pagina = 1, int itensPorPagina = 999999)
        {
            // PROVISÓRIO ATÉ SURGIR NOVO PORTAL /////////////////////////////////
            if (baseDados != "COADGED")
                return this.IndiceNumericoAtosPortal(ano, colecionadorId, uf);
            //////////////////////////////////////////////////////////////////////

            // montando o índice...
            //String txt = "ÍNDICE NUMÉRICO DOS ATOS<br><br>";
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE NUMÉRICO DOS ATOS<br>";

            bool temRegistro = false;

            // pegando todos os registros...
            var atos = _dao.RodarIndiceNumericoAtos(ano, colecionadorId).lista;
            var tpAto = "";
            foreach (var ato in atos)
            {
                temRegistro = true;

                if (tpAto != ato.PUBLICACAO.TIPO_ATO.TIP_ATO_DESCRICAO.ToString())
                {
                    // adicionando o tipo do ato...
                    tpAto = ato.PUBLICACAO.TIPO_ATO.TIP_ATO_DESCRICAO.ToString();
                    txt += "<br>" + tpAto + "<br>";
                }
                DateTime d = (DateTime)ato.PUBLICACAO.PUB_DATA_ATO;
                var dt = d.ToString("dd-MM-yyyy");
                var gg = _serviceTitulacao.FindById(ato.PUBLICACAO_TITULACAO.First().TIT_ID).TIT_DESCRICAO;
                var vb = _serviceTitulacao.FindById(ato.PUBLICACAO_TITULACAO.First().TIT_ID_VERBETE).TIT_DESCRICAO;
                var oe = _serviceOrgao.FindById(ato.PUBLICACAO.ORG_ID).ORG_DESCRICAO;
                var ln = "- " + ato.PUBLICACAO.PUB_NUMERO_ATO.ToString() + " - " + oe + " " + dt + " @GA = " + gg + " @VB = " + vb;

                // adicionando  atos...
                var pag = ato.PUB_PAGINA_INDICE.ToString();
                txt += ln + "<br>@PAGINA = " + pag + "<br>";
                //txt += ln.PadRight(100, '.') + " " + (pag != "" ? pag : "Nº Página") + "<br>";
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            // retornando o índice...
            return txt;
        }

        // índice Numérico das Alterações e Revogações...
        public String IndiceNumericoAlteracoesRevogacoes(string ano, int colecionadorId, string uf = null, string baseDados = "COADGED", int pagina = 1, int itensPorPagina = 999999)
        {
            // montando o índice...
            String txt = "";

            if (colecionadorId == 1)
                txt += "<br>IR *** " + ano + "<br>";
            if (colecionadorId == 2)
                txt += "<br>ICMS / " + uf + " *** " + ano + "<br>";
            if (colecionadorId == 3)
                txt += "<br>LTPS *** " + ano + "<br>";
            if (colecionadorId == 4)
                txt += "<br>DP *** " + ano + "<br>";
            if (colecionadorId == 5)
                txt += "<br>LC *** " + ano + "<br>";
            if (colecionadorId == 6)
                txt += "<br>IPI *** " + ano + "<br>";

            txt = "@T = ÍNDICE NUMÉRICO DAS ALTERAÇÕES E REVOGAÇÕES<br><br>";

            bool temRegistro = false;

            // pegando todos os registros...
            var atos = _dao.RodarIndiceNumericoAlteracoesRevogacoes(ano, colecionadorId).lista;
            var tpAto = "";
            foreach (var ato in atos)
            {
                temRegistro = true;

                if (tpAto != ato.PUBLICACAO.TIPO_ATO.TIP_ATO_DESCRICAO.ToString())
                {
                    // adicionando o tipo do ato...
                    tpAto = ato.PUBLICACAO.TIPO_ATO.TIP_ATO_DESCRICAO.ToString();
                    txt += "<br>" + tpAto + "<br>";
                }
                DateTime d = (DateTime)ato.PUBLICACAO.PUB_DATA_ATO;
                var dt = d.ToString("dd-MM-yyyy");
                var gg = _serviceTitulacao.FindById(ato.PUBLICACAO_TITULACAO.First().TIT_ID).TIT_DESCRICAO;
                var vb = _serviceTitulacao.FindById(ato.PUBLICACAO_TITULACAO.First().TIT_ID_VERBETE).TIT_DESCRICAO;
                var sv = _serviceTitulacao.FindById(ato.PUBLICACAO_TITULACAO.First().TIT_ID_SUBVERBETE).TIT_DESCRICAO;
                var oe = _serviceOrgao.FindById(ato.PUBLICACAO.ORG_ID).ORG_DESCRICAO;
                var rv = _servicePublicacaoAlteracaoRevogacao.PublicacaoAlteracaoRevogacao(null, ato.PUB_ID).lista.First().PUB_ALTERACAO_ATO;
                var ln = "- " + ato.PUBLICACAO.PUB_NUMERO_ATO.ToString() + " - " + oe + " " + dt + " @GA = " + gg + " @VB = " + vb + " - " + sv + " - " + rv;

                // adicionando  atos...
                var pag = ato.PUB_PAGINA_INDICE.ToString();
                txt += ln + "<br>@PAGINA = " + pag + "<br>";
                //txt += ln.PadRight(100, '.') + " " + (pag != "" ? pag : "Nº Página") + "<br>";
            }

            if (!temRegistro)
            {
                txt += ano + " *** Sem matéria registrada.";
            }

            txt = "<font face='Courier New'>" + txt + "</font>";

            // retornando o índice...
            return txt;
        }

        // substituindo as remissões da matéria impressa...
        public PublicacaoAreaConsultoriaDTO RemissaoMateriaImpressa(PublicacaoAreaConsultoriaDTO publicacaoAreaConsultoria, string sigla = "RDC")
        {
            // pegando a matéria impressa...
            String txtRemissao = "";
            if (sigla == "RDC")
                txtRemissao = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA != null) ? publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA : "";
            if (sigla == "RVT")
                txtRemissao = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT != null) ? publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT : "";
            if (sigla == "DGT")
                txtRemissao = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT != null) ? publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT : "";
            if ((sigla == "RVO") || (sigla == "DIA"))
                txtRemissao = (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO != null) ? publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO : "";

            // substituindo as remissões...
            for (var i = 0; i < publicacaoAreaConsultoria.PUBLICACAO_REMISSAO.Count; i++)
            {
                if (!String.IsNullOrWhiteSpace(publicacaoAreaConsultoria.PUBLICACAO_REMISSAO.ToList()[i].PRE_REMISSAO))
                {
                    txtRemissao = txtRemissao.Replace("&lt;&lt;" + "remissao" + (i + 1).ToString() + "&gt;&gt;", publicacaoAreaConsultoria.PUBLICACAO_REMISSAO.ToList()[i].PRE_REMISSAO);
                }
            }

            // atribui o texto transformado para o objeto...
            publicacaoAreaConsultoria.MATERIA_IMPRESSA = txtRemissao;

            // retornando o objeto...
            return publicacaoAreaConsultoria;
        }

        /// <summary>
        /// ALT: 01/06/2017 - Retorna a matéria impressa completa e formatada com manchete, ementa e titulação
        /// </summary>
        /// <param name="_pubArea"></param>
        /// <returns></returns>
        public String MateriaImpressaTexto(PublicacaoAreaConsultoriaDTO _pubArea, string sigla = "RDC")
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(_pubArea.PUBLICACAO.PUB_CONTEUDO_RESENHA))
                {
                    var txtComRemissao = this.RemissaoMateriaImpressa(_pubArea, sigla).MATERIA_IMPRESSA.ToString();
                    var txtCabecalho = this.CabecaMateriaImpressa(_pubArea).ToString();

                    if (sigla == "DGT")
                        return txtCabecalho + txtComRemissao;
                    else
                        return "Matéria: " + _pubArea.PUB_ID.ToString() + "<br>" + txtCabecalho + txtComRemissao;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// ALT: 01/06/2017 - Retorna a matéria do portal completa e formatada com manchete, ementa e titulação
        /// </summary>
        /// <param name="_pubArea"></param>
        /// <returns></returns>
        public String PublicarPortalTexto(PublicacaoAreaConsultoriaDTO _pubArea)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(_pubArea.PUBLICACAO.PUB_CONTEUDO))
                {
                    var txtCabecalho = this.CabecaMateriaImpressa(_pubArea, true).ToString();
                    return "Matéria: " + _pubArea.PUB_ID.ToString() + "<br>" + txtCabecalho + _pubArea.PUBLICACAO.PUB_CONTEUDO;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Busca tag no texto e a substitui pelo dado informado. Para suprimir o parágrafo da tag caso o parâmetro dado esteja vazio, basta informar TRUE no último parâmetro.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tag"></param>
        /// <param name="dado"></param>
        /// <param name="suprimeParagrafoVazio"></param>
        /// <returns>String com o texto trocado</returns>
        public String buscaTroca(String texto = "", String tag = null, String dado = null, bool suprimeParagrafoVazio = false)
        {
            if (!String.IsNullOrWhiteSpace(texto) && !String.IsNullOrWhiteSpace(tag))
            {
                // converte dado null em vazio...
                dado = dado == null ? "" : dado;

                // não havendo informação, suprimir o parágrafo html...
                if (String.IsNullOrWhiteSpace(dado) && suprimeParagrafoVazio)
                {
                    // encontra a tag...
                    int i = texto.IndexOf(tag);

                    string tagParagrafo = "<p style=";

                    // encontra e suprime este parágrafo '<p style='...
                    for (var p = i; p > 0; p--)
                    {
                        if (this.Substring(texto, p - tagParagrafo.Length, tagParagrafo.Length) == tagParagrafo)
                        {
                            texto = this.Substring(texto, 0, p + 1) + "display:none; " + this.Substring(texto, p + 1);
                            break;
                        }
                    }
                }
                else
                    texto = texto.Replace("&lt;&lt;" + tag + "&gt;&gt;", dado);
            }
            return texto;
        }

        // cabeça da matéria impressa...
        public String CabecaMateriaImpressa(PublicacaoAreaConsultoriaDTO pub, bool portal = false, bool ementa = true)
        {
            string dtAto = "";
            string dtPub = "";
            String texto = "";

            // se a ementa já foi publicada, então, sempre leia o conteúdo do campo <PUB_EMENTA_PORTAL>
            if ((pub.PUBLICACAO_CONFIG.Count() > 0) && (pub.PUBLICACAO_CONFIG.FirstOrDefault().PCF_DATA_PUB_EMENTA != null))
                ementa = true;

            if (pub.PUBLICACAO.TIP_MAT_ID != null)
            {
                var mat = _serviceTpMateria.FindById(pub.PUBLICACAO.TIP_MAT_ID);
                if (mat != null)
                    texto = (mat.ARE_CABECA_MATERIA != null) ? mat.ARE_CABECA_MATERIA : texto;
            }

            // preparando as datas...
            if (pub.PUBLICACAO.PUB_DATA_ATO != null)
            {
                DateTime d = (DateTime)pub.PUBLICACAO.PUB_DATA_ATO;
                dtAto = d.ToString("dd-MM-yyyy");
            }
            if (pub.PUBLICACAO.PUB_DATA_PUB_ATO != null)
            {
                DateTime d = (DateTime)pub.PUBLICACAO.PUB_DATA_PUB_ATO;
                dtPub = d.ToString("dd-MM-yyyy");
            }

            // substituindo os valores...
            foreach (var tit in pub.PUBLICACAO_TITULACAO)
            {
                if (tit.PTI_PRINCIPAL == true)
                {
                    string txtManchete = portal ? pub.PUB_MANCHETE_PORTAL : pub.PUB_MANCHETE;
                    string txtEmenta = portal ? pub.PUB_EMENTA_PORTAL : pub.PUB_EMENTA;

                    string gg = tit.TIT_ID == null ? "" : _serviceTitulacao.FindById(tit.TIT_ID).TIT_DESCRICAO;
                    string vb = tit.TIT_ID_VERBETE == null ? "" : _serviceTitulacao.FindById(tit.TIT_ID_VERBETE).TIT_DESCRICAO;
                    string svb = tit.TIT_ID_SUBVERBETE == null ? "" : _serviceTitulacao.FindById(tit.TIT_ID_SUBVERBETE).TIT_DESCRICAO;

                    string tpMat = pub.PUBLICACAO.TIP_MAT_ID == null ? "" : _serviceTpMateria.FindById(pub.PUBLICACAO.TIP_MAT_ID).TIP_MAT_DESCRICAO;
                    string tpAto = pub.PUBLICACAO.TIP_ATO_ID == null ? "" : _serviceTpAto.FindById(pub.PUBLICACAO.TIP_ATO_ID).TIP_ATO_DESCRICAO;
                    string orgao = pub.PUBLICACAO.ORG_ID == null ? "" : _serviceOrgao.FindById(pub.PUBLICACAO.ORG_ID).ORG_DESCRICAO;
                    string veicu = pub.PUBLICACAO.TVI_ID == null ? "" : _serviceVeiculo.FindById(pub.PUBLICACAO.TVI_ID).TVI_DESCRICAO;

                    texto = this.buscaTroca(texto, "manchete", txtManchete, true);
                    texto = this.buscaTroca(texto, "ementa", txtEmenta, true);

                    texto = this.buscaTroca(texto, "gg", gg, true);
                    texto = this.buscaTroca(texto, "vb", vb, true);
                    texto = this.buscaTroca(texto, "svb", svb, true);

                    texto = this.buscaTroca(texto, "complVei", pub.PUBLICACAO.PUB_COMPL_VEICULO);

                    texto = this.buscaTroca(texto, "tpMat", tpMat, true);

                    texto = this.buscaTroca(texto, "tpAto", tpAto, true);
                    texto = this.buscaTroca(texto, "nrAto", pub.PUBLICACAO.PUB_NUMERO_ATO, true);
                    texto = this.buscaTroca(texto, "dtAto", dtAto, true);
                    texto = this.buscaTroca(texto, "orgao", orgao);

                    texto = this.buscaTroca(texto, "dtPub", dtPub, true);
                    texto = this.buscaTroca(texto, "veiculo", veicu, true);
                }
            }

            string txtRev = "";

            if (portal)
            {
                // revogação...
                String[] rev = textosMateriaRevogada(pub);
                foreach (var txt in rev)
                {
                    if (!String.IsNullOrWhiteSpace(txt))
                        txtRev += txt + "<br>";
                }
                txtRev = !String.IsNullOrWhiteSpace(txtRev) ? txtRev.Substring(0, txtRev.Length - 4) : txtRev;
            }

            texto = this.buscaTroca(texto, "revogado", txtRev, true);

            // retornando a cabeça da matéria...
            return texto;
        }

        /// <summary>
        /// Identifica se o Ato foi Revogado ou Alterado, ou se Revogou ou Alterou outro, e retorna Array com dois Textos identificando tais ações.
        /// </summary>
        /// <param name="pub"></param>
        /// <returns></returns>
        public String[] textosMateriaRevogada(PublicacaoAreaConsultoriaDTO pub)
        {
            String[] aTxt = new String[2];
            string dtAto = "";

            // foi revogado/alterado por outros?
            var arev = _servicePublicacaoAlteracaoRevogacao.PublicacaoAlteracaoRevogacao(null, null, null, pub.PUBLICACAO.TIP_ATO_ID, pub.PUBLICACAO.PUB_NUMERO_ATO, pub.PUBLICACAO.PUB_DATA_ATO).lista;
            foreach (var rev in arev)
            {
                // Ato que o revogou/alterou?
                var prev = _servicePublicacao.FindById(rev.PUB_ID);
                if (prev != null)
                {
                    if (prev.PUB_DATA_ATO != null)
                    {
                        DateTime d = (DateTime)prev.PUB_DATA_ATO;
                        dtAto = d.ToString("dd-MM-yyyy");
                    }
                    if (rev.PAR_TIPO == "A")
                    {
                        aTxt[0] += "ESTE ATO FOI ALTERADO POR " + _serviceTpAto.FindById(prev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + prev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                    }
                    else
                    if (rev.PAR_TIPO == "V")
                    {
                        aTxt[0] += "ESTE ATO FOI REVIGORADO POR " + _serviceTpAto.FindById(prev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + prev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                    }
                    else
                    {
                        aTxt[0] += "ESTE ATO FOI REVOGADO POR " + _serviceTpAto.FindById(prev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + prev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                    }
                }
            }
            aTxt[0] = !String.IsNullOrWhiteSpace(aTxt[0]) ? aTxt[0].Substring(0, aTxt[0].Length - 4) : aTxt[0]; // eliminando a última quebra de linha...
            // revogou/alterou outros?
            if (pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO.Count() > 0)
            {
                foreach (var rev in pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO)
                {
                    if (rev.PUB_NUMERO_ATO != null)
                    {
                        if (rev.PUB_DATA_ATO != null)
                        {
                            DateTime d = (DateTime)rev.PUB_DATA_ATO;
                            dtAto = d.ToString("dd-MM-yyyy");
                        }
                        if (rev.PAR_TIPO == "A")
                        {
                            aTxt[1] += "ESTE ATO ALTEROU " + _serviceTpAto.FindById(rev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + rev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                        }
                        else
                        if (rev.PAR_TIPO == "V")
                        {
                            aTxt[1] += "ESTE ATO REVIGOROU " + _serviceTpAto.FindById(rev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + rev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                        }
                        else
                        {
                            aTxt[1] += "ESTE ATO REVOGOU " + _serviceTpAto.FindById(rev.TIP_ATO_ID).TIP_ATO_DESCRICAO.ToString() + " Nº " + rev.PUB_NUMERO_ATO + " DE " + dtAto + " - " + rev.PUB_ALTERACAO_ATO + "<br>";
                        }
                    }
                }
                aTxt[1] = !String.IsNullOrWhiteSpace(aTxt[1]) ? aTxt[1].Substring(0, aTxt[1].Length - 4) : aTxt[1]; // eliminando a última quebra de linha...
            }
            return aTxt;
        }

        // indice numérico dos atos...
        public Pagina<PublicacaoAreaConsultoriaDTO> RodarIndiceNumeroAtos(string anoInformativo, int colecionadorId, int pagina = 1, int itensPorPagina = 3)
        {
            var resp = _dao.RodarIndiceNumericoAtos(anoInformativo, colecionadorId, pagina, itensPorPagina);
            return resp;
        }

        // busca do index...
        public Pagina<PublicacaoAreaConsultoriaDTO> PublicacoesAreaConsultoria(string uf = null, string faseMateria = null, DateTime? dtCadastro = null, string coadgedBI = null, int?[] tpMateria = null, int? nrMateria = null, string anoAto = null, int? tpAto = null, string nrAto = null, int? nrInformativo = null, string anoInformativo = null, int? colecionadorId = null, int? colaboradorId = null, int? gg = null, int? vb = null, int? svb = null, DateTime? dtAto = null, int? ativoId = null, int pagina = 1, int itensPorPagina = 3)
        {
            var resp = _dao.PublicacoesAreaConsultoria(uf, faseMateria, dtCadastro, coadgedBI, tpMateria, nrMateria, anoAto, tpAto, nrAto, nrInformativo, anoInformativo, colecionadorId, colaboradorId, gg, vb, svb, dtAto, ativoId, pagina, itensPorPagina);
            return resp;
        }

        // busca para o Painel...
        public IQueryable<PublicacaoAreaConsultoriaDTO> Painel(int? colaboradorId = null, int? colecionadorId = null, int? nrInformativo = null, string anoInformativo = null, int pagina = 1, int itensPorPagina = 999999)
        {
            var resp = _dao.Painel(colaboradorId, colecionadorId, nrInformativo, anoInformativo, pagina, itensPorPagina);
            return resp;
        }

        // salvando pagina do sumário e índice...
        public void SalvarPaginacaoMateria(IList<PublicacaoAreaConsultoriaDTO> publicacaoAreaConsultoria)
        {
            try
            {
                // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    // salvando PublicacaoAreaConsultoria...
                    foreach (var pub in publicacaoAreaConsultoria)
                    {
                        if (pub.ARE_CONS_ID != null && pub.PUB_ID != null)
                        {
                            Merge(pub, "PUB_ID", "ARE_CONS_ID");
                        }
                    }
                    // commit - confirmando operação sem erros...
                    scope.Complete();
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

        // salvando em 11 tabelas...
        public int[] SalvarPublicacaoAreaConsultoria(PublicacaoAreaConsultoriaDTO publicacaoAreaConsultoria, Tab_30DTO t30 = null, Tab_30_htmlDTO t30h = null, Tab_31DTO t31 = null, Tab_31_htmlDTO t31h = null)
        {
            PublicacaoDTO pub;
            PublicacaoAreaConsultoriaDTO pubAreaCons;
            AreasDTO colecionador;

            Tab_31DTO tab31 = new Tab_31DTO();
            Tab_30DTO tab30 = new Tab_30DTO();

            try
            {
                // abrindo a transação...
                var txOpt = new TransactionOptions();
                txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                txOpt.Timeout = TransactionManager.MaximumTimeout;

                // pegando os dados de PublicacaoAlteracaoRevogacao...
                var pubAltRev = publicacaoAreaConsultoria.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO;

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    // retirando PublicacaoAlteracaoRevogacao de Publicacao para salvar...
                    publicacaoAreaConsultoria.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO = null;

                    // checando se houve alteração realizada pela revisão na publicacao...
                    if (publicacaoAreaConsultoria.revisao != "0") // veio da revisão != "0"; senão = redação mesmo;
                    {
                        // 
                        var publicacao = _servicePublicacao.FindById(publicacaoAreaConsultoria.PUB_ID);
                        var pubTit = _servicePublicacaoTitulacao.PublicacaoTitulacao(publicacaoAreaConsultoria.PUB_ID, publicacaoAreaConsultoria.ARE_CONS_ID).lista;
                        var pubRem = _servicePublicacaoRemissao.PublicacaoRemissao(publicacaoAreaConsultoria.PUB_ID, publicacaoAreaConsultoria.ARE_CONS_ID).lista;
                        var pubAre = Dao.FindById(publicacaoAreaConsultoria.PUB_ID, publicacaoAreaConsultoria.ARE_CONS_ID);
                        var pubRevo = _servicePublicacaoAlteracaoRevogacao.PublicacaoAlteracaoRevogacao(null, publicacaoAreaConsultoria.PUB_ID).lista;
                        var pubCfg = _servicePublicacaoConfig.PublicacaoConfig(publicacaoAreaConsultoria.PUB_ID, publicacaoAreaConsultoria.ARE_CONS_ID).lista;

                        // verifica se a revisão alterou os dados da publicação...
                        ClassMetadata c = new ClassMetadata();

                        bool lPub = c.CompararAtributosDeObjetos(publicacaoAreaConsultoria.PUBLICACAO, publicacao);
                        bool lPAr = c.CompararAtributosDeObjetos(publicacaoAreaConsultoria, pubAre);

                        // titulacao
                        var a = new List<Object>();
                        var b = new List<Object>();

                        foreach (var o in publicacaoAreaConsultoria.PUBLICACAO_TITULACAO)
                        {
                            a.Add(o);
                        }
                        foreach (var o in pubTit)
                        {
                            b.Add(o);
                        }

                        bool lTit = c.CompararAtributosDeLstObjetos(a, b);
                        //bool lTit = c.CompararAtributosDeLstObjetos((publicacaoAreaConsultoria.PUBLICACAO_TITULACAO as List<object>), (pubTit as List<object>));

                        // remissao
                        a = new List<Object>();
                        b = new List<Object>();

                        foreach (var o in publicacaoAreaConsultoria.PUBLICACAO_REMISSAO)
                        {
                            a.Add(o);
                        }
                        foreach (var o in pubRem)
                        {
                            b.Add(o);
                        }

                        bool lRem = c.CompararAtributosDeLstObjetos(a, b);
                        //bool lRem = c.CompararAtributosDeLstObjetos((publicacaoAreaConsultoria.PUBLICACAO_REMISSAO as List<object>), (pubRem as List<object>));


                        // revogacao
                        a = new List<Object>();
                        b = new List<Object>();

                        foreach (var o in pubAltRev)
                        {
                            a.Add(o);
                        }
                        foreach (var o in pubRevo)
                        {
                            b.Add(o);
                        }

                        bool lRev = c.CompararAtributosDeLstObjetos(a, b);
                        //bool lRev = c.CompararAtributosDeLstObjetos((pubAltRev as List<object>), (pubRevo as List<object>));

                        // config
                        a = new List<Object>();
                        b = new List<Object>();

                        foreach (var o in publicacaoAreaConsultoria.PUBLICACAO_CONFIG)
                        {
                            a.Add(o);
                        }
                        foreach (var o in pubCfg)
                        {
                            b.Add(o);
                        }

                        bool lCfg = c.CompararAtributosDeLstObjetos(a, b);
                        //bool lCfg = c.CompararAtributosDeLstObjetos((publicacaoAreaConsultoria.PUBLICACAO_CONFIG as List<object>), (pubCfg as List<object>));

                        if (!(lPub && lTit && lRem && lPAr && lRev && lCfg)) // se alterou, salve histórico
                        {
                            new PublicacaoRevisaoSRV().SalvarAlteracaoDaMateria(publicacaoAreaConsultoria);
                        }
                    }

                    #region Salvando publicacao

                    // determinando se inclui ou altera em Publicacao
                    publicacaoAreaConsultoria.PUBLICACAO.PUB_ID = (publicacaoAreaConsultoria.lIncluir == false) ? publicacaoAreaConsultoria.PUB_ID : null;
                    // mantendo o conteúdo dos campos de revisão
                    if (publicacaoAreaConsultoria.lIncluir == false)
                    {
                        pub = _servicePublicacao.FindById(publicacaoAreaConsultoria.PUB_ID);

                        if ((pub.PUB_CONTEUDO_RDC != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RDC == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RDC = pub.PUB_CONTEUDO_RDC;

                        if ((pub.PUB_CONTEUDO_RESENHA_DGT != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = pub.PUB_CONTEUDO_RESENHA_DGT;

                        if ((pub.PUB_CONTEUDO_RESENHA_RDC != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC = pub.PUB_CONTEUDO_RESENHA_RDC;

                        if ((pub.PUB_CONTEUDO_RESENHA_RVO != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO = pub.PUB_CONTEUDO_RESENHA_RVO;

                        if ((pub.PUB_CONTEUDO_RESENHA_RVT != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT = pub.PUB_CONTEUDO_RESENHA_RVT;

                        if ((pub.PUB_CONTEUDO_RVO != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVO = pub.PUB_CONTEUDO_RVO;

                        if ((pub.PUB_CONTEUDO_RVT != null) && (publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVT == null))
                            publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO_RVT = pub.PUB_CONTEUDO_RVT;
                    }

                    // salvando Publicacao
                    pub = _servicePublicacao.SalvarPublicacao(publicacaoAreaConsultoria.PUBLICACAO);

                    // registrando a edição da matéria pelo usuário - isso previne que dois usuários a editem ao mesmo tempo
                    if (publicacaoAreaConsultoria.lIncluir == true)
                    {
                        new PublicacaoEditadaSRV().RegistrarEdicaoMateria((int)pub.PUB_ID, false);
                    }

                    #endregion Salvando publicacao

                    // salvando PublicacaoAlteracaoRevogacao...
                    foreach (var ar in pubAltRev)
                    {
                        ar.PUB_ID = (int)pub.PUB_ID;
                    }
                    _servicePublicacaoAlteracaoRevogacao.SalvarPublicacaoAlteracaoRevogacao(pubAltRev);

                    // salvando PublicacaoAreaConsultoria...
                    pubAreaCons = new PublicacaoAreaConsultoriaDTO();
                    pubAreaCons.PUB_ID = pub.PUB_ID;
                    pubAreaCons.ARE_CONS_ID = publicacaoAreaConsultoria.ARE_CONS_ID;
                    pubAreaCons.PUB_MANCHETE = publicacaoAreaConsultoria.PUB_MANCHETE;
                    pubAreaCons.PUB_EMENTA = publicacaoAreaConsultoria.PUB_EMENTA;
                    pubAreaCons.PUB_MANCHETE_PORTAL = publicacaoAreaConsultoria.PUB_MANCHETE_PORTAL;
                    pubAreaCons.PUB_EMENTA_PORTAL = publicacaoAreaConsultoria.PUB_EMENTA_PORTAL;
                    pubAreaCons.PUB_PAGINA_SUMARIO = publicacaoAreaConsultoria.PUB_PAGINA_SUMARIO;
                    pubAreaCons.PUB_PAGINA_INDICE = publicacaoAreaConsultoria.PUB_PAGINA_INDICE;
                    pubAreaCons.CAP_ID = publicacaoAreaConsultoria.CAP_ID;
                    pubAreaCons.PUB_EXPRESSAO = publicacaoAreaConsultoria.PUB_EXPRESSAO;

                    if ((pubAreaCons.PUB_ID != null) && (pubAreaCons.ARE_CONS_ID != null))
                    {
                        if (publicacaoAreaConsultoria.lIncluir)
                        {
                            Save(pubAreaCons);
                        }
                        else
                        {
                            Merge(pubAreaCons, "PUB_ID", "ARE_CONS_ID");
                        }
                    }

                    // salvando PublicacaoTitulacao...
                    foreach (var pubTit in publicacaoAreaConsultoria.PUBLICACAO_TITULACAO)
                    {
                        pubTit.PUB_ID = pub.PUB_ID;
                        pubTit.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    }
                    _servicePublicacaoTitulacao.SalvarPublicacaoTitulacao(publicacaoAreaConsultoria.PUBLICACAO_TITULACAO);

                    // salvando PublicacaoUf...
                    foreach (var pubUf in publicacaoAreaConsultoria.PUBLICACAO_UF)
                    {
                        pubUf.PUB_ID = (int)pub.PUB_ID;
                        pubUf.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    }
                    _servicePublicacaoUf.SalvarPublicacaoUf(publicacaoAreaConsultoria.PUBLICACAO_UF);

                    // salvando PublicacaoPalavraChave...
                    foreach (var pubPchave in publicacaoAreaConsultoria.PUBLICACAO_PALAVRA_CHAVE)
                    {
                        pubPchave.PUB_ID = (int)pub.PUB_ID;
                        pubPchave.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    }
                    _servicePublicacaoPalavraChave.SalvarPublicacaoPalavraChave(publicacaoAreaConsultoria.PUBLICACAO_PALAVRA_CHAVE);

                    // salvando PublicacaoConfig...
                    foreach (var pubCfg in publicacaoAreaConsultoria.PUBLICACAO_CONFIG)
                    {
                        pubCfg.PUB_ID = (int)pub.PUB_ID;
                        pubCfg.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                        _servicePublicacaoConfig.SalvarPublicacaoConfig(pubCfg);
                    }

                    // salvando PublicacaoRemissao...
                    foreach (var pubRsao in publicacaoAreaConsultoria.PUBLICACAO_REMISSAO)
                    {
                        pubRsao.PUB_ID = (int)pub.PUB_ID;
                        pubRsao.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    }
                    _servicePublicacaoRemissao.SalvarPublicacaoRemissao(publicacaoAreaConsultoria.PUBLICACAO_REMISSAO);

                    // salvando PublicacaoRemissivo...
                    foreach (var pubRvo in publicacaoAreaConsultoria.PUBLICACAO_REMISSIVO)
                    {
                        pubRvo.PUB_ID = (int)pub.PUB_ID;
                        pubRvo.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    }
                    _servicePublicacaoRemissivo.SalvarPublicacaoRemissivo(publicacaoAreaConsultoria.PUBLICACAO_REMISSIVO);

                    // salvando PublicacaoBusca...
                    //foreach (var pubBI in publicacaoAreaConsultoria.PUBLICACAO_BUSCA)
                    //{
                    //    pubBI.PUB_ID = (int)pub.PUB_ID;
                    //    pubBI.ARE_CONS_ID = (int)publicacaoAreaConsultoria.ARE_CONS_ID;
                    //}
                    //_servicePublicacaoBusca.SalvarPublicacaoBusca(publicacaoAreaConsultoria.PUBLICACAO_BUSCA);

                    // commit - confirmando operação sem erros...
                    scope.Complete();
                }

                // devolvendo as Alterações/Revogações ao DTO PublicacaoAreaConsultoria...
                publicacaoAreaConsultoria.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO = pubAltRev;
                colecionador = _serviceAreas.Areas(publicacaoAreaConsultoria.ARE_CONS_ID).lista.FirstOrDefault();

                // publicar matéria?...
                if (publicacaoAreaConsultoria.lPublicar)
                {
                    if (colecionador != null && colecionador.ARE_CONS_DESCRICAO.Substring(0, 3) == "ATC")
                    {
                        // publicando - TAB_31 -------------------------------------------------------------------------------------- TAB_31

                        // buscar: se já foi publicada, tem ID, senão, null...
                        Tab_31DTO tb31 = _serviceTab31.Coad(pub.PUB_ID).lista.FirstOrDefault();

                        // preenchendo o objeto para gravação...
                        string area = this.Substring(_serviceAreas.FindById(pubAreaCons.ARE_CONS_ID).ARE_CONS_DESCRICAO, 0, 50);
                        int i = area.IndexOf("-");
                        area = area.Substring(i + 1);
                        area = (publicacaoAreaConsultoria.PUBLICACAO_UF.Count > 1) ? "TODOS OS ESTADOS" : area;

                        tab31.id = (tb31 != null) ? tb31.id : null;
                        tab31.idGED = pub.PUB_ID;
                        tab31.colec = area;
                        foreach (var tit in publicacaoAreaConsultoria.PUBLICACAO_TITULACAO)
                        {
                            // publicar somente a titulação principal...
                            if (tit.PTI_PRINCIPAL == true)
                            {
                                tab31.gg = (tit.TIT_ID != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID).TIT_DESCRICAO, 0, 50) : "";
                                tab31.vb = (tit.TIT_ID_VERBETE != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID_VERBETE).TIT_DESCRICAO, 0, 250) : "";
                                tab31.svb = (tit.TIT_ID_SUBVERBETE != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID_SUBVERBETE).TIT_DESCRICAO, 0, 250) : "";
                                break;
                            }
                        }
                        tab31.complemento = "";
                        foreach (var pal in publicacaoAreaConsultoria.PUBLICACAO_PALAVRA_CHAVE)
                        {
                            // palavra-chave...
                            tab31.complemento = tab31.complemento + pal.PPC_TEXTO + ' ';
                        }
                        tab31.tipo_materia = (pub.TIP_MAT_ID != null) ? this.Substring(_serviceTpMateria.FindById(pub.TIP_MAT_ID).TIP_MAT_DESCRICAO, 0, 50) : "";
                        tab31.expressao_ato = (pub.TIP_ATO_ID != null) ? this.Substring(_serviceTpAto.FindById(pub.TIP_ATO_ID).TIP_ATO_DESCRICAO, 0, 50) : "";
                        tab31.num = this.Substring(pub.PUB_NUMERO_ATO, 0, 50);
                        tab31.org = (pub.ORG_ID != null) ? this.Substring(_serviceOrgao.FindById(pub.ORG_ID).ORG_DESCRICAO, 0, 50) : "";
                        tab31.pagina = pubAreaCons.PUB_PAGINA_SUMARIO;
                        foreach (var inf in publicacaoAreaConsultoria.PUBLICACAO_UF)
                        {
                            tab31.informativo = inf.INF_NUMERO;
                            tab31.ano = inf.INF_ANO;
                        }

                        DateTime d = (DateTime)tab31.dataCadastro;
                        string ano = d.ToString("yyyy");

                        tab31.titulo = pubAreaCons.PUB_MANCHETE_PORTAL;
                        tab31.dataCadastro = (tb31 == null || (Convert.ToInt32(ano) < 1998)) ? DateTime.Now : tab31.dataCadastro;
                        tab31.LABEL = (pub.LBL_ID != null) ? this.Substring(_serviceLabel.FindById(pub.LBL_ID).LBL_DESCRICAO, 0, 50) : "";
                        tab31.Destino = "I".ToString();
                        tab31.Revisar = "N".ToString();
                        tab31.datadoato = pub.PUB_DATA_ATO;
                        tab31.publicacao = pub.PUB_DATA_PUB_ATO;
                        tab31.diario = (pub.TVI_ID != null) ? this.Substring(_serviceVeiculo.FindById(pub.TVI_ID).TVI_DESCRICAO, 0, 50) : "";
                        tab31.ementa = this.Substring(pubAreaCons.PUB_EMENTA_PORTAL, 0, 250);
                        tab31.status_colec = "S".ToString();
                        tab31.status_vb = "S".ToString();
                        tab31.newsletter = "N";
                        foreach (var cfg in publicacaoAreaConsultoria.PUBLICACAO_CONFIG)
                        {
                            tab31.newsletter = (cfg.PCF_DATA_PUB_NEWS != null) ? "S".ToString() : "N".ToString();
                        }
                        tab31.secao = pub.SEC_ID;
                        tab31.desc_news = this.Substring(pub.PUB_DESCRICAO_NEWS, 0, 250);
                        tab31.classiBuscaAvancada = 0;

                        // abrindo transação para salvar no Portal-MySQL...
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            if (tab31.id == null)
                            {
                                //1a. Gravação na Tab_31 para pegar o ID e gerar o módulo...
                                var tab_31Salvo = _serviceTab31.Save(tab31);
                                tab31.id = tab_31Salvo.id;
                            }
                            tab31.modulo = "900".ToString() + tab31.id.ToString(); // 900 + id;

                            // 2a. Gravação na Tab_31 com o módulo...
                            _serviceTab31.Merge(tab31, "id");

                            // commit...
                            scope.Complete();
                        }

                        // publicando - TAB_31_HTML ------------------------------------------------------------------------------------ TAB_31_HTML
                        Tab_31_htmlDTO tab31html = new Tab_31_htmlDTO();

                        // buscar: se já foi publicada, tem ID, senão, null...
                        Tab_31_htmlDTO tb31html = _serviceTab31Html.Coad(pub.PUB_ID).lista.FirstOrDefault();

                        String texto = this.CabecaMateriaImpressa(publicacaoAreaConsultoria, true, publicacaoAreaConsultoria.PUBLICACAO_CONFIG.FirstOrDefault().publicarEmenta);

                        // preenchendo o objeto para gravação...
                        tab31html.id = (tb31html != null) ? tb31html.id : null;
                        tab31html.idGED = pub.PUB_ID;
                        tab31html.colec = this.Substring(tab31.colec, 0, 255);
                        tab31html.modulo = Convert.ToInt32(tab31.modulo);
                        tab31html.html = texto + publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;
                        tab31html.bug = 0;

                        // abrindo transação para salvar no Portal-MySQL...
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            if (tab31html.id == null)
                            {
                                _serviceTab31Html.Save(tab31html);
                            }
                            else
                            {
                                _serviceTab31Html.Merge(tab31html, "id");
                            }

                            // commit...
                            scope.Complete();
                        }

                        // Revogando a matéria...
                        foreach (var ar in pubAltRev)
                        {
                            // achando na Tab31 o registro da matéria a revogar \\
                            tab31 = _serviceTab31.Coad(null, null, _serviceTpAto.FindById(ar.TIP_ATO_ID).TIP_ATO_DESCRICAO, ar.PUB_NUMERO_ATO, ar.PUB_DATA_ATO).lista.FirstOrDefault();
                            if (tab31 != null)
                            {
                                // marcando a (R)evogação (A)lteração re(V)igoração da matéria \\
                                tab31.GED_revoga_altera = ar.PAR_TIPO;

                                // abrindo transação para salvar no Portal-MySQL...
                                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                                {
                                    // salvando...
                                    _serviceTab31.Merge(tab31, "id");

                                    // commit...
                                    scope.Complete();
                                }

                                // achando no COADGED o registro da matéria a revogar...
                                PublicacaoAreaConsultoriaDTO pubRev = _dao.PublicacoesAreaConsultoria(null, null, null, null, null, null, null, ar.TIP_ATO_ID, ar.PUB_NUMERO_ATO, null, null, null, null, null, null, null, ar.PUB_DATA_ATO).lista.FirstOrDefault();
                                if (pubRev != null)
                                {
                                    // achando na TAB31html o registro da matéria a revogar...
                                    tb31html = _serviceTab31Html.Coad(null, null, Convert.ToInt32(tab31.modulo)).lista.FirstOrDefault();
                                    if (tb31html != null)
                                    {
                                        texto = this.CabecaMateriaImpressa(pubRev, true, publicacaoAreaConsultoria.PUBLICACAO_CONFIG.FirstOrDefault().publicarEmenta);

                                        // inserindo a mensagem de REVOGAÇÃO/ALTERAÇÃO na matéria...
                                        tb31html.html = texto + pubRev.PUBLICACAO.PUB_CONTEUDO;

                                        // abrindo transação para salvar no Portal-MySQL...
                                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                                        {
                                            if (tb31html.id == null)
                                            {
                                                _serviceTab31Html.Save(tb31html);
                                            }
                                            else
                                            {
                                                _serviceTab31Html.Merge(tb31html, "id");
                                            }

                                            // commit...
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                        }

                        // publicando - BUSCA ----------------------------------------------------------------------------------------- BUSCA
                        Busca_completa_tributarioDTO busca = new Busca_completa_tributarioDTO();

                        // buscar: se já foi publicada, tem ID, senão, null...
                        Busca_completa_tributarioDTO bus = _serviceBusca.Busca(pub.PUB_ID).lista.FirstOrDefault();

                        // preenchendo o objeto para gravação...
                        busca.id = (bus != null) ? bus.id : null;
                        busca.idGED = pub.PUB_ID;
                        busca.id_conteudo = (int)tab31.id;
                        busca.id_tipo_conteudo = 31;
                        busca.id_perfil = null;
                        busca.data_conteudo = DateTime.Now;
                        busca.palavras_indexadas = tab31.gg + " " +
                                                   tab31.vb + " " +
                                                   tab31.svb + " " +
                                                   tab31.tipo_materia + " " +
                                                   tab31.expressao_ato + " " +
                                                   tab31.num + " " +
                                                   tab31.org + " " +
                                                   tab31.complemento;

                        // abrindo transação para salvar no Portal-MySQL...
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            if (busca.id == null)
                            {
                                _serviceBusca.Save(busca);
                            }
                            else
                            {
                                _serviceBusca.Merge(busca, "id");
                            }

                            // commit...
                            scope.Complete();
                        }
                    }
                    else
                    {
                        // publicando - TAB_30 -------------------------------------------------------------------------------------- TAB_30

                        // buscar: se já foi publicada, tem ID, senão, null...
                        Tab_30DTO tb30 = _serviceTab30.Coad(pub.PUB_ID).lista.FirstOrDefault();

                        // preenchendo o objeto para gravação...
                        string area = this.Substring(_serviceAreas.FindById(pubAreaCons.ARE_CONS_ID).ARE_CONS_DESCRICAO, 0, 50);
                        int i = area.IndexOf("-");
                        area = area.Substring(i + 1);

                        tab30.id = (tb30 != null) ? tb30.id : null;
                        tab30.idGED = pub.PUB_ID;
                        tab30.colec = area;
                        foreach (var tit in publicacaoAreaConsultoria.PUBLICACAO_TITULACAO)
                        {
                            // publicar somente a titulação principal...
                            if (tit.PTI_PRINCIPAL == true)
                            {
                                tab30.gg = (tit.TIT_ID != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID).TIT_DESCRICAO, 0, 50) : "";
                                tab30.vb = (tit.TIT_ID_VERBETE != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID_VERBETE).TIT_DESCRICAO, 0, 250) : "";
                                tab30.svb = (tit.TIT_ID_SUBVERBETE != null) ? this.Substring(_serviceTitulacao.FindById(tit.TIT_ID_SUBVERBETE).TIT_DESCRICAO, 0, 250) : "";
                                break;
                            }
                        }
                        tab30.complemento = "";
                        foreach (var pal in publicacaoAreaConsultoria.PUBLICACAO_PALAVRA_CHAVE)
                        {
                            // palavra-chave...
                            tab30.complemento = tab30.complemento + pal.PPC_TEXTO + ' ';
                        }
                        tab30.tipo_materia = (pub.TIP_MAT_ID != null) ? this.Substring(_serviceTpMateria.FindById(pub.TIP_MAT_ID).TIP_MAT_DESCRICAO, 0, 50) : "";
                        tab30.expressao_ato = (pub.TIP_ATO_ID != null) ? this.Substring(_serviceTpAto.FindById(pub.TIP_ATO_ID).TIP_ATO_DESCRICAO, 0, 50) : "";
                        tab30.num = this.Substring(pub.PUB_NUMERO_ATO, 0, 50);
                        tab30.org = (pub.ORG_ID != null) ? this.Substring(_serviceOrgao.FindById(pub.ORG_ID).ORG_DESCRICAO, 0, 50) : "";
                        tab30.pagina = pubAreaCons.PUB_PAGINA_SUMARIO;
                        foreach (var inf in publicacaoAreaConsultoria.PUBLICACAO_UF)
                        {
                            tab30.informativo = inf.INF_NUMERO.ToString();
                            tab30.ano = inf.INF_ANO;
                        }

                        DateTime d = (DateTime)tab30.dataCadastro;
                        string ano = d.ToString("yyyy");

                        tab30.titulo = pubAreaCons.PUB_MANCHETE;
                        tab30.dataCadastro = (tb30 == null || (Convert.ToInt32(ano) < 1998)) ? DateTime.Now : tab30.dataCadastro;

                        // abrindo transação para salvar no Portal-MySQL...
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            if (tab30.id == null)
                            {
                                //1a. Gravação na Tab_30 para pegar o ID e gerar o módulo...
                                var tab_30Salvo = _serviceTab30.Save(tab30);
                                tab30.id = tab_30Salvo.id;
                            }
                            tab30.modulo = "900".ToString() + tab30.id.ToString(); // 900 + id;

                            // 2a. Gravação na Tab_30 com o módulo...
                            _serviceTab30.Merge(tab30, "id");

                            // commit...
                            scope.Complete();
                        }

                        // publicando - TAB_30_HTML ------------------------------------------------------------------------------------ TAB_30_HTML
                        Tab_30_htmlDTO tab30html = new Tab_30_htmlDTO();

                        // buscar: se já foi publicada, tem ID, senão, null...
                        Tab_30_htmlDTO tb30html = _serviceTab30Html.Coad(pub.PUB_ID).lista.FirstOrDefault();

                        String texto = this.CabecaMateriaImpressa(publicacaoAreaConsultoria, true, publicacaoAreaConsultoria.PUBLICACAO_CONFIG.FirstOrDefault().publicarEmenta);

                        // preenchendo o objeto para gravação...
                        tab30html.id = (tb30html != null) ? tb30html.id : null;
                        tab30html.idGED = pub.PUB_ID;
                        tab30html.colec = this.Substring(tab30.colec, 0, 255);
                        tab30html.modulo = Convert.ToInt32(tab30.modulo);
                        tab30html.html = texto + publicacaoAreaConsultoria.PUBLICACAO.PUB_CONTEUDO;

                        // abrindo transação para salvar no Portal-MySQL...
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            if (tab30html.id == null)
                            {
                                _serviceTab30Html.Save(tab30html);
                            }
                            else
                            {
                                _serviceTab30Html.Merge(tab30html, "id");
                            }

                            // commit...
                            scope.Complete();
                        }

                        // Revogando a matéria...
                        foreach (var ar in pubAltRev)
                        {
                            // achando na Tab30 o registro da matéria a revogar \\
                            tab30 = _serviceTab30.Coad(null, null, _serviceTpAto.FindById(ar.TIP_ATO_ID).TIP_ATO_DESCRICAO, ar.PUB_NUMERO_ATO, ar.PUB_DATA_ATO).lista.FirstOrDefault();
                            if (tab30 != null)
                            {
                                // marcando a (R)evogação (A)lteração re(V)igoração da matéria \\
                                tab30.GED_revoga_altera = ar.PAR_TIPO;

                                // abrindo transação para salvar no Portal-MySQL...
                                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                                {
                                    // salvando...
                                    _serviceTab30.Merge(tab30, "id");

                                    // commit...
                                    scope.Complete();
                                }

                                // achando no COADGED o registro da matéria a revogar...
                                PublicacaoAreaConsultoriaDTO pubRev = _dao.PublicacoesAreaConsultoria(null, null, null, null, null, null, null, ar.TIP_ATO_ID, ar.PUB_NUMERO_ATO, null, null, null, null, null, null, null, ar.PUB_DATA_ATO).lista.FirstOrDefault();
                                if (pubRev != null)
                                {
                                    // achando na TAB30html o registro da matéria a revogar...
                                    tb30html = _serviceTab30Html.Coad(null, null, Convert.ToInt32(tab30.modulo)).lista.FirstOrDefault();
                                    if (tb30html != null)
                                    {
                                        texto = this.CabecaMateriaImpressa(pubRev, true, publicacaoAreaConsultoria.PUBLICACAO_CONFIG.FirstOrDefault().publicarEmenta);

                                        // inserindo a mensagem de REVOGAÇÃO/ALTERAÇÃO na matéria...
                                        tb30html.html = texto + pubRev.PUBLICACAO.PUB_CONTEUDO;

                                        // abrindo transação para salvar no Portal-MySQL...
                                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                                        {
                                            if (tb30html.id == null)
                                            {
                                                _serviceTab30Html.Save(tb30html);
                                            }
                                            else
                                            {
                                                _serviceTab30Html.Merge(tb30html, "id");
                                            }

                                            // commit...
                                            scope.Complete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {   // atualiza o idGED \\
                    if (t30 != null)
                    {
                        t30.idGED = pub.PUB_ID;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            _serviceTab30.Merge(t30, "id");
                            scope.Complete();
                        }
                    }
                    if (t30h != null)
                    {
                        t30h.idGED = pub.PUB_ID;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            _serviceTab30Html.Merge(t30h, "id");
                            scope.Complete();
                        }
                    }
                    if (t31 != null)
                    {
                        t31.idGED = pub.PUB_ID;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            _serviceTab31.Merge(t31, "id");
                            scope.Complete();
                        }
                    }
                    if (t31h != null)
                    {
                        t31h.idGED = pub.PUB_ID;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                        {
                            _serviceTab31Html.Merge(t31h, "id");
                            scope.Complete();
                        }
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

            // retornando ID da matéria no COADGED e no PORTAL...
            int id = 0;

            if (colecionador != null)
            {
                if (colecionador.ARE_CONS_DESCRICAO.Substring(0, 3) == "ATC")
                {
                    tab31 = _serviceTab31.Coad(pub.PUB_ID).lista.FirstOrDefault();
                    id = (tab31 != null) ? (int)tab31.id : id;
                }
                else
                {
                    tab30 = _serviceTab30.Coad(pub.PUB_ID).lista.FirstOrDefault();
                    id = (tab30 != null) ? (int)tab30.id : id;
                }
            }

            int[] retorno = new int[2];

            retorno[0] = (int)pub.PUB_ID;
            retorno[1] = id;

            return retorno;
        }
    }
}
