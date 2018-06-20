using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Domain;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TransactionsController : BaseController
    {
        public TransactionsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // GET api/transactions/8a684cb8491ee419e7d46a0fd2438cad82d1278c340b5d01974e7beb6b72ecc2
        [HttpGet("{transactionDataHash}")]
        public IActionResult Get(string transactionDataHash)
        {
            bool found = NodeService.TryGetTransaction(transactionDataHash, out Transaction transaction);
            if (found)
            {
                return Ok(transaction.ToContract(transaction.Confirmed));
            }

            return NotFound();
        }

        // GET api/transactions/pending
        [HttpGet("pending")]
        public IActionResult GetPending()
        {
            IList<Transaction> transactions = NodeService.GetPendingTransactions();

            return Ok(transactions.Select(t => t.ToContract(false)).ToList());
        }

        // GET api/transactions/confirmed
        [HttpGet("confirmed")]
        public IActionResult GetConfirmed()
        {
            IList<Transaction> transactions = NodeService.GetConfirmedTransactions();

            return Ok(transactions.Select(t => t.ToContract()).ToList());
        }
    }
}
