using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class TiposDeNegociosDAO
    {
        public List<SelectListItem> RetornarTiposDeNegocio()
        {
            List<TIPO_PEDIDO> tiponegocio = new TiposNegocioPedidoSRV().BuscarPedidosAtivos().ToList();
          
            var tiposRecuperados = new List<SelectListItem>();

            tiposRecuperados.Add(new SelectListItem() { Value = "", Text = "Selecione", Selected = true });

            foreach (TIPO_PEDIDO tipo in tiponegocio)
            {
                tiposRecuperados.Add(new SelectListItem() { Value = tipo.TIPO_PED_ID.ToString(), Text = tipo.TIPO_PED_DESCRICAO });
            }
          
            return tiposRecuperados;
        }
    }
}