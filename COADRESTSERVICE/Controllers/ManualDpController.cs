using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coad.GenericCrud.ActionResultTools;
using COAD.COADGED.Model.DTO.Custons;
using COAD.COADGED.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace COADRESTSERVICE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManualDpController : ControllerBase
    {

        public ManualDPItemSRV manualDPItemSRV{get;set;}

        public ManualDpController(ManualDPItemSRV manualDPItemSRV)
        {
            this.manualDPItemSRV = manualDPItemSRV;
        }

        [HttpGet("buscar-ultimas-noticias")]
        public JSONResponse BuscarUltimasNoticias()
        {
            var result = new JSONResponse();
            try
            {
                DateTime dataParametro = DateTime.Now.AddDays(-7);
                var manualDpItemDTO = manualDPItemSRV.BuscarUltimosItemsAlteradosEPublicadosPorData(dataParametro);

                if (manualDpItemDTO != null && manualDpItemDTO.Count!=0)
                {

                    List<ManualDpItemNovidadeDTO> novidadesDTO = new List<ManualDpItemNovidadeDTO>();

                    for(int i=0; i<manualDpItemDTO.Count(); i++)
                    {
                        var manualItemDTO = manualDpItemDTO[i];
                        novidadesDTO.Add(new ManualDpItemNovidadeDTO()
                        {
                            MAI_DATA_ALTERA = manualItemDTO.DATA_ALTERA,
                            MAI_DATA_PUBLICACAO = manualItemDTO.MAI_DATA_PUBLICACAO,
                            MAI_ID = manualItemDTO.MAI_ID,
                            MAI_TITULO = manualItemDTO.MAI_TITULO
                        });
                    }
                    result.Add("data", novidadesDTO);
                    result.success = true;
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