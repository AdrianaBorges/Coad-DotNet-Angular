using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabelaDinamicaController : ControllerBase
    {
        private TabDinamicaConfigSRV _tabDinamicaConfigSRV;

        public TabelaDinamicaController(TabDinamicaConfigSRV tabDinamicaConfigSRV)
        {
            this._tabDinamicaConfigSRV = tabDinamicaConfigSRV;
        }

        [HttpGet("buscar-lancamentos-simuladores-trabalhistas")]
        public JSONResponse BuscarLancamentosSimuladoresTrabalhistas()
        {
            var result = new JSONResponse();
            try
            {
                DateTime dataParametro = DateTime.Now.AddDays(-30);

                var listTabDinConfUltimosSimuladores = _tabDinamicaConfigSRV.BuscarLancamentosTabelaDinamica(dataParametro,3, 2);
                if (listTabDinConfUltimosSimuladores.Count != 0)
                {
                    result.success = true;
                    result.Add("data", listTabDinConfUltimosSimuladores);
                    return result;
                }
                else
                {
                    result.success = false;
                    result.message = Message.Fail("Não existem atualizações recentes.");
                    return result;
                }
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }

        }

        [HttpGet("buscar-lancamentos-tabelas-trabalhista")]
        public JSONResponse BuscarLancamentosTabelasDinamicaTrabalhista()
        {
            var result = new JSONResponse();
            try
            {
                DateTime dataParametro = DateTime.Now.AddDays(-30);

                var listTabDinConfUltimosSimuladores = _tabDinamicaConfigSRV.BuscarLancamentosTabelaDinamica(dataParametro,3, 1);
                if (listTabDinConfUltimosSimuladores.Count != 0)
                {
                    result.success = true;
                    result.Add("data", listTabDinConfUltimosSimuladores);
                    return result;
                }
                else
                {
                    result.success = false;
                    result.message = Message.Fail("Não existem atualizações recentes.");
                    return result;
                }
            }
            catch (Exception e)
            {
                result.message = Message.Fail(e);
                result.success = false;
                return result;
            }

        }

    }
}