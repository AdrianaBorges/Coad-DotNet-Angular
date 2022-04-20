using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class TiposDeTelefone
    {
        public List<SelectListItem> RetornarTiposDeTelefone(string identificador)
        {
            List<TIPO_TELEFONE> tipostelefone = new TipoTelefoneSRV().BuscarTodos().ToList();

            var tiposRecuperados = new List<SelectListItem>();

            foreach (TIPO_TELEFONE tipo in tipostelefone)
            {
                tiposRecuperados.Add(new SelectListItem() { Value = tipo.TIPO_TEL_ID.ToString(), Text = tipo.TIPO_TEL_DESCRICAO });
            }

            return tiposRecuperados;
        }
    }
}