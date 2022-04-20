using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models.HistoryRegister
{
    public enum TipoRegistro
    {
        /// <summary>
        /// Insere no histórico do cliente
        /// </summary>
        HISTORICO_CLIENTE = 1,

        /// <summary>
        /// Insere no histórico do pedido
        /// </summary>
        HISTORICO_PEDIDO = 2,

        /// <summary>
        /// Insere nas notificacoes do representante
        /// </summary>
        NOTIFICACAO = 3,

        /// <summary>
        /// Insere no histórico do cliente, pedido e insere notificação para o representante
        /// </summary>
        TODOS = 4
    }
}
