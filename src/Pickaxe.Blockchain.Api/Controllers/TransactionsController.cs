using Microsoft.AspNetCore.Mvc;
using Pickaxe.Blockchain.Api.Mappers;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain;
using System.Collections.Generic;
using System.Linq;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Extensions;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;
using Nethereum.Hex.HexConvertors.Extensions;

namespace Pickaxe.Blockchain.Api.Controllers
{
    [Produces("application/json")]
    public class TransactionsController : BaseController
    {
        public TransactionsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        // POST api/transactions/send
        [HttpPost("api/[controller]/send")]
        public IActionResult CreateTransaction([FromBody]CreateTransactionRequest request)
        {
            Transaction newTransaction = request.ToDomainModel();
            CreateTransactionResult result = NodeService.TryCreateTransaction(
                newTransaction);

            if (result != CreateTransactionResult.Ok)
            {
                return BadRequest(new Error(result.GetEnumDescription()));
            }

            return CreatedAtRoute(
            "GetTransaction", new
            {
                transactionDataHash = newTransaction.DataHash.ToHex()
            },
            newTransaction.ToContract());
        }

        // GET api/address/f3a1e69b6176052fcc4a3248f1c5a91dea308ca9/transactions
        [HttpGet("api/address/{address}/[controller]")]
        public IActionResult GetTransactions(string address)
        {
            IEnumerable<Transaction> transactions = NodeService.GetTransactions(address);

            return Ok(transactions.Select(t => t.ToContract(t.Confirmed)).ToList());
        }

        // GET api/transactions/8a684cb8491ee419e7d46a0fd2438cad82d1278c340b5d01974e7beb6b72ecc2
        [HttpGet("api/[controller]/{transactionDataHash}", Name = "GetTransaction")]
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
        [HttpGet("api/[controller]/pending")]
        public IActionResult GetPending()
        {
            IList<Transaction> transactions = NodeService.GetPendingTransactions();

            return Ok(transactions.Select(t => t.ToContract(false)).ToList());
        }

        // GET api/transactions/confirmed
        [HttpGet("api/[controller]/confirmed")]
        public IActionResult GetConfirmed()
        {
            IList<Transaction> transactions = NodeService.GetConfirmedTransactions();

            return Ok(transactions.Select(t => t.ToContract()).ToList());
        }
    }
}
