using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using COAD.SEGURANCA.Model;

namespace COAD.PORTAL.Utils
{
    public class UtilsPortal
    {
        public List<SelectListItem> AbrangenciaObrigacoes(string escolha)
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            if (escolha.Equals("99"))
            {
                opcoes.Add(new SelectListItem { Text = "FEDERAIS", Value = "99", Selected = true });
                opcoes.Add(new SelectListItem { Text = "ESTADUAIS", Value = "97" });
                opcoes.Add(new SelectListItem { Text = "MUNICIPAIS", Value = "98" });
            }
            else if (escolha.Equals("00"))
            {
                opcoes.Add(new SelectListItem { Text = "FEDERAIS", Value = "99" });
                opcoes.Add(new SelectListItem { Text = "ESTADUAIS", Value = "97", Selected = true });
                opcoes.Add(new SelectListItem { Text = "MUNICIPAIS", Value = "98" });
            }
            else if (escolha.Equals("98"))
            {
                opcoes.Add(new SelectListItem { Text = "FEDERAIS", Value = "99" });
                opcoes.Add(new SelectListItem { Text = "ESTADUAIS", Value = "97" });
                opcoes.Add(new SelectListItem { Text = "MUNICIPAIS", Value = "98", Selected = true });
            }
            else
            {
                opcoes.Add(new SelectListItem { Text = "FEDERAIS", Value = "99" });
                opcoes.Add(new SelectListItem { Text = "ESTADUAIS", Value = "97" });
                opcoes.Add(new SelectListItem { Text = "MUNICIPAIS", Value = "98" });
            }
            return opcoes;
        }

