using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using System.Collections.Generic;

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

        // GET
        [HttpGet]
        public IActionResult GetAll()
        {
            Dictionary<string, string> peers = NodeService.GetPeers();
            return Ok(peers);
        }

        [HttpPost("notify-new-block")]
        public IActionResult NotifyNewBlock([FromBody]NewBlockNotification notification)
        {
            NodeService.SynchronizeChain(notification.NodeUrl);
            return Ok();
        }
    }
}
