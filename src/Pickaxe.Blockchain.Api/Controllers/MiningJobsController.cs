﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;

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
            MiningJob job = new MiningJob
            {
                BlockIndex = 1,
                TransactionsHash = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08",
                PreviousBlockHash = "816534932c2b7154836da6afc367695e6337db8a921823784c14378abed4f7d7",
                Difficulty = 5
            };

            return Ok(job);
        }

        // POST: api/MiningJobs/687422eEA2cB73B5d3e242bA5456b782919AFc85
        [HttpPost("{minerAddress}", Name = "Post")]
        public IActionResult Post(string minerAddress, [FromBody]MiningJobResult value)
        {
            return Ok();
        }
    }
}
