using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InfoController : BaseController
    {
        public InfoController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET info
        [HttpGet]
        public IActionResult Get()
        {
            NodeInfo info = NodeService.GetNodeInfo();
            return Ok(info.ToContract());
        }
    }
}
