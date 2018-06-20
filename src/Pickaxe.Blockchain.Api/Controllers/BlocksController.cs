using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using System.Collections.Generic;
using System.Linq;
using Block = Pickaxe.Blockchain.Domain.Models.Block;


namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BlocksController : BaseController
    {
        public BlocksController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET
        [HttpGet]
        public IActionResult GetAll()
        {
            IList<Block> blocks = NodeService.GetAllBlocks();
            return Ok(blocks.Select(b => b.ToContract()).ToList());
        }

        // GET: api/Blocks/6
        [HttpGet("{index}", Name = "GetBlock")]
        public IActionResult Get(int index)
        {
            if (index < 0)
            {
                return BadRequest(new Error("Block index must be a non-negative integer."));
            }

            Block block = NodeService.GetBlock(index);
            if (block == null)
            {
                return NotFound();
            }

            return Ok(block.ToContract());
        }
    }
}
