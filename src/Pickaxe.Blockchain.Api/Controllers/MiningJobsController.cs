using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/MiningJobs")]
    public class MiningJobsController : ControllerBase
    {
        INodeService _nodeService;

        public MiningJobsController(
            INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        // GET: api/MiningJobs/687422eEA2cB73B5d3e242bA5456b782919AFc85
        [HttpGet("{minerAddress}", Name = "Get")]
        public IActionResult Get(string minerAddress)
        {
            if (string.IsNullOrWhiteSpace(minerAddress))
            {
                return BadRequest(new Error("Invalid miner address."));
            }

            Block candidate = _nodeService.CreateCandidateBlock(minerAddress);
            MiningJob job = candidate.ToMiningJob();

            return Ok(job);
        }

        // POST: api/MiningJobs/687422eEA2cB73B5d3e242bA5456b782919AFc85
        [HttpPost("{minerAddress}", Name = "Post")]
        public IActionResult Post(string minerAddress, [FromBody]MiningJobResult value)
        {
            //_nodeService.ProcessMiningResult(minerAddress);
            return Ok();
        }
    }
}
