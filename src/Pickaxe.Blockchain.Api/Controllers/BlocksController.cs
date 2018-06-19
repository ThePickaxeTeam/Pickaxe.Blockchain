using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Extensions;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Blocks")]
    public class BlocksController : BaseController
    {
        public BlocksController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            return Ok();
        }
    }
}
