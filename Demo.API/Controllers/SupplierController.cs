using Demo.Business.Supplier;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private IMediator mediator;

        public SupplierController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("SaveSupplier")]
        public async Task<IActionResult> SaveSupplier([FromBody] SaveSupplierService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpGet]
        [Route("ListSupplier")]
        public async Task<IActionResult> ListSupplier([FromQuery] ListSupplierService values)
        {

            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpGet]
        [Route("FetchSupplier")]
        public async Task<IActionResult> FetchSupplier([FromQuery] FetchSupplierService values)
        {

            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpPost]
        [Route("SupplierActiveUpdate")]
        public async Task<IActionResult> SupplierActiveUpdate([FromBody] SupplierActiveUpdateService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }

    }
}
