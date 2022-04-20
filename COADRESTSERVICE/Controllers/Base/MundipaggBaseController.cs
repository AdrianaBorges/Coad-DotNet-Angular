using COAD.CORPORATIVO.Settings.Mundipagg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COADRESTSERVICE.Controllers.Base
{
    [ApiController]
    public abstract class MundipaggBaseController : ControllerBase
    {

        public abstract MundipaggStt MundipaggStt { get; set; }
        public MundipaggBaseController(IConfiguration configuration, params Object[] args)
        {
            this.MundipaggStt = configuration.GetSection("Mundipagg").Get<MundipaggStt>();
        }

    }
}
