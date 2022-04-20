using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Model.Custons.Pesquisas;
using COAD.SEGURANCA.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class FilaEmailDAO : DAOAdapter<FILA_EMAIL, FilaEmailDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public FilaEmailDAO()
        {
            //db = GetDb<COADCORPEntities>();
        }

        /// <summary>
        /// Retorna os emails que foram adionados em uma fila de envio.
        /// </summary>
        /// <returns></returns>
        public IList<FilaEmailDTO> ListarEmailsPendentesDeEnvio()
        {
            var query = (from fe in db.FILA_EMAIL where 
                             fe.FLE_DATA_ENVIO == null &&
                             fe.FLE_DATA_CANCELAMENTO == null
                             select fe);

            return ToDTO(query);
        }

        public Pagina<ListaFilaEmailDTO> PesquisarFilaEmail(PesquisarFilaEmailDTO pesquisaDTO)
        {
            if(pesquisaDTO == null)
            {
                return new Pagina<ListaFilaEmailDTO>();
            }
            
            var email = pesquisaDTO.Email;
            var assunto = pesquisaDTO.Assunto;
            var usuario = pesquisaDTO.Usuario;
            var dataCriacaoInicial = pesquisaDTO.DataCriacaoInicial;
            var dataCriacaoFinal = pesquisaDTO.DataCriacaoFinal;
            var dataEnvioInicial = pesquisaDTO.DataEnvioInicial;
            var dataEnvioFinal = pesquisaDTO.DataEnvioFinal;
            var exibirEnviados = pesquisaDTO.ExibirEnviados;
            var exibirCancelados = pesquisaDTO.ExibirCancelados;
            var filaId = pesquisaDTO.FilaId;

            if (string.IsNullOrWhiteSpace(email))
                email = null;
            if (string.IsNullOrWhiteSpace(assunto))
                assunto = null;
            if (string.IsNullOrWhiteSpace(usuario))
                usuario = null;

            var query = (from
                            fila in db.FILA_EMAIL
                         where
                            (filaId == null || fila.FLE_ID == filaId) &&
                            (email == null || fila.FLE_EMAIL.Contains(email)) &&
                            (assunto == null || fila.FLE_ASSUNTO.Contains(assunto)) &&
                            (usuario == null || fila.USU_LOGIN.Contains(usuario)) &&
                            (fila.FLE_DATA_CANCELAMENTO == null || exibirCancelados == true) &&
                            (dataCriacaoInicial == null || EntityFunctions.TruncateTime(fila.FLE_DATA_CRIACAO) >= EntityFunctions.TruncateTime(dataCriacaoInicial)) &&
                            (dataCriacaoFinal == null || EntityFunctions.TruncateTime(fila.FLE_DATA_CRIACAO) <= EntityFunctions.TruncateTime(dataCriacaoFinal)) &&
                            (dataEnvioInicial == null || EntityFunctions.TruncateTime(fila.FLE_DATA_ENVIO) >= EntityFunctions.TruncateTime(dataEnvioInicial)) &&
                            (dataEnvioFinal == null || EntityFunctions.TruncateTime(fila.FLE_DATA_ENVIO) <= EntityFunctions.TruncateTime(dataEnvioFinal)) &&
                            (exibirEnviados == true || fila.FLE_DATA_ENVIO == null)
                         orderby fila.FLE_DATA_CRIACAO descending
                         select new ListaFilaEmailDTO()
                         {
                             Id = fila.FLE_ID,
                             Assunto = fila.FLE_ASSUNTO,
                             Email = fila.FLE_EMAIL,
                             Data = fila.FLE_DATA_CRIACAO,
                             DataEnvio = fila.FLE_DATA_ENVIO,
                             Usuario = fila.USU_LOGIN,
                             DataCancelamento = fila.FLE_DATA_CANCELAMENTO
                         }
           );

            return query.Paginar(pesquisaDTO.requisicao);
        }

    }
}
