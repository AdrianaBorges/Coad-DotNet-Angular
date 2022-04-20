

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Enumerados;

namespace COAD.CORPORATIVO.Service
{ 
	[ServiceConfig("NFC_ID")]
	public class NotaFiscalConfigSRV : GenericService<NOTA_FISCAL_CONFIG, NotaFiscalConfigDTO, Int32>
	{

        public NotaFiscalConfigDAO _dao { get; set; }
        public ConfigImpostoSRV _configImpostoSRV { get; set; }
        public ProdutoComposicaoSRV _produtoComposicaoSRV { get; set; } 

        public NotaFiscalConfigSRV(NotaFiscalConfigDAO _dao)
        {
			this._dao = _dao;
			this.Dao = _dao;
        }

        public IList<NotaFiscalConfigDTO> ListarNotaFiscalConfig(int? cmpId, int? nctId = null, bool faturado100PorCento = false, bool prencherDados = false)
        {
            var lstNfConfig = _dao.ListarNotaFiscalConfig(cmpId, nctId, faturado100PorCento);

            if (prencherDados
                && lstNfConfig != null
                && lstNfConfig.Count > 0)
            {
                foreach(var nfConfig in lstNfConfig)
                {
                    _configImpostoSRV.PreencherConfigImposto(nfConfig, true);
                }
            }

            return lstNfConfig;
        }

        /// <summary>
        /// Verifica se um produto está configurado para emitir a nota fiscal do tipo informado no segundo argumento
        /// </summary>
        /// <param name="cmpId">Código do Produto Composição a ser checado</param>
        /// <param name="nctId">Tipo de Nota Fiscal</param>
        /// <returns></returns>
        public bool ChecarEmitirNotaFiscal(int? cmpId, int? nctId)
        {
            return _dao.ChecarEmitirNotaFiscal(cmpId, nctId);
        }

        public NotaFiscalConfigDTO FindByIdFullLoaded(int? nfcId, 
            bool trazConfigImposto = false, 
            bool trazImpostos = false)
        {
            var nfConfig = FindById(nfcId);

            if(nfConfig != null && trazConfigImposto)
            {
                _configImpostoSRV.PreencherConfigImposto(nfConfig, trazImpostos);
            }

            return nfConfig;
        }


        public void SalvarNotaFiscalConfig(NotaFiscalConfigSaveRequestDTO nfConfigSaveRequest)
        {
            var lstSalvar = nfConfigSaveRequest.NotaFiscalConfigAtualizar;
            var lstDeletar = nfConfigSaveRequest.NotaFiscalConfigExcluir;
            SalvarNotaFiscalConfig(lstSalvar, lstDeletar);
        }


        /// <summary>
        /// Salva a nota fiscal
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void SalvarNotaFiscalConfig(IEnumerable<NotaFiscalConfigDTO> lstNotaFiscalConfig, IEnumerable<NotaFiscalConfigDTO> lstNFConfigDeletar = null)
        {

            if (lstNotaFiscalConfig != null)
            {
                Validar(lstNotaFiscalConfig);
                using (TransactionScope scope = new TransactionScope())
                {
                    DeletarNotaFiscalConfig(lstNFConfigDeletar);

                    foreach (var nfConfig in lstNotaFiscalConfig)
                    {             
                        _processarSalvamentoNotaFiscalConfig(nfConfig);
                    }
                    scope.Complete();
                }
            }
        }

        private void Validar(IEnumerable<NotaFiscalConfigDTO> lstNotaFiscalConfig)
        {
            var porcentagemSomatorio = 0;

            foreach (var nfConfig in lstNotaFiscalConfig)
            {
                if (nfConfig.NFC_PORCENTAGEM_VALOR != null)
                {
                    porcentagemSomatorio += (int)nfConfig.NFC_PORCENTAGEM_VALOR;
                }

                if (porcentagemSomatorio > 100)
                {
                    throw new Exception($"Não é possível salvar a configuração da nota fiscal. A porcentagem das configurações excederam 100%. Porcentagem: {porcentagemSomatorio}%");
                }
            }
        }

        private void _processarSalvamentoNotaFiscalConfig(NotaFiscalConfigDTO notaFiscalConfig)
        {
            if (notaFiscalConfig == null)
            {
                throw new ValidacaoException("Tabela de preço não pode ser null");
            }

            if (notaFiscalConfig.CMP_ID == null)
            {
                throw new ValidacaoException("O campo CMP_ID não pode ser null");
            }
            

            var lstConfigImposto = notaFiscalConfig.CONFIG_IMPOSTO;
            notaFiscalConfig.CONFIG_IMPOSTO = null;

            var notaFiscalConfigSalva = SaveOrUpdate(notaFiscalConfig);
            notaFiscalConfig.NFC_ID = notaFiscalConfigSalva.NFC_ID;

            notaFiscalConfig.CONFIG_IMPOSTO = lstConfigImposto;
            _configImpostoSRV.SalvarEExcluirConfigImposto(notaFiscalConfig);
        }
        
        public void DeletarNotaFiscalConfig(IEnumerable<NotaFiscalConfigDTO> lstDeletar = null)
        {
            if (lstDeletar != null)
            {
                foreach (var obj in lstDeletar)
                {
                    //_regTabPrecoSRV.DeletarRegiaoTabelaPreco(obj.REGIAO_TABELA_PRECO);
                    obj.DATA_EXCLUSAO = DateTime.Now;
                }

                MergeAll(lstDeletar);
            }
        }

        public ICollection<NotaFiscalConfigDTO> ClonarConfiguracao(int? cmpIdOrigem, int? cmpIdDestino)
        {
            var lstNfClonado = new List<NotaFiscalConfigDTO>();
            if(cmpIdOrigem != null)
            {
                var lstNfConfig = ListarNotaFiscalConfig(cmpIdOrigem, prencherDados: true);

                if(lstNfConfig != null)
                {
                    foreach(var nfConfig in lstNfConfig)
                    {
                        var configClonada = ClonarConfiguracao(nfConfig, cmpIdDestino);
                        lstNfClonado.Add(configClonada);
                    }
                }
            }

            return lstNfClonado;
        }

        private NotaFiscalConfigDTO ClonarConfiguracao(NotaFiscalConfigDTO configDTO, int? cmpIdDestino)
        {
            if(configDTO != null)
            {
                var cmp = _produtoComposicaoSRV.FindById(cmpIdDestino);
                
                if(cmp == null)
                {
                    throw new Exception($"Produto Composição {cmpIdDestino} não foi encontrado.");
                }

                var clone = new NotaFiscalConfigDTO()
                {
                    CMP_ID = cmpIdDestino,
                    NCT_ID = configDTO.NCT_ID,
                    NOTA_FISCAL_CONFIG_TIPO = configDTO.NOTA_FISCAL_CONFIG_TIPO,
                    NFC_APLICAR_100_POR_CENTO_FAT = configDTO.NFC_APLICAR_100_POR_CENTO_FAT,
                    NFC_COD_LISTA_SERVICO = configDTO.NFC_COD_LISTA_SERVICO,
                    NFC_DESCRICAO_PRODUTO = cmp.CMP_DESCRICAO,
                    NFC_PORCENTAGEM_VALOR = configDTO.NFC_PORCENTAGEM_VALOR,
                    NFC_CODIGO_TRIBUTACAO_MUNICIPIO = configDTO.NFC_CODIGO_TRIBUTACAO_MUNICIPIO
                };

                clone.CONFIG_IMPOSTO = _configImpostoSRV.ClonarConfigImposto(configDTO);
                return clone;

            }

            return null;
        }
        
    }
}
