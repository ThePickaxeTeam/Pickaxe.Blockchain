using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Extensions;
using Block = Pickaxe.Blockchain.Domain.Models.Block;
using MinedBlock = Pickaxe.Blockchain.Domain.Models.MinedBlock;
using MinedBlockContract = Pickaxe.Blockchain.Contracts.MinedBlock;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MiningController : BaseController
    {
        public MiningController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET: api/mining/get-mining-job/f51362b7351ef62253a227a77751ad9b2302f911
        [HttpGet("get-mining-job/{minerAddress}", Name = "Get")]
        public IActionResult GetMiningJob(string minerAddress)
        {
            if (string.IsNullOrWhiteSpace(minerAddress))
            {
                return BadRequest(new Error("Invalid miner address."));
            }

            Block candidate = NodeService.CreateCandidateBlock(minerAddress);
            MiningJob job = candidate.ToMiningJob();

            return Ok(job);
        }

        // POST: api/mining/submit-mined-block/f51362b7351ef62253a227a77751ad9b2302f911
        [HttpPost("submit-mined-block/{minerAddress}", Name = "Post")]
        public IActionResult SubmitMinedBlock(string minerAddress, [FromBody]MinedBlockContract block)
        {
            MinedBlock minedBlock = block.ToDomainModel(minerAddress);

            BlockValidationResult validationResult = NodeService.TryAddBlock(
                minedBlock,
                out Block candidateBlock);

            if (validationResult != BlockValidationResult.Ok)
            {
                return BadRequest(new Error(validationResult.GetEnumDescription()));
            }

            return CreatedAtRoute(
                "GetBlock", new
                {
                    controller = "Blocks",
                    index = candidateBlock.Index
                },
                candidateBlock.ToContract());
        }
    }
}
