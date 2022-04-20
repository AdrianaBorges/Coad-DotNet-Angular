using COAD.SEGURANCA.Model.Dto.Custons;
using GenericCrud.Models;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace COAD.SEGURANCA.Service.Custons.Context
{
    public class BatchContext
    {
        public IDictionary<string, object> ContextualDictionary { get; set; }
        [ScriptIgnore]
        public JobAgendamentoSRV _service { get; set; }
        public int? JobID { get; set; } 
        
        public BatchContext()
        {
            this.ContextualDictionary = new Dictionary<string, object>();
            this._service = ServiceFactory.RetornarServico<JobAgendamentoSRV>();
            this.ListErros = new HashSet<ErroReportItemDTO>();
            TotalExito = 0;
            TotalFalha = 0;
        }

        public void AdicionarObjetoContextual<TSource>(string key, TSource obj)
        {
            if (!ContextualDictionary.Keys.Contains(key))
            {
                ContextualDictionary.Add(key, obj);
            }
        }

        public TSource RetornarObjetoContextual<TSource>(string key) where TSource : class
        {
            if (ContextualDictionary.Keys.Contains(key))
            {
                var obj = ContextualDictionary[key];
                if (obj is TSource)
                    return obj as TSource;
            }
            return null;
        }

        public BatchContext(int? jobID, JobAgendamentoSRV _service)
        {
            this.ContextualDictionary = new Dictionary<string, object>();
            this.JobID = jobID;
            this._service = _service;
            this.ListErros = new HashSet<ErroReportItemDTO>();
            TotalExito = 0;
            TotalFalha = 0;
        }

        public string Path { get; set; }
        public int? TotalExito { get; set; }
        public int? TotalFalha { get; set; }
        public BatchStatus BatchStatus { get; set; }

        public void IniciarPassoBatch(string descricao, bool possuiPorcentagem, int totalItens = 0)
        {
            if (this.BatchStatus == null)
            {
                this.BatchStatus = new BatchStatus();
            }

            this.BatchStatus.IsRunning = true;
            this.BatchStatus.BatchStepName = descricao;
            this.BatchStatus.HasParcent = possuiPorcentagem;
            this.BatchStatus.ProcessedItens = 0;
            this.BatchStatus.TotalItens = totalItens;
            TotalExito = 0;
            TotalFalha = 0;

            if(JobID != null)
                this._service.RegistrarPassoBatch(this.JobID, this.BatchStatus);

        }

        public void IncrementarPassoBatch(int itensProcessados = 1)
        {
            this.BatchStatus.ProcessedItens += itensProcessados;

            if(JobID != null)
                this._service.RegistrarPassoBatch(JobID, BatchStatus);
        }

        public int? RetornarCodReferencia()
        {
            return this._service.RetornarCodReferencia(JobID);
        }

        public string RetornarCodReferenciaStr()
        {
            return this._service.RetornarCodReferenciaStr(JobID);
        }

        public void AdicionarContagemSucesso()
        {
            if (TotalExito == null)
                TotalExito = 0;
            TotalExito++;
            _service.AdicionarContagemSucesso(JobID);
        }

        public void AdicionarContagemFalha()
        {
            if (TotalFalha == null)
                TotalFalha = 0;
            TotalFalha++;
            _service.AdicionarContagemFalha(JobID);
        }

        public ICollection<ErroReportItemDTO> ListErros { get; set; }

        
    }

}
