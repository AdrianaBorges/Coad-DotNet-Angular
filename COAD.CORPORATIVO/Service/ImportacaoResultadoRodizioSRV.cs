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
using COAD.SEGURANCA.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Exceptions;
using System.Web;
using System.IO;
using GenericCrud.Util;
using GenericCrud.Excel;
using COAD.CORPORATIVO.Util;
using Coad.GenericCrud.Extensions;
using GenericCrud.Metadatas;
using COAD.CORPORATIVO.Service.Custons;
using GenericCrud.Service.Formatting;
using GenericCrud.Models.HistoryRegister;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Models.MessageFormatter;
using GenericCrud.Models.Comparators;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCAO.Util;
using COAD.SEGURANCA.Exceptions;
using GenericCrud.Service;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("IRR_ID")]
    public class ImportacaoResultadoRodizioSRV : GenericService<IMPORTACAO_RESULTADO_RODIZIO, ImportacaoResultadoRodizioDTO, int>
    {

        public ImportacaoResultadoRodizioDAO _dao { get; set; }
        public ClienteSRV _clienteSRV { get; set; }
        public MunicipioSRV _municipioService { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public AssinaturaEmailSRV _assinaturaEmalSRV { get; set; }
        public AssinaturaTelefoneSRV _assinaturaTelefoneSRV { get; set; }
        public OrigemCadastroSRV _origemCadastroSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; }
        public InfoMarketingSRV _infoMkt { get; set; }
        public AreasSRV _areasSRV { get; set; }
        public BatchCustomSRV _batchSRV { get; set; }
        public ImportacaoStatusSRV _ImportacaoResultadoRodizioStatusSRV { get; set; }
        public MessageFormatterService formatterService { get; set; }
        public JobAgendamentoSRV _jobAgendamento { get; set; }
        public ImportacaoHistoricoSRV _ImportacaoResultadoRodizioHistoricoSRV { get; set; }

        public ImportacaoResultadoRodizioSRV()
        {
            this._dao = new ImportacaoResultadoRodizioDAO();
            this.Dao = _dao;
            this._clienteSRV = new ClienteSRV();
            this._municipioService = new MunicipioSRV();
            this._regiaoSRV = new RegiaoSRV();
            this._assinaturaEmalSRV = new AssinaturaEmailSRV();
            this._assinaturaTelefoneSRV = new AssinaturaTelefoneSRV();
            this._origemCadastroSRV = new OrigemCadastroSRV();
            this._produtoComposicaoSRV = new ProdutoComposicaoSRV();
            this._infoMkt = new InfoMarketingSRV();
            this._areasSRV = new AreasSRV();
            this._batchSRV = new BatchCustomSRV();
        }

        public ImportacaoResultadoRodizioSRV(ImportacaoResultadoRodizioDAO _dao)
        {
            this.Dao = _dao;
        }

        public IList<ImportacaoResultadoRodizioDTO> ListarResultadosDeRodizio(int? impID)
        {
            return _dao.ListarResultadosDeRodizio(impID);
        }

        public ImportacaoResultadoRodizioDTO BuscarResultado(int? impID, int? repID, int? rgID)
        {
            return _dao.BuscarResultado(impID, repID, rgID);
        }

        public void AdicionarResultado(int? impID, int? repID, int? rgID, int? quantidadeRecebidoSoma = null, int? quantidadePrioridadeSoma = null)
        {
            var resultado = BuscarResultado(impID, repID, rgID);

            if(resultado != null)
            {
                if (resultado.IRR_QTD == null)
                    resultado.IRR_QTD = 0;

                if (resultado.IRR_QTD_PRIORIDADES == null)
                    resultado.IRR_QTD_PRIORIDADES = 0;
            }
            else
            {
                resultado = new ImportacaoResultadoRodizioDTO()
                {
                    IMP_ID = impID,
                    IRR_QTD = 0,
                    IRR_QTD_PRIORIDADES = 0,
                    REP_ID = repID,
                    RG_ID = rgID
                };
            }

            if(quantidadeRecebidoSoma != null)
                resultado.IRR_QTD += quantidadeRecebidoSoma;

            if(quantidadePrioridadeSoma != null)
                resultado.IRR_QTD_PRIORIDADES += quantidadePrioridadeSoma;
            

            SaveOrUpdate(resultado);
        }
        
        public RetornoImportacaoResRodizioDTO RetornarResultado(int? impID)
        {
            var lstResultadoRodizio = ListarResultadosDeRodizio(impID);

            int totalRecebido = 0;
            int totalPrio = 0;
            int? total = 0;

            if(lstResultadoRodizio != null)
            {
                foreach(var res in lstResultadoRodizio)
                {
                    var qtd = (res.IRR_QTD != null) ? (int)res.IRR_QTD : 0;
                    var qtdPri = (res.IRR_QTD_PRIORIDADES != null) ? (int)res.IRR_QTD_PRIORIDADES : 0;

                    totalRecebido += qtd;
                    totalPrio += qtdPri;
                    total += res.Total;
                }
            }
            var retorno = new RetornoImportacaoResRodizioDTO()
            {
                Items = lstResultadoRodizio,
                QtdRodizio = totalRecebido,
                QtdPrioridades = totalPrio,
                Total = total
            };

            return retorno;
        }
    }
}
