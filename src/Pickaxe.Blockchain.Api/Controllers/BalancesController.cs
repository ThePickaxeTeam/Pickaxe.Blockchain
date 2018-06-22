using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    public class BalancesController : BaseController
    {
        public BalancesController(INodeService nodeService)
           : base(nodeService)
        {
        }

        // GET api/balances
        [HttpGet("api/[controller]")]
        public IActionResult GetAll()
        {
            Dictionary<string, long> balances = NodeService.GetAllBalances();
            return Ok(balances);
        }

        // GET api/address/f3a1e69b6176052fcc4a3248f1c5a91dea308ca9/balance
        [HttpGet("api/address/{address}/[controller]")]
        public IActionResult GetAccountBalances(string address)
        {
            bool found = NodeService.TryGetAccountBalances(
                address, out AccountBalances balances);
            if (found)
            {
                return Ok(balances.ToContract());
            }

            return NotFound();
        }
    }
}
