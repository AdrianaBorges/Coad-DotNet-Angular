using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using COAD.CORPORATIVO.DAO.Reflection;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("ALO_NOSSO_NUMERO")]
    public class ParcelaAlocadaSRV : GenericService<PARCELA_ALOCADA, ParcelaAlocadaDTO, int>
    {
        public ParcelaAlocadaDAO _dao = new ParcelaAlocadaDAO();

        public ParcelaAlocadaSRV()
        {
            this.Dao = _dao;
        }

        /// <summary>
        ///  ALT: 07/02/2018 - Obtendo a quantidade de envios de boletos para um título
        ///                  - Retornando numero para o próximo envio
        ///                  - Este número é usado ao gerar o [NOSSO NUMERO] do boleto, na Classe [BoletoSRV.ConverterNossoNumero]
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        public int ObterNumeroEnvioBoleto(string titulo)
        {
            var ctasIds = _dao.ObterContas(titulo).ToList();
            var ctas = new ContaSRV().ObterContas(ctasIds, true).ToList();

            return _dao.ObterNumeroEnvioBoleto(titulo, ctas);
        }

        /// <summary>
        /// ALT: 14/02/2017 - Procedure executando INSERT/UPDATE em PARCELA_ALOCADA
        /// </summary>
        /// <param name="parcelas"></param>
        public void ExecutarInsertUpdateEmParcelaAlocada(List<ParcelaAlocadaUpdateDTO> parcelaAlocada)
        {
            using (var context = new COADCORPEntities())
            {
                context.ExecutarProcedure<ParcelaAlocadaUpdateDTO>(parcelaAlocada, "PARCELA_ALOCADA_UPDATE", "@parcelaAlocada", "tp_parcela_alocada_update");
            }
        }

        public IList<ParcelaAlocadaDTO> LerParcelaAlocada(int _rem_id)
        {
            return _dao.LerParcelaAlocada(_rem_id);
        }

        public IList<ParcelaAlocadaDTO> LerParcelaAlocada(string titulo = null, int? remessa = null, int? ctaId = null, DateTime? dtTransm = null,
            string codRem = null, DateTime? dtCodRem = null, string codRet = null, DateTime? dtCodRet = null, string codErr = null)
        {
            return _dao.LerParcelaAlocada(titulo, remessa, ctaId, dtTransm, codRem, dtCodRem, codRet, dtCodRet, codErr);
        }

        // ler remessa enviada
        public IList<ParcelaAlocadaDTO> LerRemessaEnviada(string remessa = null)
        {
            var resp = _dao.LerRemessaEnviada(remessa);
            return resp;
        }

        public void SalvarParcelaAlocada(IList<ParcelaAlocadaDTO> parAloc)
        {
            try
            {
                SaveOrUpdateAll(parAloc);
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
    }
}