        public List<SelectListItem> Orientacoes(string escolha)
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            opcoes.Add(new SelectListItem { Text = "Imposto de Renda/ PIS e COFINS", Value = "IR" });
            opcoes.Add(new SelectListItem { Text = "Trabalho e Previdência", Value = "LTPS" });
            opcoes.Add(new SelectListItem { Text = "Legislação Comercial e Societária e outros assuntos Federais", Value = "LC" });
            opcoes.Add(new SelectListItem { Text = "Contabilidade e Gestão", Value = "CONTABILIDADE E GESTAO" });
            opcoes.Add(new SelectListItem { Text = "IPI/Importação e Exportação", Value = "IPI" });
            opcoes.Add(new SelectListItem { Text = "ICMS/ISS - Todos os Estados e Municípios", Value = "TODOS OS ESTADOS" });
            opcoes.Add(new SelectListItem { Text = "RJ - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "RJ" });
            opcoes.Add(new SelectListItem { Text = "MG - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "MG" });
            opcoes.Add(new SelectListItem { Text = "SP - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "SP" });
            opcoes.Add(new SelectListItem { Text = "ES - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "ES" });
            opcoes.Add(new SelectListItem { Text = "PR - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "PR" });
            opcoes.Add(new SelectListItem { Text = "RS - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "RS" });
            opcoes.Add(new SelectListItem { Text = "SC - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "SC" });
            opcoes.Add(new SelectListItem { Text = "BA - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "BA" });
            opcoes.Add(new SelectListItem { Text = "DF - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "DF" });
            opcoes.Add(new SelectListItem { Text = "GO - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "GO" });
            opcoes.Add(new SelectListItem { Text = "AC - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "AC" });
            opcoes.Add(new SelectListItem { Text = "CE - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "CE" });
            opcoes.Add(new SelectListItem { Text = "PE - ICMS/ISS e Outros Assuntos Estaduais e Municipais", Value = "PE" });

            return opcoes;
        }

        public List<SelectListItem> AtosLegais(string escolha)
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            opcoes.Add(new SelectListItem { Text = "Emenda Constitucional", Value = "Emenda Constitucional" });
            opcoes.Add(new SelectListItem { Text = "Lei Complementar", Value = "Lei Complementar" });
            opcoes.Add(new SelectListItem { Text = "Lei", Value = "Lei" });
            opcoes.Add(new SelectListItem { Text = "Medida Provisória", Value = "Medida Provisória" });
            opcoes.Add(new SelectListItem { Text = "Decreto", Value = "Decreto" });
            opcoes.Add(new SelectListItem { Text = "Decreto-lei", Value = "Decreto-lei" });
            opcoes.Add(new SelectListItem { Text = "Instrução Normativa", Value = "Instrução Normativa" });
            opcoes.Add(new SelectListItem { Text = "Portaria", Value = "Portaria" });
            opcoes.Add(new SelectListItem { Text = "Ação Direta de Inconstitucionalidade", Value = "Ação Direta de Inconstitucionalidade" });
            opcoes.Add(new SelectListItem { Text = "Acórdão ", Value = "Acórdão" });
            opcoes.Add(new SelectListItem { Text = "Adendo", Value = "Adendo" });
            opcoes.Add(new SelectListItem { Text = "Advocacia-Geral da União", Value = "Advocacia-Geral da União" });
            opcoes.Add(new SelectListItem { Text = "Agravo no Recurso Especial", Value = "Agravo no Recurso Especial" });
            opcoes.Add(new SelectListItem { Text = "Ajuste SINIEF", Value = "Ajuste SINIEF" });
            opcoes.Add(new SelectListItem { Text = "Apelação Cível", Value = "Apelação Cível" });
            opcoes.Add(new SelectListItem { Text = "Ato", Value = "Ato" });
            opcoes.Add(new SelectListItem { Text = "Ato Conjunto", Value = "Ato Conjunto" });
            opcoes.Add(new SelectListItem { Text = "Ato de Instrução Normativa", Value = "Ato de Instrução Normativa" });
            opcoes.Add(new SelectListItem { Text = "Ato de Suspensão", Value = "Ato de Suspensão" });
            opcoes.Add(new SelectListItem { Text = "Ato Declaratório", Value = "Ato Declaratório" });
            opcoes.Add(new SelectListItem { Text = "Ato Declaratório Executivo", Value = "Ato Declaratório Executivo" });
            opcoes.Add(new SelectListItem { Text = "Ato Declaratório Executivo Conjunto", Value = "Ato Declaratório Executivo Conjunto" });
            opcoes.Add(new SelectListItem { Text = "Ato Declaratório Interpretativo", Value = "Ato Declaratório Interpretativo" });
            opcoes.Add(new SelectListItem { Text = "Ato Declaratório Normativo", Value = "Ato Declaratório Normativo" });
            opcoes.Add(new SelectListItem { Text = "Ato Deliberativo", Value = "Ato Deliberativo" });
            opcoes.Add(new SelectListItem { Text = "Ato Diat", Value = "Ato Diat" });
            opcoes.Add(new SelectListItem { Text = "Ato Homologatório", Value = "Ato Homologatório" });
            opcoes.Add(new SelectListItem { Text = "Ato Normativo", Value = "Ato Normativo" });
            opcoes.Add(new SelectListItem { Text = "Ato Regimental", Value = "Ato Regimental" });
            opcoes.Add(new SelectListItem { Text = "Ato S/N", Value = "Ato S/N" });
            opcoes.Add(new SelectListItem { Text = "Aviso", Value = "Aviso" });
            opcoes.Add(new SelectListItem { Text = "Aviso de Lançamento", Value = "Aviso de Lançamento" });
            opcoes.Add(new SelectListItem { Text = "Boletim Central", Value = "Boletim Central" });
            opcoes.Add(new SelectListItem { Text = "Boletim de Preços", Value = "Boletim de Preços" });
            opcoes.Add(new SelectListItem { Text = "Carta-Circular", Value = "Carta-Circular" });
            opcoes.Add(new SelectListItem { Text = "Circular", Value = "Circular" });
            opcoes.Add(new SelectListItem { Text = "Comunicado", Value = "Comunicado" });
            opcoes.Add(new SelectListItem { Text = "Comunicado ao Mercado", Value = "Comunicado ao Mercado" });
            opcoes.Add(new SelectListItem { Text = "Comunicado S/N", Value = "Comunicado S/N" });
            opcoes.Add(new SelectListItem { Text = "Comunicado Técnico", Value = "Comunicado Técnico" });
            opcoes.Add(new SelectListItem { Text = "Comunicados", Value = "Comunicados" });
            opcoes.Add(new SelectListItem { Text = "Conselho da Justiça Federal", Value = "Conselho da Justiça Federal" });
            opcoes.Add(new SelectListItem { Text = "Constituição do Estado", Value = "Constituição do Estado" });
            opcoes.Add(new SelectListItem { Text = "Constituição do Estado do Espírito Santo", Value = "Constituição do Estado do Espírito Santo" });
            opcoes.Add(new SelectListItem { Text = "Constituição do Estado do Rio de Janeiro", Value = "Constituição do Estado do Rio de Janeiro" });
            opcoes.Add(new SelectListItem { Text = "Constituição Estadual", Value = "Constituição Estadual" });
            opcoes.Add(new SelectListItem { Text = "Constituição Federal", Value = "Constituição Federal" });
            opcoes.Add(new SelectListItem { Text = "Consulta", Value = "Consulta" });
            opcoes.Add(new SelectListItem { Text = "Convenção", Value = "Convenção" });
            opcoes.Add(new SelectListItem { Text = "Convênio", Value = "Convênio" });
            opcoes.Add(new SelectListItem { Text = "Convênio Arrecadação", Value = "Convênio Arrecadação" });
            opcoes.Add(new SelectListItem { Text = "Convênio ECF", Value = "Convênio ECF" });
            opcoes.Add(new SelectListItem { Text = "Convênio ICM", Value = "Convênio ICM" });
            opcoes.Add(new SelectListItem { Text = "Convênio ICMS", Value = "Convênio ICMS" });
            opcoes.Add(new SelectListItem { Text = "Decisão", Value = "Decisão" });
            opcoes.Add(new SelectListItem { Text = "Decisão Conjunta", Value = "Decisão Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Decisão Normativa", Value = "Decisão Normativa" });
            opcoes.Add(new SelectListItem { Text = "Decreto Legislativo", Value = "Decreto Legislativo" });
            opcoes.Add(new SelectListItem { Text = "Deliberação", Value = "Deliberação" });
            opcoes.Add(new SelectListItem { Text = "Deliberação Normativa", Value = "Deliberação Normativa" });
            opcoes.Add(new SelectListItem { Text = "Despacho", Value = "Despacho" });
            opcoes.Add(new SelectListItem { Text = "E-Comunicado", Value = "E-Comunicado" });
            opcoes.Add(new SelectListItem { Text = "E-Comunicado Conjunto", Value = "E-Comunicado Conjunto" });
            opcoes.Add(new SelectListItem { Text = "Edital", Value = "Edital" });
            opcoes.Add(new SelectListItem { Text = "Edital de Convocação", Value = "Edital de " });
            opcoes.Add(new SelectListItem { Text = "Edital de Credenciamento", Value = "Edital de " });
            opcoes.Add(new SelectListItem { Text = "Edital de Intimação", Value = "Edital de " });
            opcoes.Add(new SelectListItem { Text = "Edital de Lançamento", Value = "Edital de " });
            opcoes.Add(new SelectListItem { Text = "Edital de Notificação", Value = "Edital de " });
            opcoes.Add(new SelectListItem { Text = "Edital S/N", Value = "Edital S/N" });
            opcoes.Add(new SelectListItem { Text = "Emenda à Lei Orgânica", Value = "Emenda à Lei Orgânica" });
            opcoes.Add(new SelectListItem { Text = "Ementa Normativa", Value = "Ementa Normativa" });
            opcoes.Add(new SelectListItem { Text = "Enunciado", Value = "Enunciado" });
            opcoes.Add(new SelectListItem { Text = "Instrução", Value = "Instrução" });
            opcoes.Add(new SelectListItem { Text = "Instrução Complementar", Value = "Instrução Complementar" });
            opcoes.Add(new SelectListItem { Text = "Instrução de Serviço", Value = "Instrução de Serviço" });
            opcoes.Add(new SelectListItem { Text = "Instrução de Serviço Conjunta", Value = "Instrução de Serviço Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Instrução de Serviço Interna", Value = "Instrução de Serviço Interna" });
            opcoes.Add(new SelectListItem { Text = "Instrução Intersecretarial", Value = "Instrução Intersecretarial" });
            opcoes.Add(new SelectListItem { Text = "Instrução Normativa Conjunta", Value = "Instrução Normativa Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Instrução Normativa Interministerial", Value = "Instrução Normativa Interministerial" });
            opcoes.Add(new SelectListItem { Text = "Instrução Normativa Intersecretarial", Value = "Instrução Normativa Intersecretarial" });
            opcoes.Add(new SelectListItem { Text = "Instrução Normativa S/N", Value = "Instrução Normativa S/N" });
            opcoes.Add(new SelectListItem { Text = "Instrução Técnica", Value = "Instrução Técnica" });
            opcoes.Add(new SelectListItem { Text = "Lei Orgânica do Município do Rio de Janeiro", Value = "Lei Orgânica do Município do Rio de Janeiro" });
            opcoes.Add(new SelectListItem { Text = "Mandado de Segurança", Value = "Mandado de Segurança" });
            opcoes.Add(new SelectListItem { Text = "Memorando", Value = "Memorando" });
            opcoes.Add(new SelectListItem { Text = "Norma", Value = "Norma" });
            opcoes.Add(new SelectListItem { Text = "Norma Brasileira de Contabilidade", Value = "Norma Brasileira de Contabilidade" });
            opcoes.Add(new SelectListItem { Text = "Norma Complementar", Value = "Norma Complementar" });
            opcoes.Add(new SelectListItem { Text = "Norma Conjunta de Execução", Value = "Norma Conjunta de Execução" });
            opcoes.Add(new SelectListItem { Text = "Norma de Execução", Value = "Norma de Execução" });
            opcoes.Add(new SelectListItem { Text = "Norma de Procedimento Administrativo", Value = "Norma de Procedimento Administrativo" });
            opcoes.Add(new SelectListItem { Text = "Norma de Procedimento Fiscal", Value = "Norma de Procedimento Fiscal" });
            opcoes.Add(new SelectListItem { Text = "Norma de Procedimento Fiscal Conjunta", Value = "Norma de Procedimento Fiscal Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Norma Operacional", Value = "Norma Operacional" });
            opcoes.Add(new SelectListItem { Text = "Norma Técnica", Value = "Norma Técnica" });
            opcoes.Add(new SelectListItem { Text = "Nota Explicativa", Value = "Nota Explicativa" });
            opcoes.Add(new SelectListItem { Text = "Nota Oficial", Value = "Nota Oficial" });
            opcoes.Add(new SelectListItem { Text = "Nota Oficial S/N", Value = "Nota Oficial S/N" });
            opcoes.Add(new SelectListItem { Text = "Nota Técnica", Value = "Nota Técnica" });
            opcoes.Add(new SelectListItem { Text = "Notícia", Value = "Notícia" });
            opcoes.Add(new SelectListItem { Text = "Notificação", Value = "Notificação" });
            opcoes.Add(new SelectListItem { Text = "Ofício", Value = "Ofício" });
            opcoes.Add(new SelectListItem { Text = "Ofício-Circular", Value = "Ofício-Circular" });
            opcoes.Add(new SelectListItem { Text = "Ordem de Serviço", Value = "Ordem de Serviço" });
            opcoes.Add(new SelectListItem { Text = "Ordem de Serviço Conjunta", Value = "Ordem de Serviço Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Ordem Serviço", Value = "Ordem Serviço" });
            opcoes.Add(new SelectListItem { Text = "Orientação de Serviço", Value = "Orientação de Serviço" });
            opcoes.Add(new SelectListItem { Text = "Orientação Interna Conjunta", Value = "Orientação Interna Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Orientação Interpretativa", Value = "Orientação Interpretativa" });
            opcoes.Add(new SelectListItem { Text = "Orientação Jurisprudencial", Value = "Orientação Jurisprudencial" });
            opcoes.Add(new SelectListItem { Text = "Orientação Jurisprudencial Transitória", Value = "Orientação Jurisprudencial Transitória" });
            opcoes.Add(new SelectListItem { Text = "Orientação Normativa", Value = "Orientação Normativa" });
            opcoes.Add(new SelectListItem { Text = "Orientação Tributária", Value = "Orientação Tributária" });
            opcoes.Add(new SelectListItem { Text = "Parecer", Value = "Parecer" });
            opcoes.Add(new SelectListItem { Text = "Parecer de Orientação", Value = "Parecer de Orientação" });
            opcoes.Add(new SelectListItem { Text = "Parecer Jurídico", Value = "Parecer Jurídico" });
            opcoes.Add(new SelectListItem { Text = "Parecer Normativo", Value = "Parecer Normativo" });
            opcoes.Add(new SelectListItem { Text = "Portaria Conjunta", Value = "Portaria Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Portaria Interministerial", Value = "Portaria Interministerial" });
            opcoes.Add(new SelectListItem { Text = "Portaria Intersecretarial", Value = "Portaria Intersecretarial" });
            opcoes.Add(new SelectListItem { Text = "Portaria Intersetorial", Value = "Portaria Intersetorial" });
            opcoes.Add(new SelectListItem { Text = "Portaria Normativa", Value = "Portaria Normativa" });
            opcoes.Add(new SelectListItem { Text = "Portaria Técnica", Value = "Portaria Técnica" });
            opcoes.Add(new SelectListItem { Text = "Precedente Administrativo", Value = "Precedente Administrativo" });
            opcoes.Add(new SelectListItem { Text = "Precedente Normativo", Value = "Precedente Normativo" });
            opcoes.Add(new SelectListItem { Text = "Protocolo", Value = "Protocolo" });
            opcoes.Add(new SelectListItem { Text = "Protocolo ECF", Value = "Protocolo ECF" });
            opcoes.Add(new SelectListItem { Text = "Protocolo ICM", Value = "Protocolo ICM" });
            opcoes.Add(new SelectListItem { Text = "Protocolo ICMS", Value = "Protocolo ICMS" });
            opcoes.Add(new SelectListItem { Text = "Provimento", Value = "Provimento" });
            opcoes.Add(new SelectListItem { Text = "Recomendação", Value = "Recomendação" });
            opcoes.Add(new SelectListItem { Text = "Recurso de Revista", Value = "Recurso de Revista" });
            opcoes.Add(new SelectListItem { Text = "Recurso Especial", Value = "Recurso Especial" });
            opcoes.Add(new SelectListItem { Text = "Relatório da Câmara Técnica", Value = "Relatório da Câmara Técnica" });
            opcoes.Add(new SelectListItem { Text = "Resolução", Value = "Resolução" });
            opcoes.Add(new SelectListItem { Text = "Resolução Administrativa", Value = "Resolução Administrativa" });
            opcoes.Add(new SelectListItem { Text = "Resolução Conjunta", Value = "Resolução Conjunta" });
            opcoes.Add(new SelectListItem { Text = "Resolução Intersecretarial", Value = "Resolução Intersecretarial" });
            opcoes.Add(new SelectListItem { Text = "Resolução Normativa", Value = "Resolução Normativa" });
            opcoes.Add(new SelectListItem { Text = "Resolução Plenária", Value = "Resolução Plenária" });
            opcoes.Add(new SelectListItem { Text = "Resolução Recomendada", Value = "Resolução Recomendada" });
            opcoes.Add(new SelectListItem { Text = "Solução de Consulta", Value = "Solução de Consulta" });
            opcoes.Add(new SelectListItem { Text = "Solução de Consulta Interna", Value = "Solução de Consulta Interna" });
            opcoes.Add(new SelectListItem { Text = "Solução de Consulta Vinculada", Value = "Solução de Consulta Vinculada" });
            opcoes.Add(new SelectListItem { Text = "Solução de Divergência", Value = "Solução de Divergência" });
            opcoes.Add(new SelectListItem { Text = "Súmula", Value = "Súmula" });
            opcoes.Add(new SelectListItem { Text = "Súmula Administrativa", Value = "Súmula Administrativa" });
            opcoes.Add(new SelectListItem { Text = "Súmula Normativa", Value = "Súmula Normativa" });
            opcoes.Add(new SelectListItem { Text = "Súmula Vinculante", Value = "Súmula Vinculante" });
            opcoes.Add(new SelectListItem { Text = "Termo de Acordo", Value = "Termo de Acordo" });

            return opcoes;
        }

        public List<SelectListItem> Ano(string escolha)
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            opcoes.Add(new SelectListItem { Text = "2016", Value = "2016" });
            opcoes.Add(new SelectListItem { Text = "2015", Value = "2015" });
            opcoes.Add(new SelectListItem { Text = "2014", Value = "2014" });
            opcoes.Add(new SelectListItem { Text = "2013", Value = "2013" });
            opcoes.Add(new SelectListItem { Text = "2012", Value = "2012" });
            opcoes.Add(new SelectListItem { Text = "2011", Value = "2011" });
            opcoes.Add(new SelectListItem { Text = "2010", Value = "2010" });
            opcoes.Add(new SelectListItem { Text = "2009", Value = "2009" });
            opcoes.Add(new SelectListItem { Text = "2008", Value = "2008" });
            opcoes.Add(new SelectListItem { Text = "2007", Value = "2007" });
            opcoes.Add(new SelectListItem { Text = "2006", Value = "2006" });
            opcoes.Add(new SelectListItem { Text = "2005", Value = "2005" });
            opcoes.Add(new SelectListItem { Text = "2004", Value = "2004" });
            opcoes.Add(new SelectListItem { Text = "2003", Value = "2003" });
            opcoes.Add(new SelectListItem { Text = "2002", Value = "2002" });
            opcoes.Add(new SelectListItem { Text = "2001", Value = "2001" });
            opcoes.Add(new SelectListItem { Text = "2000", Value = "2000" });
            opcoes.Add(new SelectListItem { Text = "1999", Value = "1999" });
            opcoes.Add(new SelectListItem { Text = "1998", Value = "1998" });
            opcoes.Add(new SelectListItem { Text = "1997", Value = "1997" });
            opcoes.Add(new SelectListItem { Text = "1996", Value = "1996" });
            opcoes.Add(new SelectListItem { Text = "1995", Value = "1995" });
            opcoes.Add(new SelectListItem { Text = "1994", Value = "1994" });
            opcoes.Add(new SelectListItem { Text = "1993", Value = "1993" });
            opcoes.Add(new SelectListItem { Text = "1992", Value = "1992" });
            opcoes.Add(new SelectListItem { Text = "1991", Value = "1991" });
            opcoes.Add(new SelectListItem { Text = "1990", Value = "1990" });
            opcoes.Add(new SelectListItem { Text = "1989", Value = "1989" });
            opcoes.Add(new SelectListItem { Text = "1988", Value = "1988" });
            opcoes.Add(new SelectListItem { Text = "1985", Value = "1985" });
            opcoes.Add(new SelectListItem { Text = "1984", Value = "1984" });
            opcoes.Add(new SelectListItem { Text = "1979", Value = "1979" });
            opcoes.Add(new SelectListItem { Text = "1978", Value = "1978" });
            opcoes.Add(new SelectListItem { Text = "1977", Value = "1977" });
            opcoes.Add(new SelectListItem { Text = "1976", Value = "1976" });
            opcoes.Add(new SelectListItem { Text = "1974", Value = "1974" });
            opcoes.Add(new SelectListItem { Text = "1973", Value = "1973" });
            opcoes.Add(new SelectListItem { Text = "1972", Value = "1972" });
            opcoes.Add(new SelectListItem { Text = "1971", Value = "1971" });
            opcoes.Add(new SelectListItem { Text = "1970", Value = "1970" });
            opcoes.Add(new SelectListItem { Text = "1964", Value = "1964" });

            return opcoes;
        }

        public List<SelectListItem> TiposDeMateria()
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            opcoes.Add(new SelectListItem { Text = "Atos Legais", Value = "Atos_Legais" });
            //opcoes.Add(new SelectListItem { Text = "Consultoria", Value = "Consultoria" });
            //opcoes.Add(new SelectListItem { Text = "Doutrina", Value = "Doutrina" });
            //opcoes.Add(new SelectListItem { Text = "Jurisprudência", Value = "Jurisprudencia" });
            opcoes.Add(new SelectListItem { Text = "Orientação", Value = "Orientacao" });
            //opcoes.Add(new SelectListItem { Text = "Súmulas e Enunciados", Value = "Sumulas_e_Enunciados" });
            //opcoes.Add(new SelectListItem { Text = "Tabela Prática", Value = "Tabela_Pratica" });
            return opcoes;
        }

        public string GeradorSenha(int length=8, int strength=0)
        {
            string[] vogais = {"a","e","i","o","u","y"};
            string[] consoantes = {"b","d","g","h","j","m","n","p","q","r","s","t","v","z"};

            string password = "";

            byte[] byteRandomico = new byte[54846387];
            Random randomizador = new Random();
            int alt = randomizador.Next() % 2;

            for (int i = 0; i < length; i++) {
                if (alt == 1) {
                    password += consoantes[randomizador.Next(0, 13)];
                    alt = 0;
                } else {
                    password += vogais[randomizador.Next(0, 6)];
                    alt = 1;
                }
            }

            return password;
        }

        public List<SelectListItem> UFs()
        {
            List<SelectListItem> opcoes = new List<SelectListItem>();
            opcoes.Add(new SelectListItem { Text = "Selecione UF", Value = "Selecione o UF" });
            opcoes.Add(new SelectListItem { Text = "AC", Value = "AC" });
            opcoes.Add(new SelectListItem { Text = "AL", Value = "AL" });
            opcoes.Add(new SelectListItem { Text = "AM", Value = "AM" });
            opcoes.Add(new SelectListItem { Text = "AP", Value = "AP" });
            opcoes.Add(new SelectListItem { Text = "BA", Value = "BA" });
            opcoes.Add(new SelectListItem { Text = "CE", Value = "CE" });
            opcoes.Add(new SelectListItem { Text = "ES", Value = "ES" });
            opcoes.Add(new SelectListItem { Text = "GO", Value = "GO" });
            opcoes.Add(new SelectListItem { Text = "MG", Value = "MG" });
            opcoes.Add(new SelectListItem { Text = "MS", Value = "MS" });
            opcoes.Add(new SelectListItem { Text = "MT", Value = "MT" });
            opcoes.Add(new SelectListItem { Text = "PI", Value = "PI" });
            opcoes.Add(new SelectListItem { Text = "PA", Value = "PA" });
            opcoes.Add(new SelectListItem { Text = "PB", Value = "PB" });
            opcoes.Add(new SelectListItem { Text = "PE", Value = "PE" });
            opcoes.Add(new SelectListItem { Text = "PR", Value = "PR" });
            opcoes.Add(new SelectListItem { Text = "RJ", Value = "RJ" });
            opcoes.Add(new SelectListItem { Text = "RN", Value = "RN" });
            opcoes.Add(new SelectListItem { Text = "RO", Value = "RO" });
            opcoes.Add(new SelectListItem { Text = "RR", Value = "RR" });
            opcoes.Add(new SelectListItem { Text = "RS", Value = "RS" });
            opcoes.Add(new SelectListItem { Text = "SC", Value = "SC" });
            opcoes.Add(new SelectListItem { Text = "SE", Value = "SE" });
            opcoes.Add(new SelectListItem { Text = "SP", Value = "SP" });
            opcoes.Add(new SelectListItem { Text = "TO", Value = "TO" });
            opcoes.Add(new SelectListItem { Text = "MA", Value = "MA" });
            opcoes.Add(new SelectListItem { Text = "DF", Value = "DF" });

            return opcoes;
        }

        public bool ValidaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            
            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        public bool ValidaCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);

        }

        public EmailRequestDTO MontarEmailDTOCadastroGratuito(string nome, string usuario, string senha)
        {
            EmailRequestDTO emailDTO = new EmailRequestDTO();
            emailDTO.Assunto = "Confirmação de Cadastro - COAD";

            emailDTO.CorpoEmail = "";
            emailDTO.CorpoEmail += "<html>";
			emailDTO.CorpoEmail += "<head>";
			emailDTO.CorpoEmail += "<title>Untitled Document</title>";
			emailDTO.CorpoEmail += "<meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1'>";
			emailDTO.CorpoEmail += "<style type='text/css'>";
			emailDTO.CorpoEmail += ".style1 {";
			emailDTO.CorpoEmail += "font-family: Arial, Helvetica, sans-serif;";
			emailDTO.CorpoEmail += "font-size: 12px;";
			emailDTO.CorpoEmail += "color: #336699;";
			emailDTO.CorpoEmail += "}";
			emailDTO.CorpoEmail += "</style>";
			emailDTO.CorpoEmail += "</head>";
			emailDTO.CorpoEmail += "<body>";
			emailDTO.CorpoEmail += "<!-- Google Code for Cadastro na news Conversion Page -->";
			emailDTO.CorpoEmail += "<script language='JavaScript' type='text/javascript'>";
			emailDTO.CorpoEmail += "<!--";
			emailDTO.CorpoEmail += "var google_conversion_id = 1039828411;";
			emailDTO.CorpoEmail += "var google_conversion_language = 'pt_BR';";
			emailDTO.CorpoEmail += "var google_conversion_format = '1';";
			emailDTO.CorpoEmail += "var google_conversion_color = 'ffffff';";
			emailDTO.CorpoEmail += "if (1.0) {";
			emailDTO.CorpoEmail += "var google_conversion_value = 1.0;";
			emailDTO.CorpoEmail += "}";
			emailDTO.CorpoEmail += "var google_conversion_label = 'aGMCCNv1cRC7i-rvAw';";
			emailDTO.CorpoEmail += "//-->";
			emailDTO.CorpoEmail += "</script>";
			emailDTO.CorpoEmail += "<script language='JavaScript' src='http://www.googleadservices.com/pagead/conversion.js'>";
			emailDTO.CorpoEmail += "</script>";
			emailDTO.CorpoEmail += "<noscript>";
			emailDTO.CorpoEmail += "<img height='1' width='1' border='0' src='http://www.googleadservices.com/pagead/conversion/1039828411/?value=1.0&amp;label=aGMCCNv1cRC7i-rvAw&amp;script=0'/>";
			emailDTO.CorpoEmail += "</noscript>";
			emailDTO.CorpoEmail += "<table width='500' height='414'  border='0' cellpadding='0' cellspacing='0' background='http://site.coad.com.br/email/30_dias/email_30d_marcello_02.gif''>";
			emailDTO.CorpoEmail += "<tr>";
			emailDTO.CorpoEmail += "<td><img src='http://site.coad.com.br/email/30_dias/spacer.gif' width='100' height='80'>";
			emailDTO.CorpoEmail += "<br>";
			emailDTO.CorpoEmail += "<br>";
			emailDTO.CorpoEmail += "<table width='100%'  border='0' cellspacing='0' cellpadding='0'>";
			emailDTO.CorpoEmail += "<tr>";
			emailDTO.CorpoEmail += "<td width='22%'>&nbsp;</td>";
			emailDTO.CorpoEmail += "<td width='69%'>&nbsp;</td>";
			emailDTO.CorpoEmail += "<td width='9%'>&nbsp;</td>";
			emailDTO.CorpoEmail += "</tr>";
			emailDTO.CorpoEmail += "<tr>";
			emailDTO.CorpoEmail += "<td>&nbsp;</td>";
            emailDTO.CorpoEmail += "<td><span class='style1'>Ol&aacute; " + nome + ", </span>";
			emailDTO.CorpoEmail += "<p class='style1'>Seus dados para acesso ao Portal COAD s&atilde;o:</p>";
            emailDTO.CorpoEmail += "<p class='style1'>Usu&aacute;rio: " + usuario + "<br>";
            emailDTO.CorpoEmail += "Senha: " + senha + "</p>";
            emailDTO.CorpoEmail += "<p class='style1'><a href='http://www.coad.com.br/' target='_blank'>Clique aqui para logar.</a></p>";
			emailDTO.CorpoEmail += "<br>";
			emailDTO.CorpoEmail += "<p class='style1'>Atenciosamente,</p>";
			emailDTO.CorpoEmail += "<p class='style1'>SAC - Servi&ccedil;o de Assist&ecirc;ncia ao Cliente<br>";
			emailDTO.CorpoEmail += "<a href='http://www.coad.com.br'>www.coad.com.br";
			emailDTO.CorpoEmail += "</a> </p></td>";
			emailDTO.CorpoEmail += "<td>&nbsp;</td>";
			emailDTO.CorpoEmail += "</tr>";
			emailDTO.CorpoEmail += "<tr>";
			emailDTO.CorpoEmail += "<td>&nbsp;</td>";
			emailDTO.CorpoEmail += "<td>&nbsp;</td>";
			emailDTO.CorpoEmail += "<td>&nbsp;</td>";
			emailDTO.CorpoEmail += "</tr>";
			emailDTO.CorpoEmail += "</table></td>";
			emailDTO.CorpoEmail += "</tr>";
			emailDTO.CorpoEmail += "</table>";
			emailDTO.CorpoEmail += "</body>";
			emailDTO.CorpoEmail += "</html>";

            emailDTO.EmailDestino = usuario;
            emailDTO.EnableSsl = true;
            emailDTO.From = "coadcorp@coad.com.br";
            emailDTO.Host = "smtp.gmail.com";
            emailDTO.Port = 587;
            emailDTO.Senha = "C04dc0rp@";
            emailDTO.User = "coadcorp@coad.com.br";

            return emailDTO;
        }

        public string FormatarStringParaHTML(string original)
        {
            string nova = "";
            List<char> lista = original.ToList();
            foreach (char a in lista)
            {
                if (a.ToString() == "á")
                    nova += @"&aacute;";
                else if (a.ToString() == "Á")
                    nova += @"&Aacute;";
                else if (a.ToString() == "ã")
                    nova += @"&atilde;";
                else if (a.ToString() == "Ã")
                    nova += @"&Atilde;";
                else if (a.ToString() == "â")
                    nova += @"&acirc;";
                else if (a.ToString() == "Â")
                    nova += @"&Acirc;";
                else if (a.ToString() == "à")
                    nova += @"&agrave;";
                else if (a.ToString() == "À")
                    nova += @"&Agrave;";
                else if (a.ToString() == "é")
                    nova += @"&eacute;";
                else if (a.ToString() == "É")
                    nova += @"&Eacute;";
                else if (a.ToString() == "ê")
                    nova += @"&ecirc;";
                else if (a.ToString() == "Ê")
                    nova += @"&Ecirc;";
                else if (a.ToString() == "í")
                    nova += @"&iacute;";
                else if (a.ToString() == "Í")
                    nova += @"&Iacute;";
                else if (a.ToString() == "ó")
                    nova += @"&oacute;";
                else if (a.ToString() == "Ó")
                    nova += @"&Oacute;";
                else if (a.ToString() == "õ")
                    nova += @"&otilde;";
                else if (a.ToString() == "Õ")
                    nova += @"&Otilde;";
                else if (a.ToString() == "ô")
                    nova += @"&ocirc;";
                else if (a.ToString() == "Ô")
                    nova += @"&Ocirc;";
                else if (a.ToString() == "ú")
                    nova += @"&uacute;";
                else if (a.ToString() == "Ú")
                    nova += @"&Uacute;";
                else if (a.ToString() == "ç")
                    nova += @"&ccedil;";
                else if (a.ToString() == "Ç")
                    nova += @"&Ccedil;";
                else if (a.ToString() == "º")
                    nova += @"&ordm;";
                else if (a.ToString() == "ª")
                    nova += @"&ordf;";
                else if (a.ToString() == "&")
                    nova += @"&amp;";
                else
                    nova += a.ToString();
            }

            return nova;
        }

        public string RetirarAcentos(string original)
        {
            byte[] bytesTemporarios;
            bytesTemporarios = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(original);
            return System.Text.Encoding.UTF8.GetString(bytesTemporarios);
        }
    }
}
