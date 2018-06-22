﻿using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Domain;
using System.Collections.ObjectModel;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BalancesController : BaseController
    {
        public BalancesController(INodeService nodeService)
           : base(nodeService)
        {
        }

        // GET 
        [HttpGet]
        public IActionResult GetAll()
        {
            ReadOnlyDictionary<string, long> balances = NodeService.GetAllBalances();
            return Ok(balances);
        }
    }
}
