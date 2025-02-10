using Demo.Business.ItemMaster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemMasterController : ControllerBase
    {
        private IMediator mediator;

        public ItemMasterController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("SaveItemMaster")]
        public async Task<IActionResult> SaveItemMaster([FromBody] SaveItemMasterService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }

        [HttpGet]
        [Route("ListItemMaster")]
        public async Task<IActionResult> ListItemMaster([FromQuery] ListItemMasterService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpGet]
        [Route("FetchItemMaster")]
        public async Task<IActionResult> FetchItemMaster([FromQuery] FetchItemMasterService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }
        [HttpPost]
        [Route("ItemMasterActiveUpdate")]
        public async Task<IActionResult> ItemMasterActiveUpdate([FromBody] ItemMasterActiveUpdateService values)
        {
            var response = await mediator.Send(values);
            return Ok(response);
        }

    }
}
