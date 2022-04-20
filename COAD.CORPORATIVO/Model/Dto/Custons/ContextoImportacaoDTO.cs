using COAD.SEGURANCA.Service.Custons.Context;
using GenericCrud.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ContextoImportacaoDTO
    {
        public ContextoImportacaoDTO()
        {
            this.CronometroGeral = new Stopwatch();
            this.CronometroConversao = new Stopwatch();
            this.CronometroImportacao = new Stopwatch();
            this.RecebimentoRepresentante = new Dictionary<RepresentanteRegiaoEntry, ResumoEncarteiramentoDTO>();
            //this.BatchContext = new BatchStatus();
        }

        public ImportacaoDTO Importacao { get; set; }
        public ImportacaoSuspectDTO ImportacaoSuspect { get; set; }

        public int QuantidadeRegistros {get; set;}
        public int QuantidadeReal { get; set; }
        public int QuantidadeDuplicada { get; set; }
        public int QuantidadeDeRepresentantes { get; set; }
        public int QuantidadeRegistrosImportados { get; set; }
        public int QuantidadeDeClientesNovos { get; set; }
        public int QuantidadeClienteEncontrados { get; set; }
        public int QuantidadePrioridadesInseridas { get; set; }
        public BatchContext BatchContext { get; set; }
        
        public Stopwatch CronometroGeral { get; set; }
        public Stopwatch CronometroConversao { get; set; }
        public Stopwatch CronometroImportacao { get; set; }

        public long TempoGeral { get; set; }
        public long TempoDeConversao { get; set; }
        public long TempoDeImportacao { get; set; }

        [ScriptIgnore]
        public Dictionary<RepresentanteRegiaoEntry, ResumoEncarteiramentoDTO> RecebimentoRepresentante { get; set; }

        public IList<ResumoEncarteiramentoDTO> ListaResumo
        {
            get
            {
                IList<ResumoEncarteiramentoDTO> resumo = new List<ResumoEncarteiramentoDTO>();

                var keys = RecebimentoRepresentante.Keys;

                foreach (var key in keys)
                {
                    resumo.Add(RecebimentoRepresentante[key]);
                    resumo = resumo.OrderBy(or => or.RegiaoDoRepresentante)
                        .ToList();
                }

                return resumo;
            }
            set
            {
                var n = value;
            }
        }        

        public void IniciarTempoGeral()
        {
            this.CronometroGeral.Start();
        }

        public void PararTempoGeral()
        {
            if(this.CronometroGeral != null)
            {
                this.CronometroGeral.Stop();
                this.TempoGeral += this.CronometroGeral.ElapsedMilliseconds;
            }
        }

        public void IniciarTempoConversao()
        {
            this.CronometroConversao.Start();
        }

        public void PararTempoConversao()
        {
            if (this.CronometroConversao != null)
            {
                this.CronometroConversao.Stop();
                this.TempoDeConversao += this.CronometroGeral.ElapsedMilliseconds;
            }
        }

        public void IniciarTempoImportacao()
        {
            this.CronometroImportacao.Start();
        }

        public void PararTempoImportacao()
        {
            if (this.CronometroImportacao != null)
            {
                this.CronometroImportacao.Stop();
                this.TempoGeral += this.CronometroImportacao.ElapsedMilliseconds;
            }
        }

        public ResumoEncarteiramentoDTO CriarOuRetornarResumoEncarteiramento(int REP_ID, int RG_ID, string nomeRegiao = null)
        {
            var entry = new RepresentanteRegiaoEntry(REP_ID, RG_ID);
            if (this.RecebimentoRepresentante.Keys.Contains(entry))
            {
                var resumo = this.RecebimentoRepresentante[entry];
                if (resumo != null)
                {
                    return resumo;
                }

                resumo = new ResumoEncarteiramentoDTO();
                this.RecebimentoRepresentante.Remove(entry);
                this.RecebimentoRepresentante.Add(entry, resumo);

                return resumo;
            }
            else
            {
                var resumo = new ResumoEncarteiramentoDTO();
                resumo.RegiaoDoRepresentante = nomeRegiao;
                this.RecebimentoRepresentante.Add(entry, resumo);

                return resumo;
            }
        }

        public void IniciarPassoBatch(string descricao, bool possuiPorcentagem)
        {
            //if (this.BatchContext == null)
            //{
            //    this.BatchContext = new BatchStatus();
            //}

            //this.BatchContext.IsRunning = true;
            //this.BatchContext.BatchStepName = descricao;
            //this.BatchContext.HasParcent = possuiPorcentagem;
            //this.BatchContext.ProcessedItens = 0;
            //this.BatchContext.TotalItens = 0;
        }
    }


    public class RepresentanteRegiaoEntry
    {
        public int RepId { get; set; }
        public int RgId { get; set; }

        public RepresentanteRegiaoEntry(int RepId, int RgId)
        {
            this.RepId = RepId;
            this.RgId = RgId;
        }

        public override bool Equals(object obj)
        {
            var registry = obj as RepresentanteRegiaoEntry;
            if (obj == null)
                return false;

            if (registry.RepId == this.RepId && registry.RgId == this.RgId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int repIdHash = 0;
            int rgIdHash = 0;

            if (this.RgId != null)
            {
                rgIdHash = this.RgId.GetHashCode();
            }

            if(this.RepId != null)
            {
                repIdHash = this.RepId.GetHashCode();
            }

            return repIdHash + rgIdHash;
        }

    }

    public class ResumoEncarteiramentoDTO 
    {
        public string RegiaoDoRepresentante { get; set; }
        public string NomeRepresentante {get; set;}
        public int QuantidadeClientesRecebidos {get;set;}
        public int PrioridadesRecebidas { get; set; }
    }
}
