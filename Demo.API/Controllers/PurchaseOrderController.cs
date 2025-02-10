using Demo.Business.PurchaseOrder;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private IMediator mediator;

        public PurchaseOrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("SavePurchaseOrder")]
        public async Task<IActionResult> SavePurchaseOrder([FromBody] SavePurchaseOrderService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpPost]
        [Route("ListPurchaseOrder")]
        public async Task<IActionResult> ListPurchaseOrder([FromBody] ListPurchaseOrderService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpGet]
        [Route("FetchPurchaseOrder")]
        public async Task<IActionResult> FetchPurchaseOrder([FromQuery] FetchPurchaseOrderService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpPost]
        [Route("PurchaseOrderActiveUpdate")]
        public async Task<IActionResult> PurchaseOrderActiveUpdate([FromBody] PurchaseOrderActiveUpdateService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }

        [HttpPost]
        [Route("GetLookupDetails")]
        public async Task<IActionResult> GetLookupDetails([FromBody] GetLookupDetailsService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
    }
}
