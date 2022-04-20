using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class SetorAtendimento
    {
        public List<SelectListItem> RetornarSetorDeTelefone(int identificadorSetor)
        {
            List<OpcoesAtendimentoDTO> setortel = new OpcaoAtendimentoSRV().BuscarSetorDeTelefones().ToList();
            var setores = new List<SelectListItem>();

            foreach (OpcoesAtendimentoDTO setor in setortel)
            {
                setores.Add(new SelectListItem() { Value = setor.OPC_ID.ToString(), Text = setor.OPC_DESCRICAO, Selected = (setor.OPC_ID == identificadorSetor) ? true : false });
            }

            return setores;
        }

        public List<SelectListItem> RetornarSetorDeEmail(int identificadorSetor)
        {
            List<OPCAO_ATENDIMENTO> setorEmail = new OpcaoAtendimentoSRV().BuscarTodos().ToList();
            var setores = new List<SelectListItem>();

            foreach (OPCAO_ATENDIMENTO setor in setorEmail)
            {
                setores.Add(new SelectListItem() { Value = setor.OPC_ID.ToString(), Text = setor.OPC_DESCRICAO, Selected = (setor.OPC_ID == identificadorSetor) ? true : false });
            }

            return setores;
        }
    }
}