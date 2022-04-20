using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoCRMController : Controller
    {
        public PedidoCRMController(PedidoStatusSRV pedidoStatus)
        {
            this.PedidoStatus = pedidoStatus;
        }
        
        public PedidoCRMSRV PedidoCRMSRV { get; set; }
        private PedidoStatusSRV PedidoStatus { get; set; }

        // GET: api/Pedido
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return null;
        }

        // GET: api/Pedido/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return null;
        }

        // POST: api/Pedido
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Pedido/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
