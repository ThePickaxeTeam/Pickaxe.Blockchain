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
    [Route("api/MiningJobs")]
    public class MiningJobsController : BaseController
    {
        public MiningJobsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET: api/MiningJobs/687422eEA2cB73B5d3e242bA5456b782919AFc85
        [HttpGet("{minerAddress}", Name = "Get")]
        public IActionResult Get(string minerAddress)
        {
            if (string.IsNullOrWhiteSpace(minerAddress))
            {
                return BadRequest(new Error("Invalid miner address."));
            }

            Block candidate = NodeService.CreateCandidateBlock(minerAddress);
            MiningJob job = candidate.ToMiningJob();

            return Ok(job);
        }

        // POST: api/MiningJobs/687422eEA2cB73B5d3e242bA5456b782919AFc85
        [HttpPost("{minerAddress}", Name = "Post")]
        public IActionResult Post(string minerAddress, [FromBody]MiningJobResult value)
        {
            MiningResult miningResult = value.ToMiningResult(minerAddress);
            Block candidateBlock;
            BlockValidationResult validationResult = NodeService.TryAddBlock(
                miningResult,
                out candidateBlock);

            if (validationResult != BlockValidationResult.Ok)
            {
                return BadRequest(new Error(validationResult.GetEnumDescription()));
            }

            return CreatedAtRoute(
                "DefaultApi", new
                {
                    controller = "blocks",
                    id = candidateBlock.Index
                },
                candidateBlock);
        }
    }
}
