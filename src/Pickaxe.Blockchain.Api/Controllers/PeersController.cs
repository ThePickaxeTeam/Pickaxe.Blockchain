using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PeersController : BaseController
    {
        public PeersController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost("notify-new-block")]
        public IActionResult NotifyNewBlock([FromBody]NewBlockNotification notification)
        {
            // TODO: get all blocks from sender node and synchronize chains
            return Ok();
        }
    }
}
