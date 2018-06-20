using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class DebugController : BaseController
    {
        public DebugController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET debug
        [HttpGet]
        public IActionResult Get()
        {
            DebugInfo info = NodeService.GetDebugInfo();
            return Ok(info.ToContract());
        }

        // GET debug/reset-chain
        [HttpGet("reset-chain")]
        public IActionResult ResetChain()
        {
            NodeService.ResetChain();
            return Ok();
        }
    }
}
